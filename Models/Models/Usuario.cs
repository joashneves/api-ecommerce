using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models.Models
{
    public enum Cargo
    {
        SuperAdm = 1,
        Adm = 2,
        Comum = 3,
        Suporte = 4
    }
    [Flags] // Adiciona o atributo Flags para permitir combinações de permissões.
    public enum Permissao
    {
        Nenhuma = 0,
        Criar = 1,
        Excluir = 2,
        Atualizar = 4,
        Ler = 8,
        GerenciarUsuarios = 16,
        GerenciarProdutos = 32
    }

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

        // Definindo padrão: Cargo = Comum
        public Cargo Cargo { get; set; } = Cargo.Comum;

        // Definindo padrão: Permissão = Ler (ou outras permissões básicas que você quiser)
        public Permissao Permissoes { get; set; } = Permissao.Ler;
        // Datas de controle
        [Column(TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime UpdatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

        [Column(TypeName = "timestamp")]
        public DateTime? DeletedAt { get; set; } = null;
    }
}
