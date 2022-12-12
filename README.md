# Etch.OrchardCore.OutputCache

Orchard Core module that provides caching using [Output Cache](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/overview?view=aspnetcore-7.0#output-caching).

## Build Status

[![NuGet](https://img.shields.io/nuget/v/Etch.OrchardCore.OutputCache.svg)](https://www.nuget.org/packages/Etch.OrchardCore.OutputCache)

## Orchard Core Reference

This module is referencing a stable build of Orchard Core ([`1.4.0`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.4.0)).

## Installing

This module is available on [NuGet](https://www.nuget.org/packages/Etch.OrchardCore.OutputCache). Add a reference to your Orchard Core web project via the NuGet package manager. Search for "Etch.OrchardCore.OutputCache", ensuring include prereleases is checked.

Alternatively you can [download the source](https://github.com/etchuk/Etch.OrchardCore.OutputCache/archive/master.zip) or clone the repository to your local machine. Add the project to your solution that contains an Orchard Core project and add a reference to Etch.OrchardCore.OutputCache.

## Usage

First step is to enable "Output Caching" within the features section of the admin dashboard. Once enabled, any unauthorised requests to the tenant will cache the page using a memory store. To verify output cache is working, make multiple requests to a page and check the response header to see if an "age" is present.

**Known issue is that output caching will not work when the Forms module is enabled.**

### Configuration

By default entries added to Output Cache will expire after 10 minutes. This can be changed via the admin area by selecting "Configuration" then "Output Cache" in the admin menu. Within the settings view the expiration length can be configured and will cause the tenant to be reoloaded once the settings have been saved. From the settings the tag associated to entries can be managed as well as specifying which query strings should be a variation on the cache entry.

### Redis

When Orchard Core is running on a distributed environment it's recommended that the cache store is using an external source instead of using the default memory store. Orchard Core provides various [Redis integrations](https://docs.orchardcore.net/en/latest/docs/reference/modules/Redis/) out the box that are ideal for a distributed cache storage. To change the output cache store to use Redis, enable the "Redis Output Caching" feature. 

**Redis must be configured otherwise the tenant will become inaccessible.**

## Packaging

When the module is compiled (using `dotnet build`) it's configured to generate a `.nupkg` file (this can be found in `\bin\Debug\` or `\bin\Release`).

## Notes

This module was created using `v1.3.0` of [Etch.OrchardCore.ModuleBoilerplate](https://github.com/EtchUK/Etch.OrchardCore.ModuleBoilerplate) template.
