using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Models
{
    public class Pedido
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UsuarioId { get; set; } // Relacionamento com a tabela de usuários

        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

        public bool Atendido { get; set; } = false;

        public string Acao { get; set; } = "Compra"; // "Compra", "Reserva", etc.

        // Relação com localização
        public Guid LocalizacaoId { get; set; }
        public Localizacao Localizacao { get; set; }

        public decimal ValorTotal { get; set; }

        // Relação com produtos do pedido
        public List<PedidoProduto> Produtos { get; set; } = new List<PedidoProduto>();

        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime? DeletedAt { get; set; } = null;
    }
}
