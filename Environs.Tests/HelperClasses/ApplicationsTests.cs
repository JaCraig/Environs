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
                throw Xunit.Sdk.SkipException.ForSkip("This test only runs on Windows.");
            }
            System.Collections.Generic.IEnumerable<string> Results = Applications.GetInstalledApplications();
            Assert.NotEmpty(Results);
        }
    }
}