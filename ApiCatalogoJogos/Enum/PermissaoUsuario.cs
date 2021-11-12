namespace ApiCatalogoJogos.Enum
{
    /// <summary>
    /// Enum dos níveis de permissão dos usuários.
    /// </summary>
    public enum PermissaoUsuario
    {
        /// <summary>
        /// Permissao para visualizar produtoras e jogos e atualizar suas próprias informações
        /// </summary>
        Usuario = 0,
        /// <summary>
        /// Permissao para adicionar atualizar e remover produtoras e jogos
        /// </summary>
        Moderador = 1,
        /// <summary>
        /// Permissao máxima
        /// </summary>
        Administrador = 2
    }
}
