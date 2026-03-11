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
using System.Management;
using System.Runtime.Versioning;

namespace Environs.HelperClasses
{
    /// <summary>
    /// Printer manager
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class PrinterManager
    {
        /// <summary>
        /// Adds the printer.
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        /// <returns>True if the printer is added, false otherwise</returns>
        public static bool AddPrinter(string printerName)
        {
            try
            {
                if (IsPrinterInstalled(printerName))
                    return true;
                var TempManagementScope = new ManagementScope(ManagementPath.DefaultPath);
                TempManagementScope.Connect();

                using var PrinterClass = new ManagementClass(new ManagementPath("Win32_Printer"), null);
                using ManagementBaseObject InputParameters = PrinterClass.GetMethodParameters("AddPrinterConnection");
                InputParameters.SetPropertyValue("Name", printerName);
                _ = PrinterClass.InvokeMethod("AddPrinterConnection", InputParameters, null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the printer.
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        /// <returns>True if the printer is deleted, false otherwise</returns>
        public static bool DeletePrinter(string printerName)
        {
            if (!IsPrinterInstalled(printerName))
                return false;
            var TempEnvironment = new Environment();
            ManagementObjectCollection Results = TempEnvironment.Execute("SELECT * FROM Win32_Printer WHERE Name = '" + printerName.Replace("\\", "\\\\") + "'");
            if (Results.Count != 0)
            {
                foreach (ManagementObject Item in Results.Cast<ManagementObject>())
                {
                    Item.Delete();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is printer installed] [the specified printer name].
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        /// <returns>
        /// <c>true</c> if [is printer installed] [the specified printer name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPrinterInstalled(string printerName)
        {
            var TempEnvironment = new Environment();
            return TempEnvironment.Execute("SELECT * FROM Win32_Printer WHERE Name = '" + printerName.Replace("\\", "\\\\") + "'").Count > 0;
        }

        /// <summary>
        /// Renames the printer.
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        /// <param name="newName">The new name.</param>
        public static void RenamePrinter(string printerName, string newName)
        {
            if (!IsPrinterInstalled(printerName))
                return;
            var TempEnvironment = new Environment();
            ManagementObjectCollection Results = TempEnvironment.Execute("SELECT * FROM Win32_Printer WHERE Name = '" + printerName.Replace("\\", "\\\\") + "'");
            if (Results.Count != 0)
            {
                foreach (ManagementObject Item in Results.Cast<ManagementObject>())
                {
                    _ = Item.InvokeMethod("RenamePrinter", [newName]);
                }
            }
        }

        /// <summary>
        /// Sets the default printer.
        /// </summary>
        /// <param name="printerName">Name of the printer.</param>
        public static void SetDefaultPrinter(string printerName)
        {
            if (!IsPrinterInstalled(printerName))
                return;
            var TempEnvironment = new Environment();
            ManagementObjectCollection Results = TempEnvironment.Execute("SELECT * FROM Win32_Printer WHERE Name = '" + printerName.Replace("\\", "\\\\") + "'");
            if (Results.Count != 0)
            {
                foreach (ManagementObject Item in Results.Cast<ManagementObject>())
                {
                    _ = Item.InvokeMethod("SetDefaultPrinter", [printerName]);
                }
            }
        }
    }
}