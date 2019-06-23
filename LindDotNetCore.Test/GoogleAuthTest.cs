using System;
using Xunit;
using LindDotNetCore.Utils;
using System.Threading;

namespace LindDotNetCore.Test
{
    public class GoogleAuthTest
    {
        [Fact]
        public void Test1()
        {
            var key = "abc";
            var googleCode = new GoogleAuth(key, 1);
            Thread.Sleep(5000);
            var googleCode2 = new GoogleAuth(key, 1);
            Assert.Equal(googleCode.GenerateCode(), googleCode2.GenerateCode());
        }
        [Fact]
        public void Test2()
        {
            Assert.Equal("1", "1");
        }
    }
}
