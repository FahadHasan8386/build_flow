using BuildFlow.Infrastructure.Security;
using Xunit;

namespace BuildFlow.Tests;

public class SecurityTests
{
    [Fact]
    public void PasswordHasher_VerifiesMatchingPassword()
    {
        var hasher = new PasswordHasher();
        var hash = hasher.HashPassword("Test@1234");

        Assert.True(hasher.VerifyPassword("Test@1234", hash));
    }
}
