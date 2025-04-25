using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace Model.DTO
{
    public class ProdutosDTO
    {
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
    }
}
