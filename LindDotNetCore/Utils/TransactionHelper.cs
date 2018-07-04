using Microsoft.EntityFrameworkCore;
using System;

namespace LindDotNetCore.Utils
{
    /// <summary>
    /// 功能描述    ：事务管理者
    /// 创 建 者    ：lind
    /// 创建日期    ：2017/10/13 10:11:42
    /// 最后修改者  ：lind
    /// 最后修改日期：2017/10/13 10:11:42
    /// </summary>
    public class TransactionHelper
    {
        #region Public Methods

        public static void UseTransaction(DbContext db, Action action)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                action();
                transaction.Commit();
            }
        }

        #endregion Public Methods
    }
}