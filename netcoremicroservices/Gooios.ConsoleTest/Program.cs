using Qiniu.Http;
using Qiniu.Storage;
using System;

namespace Gooios.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var config = new Config();
            config.Zone = Zone.ZoneCnEast;
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U512K;
            var target = new FormUploader(config);
            var key = "cookapp_6d96801a43204d5e9892dc0cd2bc22d2.png";
            var upload_token = "72vGWO8zK9EqGIK_-qUFRjG1JIogApCV13ls57Dv:UKGPe9XBpitE1_nAqS6xc1XxX4A=:eyJzY29wZSI6Imdvb2lvc2Nvb2s6Y29va2FwcF82ZDk2ODAxYTQzMjA0ZDVlOTg5MmRjMGNkMmJjMjJkMi5wbmciLCJkZWFkbGluZSI6MTU4NjIyNzU1Nn0=";
            HttpResult result = target.UploadFile("d:/test.png", key, upload_token, null).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.WriteLine("form upload result: " + result.Code.ToString());
            Console.WriteLine("form upload result: " + result.ToString());

            Console.ReadLine();
        }
    }
}
