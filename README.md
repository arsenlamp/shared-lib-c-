## Supported Use Cases

- [x] Shared services
- [x] Per-tenant services
- [x] Schema per-tenant (Data Isolation)
- [x] Database per-tenant (Data Isolation)
- [x] Shared database (Data Isolation)
- [x] Shared options
- [x] Per-tenant options
- [x] Shared Authentication and Authorization
- [x] Per-tenant Authentication and Authorization

## Quickstart

- Add dependency to [CodEaisy.TinySaas][nuget_link] from Nuget

```bash
dotnet add package CodEaisy.TinySaas.AspNetCore --version 1.0.0
```

- In `Startup.cs`, add the following inside the `ConfigureServices` method.

  ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        // register all global singleton services here, and also dependencies for your TenantStore and ResolutionStrategy if any

        // ...

        // OPTION 1
        services.AddMultitenancy<Tenant, TenantStore<Tenant>, TenantResolutionStrategy>();

        // OPTION 2
        // uses default `CodEaisy.TinySaas.Model.TinyTenant` as tenant model
        services.AddMultitenancy<TenantStore<TinyTenant>, TenantResolutionStrategy>();

        // ...

        // services.AddControllers();
    }
  ```

  Then, add the following in the `Configure` method

  ```csharp
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
      if (env.IsDevelopment())
      {
          app.UseDeveloperExceptionPage();
      }

      // enable multitenant support, with missing tenant handler and tenant container

      // OPTION 1
      // missing tenant handler has a dependency that can be provided immediately
      app.UseMultitenancy<Tenant, MissingTenantHandler, MissingTenantOptions>(missingTenantOptions);

      // OPTION 2
      // missing tenant handler does not have a dependency or dependency is already registered in services
      app.UseMultitenancy<Tenant, MissingTenantHandler>();

      // OPTION 3
      // Use `SimpleTenant` as tenant model, and missing tenant handler does not have a dependency or dependency is already registered in services
      app.UseMultitenancy<TMissingTenantHandler>()

      // ...
  }
  ```

- In `Program.cs`, add the following in the `CreateHostBuilder` method.

  ```csharp
  public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            // OPTION 1: add multitenant support via TenantStartup class
            .ConfigureMultitenancy<TenantStartup, Tenant>();
            // OPTION 2: add multitenant support via static method
            .ConfigureMultitenancy<Tenant>(ClassName.StaticMethodName);
  ```

  **NOTE**:
  - `Tenant` must implement `CodEaisy.TinySaas.Interface`  `ITenant`.
  - `TenantStore` must implement `CodEaisy.TinySaas.Interface.ITenantStore`.
  - `TenantResolutionStrategy` must implement `CodEaisy.TinySaas.Interface.ITenantResolutionStrategy` respectively.
  - `TenantStartup` must implement `IMultitenantStartup`
  - `ClassName.StaticMethodName` must be of type `System.Action<TTenant, Autofac.ContainerBuilder>` where `TTenant` implements `ITenant`

## Benchmarks

Here, we show the performance report of an application singleton in a default ASP.NET application and an application singleton in a TinySaas ASP.NET application.

``` ini
BenchmarkDotNet=v0.13.1, OS=macOS Big Sur 11.5.2 (20G95) [Darwin 20.6.0]
Intel Core i9-9880H CPU 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK=5.0.400
  [Host]        : .NET Core 3.1.5 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.27001), X64 RyuJIT
  .NET 5.0      : .NET 5.0.9 (5.0.921.35908), X64 RyuJIT
  .NET Core 3.1 : .NET Core 3.1.5 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.27001), X64 RyuJIT
```

### App Singleton in Default ASP.NET vs App Singleton in TinySaas

|  Method |           Job |       Runtime | Instance |     Mean |    Error |   StdDev | Ratio | RatioSD |
|-------- |-------------- |-------------- |--------- |---------:|---------:|---------:|------:|--------:|
| HttpGet |      .NET 5.0 |      .NET 5.0 |  Default | 43.95 μs | 0.791 μs | 1.598 μs |  0.90 |    0.04 |
| HttpGet | .NET Core 3.1 | .NET Core 3.1 |  Default | 50.06 μs | 0.979 μs | 1.088 μs |  1.00 |    0.00 |
|         |               |               |          |          |          |          |       |         |
| HttpGet |      .NET 5.0 |      .NET 5.0 | TinySaas | 64.24 μs | 0.291 μs | 0.272 μs |  0.92 |    0.03 |
| HttpGet | .NET Core 3.1 | .NET Core 3.1 | TinySaas | 69.96 μs | 1.389 μs | 1.993 μs |  1.00 |    0.00 |

### Tenant Singleton in TinySaas

|  Method |           Job |       Runtime | Instance |     Mean |    Error |   StdDev | Ratio | RatioSD |
|-------- |-------------- |-------------- |--------- |---------:|---------:|---------:|------:|--------:|
| HttpGet |      .NET 5.0 |      .NET 5.0 | TinySaas | 64.89 μs | 0.237 μs | 0.185 μs |  0.88 |    0.02 |
| HttpGet | .NET Core 3.1 | .NET Core 3.1 | TinySaas | 74.95 μs | 1.437 μs | 1.869 μs |  1.00 |    0.00 |

## Requirements

ASP.NET Core 3.1+

## Changelog

[Learn about the latest improvements][changelog].

## Credits

[Gunnar Peipman](https://gunnarpeipman.com/) and [Michael McKenna](https://michael-mckenna.com/) for their awesome works on Saas in ASP.NET Core.

## Want to help ?

Want to file a bug, contribute some code, or improve documentation? Excellent! Read up on our
guidelines for [contributing][contributing] and then check out one of our issues in the [hotlist: community-help](https://github.com/codeaisy/tinysaas/labels/hotlist%3A%20community-help).

[contributing]: https://github.com/codeaisy/tinysaas/blob/master/CONTRIBUTING.md
[changelog]: https://github.com/codeaisy/tinysaas/blob/master/CHANGELOG.md
[nuget_link]: https://www.nuget.org/packages/CodEaisy.TinySaas
