using System;
using Xunit;
using LindDotNetCore.Utils;
namespace LindDotNetCore.Test
{
    public class Base32Test
    {
        [Fact]
        public void Test1()
        {
            var key = "abc";
            var base32Key = Base32Utils.Encode(key);
            Console.WriteLine($"key={key},base32Key={base32Key}");
        }

    }
}
