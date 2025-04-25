using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models.Models
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome_completo { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "O nome deve ter entre 6 e 50 caracteres")]
        // Unico
        public string Nome_usuario { get; set; }

        public string Senha { get; set; }
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }  
        public string CPF { get; set; }
                // Datas de controle
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime? DeletedAt { get; set; } = null;
    }
}
