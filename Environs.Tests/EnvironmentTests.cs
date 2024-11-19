using System;
using System.Linq;
using Xunit;

namespace Environs.Tests
{
    public class EnvironmentTests
    {
        [Fact]
        public void BiosExecute()
        {
            if (!OperatingSystem.IsWindows())
            {
                throw Xunit.Sdk.SkipException.ForSkip("This test only runs on Windows.");
            }
            var TestObject = new Environment();
            System.Collections.Generic.IEnumerable<dynamic> Results = TestObject.Execute(CommonClasses.Bios);
            Assert.Single(Results);
            Assert.NotEmpty(Results.First().Manufacturer);
        }

        [Fact]
        public void Creation()
        {
            if (!OperatingSystem.IsWindows())
            {
                throw Xunit.Sdk.SkipException.ForSkip("This test only runs on Windows.");
            }
            var TestObject = new Environment();
            Assert.Contains("cimv2", TestObject.Namespaces);
            Assert.Equal("localhost", TestObject.Server);
        }

        [Fact]
        public void OperatingSystemExecute()
        {
            if (!OperatingSystem.IsWindows())
            {
                throw Xunit.Sdk.SkipException.ForSkip("This test only runs on Windows.");
            }
            var TestObject = new Environment();
            System.Collections.Generic.IEnumerable<dynamic> Results = TestObject.Execute(CommonClasses.OperatingSystem);
            Assert.Single(Results);
            Assert.NotEmpty(Results.First().CSName);
        }

        [Fact]
        public void QueryExecute()
        {
            if (!OperatingSystem.IsWindows())
            {
                throw Xunit.Sdk.SkipException.ForSkip("This test only runs on Windows.");
            }
            var TestObject = new Environment();
            System.Management.ManagementObjectCollection Results = TestObject.Execute("SELECT * FROM Win32_LoggedOnUser");
            Assert.NotEmpty(Results);
            foreach (System.Management.ManagementBaseObject Object in Results)
            {
                Assert.NotEmpty(Object["Antecedent"].ToString());
            }
        }
    }
}