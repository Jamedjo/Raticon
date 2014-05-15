using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Raticon.Service
{
    public abstract class IHttpService
    {
        public abstract string Get(string url);
        public abstract void GetBinary(string url, string fileName);
    }
    public class HttpService : IHttpService
    {
        public override string Get(string url)
        {
            return new WebClient().DownloadString(url);
        }

        public override void GetBinary(string url, string fileName)
        {
            new WebClient().DownloadFile(url, fileName);
        }
    }
}
