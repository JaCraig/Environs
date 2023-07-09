# Environs

Environs is a small C# library that provides a simplified interface for working with Windows Management Instrumentation (WMI), Lightweight Directory Access Protocol (LDAP), and related functionality. It aims to make it easier to perform common operations and tasks in these areas without the need for extensive knowledge of the underlying protocols and APIs.

## Features

- **WMI:** Simplifies querying and managing WMI classes, objects, and properties.
- **LDAP:** Provides an intuitive interface for interacting with LDAP directories, searching for entries, and performing CRUD operations.
- **System Helper Classes** Provides a simple to use class to manage printers, discovering IP addresses, and application discovery.

## Installation

The library can be installed via NuGet Package Manager or by manually including the DLL in your project.

### NuGet Package Manager

1. Open the Console.
2. Run the following command:

```shell
dotnet add package Environs
```

## Usage

Below are some examples of how to use the Environs library in your C# code:

```csharp
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
```


## Contributing

Contributions are welcome! If you find a bug, have a feature request, or want to contribute code, please open an issue or submit a pull request. Make sure to follow the guidelines outlined in the [CONTRIBUTING](https://github.com/JaCraig/Environs/blob/master/CONTRIBUTING.md) file.

## License

This library is licensed under the [Apache 2 License](https://github.com/JaCraig/Environs/blob/master/LICENSE.md).