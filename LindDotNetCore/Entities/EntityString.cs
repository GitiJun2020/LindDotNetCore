using LindDotNetCore.Domain;

namespace LindDotNetCore.Entities
{
    /// <summary>
    /// 字符型主键的实体
    /// </summary>
    public class EntityString : IEntity<string>
    {
        public EntityString()
        {
            this.Id = PrimaryKey.GenerateNewId().ToString();
        }

        public string Id
        {
            get; set;
        }

        public override bool Equals(object obj)
        {
            return (obj as EntityString).Id == Id;
        }

        public bool Equals(string other)
        {
            return Id.Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}