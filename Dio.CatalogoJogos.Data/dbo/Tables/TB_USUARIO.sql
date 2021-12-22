CREATE TABLE [dbo].[TB_USUARIO] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [Email]     NVARCHAR (MAX)   NULL,
    [SenhaHash] NVARCHAR (MAX)   NULL,
    [Fundos]    REAL             NOT NULL,
    [Permissao] NVARCHAR (MAX)   NULL,
    [Nome]      NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_TB_USUARIO] PRIMARY KEY CLUSTERED ([Id] ASC)
);

