using LindDotNetCore.Domain;
using System;
using System.ComponentModel;

namespace LindDotNetCore.Entities
{
    /// <summary>
    /// 整型主键的实体
    /// </summary>
    public abstract class Entity : IEntity<int>
    {
        [DisplayName("编号")]
        public int Id { get; set; }

        public bool Equals(int other)
        {
            return Id.Equals(other);
        }

        #region override Methods

        public override bool Equals(object obj)
        {
            Console.WriteLine("Equals()");
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;
            if (item.Id == default(int) || this.Id == default(int))
                return false;
            else
                return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            Console.WriteLine("GetHashCode()");
            if (this.Id != default(int))
                return this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
            else
                return base.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        #endregion override Methods
    }
}