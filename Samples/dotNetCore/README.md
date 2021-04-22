#Introduction 
The repository contains a client sample application for the following APIs using QwickPIN service.
    1. Generate PINOffset
    2. Verify PINOffset
    

# Clone the repository
    1. Navigate to the main page of the  **repository**
    2. Under the **repository** name, click  **Clone**
    3. Use any Git Client (e.g.: GitBash, Git Hub for windows, Source tree ) to  **clone**  the  **repository**  using HTTPS

Note: reference [Cloning a Github Repository](https://help.github.com/en/articles/cloning-a-repository)


# Getting Started

    1.  Install .net core 3.1 LTS

        - Client app requires dotnet core 3.1 LTS


    2.  Software dependencies (following NUGET packages are automatically installed when we open and run the project), please recheck and add the references from NUGET

		- Microsoft.Extensions.Configuration.Json
		- Microsoft.Extensions.DependencyInjection
		- Newtonsoft.Json        
        - Microsoft.Extensions.Configuration.Binder
        - System.ServiceModel.Primitives


###Latest releases
- Initial release with all commits and changes as on Mar 2021


# Build and Test

 From the cmd, navigate to the cloned folder and go to "QwickPIN_DotNetCore/src"
    
 Run the following commands
    
 ```dotnet clean QPINSampleCode.sln```

 ```dotnet build QPINSampleCode.sln```


 Navigate from command prompt to QPINSampleCode.App folder and run below command

 ```dotnet run --project QPINSampleCode.App.csproj```

 This should open the application running in console.