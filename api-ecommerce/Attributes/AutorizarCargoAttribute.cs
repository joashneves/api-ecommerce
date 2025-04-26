using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Models;

namespace api_ecommerce.Attributes
{
    public class AutorizarCargoAttribute : Attribute, IAuthorizationFilter
    {
        private readonly Cargo _requiredCargo;

        public AutorizarCargoAttribute(Cargo requiredCargo)
        {
            _requiredCargo = requiredCargo;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var cargoClaim = context.HttpContext.User.FindFirst("Cargo")?.Value;

            if (string.IsNullOrEmpty(cargoClaim) || !Enum.TryParse(cargoClaim, out Cargo cargo) || cargo != _requiredCargo)
            {
                context.Result = new UnauthorizedResult(); // Ou você pode redirecionar para uma página de erro
            }
        }
    }
}
