using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Model.Models
{
    public class PedidoProduto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }

        // Navegação (opcional, facilita consultas depois)
        public Pedido Pedido { get; set; }
        public Produto Produto { get; set; }
        // Datas de controle
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime? DeletedAt { get; set; } = null;
    }
}
