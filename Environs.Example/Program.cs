namespace Environs.Example
{
    /// <summary>
    /// Example program to demonstrate the use of the Environment class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            // Example of using the Environment class to query WMI for printers on the local machine
            // using impersonation
            var ExampleObject = new Environment(options: new AuthenticationOptions { Impersonate = true });

            // Call the Execute method to get the results of the query
            IEnumerable<dynamic> Results = ExampleObject.Execute(CommonClasses.Printers, "root\\cimv2");

            // Iterate through the results and print them to the console
            foreach (dynamic Result in Results)
            {
                Console.WriteLine(Result.Name);
            }
        }
    }
}