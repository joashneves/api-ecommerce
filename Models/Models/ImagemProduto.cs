using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class ImagemProduto
    {
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Caminho { get; set; }

    // Chave estrangeira
    public Guid ProdutoId { get; set; }

    }
}