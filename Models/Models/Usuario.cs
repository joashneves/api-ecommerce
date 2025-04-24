using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Models.Models
{
    public class Usuario
    {
        [Key]
        public string Id { get; set; }
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
    }
}
