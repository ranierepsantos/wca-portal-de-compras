namespace wca.share.application.Features.Clientes.Common
{
    public record ClienteResponse (
        int Id,
        int FilialId, 
        string Nome,
        string CNPJ,
        string InscricaoEstadual,
        string Endereco,
        string Numero,
        string CEP,
        string Cidade,
        string UF,
        bool Ativo
    );

}
