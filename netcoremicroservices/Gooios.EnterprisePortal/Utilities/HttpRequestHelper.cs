using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gooios.EnterprisePortal.Utilities
{
    public static class HttpRequestHelper
    {
        public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> param, string callerAppKey, string userId = "")
          where T : class
        {
            T ret = default(T);

            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            var p = string.Join("&", param.Select(o => { return o.Key + "=" + o.Value; }));
            url = string.Join("?", url, p);

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if (!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var response = await client.GetAsync(url);//await异步等待回应
                response.EnsureSuccessStatusCode(); //确保HTTP成功状态值
                var res = (await response.Content.ReadAsStringAsync());//await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
            }

            return ret;
        }

        /// <summary>
        /// GetAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">example: http://www.51aldin.com/resource </param>
        /// <param name="param">example: a=1&b=2&c=s</param>
        /// <param name="callerAppKey"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string url, string param, string callerAppKey, string userId = "")
            where T : class
        {
            T ret = default(T);

            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            var p = param;
            url = string.Join("?", url, p);

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if (!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var response = await client.GetAsync(url);//await异步等待回应
                response.EnsureSuccessStatusCode(); //确保HTTP成功状态值
                var res = (await response.Content.ReadAsStringAsync());//await异步读取最后的JSON（注意此时gzip已经被自动解压缩了，因为上面的AutomaticDecompression = DecompressionMethods.GZip）
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
            }

            return ret;
        }

        public static async Task<T> PostAsync<T>(string url, Dictionary<string, string> param, string callerAppKey, string userId = "")
            where T : class
        {
            T ret = default(T);
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if (!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var content = new FormUrlEncodedContent(param);
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var res = (await response.Content.ReadAsStringAsync());
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
            }

            return ret;
        }

        public static async Task<T> PostAsync<T>(string url, string jsonStr, string callerAppKey,string userId="")
            where T : class
        {
            T ret = default(T);
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if(!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var res = (await response.Content.ReadAsStringAsync());
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
            }

            return ret;
        }

        public static async Task PostNoResultAsync(string url, string jsonStr, string callerAppKey, string userId = "")
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if (!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var res = (await response.Content.ReadAsStringAsync());
            }
        }

        public static async Task<T> PutAsync<T>(string url, Dictionary<string, string> param, string callerAppKey, string userId = "")
            where T : class
        {
            T ret = default(T);
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if (!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var content = new FormUrlEncodedContent(param);
                var response = await client.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                var res = (await response.Content.ReadAsStringAsync());
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
            }

            return ret;
        }
        public static async Task PutNoResultAsync(string url, Dictionary<string, string> param, string callerAppKey, string userId = "")
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if (!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var content = new FormUrlEncodedContent(param);
                var response = await client.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                var res = (await response.Content.ReadAsStringAsync());
            }
        }

        public static async Task<T> PutAsync<T>(string url, string jsonStr, string callerAppKey, string userId = "")
            where T : class
        {
            T ret = default(T);
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if (!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                var res = (await response.Content.ReadAsStringAsync());
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
            }

            return ret;
        }

        public static async Task PutNoResultAsync(string url, string jsonStr, string callerAppKey, string userId = "")
        {
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if (!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);
                response.EnsureSuccessStatusCode();
                var res = (await response.Content.ReadAsStringAsync());
            }
        }

        public static async Task<T> DeleteAsync<T>(string url, Dictionary<string, string> param, string callerAppKey, string userId = "")
            where T : class
        {
            T ret = default(T);
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            //var p = string.Join("&", param.Select(o => { return o.Key + "=" + o.Value; }));
            //url = string.Join("?", url, p);

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if (!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var content = new FormUrlEncodedContent(param);
                var response = await client.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                var res = (await response.Content.ReadAsStringAsync());
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
            }

            return ret;
        }

        public static async Task<T> DeleteAsync<T>(string url, string jsonStr, string callerAppKey, string userId = "")
            where T : class
        {
            T ret = default(T);
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };

            //var p = string.Join("&", param.Select(o => { return o.Key + "=" + o.Value; }));
            //url = string.Join("?", url, p);

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("gooiosapikey", callerAppKey);

                if (!string.IsNullOrEmpty(userId)) client.DefaultRequestHeaders.Add("userId", userId);

                var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                var response = await client.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
                var res = (await response.Content.ReadAsStringAsync());
                ret = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(res);
            }

            return ret;
        }
        
    }
}
