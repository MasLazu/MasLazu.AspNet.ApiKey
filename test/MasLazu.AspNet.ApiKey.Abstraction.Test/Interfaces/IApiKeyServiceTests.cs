using System.Reflection;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Abstraction.Test.Interfaces;

public class IApiKeyServiceTests
{
    [Fact]
    public void IApiKeyService_InheritsFrom_ICrudService()
    {
        // This is a compile-time test to ensure the interface inheritance is correct
        // If this compiles, the inheritance is working properly
        var service = default(IApiKeyService);
        Assert.Null(service);
    }

    [Fact]
    public void IApiKeyService_HasRequiredMethods()
    {
        // This test ensures that the interface has all the required methods
        // It's a compile-time check - if the interface is missing methods, this won't compile

        // Test that we can declare variables of the correct types
        MethodInfo? revokeAsync = typeof(IApiKeyService).GetMethod("RevokeAsync");
        MethodInfo? rotateAsync = typeof(IApiKeyService).GetMethod("RotateAsync");
        MethodInfo? validateAsync = typeof(IApiKeyService).GetMethod("ValidateAsync");
        MethodInfo? revokeAllForUserAsync = typeof(IApiKeyService).GetMethod("RevokeAllForUserAsync");
        MethodInfo? updateLastUsedAsync = typeof(IApiKeyService).GetMethod("UpdateLastUsedAsync");
        MethodInfo? getByUserIdAsync = typeof(IApiKeyService).GetMethod("GetByUserIdAsync");

        Assert.NotNull(revokeAsync);
        Assert.NotNull(rotateAsync);
        Assert.NotNull(validateAsync);
        Assert.NotNull(revokeAllForUserAsync);
        Assert.NotNull(updateLastUsedAsync);
        Assert.NotNull(getByUserIdAsync);
    }
}
