using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaticonTest
{
    class MockHttpService : IHttpService
    {
        private string returnValue;
        public MockHttpService(string returnValue)
        {
            this.returnValue = returnValue;
        }
        public override string Get(string url)
        {
            return returnValue;
        }

        public override void GetBinary(string url, string fileName)
        {
            throw new NotImplementedException();
        }
    }

    class AssertDontCallHttpService : IHttpService
    {
        public override string Get(string url)
        {
            throw new AssertFailedException("Http service Get was called with url '" + url + "'");
        }

        public override void GetBinary(string url, string fileName)
        {
            throw new AssertFailedException("Http service GetBinary was called with url '" + url + "' and fileName '" + fileName + "'");
        }
    }
}
