using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Repositories.Core
{
	public interface IIdentityRepository<TEntity> where TEntity : class, IIdentity
	{
		int Save(TEntity entity);
		bool Save(IEnumerable<TEntity> items);
		bool Delete(int id);
		bool Delete(IEnumerable<int> id);
		bool Destroy(int id);
		TEntity FindBy(int id);
		IEnumerable<TEntity> All();
		void ReseedKey();
	}

	public interface IIdentity
	{
		int Id { get; }
	}


	public interface IKeyedRepository<TKey, TEntity> where TEntity : class, IKeyed<TKey>
	{
		bool Insert(TEntity entity);
		bool Update(TEntity entity);
		bool Delete(TKey id);
		bool Delete(IEnumerable<TKey> id);
		bool Destroy(TKey id);
		TEntity FindBy(TKey id);
		IEnumerable<TEntity> All();
		TKey MaxId();
	}

	public interface IKeyed<TKey>
	{
		TKey Id { get; set; }
	}

}