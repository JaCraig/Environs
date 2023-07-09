using Environs.HelperClasses;
using Xunit;

namespace Environs.Tests.HelperClasses
{
    public class NetworkHelperTests
    {
        [Fact]
        public void LocalIP4() => Assert.NotNull(NetworkHelper.GetIPv4Address());

        [Fact]
        public void PingHost() => Assert.True(NetworkHelper.PingHost("www.google.com"));
    }
}