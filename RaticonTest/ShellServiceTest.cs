using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raticon.Service;

namespace RaticonTest
{
    [TestClass]
    public class ShellServiceTest
    {
        [TestMethod]
        public void It_should_execute_script_and_return_response()
        {
            StringAssert.Contains(new ShellService().Execute("echo %PATH%"), "Program Files");
        }
    }
}
