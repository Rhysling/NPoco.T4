using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco.T4;
using MyApp.Repositories.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NPoco.T4.Tests.Common
{
	public static class CrudTesterIdentity<T> where T : class, IIdentity, new()
	{
		public static void TestRepo(IIdentityRepository<T> repo, List<string> protectedPropertyNames)
		{
			T entity1 = Random<T>.Create(protectedPropertyNames);
			Test(repo, protectedPropertyNames, entity1);
		}

		public static void Test(IIdentityRepository<T> repo)
		{
			Test(repo, null);
		}

		public static void Test(IIdentityRepository<T> repo, List<string> protectedPropertyNames)
		{
			T entity1 = Random<T>.Create(protectedPropertyNames);
			Test(repo, protectedPropertyNames, entity1);
		}

		public static void Test(IIdentityRepository<T> repo, List<string> protectedPropertyNames, T seed)
		{
			// Get the inital count
			int countBefore = repo.All().Count();

			// create
			repo.Save(seed);
			Assert.IsTrue(seed.Id > 0);
			
			
			// read
			T entity2 = repo.FindBy(seed.Id);
			//Assert.IsTrue(seed.Equals(entity2));
			Assert.IsTrue(Comparison.PublicInstancePropertiesEqual(seed, entity2, protectedPropertyNames));

			// update
			seed = Random<T>.Update(seed, protectedPropertyNames);
			repo.Save(seed);
			T entity3 = repo.FindBy(seed.Id);
			Assert.IsFalse(Comparison.PublicInstancePropertiesEqual(entity2, seed, protectedPropertyNames));
			Assert.IsTrue(Comparison.PublicInstancePropertiesEqual(entity3, seed, protectedPropertyNames));

			// delete
			repo.Delete(seed.Id);
			int countAfter = repo.All().Count();
			Assert.IsTrue(countAfter == countBefore);

			// reseed key
			repo.ReseedKey();
			
		}

	}
}
