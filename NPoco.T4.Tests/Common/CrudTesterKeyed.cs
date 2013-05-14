using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPoco.T4;
using MyApp.Repositories.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NPoco.T4.Tests.Common
{
	public static class CrudTesterKeyed<TKey, TEntity> where TEntity : class, IKeyed<TKey>, new()
	{
		public static void TestRepo(IKeyedRepository<TKey, TEntity> repo, List<string> protectedPropertyNames)
		{
			TEntity entity1 = Random<TEntity>.Create(protectedPropertyNames);
			Test(repo, protectedPropertyNames, entity1);
		}

		public static void Test(IKeyedRepository<TKey, TEntity> repo)
		{
			Test(repo, null);
		}

		public static void Test(IKeyedRepository<TKey, TEntity> repo, List<string> protectedPropertyNames)
		{
			TEntity entity1 = Random<TEntity>.Create(protectedPropertyNames);
			Test(repo, protectedPropertyNames, entity1);
		}

		public static void Test(IKeyedRepository<TKey, TEntity> repo, List<string> protectedPropertyNames, TEntity seed)
		{
			// Get the inital count
			int countBefore = repo.All().Count();

			// Look for the seed.Id item in the DB; if found, refresh up to 5 times then error.
			int loopCount = 0;
			while (true)
			{
				var item = repo.FindBy(seed.Id);
				if (item == null) break;
				if (loopCount > 4)
				{
					Assert.Fail("Can't find a unique new key for: " + repo.GetType().Name);
					return;
				}
				seed = Random<TEntity>.UpdateSpecifiedProperties(seed, new List<string> { "Id" });
				loopCount += 1;
			}
			

			// add
			repo.Insert(seed);
			int countAfter = repo.All().Count();
			Assert.IsTrue(countBefore + 1 == countAfter);


			// read
			TEntity entity2 = repo.FindBy(seed.Id);
			//Assert.IsTrue(seed.Equals(entity2));
			Assert.IsTrue(Comparison.PublicInstancePropertiesEqual(seed, entity2, protectedPropertyNames));

			// update
			protectedPropertyNames.Add("Id"); // Make sure Id is now in the list.
			seed = Random<TEntity>.Update(seed, protectedPropertyNames);
			repo.Update(seed);
			TEntity entity3 = repo.FindBy(seed.Id);
			Assert.IsFalse(Comparison.PublicInstancePropertiesEqual(entity2, seed, protectedPropertyNames));
			Assert.IsTrue(Comparison.PublicInstancePropertiesEqual(entity3, seed, protectedPropertyNames));

			// delete
			repo.Delete(seed.Id);
			countAfter = repo.All().Count();
			Assert.IsTrue(countAfter == countBefore);

		}
	}
}
