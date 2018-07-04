using System;

namespace LindDotNetCore.Repository
{
    public class EFConfig : RepositoryConfig
    {
        public Type DbContextType { get; set; }
    }
}