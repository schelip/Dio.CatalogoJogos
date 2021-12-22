:r .\dbo.__EFMigrationsHistory.Table.sql
Go
ALTER TABLE dbo.TB_PRODUTORA
	DROP CONSTRAINT [FK_TB_PRODUTORA_TB_PRODUTORA_ProdutoraMaeId]
:r .\dbo.TB_PRODUTORA.Table.sql
Go
ALTER TABLE dbo.TB_PRODUTORA
	ADD CONSTRAINT [FK_TB_PRODUTORA_TB_PRODUTORA_ProdutoraMaeId]
	FOREIGN KEY ([ProdutoraMaeId]) REFERENCES [dbo].[TB_PRODUTORA] ([Id])
Go
:r .\dbo.TB_JOGO.Table.sql
Go
:r .\dbo.TB_USUARIO.Table.sql
Go
:r .\dbo.TB_USUARIOJOGO.Table.sql