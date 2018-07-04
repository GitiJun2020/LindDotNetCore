using System;

namespace LindDotNetCore.Domain
{
    /// <summary>
    /// 实体类标示接口（主键类型由派生类确定）
    /// </summary>
    public interface IEntity<TKey> : IEquatable<TKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        TKey Id { get; set; }
    }
}