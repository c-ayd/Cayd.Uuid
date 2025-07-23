## About
Cayd.Uuid is a library containing static classes to generate `System.Guid` types following all versions of UUID defined in RFC 9562.

## Quick Start
After installing the package, you can generate `System.Guid` by using the static `Uuid` class in the library. This class includes all versions of UUID.

A few examples of how to generate UUIDs:
```csharp
using Cayd.Uuid;

Guid uuidV1 = Uuid.V1.Generate();
// UUID V2 is outside of RFC 9562's scope.
Guid uuidV3 = Uuid.V3.Generate(Uuid.V3.DnsNamespaceId, "example.com");
Guid uuidV4 = Uuid.V4.Generate();
Guid uuidV5 = Uuid.V5.Generate(Uuid.V5.UrlNamespaceId, "https://www.example.com");
Guid uuidV6 = Uuid.V6.Generate();
Guid uuidV7 = Uuid.V7.Generate();
Guid uuidV8 = Uuid.V8.Generate(myCustomData);
```

## Which Version To Use?
It is recommended to check RFC 9562 to understand how each version generates UUIDs. For reference, `V4` is the same as the default `Guid.NewGuid`. Starting from .Net 9, `System.Guid` introduces `Guid.CreateVersion7`, which is the same as `V7`.

## Extras
Depending on your need, you can adjust some parameters in some versions while generating UUIDs.
- The `V1` and `V6` classes have their own static `UseRandomNodeId` boolean variables to decide if they should use real MAC addresses of a current machine for the node bits or should use random bits. The default is set to `false`.
  - The default behavior of these classes is to try to retrieve a MAC address once and use it for all UUID generations. If no MAC address is found or the `UseRandomNodeId` is set to `true`, they generate a random node ID. If you want to update the node bits in generated UUIDs, you can call the `RefreshNodeId` in these clases to refresh their node IDs.
- The `Generate` methods of the `V1` and `V6` classes have `useLock` boolean parameter to decide if they should use `lock` to ensure uniquness of generated UUIDs in multithreading siutations. The default value is `false`.
- The `Generate` method of the `V7` have `offset` parameter to adjust time bits in generated UUIDs. The default value is `DateTimeOffset.UtcNow`.
