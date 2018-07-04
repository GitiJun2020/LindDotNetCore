using System.Data;

namespace LindDotNetCore.Repository
{
    /// <summary>
    /// 功能描述    ：IRepository
    /// 创 建 者    ：lind
    /// 创建日期    ：2017/10/11 10:49:33
    /// 最后修改者  ：lind
    /// 最后修改日期：2017/10/11 10:49:33
    /// </summary>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        IDbConnection DbConnection { get; }

        /// <summary>
        /// 通过主键拿一个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Find(params object[] id);

        /// <summary>
        /// 拿到可查询结果集
        /// </summary>
        /// <returns></returns>
        System.Linq.IQueryable<TEntity> GetModel();

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="item"></param>
        void Insert(TEntity item);

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="item"></param>
        void Update(TEntity item);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="item"></param>
        void Delete(TEntity item);
    }
}