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

using System.Collections.Generic;
using System.Management;

namespace Environs.HelperClasses
{
    /// <summary>
    /// Applications helpers
    /// </summary>
    public static class Applications
    {
        /// <summary>
        /// Gets the installed applications.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="options">The options.</param>
        /// <returns>The installed applications.</returns>
        public static IEnumerable<string> GetInstalledApplications(string machineName = "localhost", AuthenticationOptions options = null)
        {
            options = options ?? new AuthenticationOptions();
            List<string> ReturnValues = new List<string>();
            var Scope = SetScope(machineName, options);
            using (ManagementClass Class = new ManagementClass(Scope, new ManagementPath("StdRegProv"), null))
            {
                const uint HKEY_LOCAL_MACHINE = unchecked((uint)0x80000002);
                object[] Args = { HKEY_LOCAL_MACHINE, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", null };
                Class.InvokeMethod("EnumKey", Args);
                string[] Keys = Args[2] as string[];
                using (ManagementBaseObject MethodParams = Class.GetMethodParameters("GetStringValue"))
                {
                    MethodParams["hDefKey"] = HKEY_LOCAL_MACHINE;
                    foreach (string SubKey in Keys)
                    {
                        MethodParams["sSubKeyName"] = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + SubKey;
                        MethodParams["sValueName"] = "DisplayName";
                        using (ManagementBaseObject Results = Class.InvokeMethod("GetStringValue", MethodParams, null))
                        {
                            if (Results != null && (uint)Results["ReturnValue"] == 0)
                            {
                                ReturnValues.Add(Results["sValue"].ToString());
                            }
                        }
                    }
                }
            }
            return ReturnValues;
        }

        /// <summary>
        /// Sets the scope.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="options">The options.</param>
        /// <returns>The management scope</returns>
        private static ManagementScope SetScope(string machineName, AuthenticationOptions options)
        {
            if (options.Impersonate)
                return new ManagementScope(@"\\" + machineName + @"\root\default", new ConnectionOptions() { EnablePrivileges = true, Impersonation = ImpersonationLevel.Impersonate });
            if (!string.IsNullOrEmpty(options.UserName))
                return new ManagementScope(@"\\" + machineName + @"\root\default", new ConnectionOptions() { Username = options.UserName, Password = options.Password });
            return new ManagementScope(@"\\" + machineName + @"\root\default");
        }
    }
}