namespace api_ecommerce.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Pega o cargo do usuário a partir do token JWT
            var cargo = context.User.FindFirst("Cargo")?.Value;

            // Procura o endpoint da requisição
            var endpoint = context.GetEndpoint();

            // Verifica se tem [AllowAnonymous]
            var allowAnonymous = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>();

            // Se o endpoint permitir acesso anônimo, não faz nada
            if (allowAnonymous != null)
            {
                // Se tiver [AllowAnonymous], pula o middleware e deixa passar
                await _next(context);
                return;
            }
            if (string.IsNullOrEmpty(cargo))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Usuário não autorizado.");
                return;
            }

            // Verifique a lógica de permissões aqui se necessário, com base no cargo.

            await _next(context);
        }
    }
}
