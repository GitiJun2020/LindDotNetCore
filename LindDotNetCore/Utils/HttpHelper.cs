using LindDotNetCore.Logger;
using Polly;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace LindDotNetCore.Utils
{
    /// <summary>
    /// Http相关操作
    /// Author:lind
    /// </summary>
    public class HttpHelper
    {
        #region Private Fields

        /// <summary>
        /// http请求对象
        /// </summary>
        private static readonly HttpClient httpClient;

        /// <summary>
        /// 日志
        /// </summary>
        private static ILogger logger = new LindLogger();

        /// <summary>
        /// polly重试机制
        /// </summary>
        private static HttpResponseMessage retryTwoTimesPolicy(Func<HttpResponseMessage> action)
        {
            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                 5,
                 retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)),
                 (ex, timer, c, i) =>
                 {
                     logger.Info($"执行失败! 重试次数 {c}");
                     logger.Info($"异常来自 {ex.GetType().Name}");
                 });
            return policy.Execute(action);
        }

        #endregion Private Fields

        #region Public Constructors

        static HttpHelper()
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip
            };
            httpClient = new HttpClient(handler);
            httpClient.Timeout = new TimeSpan(0, 0, 30);
            httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// 加工当前Uri
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="nv"></param>
        /// <returns></returns>
        private static string GeneratorUri(string requestUri, NameValueCollection nv)
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

        /// <summary>
        /// 对流进行解压
        /// </summary>
        /// <param name="response"></param>
        private static void UnGZip(HttpResponseMessage response)
        {
            bool isGzip = response.Content.Headers.ContentEncoding.Contains("gzip");
            if (isGzip)
            {
                Stream decompressedStream = new MemoryStream();
                using (var gzipStream = new GZipStream(response.Content.ReadAsStreamAsync().Result, CompressionMode.Decompress))
                {
                    gzipStream.CopyToAsync(decompressedStream);
                }
                decompressedStream.Seek(0, SeekOrigin.Begin);

                var originContent = response.Content;
                response.Content = new StreamContent(decompressedStream);
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// DELETE请求
        /// </summary>
        /// <param name="requestUri">源地址</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Delete(
            string requestUri)
        {
            return await Task.Run(() =>
           {
               return retryTwoTimesPolicy(() =>
               {
                   var result =httpClient.DeleteAsync(requestUri).Result;
                   UnGZip(result);
                   return result;
               });
           });
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="requestUri">服务地址</param>
        /// <param name="nv">参数键值</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Get(
            string requestUri)
        {
            return await Task.Run(() =>
            {
                return retryTwoTimesPolicy(() =>
                {
                    var result =httpClient.GetAsync(requestUri).Result;
                    UnGZip(result);
                    return result;
                });
            });
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="requestUri">请求地址</param>
        /// <param name="nv">参数键值</param>
        /// <returns>响应流</returns>
        public static async Task<HttpResponseMessage> Post(
            string requestUri,
            NameValueCollection nv)
        {
            return await Task.Run(() =>
           {
               return retryTwoTimesPolicy(() =>
               {
                   var body = new Dictionary<string, string>();
                   foreach (string item in nv.Keys)
                   {
                       body.Add(item, nv[item]);
                   }
                   var result = httpClient.PostAsync(requestUri, new FormUrlEncodedContent(body)).Result;
                   UnGZip(result);
                   return result;
               });
           });
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Post<T>(
            string requestUri,
            T entity)
        {
            return await Post(requestUri, entity.ToNameValueCollection());
        }

        /// <summary>
        ///  PUT请求
        /// </summary>
        /// <param name="requestUri">请求地址</param>
        /// <param name="nv">参数键值</param>
        /// <returns>响应流</returns>
        public static async Task<HttpResponseMessage> Put(
            string requestUri,
            NameValueCollection nv)
        {
            return await Task.Run(() =>
            {
                return retryTwoTimesPolicy(() =>
                {
                    var body = new Dictionary<string, string>();
                    foreach (string item in nv.Keys)
                    {
                        body.Add(item, nv[item]);
                    }
                    var result = httpClient.PutAsync(requestUri, new FormUrlEncodedContent(body)).Result;
                    UnGZip(result);
                    return result;
                });
            });
        }

        /// <summary>
        /// PUT请求
        /// </summary>
        /// <param name="requestUri">请求对象</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> Put<T>(
            string requestUri,
            T entity)
        {
            return await Put(requestUri, entity.ToNameValueCollection());
        }

        #endregion Public Methods
    }
}