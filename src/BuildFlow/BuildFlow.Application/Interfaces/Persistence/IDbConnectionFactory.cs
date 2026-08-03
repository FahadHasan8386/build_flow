using System.Data;

namespace BuildFlow.Application.Interfaces.Persistence;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
