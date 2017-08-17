using Environs.HelperClasses;
using System.Linq;
using Xunit;

namespace Environs.Tests.HelperClasses
{
    public class ApplicationsTests
    {
        [Fact]
        public void GetInstalledApplications()
        {
            var Results = Applications.GetInstalledApplications();
            Assert.NotEqual(0, Results.Count());
        }
    }
}