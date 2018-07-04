using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LindDotNetCore.Repository
{
    /// <summary>
    /// 使用Dapper实现的仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DapperRepository<TEntity> :
        IRepository<TEntity>
        where TEntity : class
    {
        #region Fields

        private IDbConnection conn;

        #endregion Fields

        #region Constructors

        static DapperRepository()
        {
            //表名为类型名，如果不加则是实体名的复数形式
            SqlMapperExtensions.TableNameMapper = (type) =>
            {
                var newName = typeof(TEntity).GetCustomAttributes(false).FirstOrDefault(i => i.GetType() == typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute));
                return newName == null
                     ? type.Name
                     : (newName as TableAttribute) != null
                     ? (newName as TableAttribute).Name
                     : (newName as System.ComponentModel.DataAnnotations.Schema.TableAttribute)?.Name;
            };
        }

        public DapperRepository(RepositoryConfig config)
        {
            if (config.DbType == DbType.Sqlserver)
                conn = new SqlConnection(config.ConnString);
            else if (config.DbType == DbType.MySql)
                conn = new MySql.Data.MySqlClient.MySqlConnection(config.ConnString);
            else if (config.DbType == DbType.SqlLite)
                conn = new SqliteConnection(config.ConnString);
            else
                throw new ArgumentException($"repository only support serversql,mysql,sqllite,current config:{config.ToString()}");
        }

        public IDbConnection DbConnection => conn;

        #endregion Constructors

        #region IRepository<TEntity> 成员

        public void Delete(TEntity item)
        {
            conn.Delete(item);
        }

        public TEntity Find(params object[] id)
        {
            return conn.Get<TEntity>(id);
        }

        public IQueryable<TEntity> GetModel()
        {
            return conn.GetAll<TEntity>().AsQueryable();
        }

        public void Insert(TEntity item)
        {
            conn.Insert(item);
        }

        public void Update(TEntity item)
        {
            conn.Update(item);
        }

        #endregion IRepository<TEntity> 成员
    }
}