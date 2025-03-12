# AdvancedDevelopment

## Introduction
This guide provides step-by-step instructions to load the AdvancedDevelopment web application and run unit tests.

## Prerequisites
- **Operating System**: Windows, macOS, or Linux
- **Software**: .NET SDK, Visual Studio (optional)
- **Network**: Stable internet connection

## Cloning the Repository
### Open Terminal (or Command Prompt)
`git clone https://github.com/yourusername/AdvancedDevelopment.git cd AdvancedDevelopment`

## Running the Application Locally

### Install .NET SDK
- Download and install the .NET SDK from [Microsoft's official website](https://dotnet.microsoft.com/download).

### Navigate to the project directory
`cd AdvancedDevelopment`

### Restore NuGet packages
`dotnet restore`

### Run the application
`dotnet run`

### Access the application
- Open a browser and navigate to `http://localhost:5248`.

## Running Unit Tests

### Run the unit tests in `AdvancedDevelopment.tests`

- Open a terminal or use Visual Studio.
- Navigate to the `AdvancedDevelopment.tests` directory if you're using terminal:
`cd AdvancedDevelopment.tests`

- Run the tests with the following command:
`dotnet test`


- Alternatively, you can run the tests directly within Visual Studio by opening the `AdvancedDevelopment.tests` project and using the **Test Explorer** to run all tests.








