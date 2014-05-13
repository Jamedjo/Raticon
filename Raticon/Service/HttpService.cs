using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Raticon.Service
{
    public abstract class IHttpService
    {
        public abstract string get(string url);
    }
    public class HttpService : IHttpService
    {
        public override string get(string url)
        {
            return new WebClient().DownloadString(url);
        }
    }
}
