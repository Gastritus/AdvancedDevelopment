# AdvancedDevelopment

## Introduction
This guide provides step-by-step instructions to deploy the AdvancedDevelopment application.

## Prerequisites
- **Operating System**: Windows, macOS, or Linux
- **Software**: Docker, .NET SDK, Visual Studio (optional)
- **Network**: Stable internet connection

## Cloning the Repository
### Open Terminal (or Command Prompt)

```
git clone https://github.com/yourusername/AdvancedDevelopment.git
cd AdvancedDevelopment
```

## Setting Up Docker
### Ensure Docker is installed and running
- Download and install Docker from [Docker's official website](https://www.docker.com/products/docker-desktop).

### Build the Docker image

```
docker build -t advanceddevelopment .
```

### Run the Docker container

```
docker run -p 8080:80 advanceddevelopment
```

# Alternatively you can use the start_container.bat

## Running the Application Locally
### Install .NET SDK
- Download and install the .NET SDK from [Microsoft's official website](https://dotnet.microsoft.com/download).

### Navigate to the project directory

```
cd AdvancedDevelopment
```

### Restore NuGet packages

```
dotnet restore
```

### Run the application

```
dotnet run
```

### Access the application
- Open a browser and navigate to `http://localhost:5248`.


