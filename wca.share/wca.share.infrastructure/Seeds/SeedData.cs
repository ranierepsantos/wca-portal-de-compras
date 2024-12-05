using Microsoft.EntityFrameworkCore;
using wca.share.domain.Common.Enum;
using wca.share.domain.Entities;

namespace wca.share.infrastructure.Seeds
{
    public static class SeedData
    {

        public static void FillDatabase(ModelBuilder builder)
        {
            SeedStatusSolicitacao(builder);
            SeedItemMudanca(builder);  
        }

        private static void SeedStatusSolicitacao(ModelBuilder builder)
        {
            builder.Entity<StatusSolicitacao>().HasData(
                new StatusSolicitacao { Id = 1, Status = "Pendente", StatusIntermediario = "Pendente", Color = "warning", Notifica = EnumNotificaQuem.WCA, Autorizar = false, TemplateNotificacao = "Novo(a) {TipoSolicitacao} solicitado(a) - código: #{id}!", ProximoStatusId = null }, 
                new StatusSolicitacao { Id = 2, Status = "Em Andamento", StatusIntermediario = "Em Andamento", Color = "info", Notifica = EnumNotificaQuem.NaoNotificar, Autorizar = false, TemplateNotificacao = null, ProximoStatusId = null }, 
                new StatusSolicitacao { Id = 3, Status = "Concluído", StatusIntermediario = "Concluído", Color = "success", Notifica = EnumNotificaQuem.NaoNotificar, Autorizar = false, TemplateNotificacao = null, ProximoStatusId = null }, 
                new StatusSolicitacao { Id = 4, Status = "Pendente", StatusIntermediario = "Aguardando Aprovação", Color = "gray", Notifica = EnumNotificaQuem.WCA, Autorizar = true, TemplateNotificacao = "{TipoSolicitacao} #{id} está aguardando aprovação!", ProximoStatusId = 1 }, 
                new StatusSolicitacao { Id = 5, Status = "Concluído", StatusIntermediario = "Reprovado", Color = "red", Notifica = EnumNotificaQuem.WCA, Autorizar = false, TemplateNotificacao = "{TipoSolicitacao} #{id} reprovado(a)!", ProximoStatusId = null }, 
                new StatusSolicitacao { Id = 6, Status = "Concluído", StatusIntermediario = "Cancelado", Color = "red", Notifica = EnumNotificaQuem.NaoNotificar, Autorizar = false, TemplateNotificacao = null, ProximoStatusId = null }
            );
        }

        private static void SeedItemMudanca (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemMudanca>().HasData(
                new ItemMudanca { Id = 1, Descricao = "Alteração de benefícios", Ativo = true },
                new ItemMudanca { Id = 2, Descricao = "Alteração de salário", Ativo = true },
                new ItemMudanca { Id = 3, Descricao = "Alteração de horário", Ativo = true },
                new ItemMudanca { Id = 4, Descricao = "Exclusão insalubridade", Ativo = true },
                new ItemMudanca { Id = 5, Descricao = "Exclusão periculosidade", Ativo = true },
                new ItemMudanca { Id = 6, Descricao = "Inclusão insalubridade", Ativo = true },
                new ItemMudanca { Id = 7, Descricao = "Inclusão periculosidade", Ativo = true }
            );
        }
    }
}
