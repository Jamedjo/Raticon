using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    public class PosterService
    {
        IFileSystem fileSystem;
        IHttpService httpService;
        public PosterService(IFileSystem fileSystem  = null, IHttpService httpService = null)
        {
            this.fileSystem = fileSystem ?? new FileSystem();
            this.httpService = httpService ?? new HttpService();
        }

        public void Download(string url, string imagePath, Action<string, string> errorHandler = null)
        {
            if (fileSystem.File.Exists(imagePath) || String.IsNullOrWhiteSpace(url))
            {
                return;
            }

            try
            {
                httpService.GetBinary(url, imagePath);
            }
            catch
            {
                if (errorHandler == null) { throw; }
                else { errorHandler(url, imagePath); }
            }

        }
    }
}
