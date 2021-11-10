namespace ApiCatalogoJogos.Enum
{
    /// <summary>
    /// Enum dos níveis de permissão dos usuários.
    /// </summary>
    public enum PermissaoUsuario
    {
        /// <summary>
        /// Permissao para visualizar classes (outros usuarios exclusos)
        /// </summary>
        Usuario = 0,
        /// <summary>
        /// Permissao para atualizar e remover usuarios
        /// </summary>
        Moderardor = 1,
        /// <summary>
        /// Permissao para visualizar, adicionar, atualizar e remover todas as entidades
        /// </summary>
        Administrador = 2
    }
}
