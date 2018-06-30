using System;
using System.ComponentModel;

namespace Lind.DotNetCore.Entities
{
    /// <summary>
    /// 具有审记行为的实体接口
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    public interface IAuditable
    {
        /// <summary>
        /// 建立者
        /// </summary>
        [DisplayName("建立者")]
        string CreatedUser { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [DisplayName("建立日期")]
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// 最后更新者
        /// </summary>
        [DisplayName("最后更新者")]
        string LastUpdatedUser { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        [DisplayName("最后更新日期")]
        DateTime LastUpdatedDate { get; set; }
    }
}