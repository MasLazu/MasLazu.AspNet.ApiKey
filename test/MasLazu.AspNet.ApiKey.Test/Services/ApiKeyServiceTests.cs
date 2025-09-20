using MasLazu.AspNet.ApiKey.Services;
using MasLazu.AspNet.ApiKey.Domain.Entities;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using Moq;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Application.Models;
using FluentValidation;
using Xunit;
using Mapster;

namespace MasLazu.AspNet.ApiKey.Test.Services;

public class ApiKeyServiceTests
{
    private readonly Mock<IRepository<Domain.Entities.ApiKey>> _mockRepository;
    private readonly Mock<IReadRepository<Domain.Entities.ApiKey>> _mockReadRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEntityPropertyMap<Domain.Entities.ApiKey>> _mockPropertyMap;
    private readonly Mock<IPaginationValidator<Domain.Entities.ApiKey>> _mockPaginationValidator;
    private readonly Mock<ICursorPaginationValidator<Domain.Entities.ApiKey>> _mockCursorPaginationValidator;
    private readonly ApiKeyService _service;

    public ApiKeyServiceTests()
    {
        _mockRepository = new Mock<IRepository<Domain.Entities.ApiKey>>();
        _mockReadRepository = new Mock<IReadRepository<Domain.Entities.ApiKey>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPropertyMap = new Mock<IEntityPropertyMap<Domain.Entities.ApiKey>>();
        _mockPaginationValidator = new Mock<IPaginationValidator<Domain.Entities.ApiKey>>();
        _mockCursorPaginationValidator = new Mock<ICursorPaginationValidator<Domain.Entities.ApiKey>>();

        _service = new ApiKeyService(
            _mockRepository.Object,
            _mockReadRepository.Object,
            _mockUnitOfWork.Object,
            _mockPropertyMap.Object,
            _mockPaginationValidator.Object,
            _mockCursorPaginationValidator.Object);
    }

    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        // Assert
        Assert.NotNull(_service);
        Assert.IsType<ApiKeyService>(_service);
    }

    [Fact]
    public void Constructor_InheritsFromCrudService()
    {
        // Assert
        Assert.IsAssignableFrom<CrudService<Domain.Entities.ApiKey, ApiKeyDto, CreateApiKeyRequest, UpdateApiKeyRequest>>(_service);
    }

    #region RevokeAsync Tests

    [Fact]
    public async Task RevokeAsync_WithValidApiKeyId_RevokesApiKey()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apiKey = new Domain.Entities.ApiKey
        {
            Id = apiKeyId,
            UserId = userId,
            Key = "test-key",
            RevokedDate = null
        };

        _mockRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiKey);
        _mockRepository.Setup(r => r.UpdateAsync(apiKey, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        await _service.RevokeAsync(userId, apiKeyId);

        // Assert
        Assert.NotNull(apiKey.RevokedDate);
        Assert.True(apiKey.RevokedDate > DateTime.UtcNow.AddSeconds(-1));
        _mockRepository.Verify(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(apiKey, It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RevokeAsync_WithNonExistentApiKeyId_ThrowsNotFoundException()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.ApiKey?)null);

        // Act & Assert
        NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.RevokeAsync(userId, apiKeyId));

        Assert.Equal("API key not found.", exception.Message);
        _mockRepository.Verify(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.ApiKey>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region RotateAsync Tests

    [Fact]
    public async Task RotateAsync_WithValidApiKeyId_RotatesKeyAndReturnsDto()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        string originalKey = "original-key";
        var apiKey = new Domain.Entities.ApiKey
        {
            Id = apiKeyId,
            UserId = userId,
            Key = originalKey,
            Name = "Test API Key",
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiKey);
        _mockRepository.Setup(r => r.UpdateAsync(apiKey, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        ApiKeyDto result = await _service.RotateAsync(userId, apiKeyId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApiKeyDto>(result);
        Assert.Equal(apiKeyId, result.Id);
        Assert.Equal(userId, result.UserId);
        Assert.NotEqual(originalKey, apiKey.Key); // Key should be changed
        Assert.NotEqual(originalKey, result.Key); // DTO should have new key
        _mockRepository.Verify(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(apiKey, It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RotateAsync_WithNonExistentApiKeyId_ThrowsNotFoundException()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.ApiKey?)null);

        // Act & Assert
        NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.RotateAsync(userId, apiKeyId));

        Assert.Equal("API key not found.", exception.Message);
        _mockRepository.Verify(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.ApiKey>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region RevokeAllForUserAsync Tests

    [Fact]
    public async Task RevokeAllForUserAsync_WithMultipleApiKeys_RevokesAllKeys()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apiKeys = new List<Domain.Entities.ApiKey>
        {
            new Domain.Entities.ApiKey { Id = Guid.NewGuid(), UserId = userId, Key = "key1", RevokedDate = null },
            new Domain.Entities.ApiKey { Id = Guid.NewGuid(), UserId = userId, Key = "key2", RevokedDate = null },
            new Domain.Entities.ApiKey { Id = Guid.NewGuid(), UserId = userId, Key = "key3", RevokedDate = null }
        };

        _mockReadRepository.Setup(r => r.FindAsync(x => x.UserId == userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiKeys);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Domain.Entities.ApiKey>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        await _service.RevokeAllForUserAsync(userId);

        // Assert
        foreach (Domain.Entities.ApiKey apiKey in apiKeys)
        {
            Assert.NotNull(apiKey.RevokedDate);
            Assert.True(apiKey.RevokedDate > DateTime.UtcNow.AddSeconds(-1));
        }
        _mockReadRepository.Verify(r => r.FindAsync(x => x.UserId == userId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.ApiKey>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RevokeAllForUserAsync_WithNoApiKeys_DoesNothing()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apiKeys = new List<Domain.Entities.ApiKey>();

        _mockReadRepository.Setup(r => r.FindAsync(x => x.UserId == userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Domain.Entities.ApiKey>)apiKeys);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        await _service.RevokeAllForUserAsync(userId);

        // Assert
        _mockReadRepository.Verify(r => r.FindAsync(x => x.UserId == userId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.ApiKey>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateLastUsedAsync Tests

    [Fact]
    public async Task UpdateLastUsedAsync_WithValidApiKeyId_UpdatesLastUsed()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apiKey = new Domain.Entities.ApiKey
        {
            Id = apiKeyId,
            UserId = userId,
            Key = "test-key",
            LastUsed = null
        };

        _mockRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiKey);
        _mockRepository.Setup(r => r.UpdateAsync(apiKey, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        await _service.UpdateLastUsedAsync(userId, apiKeyId);

        // Assert
        Assert.NotNull(apiKey.LastUsed);
        Assert.True(apiKey.LastUsed > DateTime.UtcNow.AddSeconds(-1));
        _mockRepository.Verify(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(apiKey, It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateLastUsedAsync_WithNonExistentApiKeyId_ThrowsNotFoundException()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.ApiKey?)null);

        // Act & Assert
        NotFoundException exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.UpdateLastUsedAsync(userId, apiKeyId));

        Assert.Equal("API key not found.", exception.Message);
        _mockRepository.Verify(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.ApiKey>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region GetByUserIdAsync Tests

    [Fact]
    public async Task GetByUserIdAsync_WithValidUserId_ReturnsApiKeyDtos()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apiKeys = new List<Domain.Entities.ApiKey>
        {
            new Domain.Entities.ApiKey
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Key = "key1",
                Name = "API Key 1",
                CreatedAt = DateTime.UtcNow
            },
            new Domain.Entities.ApiKey
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Key = "key2",
                Name = "API Key 2",
                CreatedAt = DateTime.UtcNow
            }
        };

        _mockReadRepository.Setup(r => r.FindAsync(x => x.UserId == userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Domain.Entities.ApiKey>)apiKeys);

        // Act
        IEnumerable<ApiKeyDto> result = await _service.GetByUserIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, dto => Assert.Equal(userId, dto.UserId));
        _mockReadRepository.Verify(r => r.FindAsync(x => x.UserId == userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByUserIdAsync_WithNoApiKeys_ReturnsEmptyCollection()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apiKeys = new List<Domain.Entities.ApiKey>();

        _mockReadRepository.Setup(r => r.FindAsync(x => x.UserId == userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiKeys);

        // Act
        IEnumerable<ApiKeyDto> result = await _service.GetByUserIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockReadRepository.Verify(r => r.FindAsync(x => x.UserId == userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region CRUD Operation Tests (Inherited from CrudService)

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesApiKey()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateApiKeyRequest(userId, "Test API Key", DateTime.UtcNow.AddDays(30));
        var expectedApiKey = new Domain.Entities.ApiKey
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Key = Guid.NewGuid().ToString("N"),
            Name = request.Name,
            ExpiresDate = request.ExpiresDate,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.ApiKey>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedApiKey);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedApiKey);

        // Act
        ApiKeyDto result = await _service.CreateAsync(userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApiKeyDto>(result);
        Assert.Equal(request.UserId, result.UserId);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.ExpiresDate, result.ExpiresDate);
        Assert.NotNull(result.Key);
        Assert.NotEmpty(result.Key);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsApiKey()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apiKey = new Domain.Entities.ApiKey
        {
            Id = apiKeyId,
            UserId = userId,
            Key = "test-key-123",
            Name = "Test API Key",
            ExpiresDate = DateTime.UtcNow.AddDays(30),
            LastUsed = DateTime.UtcNow.AddHours(-1),
            RevokedDate = null,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        _mockReadRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiKey);

        // Act
        ApiKeyDto result = await _service.GetByIdAsync(userId, apiKeyId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApiKeyDto>(result);
        Assert.Equal(apiKeyId, result.Id);
        Assert.Equal(userId, result.UserId);
        Assert.Equal("test-key-123", result.Key);
        Assert.Equal("Test API Key", result.Name);
        Assert.Equal(apiKey.ExpiresDate, result.ExpiresDate);
        Assert.Equal(apiKey.LastUsed, result.LastUsed);
        Assert.Equal(apiKey.RevokedDate, result.RevokedDate);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _mockReadRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.ApiKey?)null);

        // Act
        ApiKeyDto? result = await _service.GetByIdAsync(userId, apiKeyId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_UpdatesApiKey()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var request = new UpdateApiKeyRequest(apiKeyId, "Updated API Key", DateTime.UtcNow.AddDays(60), null);
        var existingApiKey = new Domain.Entities.ApiKey
        {
            Id = apiKeyId,
            UserId = userId,
            Key = "original-key",
            Name = "Original API Key",
            ExpiresDate = DateTime.UtcNow.AddDays(30),
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        _mockRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingApiKey);
        _mockRepository.Setup(r => r.UpdateAsync(existingApiKey, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        ApiKeyDto result = await _service.UpdateAsync(userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApiKeyDto>(result);
        Assert.Equal(request.Id, result.Id);
        Assert.Equal(request.Name, existingApiKey.Name);
        Assert.Equal(request.ExpiresDate, existingApiKey.ExpiresDate);
        Assert.Equal(request.RevokedDate, existingApiKey.RevokedDate);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentId_ThrowsException()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var request = new UpdateApiKeyRequest(apiKeyId, "Updated API Key", null, null);

        _mockRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.ApiKey?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(userId, request));
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesApiKey()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apiKey = new Domain.Entities.ApiKey
        {
            Id = apiKeyId,
            UserId = userId,
            Key = "test-key",
            Name = "Test API Key",
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiKey);
        _mockRepository.Setup(r => r.DeleteAsync(apiKey, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        await _service.DeleteAsync(userId, apiKeyId);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(apiKey, It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentId_ThrowsException()
    {
        // Arrange
        var apiKeyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(apiKeyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.ApiKey?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(userId, apiKeyId));
    }

    #endregion

    #region ValidateAsync Tests

    [Fact]
    public async Task ValidateAsync_WithValidKeyAndPermission_ReturnsTrue()
    {
        // Arrange
        var request = new ValidateApiKeyRequest("valid-key", Guid.NewGuid());
        var apiKey = new Domain.Entities.ApiKey
        {
            Id = Guid.NewGuid(),
            Key = "valid-key",
            RevokedDate = null,
            ExpiresDate = null,
            Scopes = new List<ApiKeyScope>
            {
                new ApiKeyScope { PermissionId = request.PermissionId ?? Guid.NewGuid() }
            }
        };

        _mockReadRepository.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Entities.ApiKey, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        bool result = await _service.ValidateAsync(request);

        // Assert
        Assert.True(result);
        _mockReadRepository.Verify(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Entities.ApiKey, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ValidateAsync_WithRevokedKey_ReturnsFalse()
    {
        // Arrange
        var request = new ValidateApiKeyRequest("revoked-key", Guid.NewGuid());

        _mockReadRepository.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Entities.ApiKey, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        bool result = await _service.ValidateAsync(request);

        // Assert
        Assert.False(result);
        _mockReadRepository.Verify(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Entities.ApiKey, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ValidateAsync_WithExpiredKey_ReturnsFalse()
    {
        // Arrange
        var request = new ValidateApiKeyRequest("expired-key", Guid.NewGuid());

        _mockReadRepository.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Entities.ApiKey, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        bool result = await _service.ValidateAsync(request);

        // Assert
        Assert.False(result);
        _mockReadRepository.Verify(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Entities.ApiKey, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ValidateAsync_WithInvalidPermission_ReturnsFalse()
    {
        // Arrange
        var request = new ValidateApiKeyRequest("valid-key", Guid.NewGuid());

        _mockReadRepository.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Entities.ApiKey, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        bool result = await _service.ValidateAsync(request);

        // Assert
        Assert.False(result);
        _mockReadRepository.Verify(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Entities.ApiKey, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ValidateAsync_WithNullPermissionId_ReturnsFalse()
    {
        // Arrange
        var request = new ValidateApiKeyRequest("valid-key", null);

        _mockReadRepository.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Entities.ApiKey, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        bool result = await _service.ValidateAsync(request);

        // Assert
        Assert.False(result);
        _mockReadRepository.Verify(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Domain.Entities.ApiKey, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
