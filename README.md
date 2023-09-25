# My DotNet API

# Getting Started
```shell
dotnet clean
dotnet nuget locals all --clear
dotnet restore
dotnet build --no-restore
#ENVIRONMENT_NAME="Development";
ENVIRONMENT_NAME="UAT";
dotnet watch run --environment=$ENVIRONMENT_NAME --project=MyDotNetAPI/MyDotNetAPI.csproj
```

### Testing a Compiled App
Testing out the DLL / compiled app
```shell
dotnet clean
dotnet nuget locals all --clear
dotnet restore
dotnet build --no-restore
dotnet publish
DLL_PATH="bin/Debug/net7.0/MyDotNetAPI.dll"
ENVIRONMENT_NAME="UAT";
dotnet $DLL_PATH -- --no-build --environment=$ENVIRONMENT_NAME;
```
---

### Environment

* Using `launchSettings.json`
    * Navigate to `MyDotNetAPI/Properties/launchSettings.json`
    * There will be launch profiles for each environment
        * DEV
        * UAT
        * PROD
* Using CLI
    *

---

### Scaffolding
```shell
dotnet new webapi --framework net7.0 --name MyDotNetAPI --output MyDotNetAPI
git init
dotnet new gitignore
dotnet new nunit
```

---

### Rate Limiting
Requires .NET 7

Resources:
* https://www.youtube.com/watch?v=bOfOo3Zsfx0&t=1396s
* https://www.infoworld.com/article/3696320/how-to-use-the-rate-limiting-algorithms-in-aspnet-core.html

Program.cs

```c#
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    rateLimiterOptions.AddFixedWindowLimiter("fixed-window", fixedWindowOptions =>
    {
        fixedWindowOptions.Window = TimeSpan.FromSeconds(5);
        fixedWindowOptions.PermitLimit = 5;
        fixedWindowOptions.QueueLimit = 10;
        fixedWindowOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

...

app.UseRateLimiter();
```

In your controller
```c#
using Microsoft.AspNetCore.RateLimiting;
...
[EnableRateLimiting("fixed-window")]
```

----

### Security Headers
Because there's a few headers we need to add, we'll create a middleware implementation

```shell
mkdir -p Middleware
touch Middleware/SecurityHeaders.cs
```

see `Middleware/SecurityHeaders.cs` for contents

In `Program.cs`

```c#
using MyDotNetAPI.Middleware;
...

var builder = WebApplication.CreateBuilder(args);
// add the below
builder.WebHost.UseKestrel(option => option.AddServerHeader = false);

//

builder.Services.AddHsts(options =>
{
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

//

// before app.MapControllers();
app.UseSecurityHeaders();
```

Resources:
* https://dotnetthoughts.net/implementing-content-security-policy-in-aspnetcore/
* https://blog.elmah.io/the-asp-net-core-security-headers-guide/
* https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-7.0

---

### GitHub Actions
```shell
mkdir -p .github/workflows
touch .github/workflows/build_and_test.yml
```
----

### Migration
Create Migration
```shell
dotnet tool install --global dotnet-ef
dotnet ef migrations add Initial -o Migrations --context MyDbContextSqLite -v
```

Update Migration
```shell
dotnet ef database update --context MyDbContextSqLite -v
```

---

### Terms
| Terms      | Short Description                                                                          |
|------------|--------------------------------------------------------------------------------------------|
| Migrations | Basically DB as Code. Allows you to use your code as the source of truth for the DB Schema |


---

### Adding Unit Tests

```shell
cd ~/code
mkdir -p unit-testing-using-nunit
git init
cd unit-testing-using-nunit
dotnet new sln
mkdir -p PrimeService 
cd PrimeService
dotnet new classlib
mv Class1.cs PrimeService.cs
cd ../
dotnet sln add PrimeService/PrimeService.csproj
mkdir -p PrimeService.Tests
cd PrimeService.Tests
dotnet new nunit
dotnet add reference ../PrimeService/PrimeService.csproj
cd ../
mv PrimeService.Tests/UnitTest1.cs PrimeService.Tests/PrimeService_IsPrimeShould.cs
echo "using NUnit.Framework;
using Prime.Services;

namespace Prime.UnitTests.Services;

[TestFixture]
public class PrimeService_IsPrimeShould
{
    private PrimeService _primeService;

    [SetUp]
    public void SetUp()
    {
        _primeService = new PrimeService();
    }

    [Test]
    public void IsPrime_InputIs1_ReturnFalse()
    {
        var result = _primeService.IsPrime(1);

        Assert.That(false, Is.True, "1 should not be prime");
    }
}" > PrimeService.Tests/PrimeService_IsPrimeShould.cs
dotnet sln add PrimeService.Tests/PrimeService.Tests.csproj
dotnet test
```

----

### References:
* https://docs.github.com/en/actions/automating-builds-and-tests/building-and-testing-net
* [NUnit Test](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-nunit#creating-the-source-project)
* [Project Structure](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio-mac#add-a-model-class)
* [DBContext](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
* [Custom Logging for Endpoints](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/controllers-and-routing/understanding-action-filters-cs)
* [Output Caching](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/controllers-and-routing/improving-performance-with-output-caching-cs)
* [Getting Started with Entity Framework Core](https://www.youtube.com/watch?v=JzfWpiowtqI)
* [Migrations Explained](https://www.youtube.com/watch?v=fl6r-9rQjns)
* [Seed DB](https://www.youtube.com/watch?v=z-Hll4Xddjs)
* [Sqlite & Entity Framework Core](https://www.youtube.com/watch?v=z-Hll4Xddjs)
* [Registering Services](https://www.youtube.com/watch?v=sSq3GtriFuM)