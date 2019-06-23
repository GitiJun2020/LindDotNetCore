using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LindDotNetCore.Utils
{
    /// <summary>
    /// google二步校验类
    /// </summary>
    public class GoogleAuth
    {
        /// <summary>
        /// 初始化验证码生成规则
        /// </summary>
        /// <param name="key">秘钥(手机使用Base32码)</param>
        /// <param name="duration">验证码间隔多久刷新一次（默认30秒和google同步）</param>
        public GoogleAuth(string key, long duration = 30)
        {
            this.serectKey = key;
            this.serectMobileKey = Base32Utils.Encode(Encoding.UTF8.GetBytes(key));
            this.durationTime = duration;
        }

        /// <summary>
        /// 间隔时间,官方推荐默认30秒
        /// </summary>
        private long durationTime { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        private long count
        {
            get
            {
                return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds / durationTime;
            }
        }

        /// <summary>
        /// 秘钥
        /// </summary>
        private string serectKey { get; set; }

        /// <summary>
        /// 手机端输入的秘钥
        /// </summary>
        private string serectMobileKey { get; set; }

        /// <summary>
        /// 到期秒数
        /// </summary>
        public long expireSeconds
        {
            get
            {
                return (durationTime - (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds % durationTime);
            }
        }

        /// <summary>
        /// 获取手机端秘钥
        /// </summary>
        /// <returns></returns>
        public string GetMobilePhoneKey()
        {
            if (serectMobileKey == null)
                throw new ArgumentNullException("SERECT_KEY_MOBILE");
            return serectMobileKey;
        }

        /// <summary>
        /// 生成认证码
        /// </summary>
        /// <returns>返回验证码</returns>
        public string GenerateCode()
        {
            return GenerateHashedCode(serectKey, count);
        }

        /// <summary>
        /// 按着密钥和时间戳生成6位验证码
        /// </summary>
        /// <param name="secret">密钥</param>
        /// <param name="iterationNumber">时间戳</param>
        /// <param name="digits">生成位数</param>
        /// <returns>返回验证码</returns>
        private string GenerateHashedCode(string secret, long iterationNumber, int digits = 6)
        {
            byte[] counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(counter);

            byte[] key = Encoding.ASCII.GetBytes(secret);

            HMACSHA1 hmac = new HMACSHA1(key, true);

            byte[] hash = hmac.ComputeHash(counter);

            int offset = hash[hash.Length - 1] & 0xf;

            int binary =
                ((hash[offset] & 0x7f) << 24)
                | ((hash[offset + 1] & 0xff) << 16)
                | ((hash[offset + 2] & 0xff) << 8)
                | (hash[offset + 3] & 0xff);

            int password = binary % (int)Math.Pow(10, digits); // 6 digits

            return password.ToString(new string('0', digits));
        }

     
        /// <summary>
        /// 授权校验
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool AuthValidate(Dictionary<string, string> db, string code)
        {
            bool isSuccess = false;
            foreach (var item in db)
            {
                GoogleAuth authenticator = new GoogleAuth(item.Key, 30);
                if (authenticator.GenerateCode() == code)
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }
    }
}