using MasLazu.AspNet.ApiKey.Services;
using MasLazu.AspNet.ApiKey.Domain.Entities;
using MasLazu.AspNet.ApiKey.Abstraction.Models;
using Moq;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;
using MasLazu.AspNet.Framework.Application.Exceptions;
using FluentValidation;
using Xunit;
using Mapster;

namespace MasLazu.AspNet.ApiKey.Test.Services;

public class ApiKeyScopeServiceTests
{
    private readonly Mock<IRepository<ApiKeyScope>> _mockRepository;
    private readonly Mock<IReadRepository<ApiKeyScope>> _mockReadRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEntityPropertyMap<ApiKeyScope>> _mockPropertyMap;
    private readonly Mock<IPaginationValidator<ApiKeyScope>> _mockPaginationValidator;
    private readonly Mock<ICursorPaginationValidator<ApiKeyScope>> _mockCursorPaginationValidator;
    private readonly ApiKeyScopeService _service;

    public ApiKeyScopeServiceTests()
    {
        _mockRepository = new Mock<IRepository<ApiKeyScope>>();
        _mockReadRepository = new Mock<IReadRepository<ApiKeyScope>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPropertyMap = new Mock<IEntityPropertyMap<ApiKeyScope>>();
        _mockPaginationValidator = new Mock<IPaginationValidator<ApiKeyScope>>();
        _mockCursorPaginationValidator = new Mock<ICursorPaginationValidator<ApiKeyScope>>();

        _service = new ApiKeyScopeService(
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
        Assert.IsType<ApiKeyScopeService>(_service);
    }

    [Fact]
    public void Constructor_InheritsFromCrudService()
    {
        // Assert
        Assert.IsAssignableFrom<CrudService<ApiKeyScope, ApiKeyScopeDto, CreateApiKeyScopeRequest, UpdateApiKeyScopeRequest>>(_service);
    }

    #region AddScopeAsync Tests

    [Fact]
    public async Task AddScopeAsync_WithValidRequest_AddsScopeAndReturnsDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();
        var request = new ApiKeyScopeRequest(apiKeyId, permissionId);
        var expectedScope = new ApiKeyScope
        {
            Id = Guid.NewGuid(),
            ApiKeyId = apiKeyId,
            PermissionId = permissionId,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<ApiKeyScope>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedScope);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Mock the repository to return the expected scope when GetByIdAsync is called
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedScope);

