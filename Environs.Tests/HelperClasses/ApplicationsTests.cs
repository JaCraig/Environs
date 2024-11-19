using Environs.HelperClasses;
using System;
using Xunit;

namespace Environs.Tests.HelperClasses
{
    public class ApplicationsTests
    {
        [Fact]
        public void GetInstalledApplications()
        {
            if (!OperatingSystem.IsWindows())
            {
                return;
            }
            System.Collections.Generic.IEnumerable<string> Results = Applications.GetInstalledApplications();
            Assert.NotEmpty(Results);
        }
    }
}