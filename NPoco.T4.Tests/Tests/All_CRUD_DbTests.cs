using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NPoco.T4.Tests.Common;

using MyApp.Models;
using MyApp.Repositories;
using MyApp.Repositories.Core;

namespace NPoco.T4.Tests.Tests
{
	[TestClass]
	public class All_CRUD_RepoTests
	{
		private static Assembly activeAssembly;

		[ClassInitialize()]
		public static void Initialize(TestContext testContext)
		{
			// Grab it from a class in the target assembly:
			activeAssembly = Assembly.GetAssembly((new MyApp.Models.IdentityObject()).GetType());
		}


		[ClassCleanup()]
		public static void Cleanup() { }

		[TestMethod]
		public void IIdentity_Repos()
		{
			// Get all the classes implementing IIdentity
			var entityInterfaceType = typeof(IIdentity);
			var entityTypes = activeAssembly.GetTypes().Where(p => entityInterfaceType.IsAssignableFrom(p) && p.IsClass).ToList();


			foreach (var entityType in entityTypes)
			{
				var repoType = activeAssembly.GetTypes().Where(p =>
				p.IsClass &&
				p.GetInterfaces().Any(x =>
					x.IsGenericType &&
					x.GetGenericTypeDefinition() == typeof(IIdentityRepository<>) &&
					x.GetGenericArguments().Any(y =>
						y == entityType)
					)
				).FirstOrDefault();

				if (repoType == null)
				{
					Assert.Fail("CRUD repo missing for " + entityType.Name);
				}

				var repo = Activator.CreateInstance(repoType);
				
				// Grabbing the type that has the static generic method
				Type ctiClosedType = typeof(CrudTesterIdentity<>).MakeGenericType(entityType);

				// Grabbing the specific static method
				MethodInfo methodInfo = ctiClosedType.GetMethod("TestRepo", System.Reflection.BindingFlags.Static | BindingFlags.Public);

				Trace.WriteLine( Environment.NewLine + "Begin Testing - " + repoType.Name + " - ***************************");

				// Simply invoking the method and passing parameters
				// The null parameter is the object to call the method from. Since the method is static, pass null.
				object returnValue = methodInfo.Invoke(null,
					new object[] {
						repo,
						new List<string> { "Id", "OtherPropsHere" }
					});

				Trace.WriteLine("Tested - " + repoType.Name + " - ***************************");
				Trace.Flush();
			}
			
		}

		[TestMethod]
		public void IKeyed_Repos()
		{
			// Arrange ***

			// Get all the classes implementing IKeyed<>
			//var entityInterfaceType = typeof(IKeyed<>).MakeGenericType(typeof(int));

			var entityTypes = activeAssembly.GetTypes().Where(p => 
				p.IsClass &&
				p.GetInterfaces().Any(x =>
					x.IsGenericType &&
					x.GetGenericTypeDefinition() == typeof(IKeyed<>)
				)
			).ToList();


			foreach (var entityType in entityTypes)
			{
				var repoType = activeAssembly.GetTypes().Where(p =>
					p.IsClass &&
					p.GetInterfaces().Any(x =>
						x.IsGenericType &&
						x.GetGenericTypeDefinition() == typeof(IKeyedRepository<,>) &&
						x.GetGenericArguments().Any(y =>
							y == entityType)
					)
				).FirstOrDefault();

				if (repoType == null)
				{
					Assert.Fail("CRUD repo missing for " + entityType.Name);
				}

				var repo = Activator.CreateInstance(repoType);

				//Get TKey from entityType
				Type entityKeyType = entityType.GetInterfaces().Where(x =>
					x.IsGenericType &&
					x.GetGenericTypeDefinition() == typeof(IKeyed<>)
				).First().GetGenericArguments()[0];

				// Grabbing the type that has the static generic method
				Type ctClosedType = typeof(CrudTesterKeyed<,>).MakeGenericType(new Type[] {entityKeyType, entityType});

				// Grabbing the specific static method
				MethodInfo methodInfo = ctClosedType.GetMethod("TestRepo", System.Reflection.BindingFlags.Static | BindingFlags.Public);

				Trace.WriteLine(Environment.NewLine + "Begin Testing - " + repoType.Name + " (KeyType: " + entityKeyType.Name + ")" + " - ***************************");

				// Simply invoking the method and passing parameters
				// The null parameter is the object to call the method from. Since the method is static, pass null.
				object returnValue = methodInfo.Invoke(null,
					new object[] {
						repo,
						new List<string> { "DontProtectIdHere", "OtherPropsHere" }
					});

				Trace.WriteLine("Tested - " + repoType.Name + " (KeyType: " + entityKeyType.Name + ")" + " - ***************************");
				Trace.Flush();
			}
		
		}

	}
}
