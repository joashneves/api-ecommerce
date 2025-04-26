using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api_ecommerce.Attributes
{
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

    public class AutorizarPermissaoAttribute : Attribute, IAuthorizationFilter
    {
        private readonly Permissao _requiredPermissao;

        public AutorizarPermissaoAttribute(Permissao requiredPermissao)
        {
            _requiredPermissao = requiredPermissao;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var permissoesClaim = context.HttpContext.User.FindFirst("Permissoes")?.Value;
            var permissoesInt = int.Parse(permissoesClaim ?? "0");
            var permissoesUsuario = (Permissao)permissoesInt;

            if (!permissoesUsuario.HasFlag(_requiredPermissao))
            {
                context.Result = new UnauthorizedResult(); // Ou um redirecionamento
            }
        }
    }
}
