/*
Copyright 2017 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Environs.HelperClasses
{
    /// <summary>
    /// Helper methods for network related items.
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// Gets the local IPv4 address.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <returns>The local IPv4 address.</returns>
        public static string GetIPv4Address(string machineName = "localhost")
        {
            return Dns.GetHostEntry(machineName)
                      .AddressList
                      .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork)
                      ?.ToString();
        }

        /// <summary>
        /// Gets the local IPv6 address.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <returns>The local IPv6 address</returns>
        public static string GetIPv6Address(string machineName = "localhost")
        {
            return Dns.GetHostEntry(machineName)
                      .AddressList
                      .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetworkV6)
                      ?.ToString();
        }

        /// <summary>
        /// Pings the host.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="timeOut">The time out.</param>
        /// <returns>True if it is pinged successfully, false otherwise</returns>
        public static bool PingHost(string address, int timeOut = 100)
        {
            using (Ping PingSender = new Ping())
            {
                PingOptions Options = new PingOptions()
                {
                    DontFragment = true
                };
                string Data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] DataBuffer = Encoding.ASCII.GetBytes(Data);
                PingReply Reply = PingSender.Send(address, timeOut, DataBuffer, Options);
                return Reply.Status == IPStatus.Success;
            }
        }
    }
}