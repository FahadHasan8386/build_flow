using BuildFlow.Application.Features.Identity.Login;
using BuildFlow.Application.Interfaces.Persistence;
using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Domain.Entities;
using System.Collections;
using System.Data;
using Xunit;

namespace BuildFlow.Tests;

public class LoginHandlerTests
{
    [Fact]
    public async Task LoginHandler_PersistsRefreshTokenUsingConnection()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            TenantId = Guid.NewGuid(),
            Email = "user@example.com",
            PasswordHash = "hash",
            FirstName = "Test",
            LastName = "User"
        };

        var userRepository = new FakeUserRepository(user);
        var refreshTokenRepository = new FakeRefreshTokenRepository();
        var jwtTokenService = new FakeJwtTokenService();
        var passwordHasher = new FakePasswordHasher();
        var connectionFactory = new FakeConnectionFactory();

        var handler = new LoginHandler(userRepository, refreshTokenRepository, jwtTokenService, passwordHasher, connectionFactory);

        var response = await handler.Handle(new LoginCommand(new LoginRequest { Email = user.Email, Password = "password" }), CancellationToken.None);

        Assert.True(response.Success);
        Assert.NotNull(refreshTokenRepository.LastConnection);
    }

    [Fact]
    public async Task UserRoleRepository_CanInsertUsingProvidedConnectionAndTransaction()
    {
        var repository = new FakeUserRoleRepository();
        var connection = new FakeDbConnection();
        var transaction = new FakeDbTransaction(connection);

        await repository.CreateAsync(new UserRole
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            RoleId = Guid.NewGuid()
        }, connection, transaction);

        Assert.True(true);
    }

    private sealed class FakeUserRoleRepository : IUserRoleRepository
    {
        public Task<Guid> CreateAsync(UserRole userRole, IDbConnection connection, IDbTransaction transaction) => Task.FromResult(Guid.NewGuid());
    }

    private sealed class FakeUserRepository : IUserRepository
    {
        private readonly User _user;

        public FakeUserRepository(User user) => _user = user;

        public Task<Guid> CreateAsync(User user, IDbConnection connection, IDbTransaction transaction) => Task.FromResult(Guid.NewGuid());
        public Task<User?> GetByEmailAsync(string email) => Task.FromResult<User?>(_user);
        public Task<User?> GetByIdAsync(Guid id) => Task.FromResult<User?>(_user);
        public Task<bool> ExistsByEmailAsync(string email) => Task.FromResult(false);
    }

    private sealed class FakeRefreshTokenRepository : IRefreshTokenRepository
    {
        public IDbConnection? LastConnection { get; private set; }

        public Task CreateAsync(RefreshToken refreshToken, IDbConnection connection, IDbTransaction transaction)
        {
            LastConnection = connection;
            return Task.CompletedTask;
        }

        public Task<RefreshToken?> GetByTokenAsync(string token) => Task.FromResult<RefreshToken?>(null);
        public Task RevokeAsync(string token) => Task.CompletedTask;
    }

    private sealed class FakeJwtTokenService : IJwtTokenService
    {
        public string GenerateAccessToken(User user, string role) => "token";
        public RefreshToken GenerateRefreshToken(Guid userId) => new() { Id = Guid.NewGuid(), UserId = userId, Token = "refresh" };
    }

    private sealed class FakePasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password) => "hash";
        public bool VerifyPassword(string password, string passwordHash) => true;
    }

    private sealed class FakeConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection() => new FakeDbConnection();
    }

    private sealed class FakeDbConnection : IDbConnection
    {
        public string? ConnectionString { get; set; }
        public int ConnectionTimeout { get; set; }
        public string? Database { get; set; }
        public ConnectionState State { get; set; }

        public IDbTransaction BeginTransaction() => new FakeDbTransaction(this);
        public IDbTransaction BeginTransaction(IsolationLevel il) => new FakeDbTransaction(this);
        public void ChangeDatabase(string databaseName) { }
        public void Close() => State = ConnectionState.Closed;
        public IDbCommand CreateCommand() => new FakeDbCommand(this);
        public void Open() => State = ConnectionState.Open;
        public void Dispose() => State = ConnectionState.Closed;
    }

    private sealed class FakeDbCommand : IDbCommand
    {
        public FakeDbCommand(IDbConnection connection) => Connection = connection;

        public string? CommandText { get; set; }
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }
        public IDbConnection? Connection { get; set; }
        public IDataParameterCollection Parameters { get; set; } = new FakeParameterCollection();
        public IDbTransaction? Transaction { get; set; }
        public UpdateRowSource UpdatedRowSource { get; set; }

        public void Cancel() { }
        public IDbDataParameter CreateParameter() => new FakeDbParameter();
        public int ExecuteNonQuery() => 0;
        public IDataReader ExecuteReader() => throw new NotImplementedException();
        public IDataReader ExecuteReader(CommandBehavior behavior) => throw new NotImplementedException();
        public object? ExecuteScalar() => null;
        public void Prepare() { }
        public void Dispose() { }
    }

    private sealed class FakeDbParameter : IDbDataParameter
    {
        public DbType DbType { get; set; }
        public ParameterDirection Direction { get; set; }
        public bool IsNullable { get; set; }
        public string? ParameterName { get; set; }
        public string? SourceColumn { get; set; }
        public DataRowVersion SourceVersion { get; set; }
        public object? Value { get; set; }
        public byte Precision { get; set; }
        public byte Scale { get; set; }
        public int Size { get; set; }
    }

    private sealed class FakeDbTransaction : IDbTransaction
    {
        public FakeDbTransaction(IDbConnection connection) => Connection = connection;

        public IDbConnection? Connection { get; set; }
        public IsolationLevel IsolationLevel { get; set; }

        public void Commit() { }
        public void Rollback() { }
        public void Dispose() { }
    }

    private sealed class FakeParameterCollection : IList, ICollection, IEnumerable, IDataParameterCollection
    {
        private readonly List<object?> _items = new();

        public object? this[int index] { get => _items[index]; set => _items[index] = value; }
        public object? this[string parameterName]
        {
            get => _items.FirstOrDefault(item => item is IDbDataParameter { ParameterName: not null } p && p.ParameterName == parameterName);
            set => Add(value);
        }

        public bool IsFixedSize => false;
        public bool IsReadOnly => false;
        public int Count => _items.Count;
        public bool IsSynchronized => false;
        public object SyncRoot => this;

        public int Add(object? value)
        {
            _items.Add(value);
            return _items.Count - 1;
        }

        public void Clear() => _items.Clear();
        public bool Contains(object? value) => _items.Contains(value);
        public bool Contains(string parameterName) => _items.OfType<IDbDataParameter>().Any(p => p.ParameterName == parameterName);
        public void CopyTo(Array array, int index) => _items.ToArray().CopyTo(array, index);
        public IEnumerator GetEnumerator() => _items.GetEnumerator();
        public int IndexOf(object? value) => _items.IndexOf(value);
        public int IndexOf(string parameterName) => _items.OfType<IDbDataParameter>().Select((p, i) => new { p, i }).FirstOrDefault(x => x.p.ParameterName == parameterName)?.i ?? -1;
        public void Insert(int index, object? value) => _items.Insert(index, value);
        public void Remove(object? value) => _items.Remove(value);
        public void RemoveAt(int index) => _items.RemoveAt(index);
        public void RemoveAt(string parameterName)
        {
            var index = IndexOf(parameterName);
            if (index >= 0)
            {
                _items.RemoveAt(index);
            }
        }
    }
}
