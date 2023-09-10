namespace wca.compras.domain.Dtos
{
    public record CreateFilialDto (string nome, int sistemaId);

    public record FilialDto(int Id, string Nome, bool Ativo);
}
