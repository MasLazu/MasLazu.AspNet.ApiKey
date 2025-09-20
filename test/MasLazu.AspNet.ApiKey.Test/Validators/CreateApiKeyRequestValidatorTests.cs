using MasLazu.AspNet.ApiKey.Validators;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using FluentValidation.TestHelper;
using Xunit;

namespace MasLazu.AspNet.ApiKey.Test.Validators;

public class CreateApiKeyRequestValidatorTests
{
    private readonly CreateApiKeyRequestValidator _validator;

    public CreateApiKeyRequestValidatorTests()
    {
        _validator = new CreateApiKeyRequestValidator();
    }

    [Fact]
    public void CreateApiKeyRequestValidator_CanBeInstantiated()
    {
        // Assert
        Assert.NotNull(_validator);
        Assert.IsType<CreateApiKeyRequestValidator>(_validator);
    }

    [Fact]
    public void UserId_Empty_ShouldHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(Guid.Empty, null, null);

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void UserId_Valid_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(Guid.NewGuid(), null, null);

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Name_TooLong_ShouldHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.NewGuid(),
            new string('a', 101), // 101 characters, exceeds max length of 100
            null
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Name_ValidLength_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.NewGuid(),
            "Valid Name",
            null
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Name_Null_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.NewGuid(),
            null,
            null
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Name_EmptyString_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.NewGuid(),
            string.Empty,
            null
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Name_Exactly100Characters_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.NewGuid(),
            new string('a', 100), // Exactly 100 characters
            null
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void ExpiresDate_InPast_ShouldHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.NewGuid(),
            null,
            DateTime.UtcNow.AddDays(-1)
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.ExpiresDate)
              .WithErrorMessage("Expiration date must be in the future.");
    }

    [Fact]
    public void ExpiresDate_InFuture_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.NewGuid(),
            null,
            DateTime.UtcNow.AddDays(1)
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.ExpiresDate);
    }

    [Fact]
    public void ExpiresDate_Null_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.NewGuid(),
            null,
            null
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.ExpiresDate);
    }

    [Fact]
    public void ExpiresDate_OneSecondInFuture_ShouldNotHaveValidationError()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.NewGuid(),
            null,
            DateTime.UtcNow.AddSeconds(1)
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(x => x.ExpiresDate);
    }

    [Fact]
    public void AllFields_Valid_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.NewGuid(),
            "Valid API Key Name",
            DateTime.UtcNow.AddDays(30)
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void AllFields_Invalid_ShouldHaveMultipleValidationErrors()
    {
        // Arrange
        var request = new CreateApiKeyRequest(
            Guid.Empty,
            new string('a', 101), // Too long
            DateTime.UtcNow.AddDays(-1) // In past
        );

        // Act & Assert
        TestValidationResult<CreateApiKeyRequest> result = _validator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.ExpiresDate);
    }
}
