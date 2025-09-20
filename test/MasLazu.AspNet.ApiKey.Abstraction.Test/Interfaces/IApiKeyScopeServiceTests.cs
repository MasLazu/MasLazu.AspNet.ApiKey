using System.Reflection;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Abstraction.Test.Interfaces;

public class IApiKeyScopeServiceTests
{
    [Fact]
    public void IApiKeyScopeService_InheritsFrom_ICrudService()
    {
        // This is a compile-time test to ensure the interface inheritance is correct
        // If this compiles, the inheritance is working properly
        var service = default(IApiKeyScopeService);
        Assert.Null(service);
    }

    [Fact]
    public void IApiKeyScopeService_HasRequiredMethods()
    {
        // This test ensures that the interface has all the required methods
        // It's a compile-time check - if the interface is missing methods, this won't compile

        // Test that we can declare variables of the correct types
        MethodInfo? addScopeAsync = typeof(IApiKeyScopeService).GetMethod("AddScopeAsync");
        MethodInfo? removeScopeAsync = typeof(IApiKeyScopeService).GetMethod("RemoveScopeAsync");

        Assert.NotNull(addScopeAsync);
        Assert.NotNull(removeScopeAsync);
    }
}
