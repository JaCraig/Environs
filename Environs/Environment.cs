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

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Management;

namespace Environs
{
    /// <summary>
    /// WMI environment query helper
    /// </summary>
    public class Environment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Environment"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="options">The options.</param>
        public Environment(string server = "localhost", AuthenticationOptions options = null)
        {
            Server = server ?? throw new ArgumentNullException(nameof(server));
            Options = options ?? new AuthenticationOptions();
            Namespaces = Execute((CommonClasses)"__Namespace", "root").Select(x => ((string)x.Name).ToLower(CultureInfo.CurrentCulture)).ToArray();
        }

        /// <summary>
        /// Gets or sets the namespaces.
        /// </summary>
        /// <value>The namespaces.</value>
        public string[] Namespaces { get; set; }

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>The options.</value>
        public AuthenticationOptions Options { get; }

        /// <summary>
        /// Gets the server.
        /// </summary>
        /// <value>The server.</value>
        public string Server { get; }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>The scope.</value>
        private ManagementScope Scope { get; set; }

        /// <summary>
        /// Executes the specified management path.
        /// </summary>
        /// <param name="managementPath">The management path.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns>The values of the management path</returns>
        public IEnumerable<dynamic> Execute(CommonClasses managementPath, string nameSpace = @"root\cimv2")
        {
            SetScope(nameSpace);
            List<dynamic> ReturnValues = new List<dynamic>();
            using (ManagementClass Class = new ManagementClass(Scope, new ManagementPath(managementPath), new ObjectGetOptions()))
            {
                foreach (ManagementObject NameSpaceInfo in Class.GetInstances())
                {
                    IDictionary<string, object> TempValue = new ExpandoObject();
                    foreach (var Property in NameSpaceInfo.Properties)
                    {
                        TempValue[Property.Name] = Property.Value;
                    }
                    ReturnValues.Add(TempValue);
                }
            }
            return ReturnValues;
        }

        /// <summary>
        /// Executes the specified management path.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="nameSpace">The name space.</param>
        /// <returns>The values of the management path</returns>
        public ManagementObjectCollection Execute(string query, string nameSpace = @"root\cimv2")
        {
            SetScope(nameSpace);
            List<dynamic> ReturnValues = new List<dynamic>();
            ObjectQuery TempQuery = new ObjectQuery(query);
            using (ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, TempQuery))
            {
                return Searcher.Get();
            }
        }

        /// <summary>
        /// Sets the scope.
        /// </summary>
        /// <param name="nameSpace">The name space.</param>
        private void SetScope(string nameSpace)
        {
            if (Options.Impersonate)
                Scope = new ManagementScope(@"\\" + Server + @"\" + nameSpace, new ConnectionOptions() { EnablePrivileges = true, Impersonation = ImpersonationLevel.Impersonate });
            else if (!string.IsNullOrEmpty(Options.UserName))
                Scope = new ManagementScope(@"\\" + Server + @"\" + nameSpace, new ConnectionOptions() { Username = Options.UserName, Password = Options.Password });
            else
                Scope = new ManagementScope(@"\\" + Server + @"\" + nameSpace);
        }
    }
}