using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models.Models;
using System;
using System.Linq;

namespace api_ecommerce.Attributes
{
    public class AutorizarCargoAttribute : Attribute, IAuthorizationFilter
    {
        private readonly Cargo[] _cargos;

        public AutorizarCargoAttribute(params Cargo[] cargos)
        {
            _cargos = cargos;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var cargoClaim = context.HttpContext.User.FindFirst("Cargo")?.Value;

            if (string.IsNullOrEmpty(cargoClaim) || !Enum.TryParse(cargoClaim, out Cargo cargo) || !_cargos.Contains(cargo))
            {
                context.Result = new ForbidResult(); 
            }
        }
    }
}
