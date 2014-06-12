using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Raticon.Service
{
    public abstract class ResourceServiceBase
    {
        public abstract Stream GetAsStream(string resource);

        //private string GetAsText(string resource)
        //{
        //    var stream = GetAsStream(resource);
        //    string text;
        //    using (System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, Encoding.UTF8))
        //    {
        //        text = streamReader.ReadToEnd();
        //    }
        //    return text;
        //}

        public void ExtractTo(string resource, string path)
        {
            ExtractTo(GetAsStream(resource), path, resource);
        }

        protected void ExtractTo(Stream stream, string path, string resourceName)
        {
            try
            {
                using (var file = File.Create(path))
                {
                    stream.CopyTo(file);
                }
            }
            catch (System.UnauthorizedAccessException)
            {
#if DEBUG
                throw;
#else
                System.Windows.MessageBox.Show("Couldn't write to file '" + path + "' while trying to extract '" + resourceName + "'.", "Unauthorized Access Exception", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
#endif
            }

        }
        
    }

    public class ResourceService : ResourceServiceBase
    {
        public ResourceService()
        {
            if (!UriParser.IsKnownScheme("pack"))
            {
                //Initialize pack:// Uri scheme
                string packUriScheme = System.IO.Packaging.PackUriHelper.UriSchemePack;
            }
        }

        public override Stream GetAsStream(string resource)
        {
            return GetAsStream(new Uri(@"pack://application:,,,/" + resource));
        }

        public Stream GetAsStream(Uri resource)
        {
            return Application.GetResourceStream(resource).Stream;
        }

        public void ExtractTo(Uri resource, string path)
        {
            ExtractTo(GetAsStream(resource), path, resource.ToString());
        }
    }

    public class EmbeddedResourceService : ResourceServiceBase
    {
        private Assembly assembly;
        public EmbeddedResourceService(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public EmbeddedResourceService() : this(Assembly.GetExecutingAssembly())
        {
        }

        public EmbeddedResourceService(string assemblyName) : this(Assembly.LoadFrom(assemblyName))
        {
        }

        public EmbeddedResourceService(Type classInAssembly)
            : this(Assembly.GetAssembly(classInAssembly))
        {
        }

        public override Stream GetAsStream(string resource)
        {
            return assembly.GetManifestResourceStream(resource);
        }
    }

}
