using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace LindDotNetCore.Utils
{
    /// <summary>
    /// 校验结果
    /// </summary>
    public class HttpReturn
    {
        #region Public Properties

        public HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// 服务端配置相关租户列表
    /// </summary>
    public class ValidateConfig
    {
        #region Public Properties

        /// <summary>
        /// 项目键，用于网络传输
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 租户app的最终过期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string PassKey { get; set; }

        /// <summary>
        /// 一个连接的有效分钟
        /// </summary>
        public int ValidateMinutes { get; set; }

        #endregion Public Properties
    }

    /// <summary>
    /// 校验相关
    /// MD5(参数+密钥)＝密文
    /// </summary>
    public class ValidateHelper
    {
        #region Public Fields

        /// <summary>
        /// 密文键，追加到url地址上
        /// </summary>
        private const string CipherText = "ciphertext";

        #endregion Public Fields

        #region Public Methods

        #region 生成校验码

        /// <summary>
        /// 生成秘文，并返回[在url上加这个键ciphertext]
        /// </summary>
        /// <param name="coll">已有的集合</param>
        /// <param name="appKey">当前项目的appKey</param>
        /// <param name="passKey">appkey对应的passKey</param>
        /// <returns></returns>
        public static NameValueCollection GenerateCipherText(
            NameValueCollection coll,
            string appKey,
            string passKey,
            bool isClearCode = false)
        {
            if (coll == null)
                coll = new NameValueCollection();
            var timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds.ToString();//统一的UTC时间戳
            if (string.IsNullOrWhiteSpace(coll.Get("appkey")))
                coll.Add("appkey", appKey);
            if (string.IsNullOrWhiteSpace(coll.Get("timestamp")))
                coll.Add("timestamp", timeStamp);
            var paramStr = new StringBuilder();
            var keys = new List<string>();

            #region 验证算法

            foreach (string param in coll.Keys)
            {
                if (!string.IsNullOrEmpty(param))
                {
                    keys.Add(param.ToLower());
                }
            }
            keys.Sort();
            foreach (string p in keys)
            {
                if (!string.IsNullOrEmpty(coll[p]))
                {
                    paramStr.Append(coll[p]);
                }
            }

            paramStr.Append(passKey);

            #endregion 验证算法

            if (isClearCode)//post,put,不需要保留明文，只需要保存appkey和passkey
            {
                coll = new NameValueCollection();//清空明文列表
                coll.Add("appkey", appKey);
                coll.Add("timestamp", timeStamp);
            }
            if (string.IsNullOrWhiteSpace(coll.Get(CipherText)))//避免重复添加
                coll.Add(CipherText, MD5(paramStr.ToString()).ToString());

            return coll;
        }

        /// <summary>
        /// 把对象转成k/v串并加密返回密文（密文一般会添加到URL上参数名为CipherText）
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="appKey">项目的key</param>
        /// <param name="passKey">为项目颁发的密钥</param>
        /// <returns></returns>
        public static NameValueCollection GenerateCipherText<T>(List<T> list, string appKey, string passKey, bool isClearCode = false)
        {
            var nv = new NameValueCollection(); ;
            var prefix = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                prefix.Add("Root_" + i.ToString());
                ReGenerate(list[i], i, prefix, nv);
                prefix.Clear();
            }

            return GenerateCipherText(nv, appKey, passKey, isClearCode);
        }

        /// <summary>
        /// 把对象转成k/v串并加密返回密文（密文一般会添加到URL上参数名为CipherText）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="appKey"></param>
        /// <param name="passKey"></param>
        /// <returns></returns>
        public static NameValueCollection GenerateCipherText<T>(T entity, string appKey, string passKey, bool isClearCode = false)
        {
            return GenerateCipherText(new List<T> { entity }, appKey, passKey, isClearCode);
        }

        /// <summary>
        /// 加工当前Uri
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="nv"></param>
        /// <returns></returns>
        public static string GeneratorUri(string requestUri, NameValueCollection nv)
        {
            if (nv != null)
            {
                if (requestUri.IndexOf("?") > -1)
                {
                    requestUri += "&" + nv.ToUrl();
                }
                else
                {
                    requestUri += "?" + nv.ToUrl();
                }
            }
            return requestUri;
        }

        #endregion 生成校验码

        /// <summary>
        /// 校验合法性
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static HttpReturn Validate(HttpRequest request)
        {
            var coll = DictionaryExtensions.FromUrl(request.QueryString.Value.ToLower());

            StringBuilder paramStr = new StringBuilder();

            var config = ConfigFileHelper.Get<List<ValidateConfig>>()
                                         .FirstOrDefault(i => i.AppKey == coll["appkey"]);
            if (config == null)
            {
                return new HttpReturn { HttpStatusCode = HttpStatusCode.Forbidden, Message = "AppKey不是合法的，请先去组织生成有效的Key！" };
            }
            if (config.ExpireDate < DateTime.Now)
            {
                return new HttpReturn { HttpStatusCode = HttpStatusCode.Forbidden, Message = "AppKey已经过期！" };
            }

            var keys = new List<string>();
            foreach (string param in coll.Keys)
            {
                if (!string.IsNullOrEmpty(param))
                {
                    keys.Add(param.ToLower());
                }
            }
            keys.Sort();
            foreach (string p in keys)
            {
                if (p != CipherText)
                {
                    if (!string.IsNullOrEmpty(coll[p]))
                    {
                        paramStr.Append(coll[p]);
                    }
                }
            }
            paramStr.Append(config.PassKey);

            double timeStamp;
            if (double.TryParse(coll["timestamp"], out timeStamp))
            {
                //当前UTC时间与1700/01/01的时间戳
                if (((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds - timeStamp) / 60 > config.ValidateMinutes)
                {
                    return new HttpReturn { HttpStatusCode = HttpStatusCode.Forbidden, Message = "请求已过期！" };
                }
            }
            else
            {
                return new HttpReturn { HttpStatusCode = HttpStatusCode.Forbidden, Message = "时间戳异常！" };
            }
            if (MD5(paramStr.ToString()).ToString() != coll[CipherText])
            {
                return new HttpReturn { HttpStatusCode = HttpStatusCode.Forbidden, Message = "验证失败，请求非法！" };
            }

            return new HttpReturn { HttpStatusCode = HttpStatusCode.OK, Message = "校验通过！" };
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        private static string MD5(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = System.Security.Cryptography.MD5.Create().ComputeHash(b);
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
                ret.Append(b[i].ToString("x").PadLeft(2, '0'));
            return ret.ToString();
        }

        private static void ReGenerate(object obj, int i, List<string> prefix, NameValueCollection nv)
        {
            if (obj != null)
            {
                prefix.Add(obj.GetType().Name);

                foreach (var item in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (item.PropertyType.IsClass && item.PropertyType != typeof(string))
                    {
                        var sub = item.GetValue(obj);
                        ReGenerate(sub, i, prefix, nv);
                    }
                    else
                    {
                        if (item.GetValue(obj) != null)
                            nv.Add(string.Join("_", prefix) + "_" + item.Name, item.GetValue(obj).ToString());
                    }
                }
            }
        }

        #endregion Private Methods
    }
}