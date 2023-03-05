using System.ComponentModel.DataAnnotations;
using wca.compras.domain.Entities;

namespace wca.compras.domain.Dtos
{
    public record ClienteDto(
        int Id,
        string Nome,
        string CNPJ,
        string InscricaoEstadual,
        string Endereco,
        string Numero,
        string CEP,
        string Cidade,
        string UF,
        bool Ativo,
        string PeriodoEntrega,
        int FilialId,
        bool NaoUltrapassarLimitePorRequisicao,
        decimal ValorLimiteRequisicao,
        IList<ClienteContatoDto> ClienteContatos,
        IList<ClienteOrcamentoConfiguracaoDto> ClienteOrcamentoConfiguracao
    );

    public record UpdateClienteDto(
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int Id,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string CNPJ,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        string InscricaoEstadual,
        string Endereco,
        string Numero,
        string CEP,
        string Cidade,
        string UF,
        bool Ativo,
        string PeriodoEntrega,
        bool NaoUltrapassarLimitePorRequisicao,
        decimal ValorLimiteRequisicao,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int FilialId,
        IList<ClienteContatoDto> ClienteContatos,
        IList<ClienteOrcamentoConfiguracaoDto> ClienteOrcamentoConfiguracao
    );

    public record CreateClienteDto (
        
        [Required(ErrorMessage = "O campo é obrigatório!")] 
        string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")] 
        string CNPJ,
        [Required(ErrorMessage = "O campo é obrigatório!")] 
        string InscricaoEstadual,
        string Endereco,
        string Numero,
        string CEP,
        string Cidade,
        string UF,
        bool Ativo,
        string PeriodoEntrega,
        bool NaoUltrapassarLimitePorRequisicao,
        decimal ValorLimiteRequisicao,
        [Required(ErrorMessage = "O campo é obrigatório!")] 
        int FilialId,
        IList<ClienteContatoDto> ClienteContatos,
        IList<ClienteOrcamentoConfiguracaoDto> ClienteOrcamentoConfiguracao
    );

    public record ClienteContatoDto (
        int Id,
        int ClienteId,
        [Required(ErrorMessage = "O campo é obrigatório!")] 
        string Nome,
        [Required(ErrorMessage = "O campo é obrigatório!")] 
        string Email,
        string Telefone,
        string Celular,
        bool AprovaPedido
    );

    public record ClienteOrcamentoConfiguracaoDto (
        int Id,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int ClienteId,
        [Required(ErrorMessage = "O campo é obrigatório!")]
        int TipoFornecimentoId,
        decimal ValorPedido,
        decimal QuantidadeMes,
        decimal Tolerancia,
        EnumAprovadoPor AprovadoPor,
        bool Ativo = false
    );
}
