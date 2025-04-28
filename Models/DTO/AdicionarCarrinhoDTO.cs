using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class AdicionarCarrinhoDTO
    {
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
    }
}
