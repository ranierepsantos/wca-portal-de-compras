namespace wca.reembolso.domain.Common.Enum
{

    public enum EnumTipoSolicitacao
    {
        Reembolso =1,
        Adiantamento =2,
        SolicitacaoEspecial = 3
    }
    public enum EnumTipoDespesaTipo
    {
        Consumo =  1,
        Distancia = 2
    }

    public enum EnumNotificaQuem
    {
        WCA =1,
        Cliente = 2,
        Usuario = 3
    }

    public enum EnumListarTipoDespesa
    {
        MostrarTodas = 1,
        ExibirColaborador = 2,
        NaoExibirColaborador =3
    }

}
