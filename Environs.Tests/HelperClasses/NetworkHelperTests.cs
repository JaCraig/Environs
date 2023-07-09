using Environs.HelperClasses;
using Xunit;

namespace Environs.Tests.HelperClasses
{
    public class NetworkHelperTests
    {
        [Fact]
        public void LocalIP4() => Assert.NotNull(NetworkHelper.GetIPv4Address());

        [Fact]
        public void LocalIP6() => Assert.NotNull(NetworkHelper.GetIPv6Address());
    }
}