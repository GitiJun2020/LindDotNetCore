using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LindDotNetCore.Repository
{
    /// <summary>
    /// 沙箱数据库
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class MockRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        #region Fields

        /// <summary>
        /// 库
        /// </summary>
        private static readonly ConcurrentDictionary<string, List<TEntity>> db = new ConcurrentDictionary<string, List<TEntity>>();

        /// <summary>
        /// 表
        /// </summary>
        private List<TEntity> tbl;

        public IDbConnection DbConnection => throw new NotImplementedException();

        #endregion Fields

        public MockRepository()
        {
            var tblName = typeof(TEntity).Name;
            if (!db.ContainsKey(tblName))
            {
                db.TryAdd(tblName, new List<TEntity>());
            }

            db.TryGetValue(tblName, out tbl);
        }

        #region IRepository<TEntity> 成员

        public TEntity Find(params object[] id)
        {
            return db[typeof(TEntity).Name].Find(i => i.GetType().GetProperty("Id").GetValue(i) == id[0]);
        }

        public IQueryable<TEntity> GetModel()
        {
            return db[typeof(TEntity).Name].AsQueryable();
        }

        public void SetDataContext(object db)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity item)
        {
            tbl.Add(item);
        }

        public void Update(TEntity item)
        {
            tbl.Remove(item);
            tbl.Add(item);
        }

        public void Delete(TEntity item)
        {
            tbl.Remove(item);
        }

        public IList<TEntity> GetBySql(string sql)
        {
            throw new NotImplementedException();
        }

        #endregion IRepository<TEntity> 成员
    }
}