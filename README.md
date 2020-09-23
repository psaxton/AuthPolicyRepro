AuthPolicyRepro
===============

This project demonstrates a difference in how Authorization Policies are handled in AspNetCore 2.1. The difference is
confirmed to exist in AspNetCore3.1 in the dotnet3.1 branch of this repository. When an authentication scheme is
specifically required via the `.AddAuthenticationSchemes()` method the context principal which may have been modified
in a previous step of the AAA pipeline is overwritten. The behaviour is different when the
`.AddAuthenticationSchemes()` method is not called.

Source/Startup.cs
-----------------
An authentication scheme is set up which replacess the context Principal with an instance of a custom principal type.

```csharp
    context.Principal = new CustomPrincipal(context.Principal);
```

Two authorization policies are created. `ExplicitSchemePolicy` requires the `CustomPrincipalScheme` be used.

```csharp
    policyBuilder
        .AddAuthenticationSchemes(CustomPrincipalScheme)
    // ...
```

`ImplicitSchemePolicy` relies on the `defaultScheme` set when the authentication is added to the service.

```csharp
    services
        .AddAuthenticaiton(CustomPrincipalScheme)
    // ...
```

Source/Controllers/ValuesController.cs
--------------------------------------
Two actions are created. `ExplicitGet` is decorated to require the `ExplicitSchemePolicy` authorization policy.
`ImplicitGet` is decorated to require the `ImplicitSchemePolicy` authorization policy. Each action should return
the value `"true"` indicating that the `User` claims principal has been set to the `CustomPrincipal` in the
authentication scheme `OnTokenValidated` event.

### ExplicitGet
Returns HTTP status code 401. The policy assertion (`.RequireAssertion(context => context.User is CustomPrincipal)`)
fails causing the authorization to fail. Debugging shows that the `OnTokenValidated` event executes and successfully
assigns the `Principal` value prior to this assertion.

Commenting out the assertion from Startup.cs will allow the authorization to succeed. The `User` property of the
controller is an instance of `ClaimsPrincipal` but not `CustomPrincipal`. The action then returns `"false"`.

### ImplicitGet
Works as expected. The policy assertion that `context.User is CustomPrincipal` succeeds and the controller `User`
property is an instance of `CustomPrincipal`.

Tests/PolicyTests.cs
--------------------
A `WebApplicationFactory` and valid bearer token are created in the `Setup()` method. Both controller actions are
tested and the expected results are asserted. The `TestExplicitSchemePolicy()` method fails. It is expected to succeed.
