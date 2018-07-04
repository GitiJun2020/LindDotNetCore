namespace LindDotNetCore.Repository
{
    /// <summary>
    /// 功能描述    ：EFOptions
    /// 创 建 者    ：lind
    /// 创建日期    ：2017/10/11 10:58:59
    /// 最后修改者  ：lind
    /// 最后修改日期：2017/10/11 10:58:59
    /// </summary>
    public class RepositoryConfig
    {
        /// <summary>
        /// 数据连接串
        /// </summary>
        public string ConnString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType DbType { get; set; }

        public override string ToString()
        {
            return $"ConnString:{ConnString},DbType:{DbType}";
        }
    }
}