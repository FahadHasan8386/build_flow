CREATE TABLE RefreshTokens
(
    Id UNIQUEIDENTIFIER PRIMARY KEY,

    UserId UNIQUEIDENTIFIER NOT NULL,

    Token NVARCHAR(MAX) NOT NULL,

    ExpiresAt DATETIME2 NOT NULL,

    RevokedAt DATETIME2 NULL,

    CreatedBy NVARCHAR(100),
    CreatedAt DATETIME2 NOT NULL,
    ModifiedBy NVARCHAR(100),
    ModifiedAt DATETIME2 NULL,
    InActive BIT NOT NULL,

    CONSTRAINT FK_RefreshTokens_Users
        FOREIGN KEY(UserId)
        REFERENCES Users(Id)
);