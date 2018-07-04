using System.Threading.Tasks;

namespace LindDotNetCore.Repository
{
    /// <summary>
    /// 仓储异步接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepositoryAsync<TEntity>
    {
        /// <summary>
        /// 通过主键拿一个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(params object[] id);

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="item"></param>
        Task InsertAsync(TEntity item);

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="item"></param>
        Task UpdateAsync(TEntity item);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="item"></param>
        Task DeleteAsync(TEntity item);
    }
}