using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Pedido
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UsuarioId { get; set; } // Username, Email ou UserId, conforme seu sistema

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        public bool Atendido { get; set; } = false;

        public string Acao { get; set; } = "Compra"; // Ex: "Compra", "Troca", "Reserva", etc

        public Localizacao Localizacao { get; set; } // Ex: endereço, cidade, ou até um produto especial que você cria

        public decimal ValorTotal { get; set; } // Soma do preço dos produtos

        public List<PedidoProduto> Produtos { get; set; } = new List<PedidoProduto>();
        // Datas de controle
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime? DeletedAt { get; set; } = null;
    }
}
