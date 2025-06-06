﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Models.Models
{
    public class Produto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        [Precision(18, 2)]
        public decimal Preco { get; set; }
        public string Categoria { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser maior ou igual a zero")]
        public int Quantidade { get; set; } = 0;
            // Lista de imagens relacionadas
        public List<ImagemProduto> Imagens { get; set; } = new();
                // Datas de controle
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime? DeletedAt { get; set; } = null;

    }
}
