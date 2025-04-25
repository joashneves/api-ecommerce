using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Model.ViewModel
{
    public class ProdutoViewModel
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
        public List<IFormFile> Imagens { get; set; }

    }
}
