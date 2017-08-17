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

namespace Environs
{
    /// <summary>
    /// Common WMI classes
    /// </summary>
    public class CommonClasses
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonClasses"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected CommonClasses(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the battery.
        /// </summary>
        /// <value>The battery.</value>
        public static CommonClasses Battery => new CommonClasses("win32_battery");

        /// <summary>
        /// Gets the bios.
        /// </summary>
        /// <value>The bios.</value>
        public static CommonClasses Bios => new CommonClasses("Win32_BIOS");

        /// <summary>
        /// Gets the computer system product.
        /// </summary>
        /// <value>The computer system product.</value>
        public static CommonClasses ComputerSystemProduct => new CommonClasses("Win32_ComputerSystemProduct");

        /// <summary>
        /// Gets the data files.
        /// </summary>
        /// <value>The data files.</value>
        public static CommonClasses DataFiles => new CommonClasses("CIM_DataFile");

        /// <summary>
        /// Gets the disk drive.
        /// </summary>
        /// <value>The disk drive.</value>
        public static CommonClasses DiskDrive => new CommonClasses("win32_DiskDrive");

        /// <summary>
        /// Gets the installed software.
        /// </summary>
        /// <value>The installed software.</value>
        public static CommonClasses InstalledSoftware => new CommonClasses("win32_Product");

        /// <summary>
        /// Gets the logged on user.
        /// </summary>
        /// <value>The logged on user.</value>
        public static CommonClasses LoggedOnUser => new CommonClasses("Win32_LoggedOnUser");

        /// <summary>
        /// Gets the memory.
        /// </summary>
        /// <value>The memory.</value>
        public static CommonClasses Memory => new CommonClasses("win32_Physicalmemory");

        /// <summary>
        /// Gets the monitor.
        /// </summary>
        /// <value>The monitor.</value>
        public static CommonClasses Monitor => new CommonClasses("win32_DesktopMonitor");

        /// <summary>
        /// Gets the network adapter configuration.
        /// </summary>
        /// <value>The network adapter configuration.</value>
        public static CommonClasses NetworkAdapterConfig => new CommonClasses("Win32_NetworkAdapterConfiguration");

        /// <summary>
        /// Gets the network drives.
        /// </summary>
        /// <value>The network drives.</value>
        public static CommonClasses NetworkDrives => new CommonClasses("Win32_NetworkConnection");

        /// <summary>
        /// Gets the operating system.
        /// </summary>
        /// <value>The operating system.</value>
        public static CommonClasses OperatingSystem => new CommonClasses("Win32_OperatingSystem");

        /// <summary>
        /// Gets the print drivers.
        /// </summary>
        /// <value>The print drivers.</value>
        public static CommonClasses PrintDrivers => new CommonClasses("win32_PrinterdRIVER");

        /// <summary>
        /// Gets the printers.
        /// </summary>
        /// <value>The printers.</value>
        public static CommonClasses Printers => new CommonClasses("Win32_Printer");

        /// <summary>
        /// Gets the running processes.
        /// </summary>
        /// <value>The running processes.</value>
        public static CommonClasses RunningProcesses => new CommonClasses("win32_process");

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>The services.</value>
        public static CommonClasses Services => new CommonClasses("WIN32_Service");

        /// <summary>
        /// Gets the startup commands.
        /// </summary>
        /// <value>The startup commands.</value>
        public static CommonClasses StartupCommands => new CommonClasses("win32_startupCommand");

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        private string Name { get; }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.String"/> to <see cref="CommonClasses"/>.
        /// </summary>
        /// <param name="wmiClass">The WMI class.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator CommonClasses(string wmiClass)
        {
            return new CommonClasses(wmiClass);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CommonClasses"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="wmiClass">The WMI class.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(CommonClasses wmiClass)
        {
            return wmiClass.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}