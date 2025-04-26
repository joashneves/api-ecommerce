using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Model.DTO
{
    public class AlterarUsuarioCargosPorAdminDTO
    {
        public Cargo? Cargo { get; set; }
        public Permissao? Permissoes { get; set; }
    }
}
