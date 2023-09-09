﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace wca.reembolso.domain.Entities
{

    public sealed class Usuario
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Column("nome", TypeName = "varchar(100)")]
        public string Nome { get; set; }

        [Required, MaxLength(150)]
        [Column("email", TypeName = "varchar(150)")]
        public string Email { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }

        [JsonIgnore]
        public IList<FilialUsuario> FilialUsuario { get; set; } = new List<FilialUsuario>();
    }
}