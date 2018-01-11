using System.Linq;
using Xunit;

namespace Environs.Tests
{
    public class EnvironmentTests
    {
        [Fact]
        public void BiosExecute()
        {
            var TestObject = new Environment();
            var Results = TestObject.Execute(CommonClasses.Bios);
            Assert.Single(Results);
            Assert.NotEmpty(Results.First().Manufacturer);
        }

        [Fact]
        public void Creation()
        {
            var TestObject = new Environment();
            Assert.Contains("cimv2", TestObject.Namespaces);
            Assert.Equal("localhost", TestObject.Server);
        }

        [Fact]
        public void OperatingSystemExecute()
        {
            var TestObject = new Environment();
            var Results = TestObject.Execute(CommonClasses.OperatingSystem);
            Assert.Single(Results);
            Assert.NotEmpty(Results.First().CSName);
        }

        [Fact]
        public void QueryExecute()
        {
            var TestObject = new Environment();
            var Results = TestObject.Execute("SELECT * FROM Win32_LoggedOnUser");
            Assert.NotEmpty(Results);
            foreach (var Object in Results)
            {
                Assert.NotEmpty(Object["Antecedent"].ToString());
            }
        }
    }
}