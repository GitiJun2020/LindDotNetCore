using System;
using System.ComponentModel;

namespace Lind.DotNetCore.Entities
{
    /// <summary>
    /// 具有软删除行为的实体接口
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 删除日期
        /// </summary>
        [DisplayName("删除日期")]
        DateTime DeletedDate { get; set; }

        /// <summary>
        /// 删除者
        /// </summary>
        [DisplayName("删除者")]
        string DeletedUser { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        [DisplayName("是否删除")]
        bool IsDeleted { get; set; }
    }
}