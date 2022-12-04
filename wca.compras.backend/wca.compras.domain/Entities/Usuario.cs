﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using wca.compras.domain.Interfaces;

namespace wca.compras.domain.Entities
{
    public class Usuario: IEntity
    {
        [Column("id")]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Column("nome", TypeName = "varchar(100)")]
        public string Nome { get; set; }

        [Required, MaxLength(150)]
        [Column("email", TypeName = "varchar(150)")]
        public string Email { get; set; }

        [MaxLength(200)]
        [Column("password", TypeName = "varchar(200)")]
        public string? Password { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }
        
        [JsonIgnore]
        public Cliente Cliente { get; set; }

        [Column("cliente_id")]
        public int? ClienteId { get; set; }

        [JsonIgnore]
        public Perfil Perfil { get; set; }

        [Column("perfil_id")]
        public int? PerfilId { get; set; }

        [JsonIgnore]
        public Filial Filial { get; set; }

        [Column("filial_id")]
        public int? FilialId { get; set; }


    }
}