        // Act
        ApiKeyScopeDto result = await _service.AddScopeAsync(userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApiKeyScopeDto>(result);
        Assert.Equal(apiKeyId, result.ApiKeyId);
        Assert.Equal(permissionId, result.PermissionId);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<ApiKeyScope>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddScopeAsync_WithValidRequest_CreatesScopeWithCorrectProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();
        var request = new ApiKeyScopeRequest(apiKeyId, permissionId);
        ApiKeyScope capturedScope = null!;

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<ApiKeyScope>(), It.IsAny<CancellationToken>()))
            .Callback<ApiKeyScope, CancellationToken>((scope, _) => capturedScope = scope)
            .ReturnsAsync(() => capturedScope);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        await _service.AddScopeAsync(userId, request);

        // Assert
        Assert.NotNull(capturedScope);
        Assert.Equal(apiKeyId, capturedScope.ApiKeyId);
        Assert.Equal(permissionId, capturedScope.PermissionId);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<ApiKeyScope>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region RemoveScopeAsync Tests

    [Fact]
    public async Task RemoveScopeAsync_WithExistingScope_RemovesScope()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();
        var request = new ApiKeyScopeRequest(apiKeyId, permissionId);
        var existingScope = new ApiKeyScope
        {
            Id = Guid.NewGuid(),
            ApiKeyId = apiKeyId,
            PermissionId = permissionId,
            CreatedAt = DateTime.UtcNow
        };

        _mockReadRepository.Setup(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<ApiKeyScope, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingScope);
        _mockRepository.Setup(r => r.DeleteAsync(existingScope, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        await _service.RemoveScopeAsync(userId, request);

        // Assert
        _mockReadRepository.Verify(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<ApiKeyScope, bool>>>(),
            It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(existingScope, It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveScopeAsync_WithNonExistentScope_DoesNothing()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();
        var request = new ApiKeyScopeRequest(apiKeyId, permissionId);

        _mockReadRepository.Setup(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<ApiKeyScope, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApiKeyScope?)null);

        // Act
        await _service.RemoveScopeAsync(userId, request);

        // Assert
        _mockReadRepository.Verify(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<ApiKeyScope, bool>>>(),
            It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<ApiKeyScope>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RemoveScopeAsync_WithCorrectScopeLookup_UsesCorrectFilter()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();
        var request = new ApiKeyScopeRequest(apiKeyId, permissionId);

        _mockReadRepository.Setup(r => r.FirstOrDefaultAsync(
            It.Is<System.Linq.Expressions.Expression<System.Func<ApiKeyScope, bool>>>(expr =>
                expr.Compile()(new ApiKeyScope { ApiKeyId = apiKeyId, PermissionId = permissionId }) &&
                !expr.Compile()(new ApiKeyScope { ApiKeyId = Guid.NewGuid(), PermissionId = permissionId }) &&
                !expr.Compile()(new ApiKeyScope { ApiKeyId = apiKeyId, PermissionId = Guid.NewGuid() })),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((ApiKeyScope?)null);

        // Act
        await _service.RemoveScopeAsync(userId, request);

        // Assert
        _mockReadRepository.Verify(r => r.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<ApiKeyScope, bool>>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region CRUD Operation Tests (Inherited from CrudService)

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesApiKeyScope()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateApiKeyScopeRequest(Guid.NewGuid(), Guid.NewGuid());
        var expectedScope = new ApiKeyScope
        {
            Id = Guid.NewGuid(),
            ApiKeyId = request.ApiKeyId,
            PermissionId = request.PermissionId,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<ApiKeyScope>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedScope);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedScope);

        // Act
        ApiKeyScopeDto result = await _service.CreateAsync(userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApiKeyScopeDto>(result);
        Assert.Equal(request.ApiKeyId, result.ApiKeyId);
        Assert.Equal(request.PermissionId, result.PermissionId);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsApiKeyScope()
    {
        // Arrange
        var scopeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apiKeyId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();
        var scope = new ApiKeyScope
        {
            Id = scopeId,
            ApiKeyId = apiKeyId,
            PermissionId = permissionId,
            CreatedAt = DateTime.UtcNow
        };

        _mockReadRepository.Setup(r => r.GetByIdAsync(scopeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(scope);

        // Act
        ApiKeyScopeDto result = await _service.GetByIdAsync(userId, scopeId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApiKeyScopeDto>(result);
        Assert.Equal(scopeId, result.Id);
        Assert.Equal(apiKeyId, result.ApiKeyId);
        Assert.Equal(permissionId, result.PermissionId);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_UpdatesApiKeyScope()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var scopeId = Guid.NewGuid();
        var request = new UpdateApiKeyScopeRequest(scopeId, Guid.NewGuid(), Guid.NewGuid());
        var existingScope = new ApiKeyScope
        {
            Id = scopeId,
            ApiKeyId = Guid.NewGuid(),
            PermissionId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetByIdAsync(scopeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingScope);
        _mockRepository.Setup(r => r.UpdateAsync(existingScope, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        ApiKeyScopeDto result = await _service.UpdateAsync(userId, request);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ApiKeyScopeDto>(result);
        Assert.Equal(request.ApiKeyId, existingScope.ApiKeyId);
        Assert.Equal(request.PermissionId, existingScope.PermissionId);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_DeletesApiKeyScope()
    {
        // Arrange
        var scopeId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var scope = new ApiKeyScope
        {
            Id = scopeId,
            ApiKeyId = Guid.NewGuid(),
            PermissionId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetByIdAsync(scopeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(scope);
        _mockRepository.Setup(r => r.DeleteAsync(scope, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));

        // Act
        await _service.DeleteAsync(userId, scopeId);

        // Assert
        _mockRepository.Verify(r => r.GetByIdAsync(scopeId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.DeleteAsync(scope, It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
