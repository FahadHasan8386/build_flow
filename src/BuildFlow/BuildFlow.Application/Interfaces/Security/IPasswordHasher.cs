using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Interfaces.Security;

public interface IPasswordHasher
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHash);
}
