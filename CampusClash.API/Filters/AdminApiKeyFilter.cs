using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CampusClash.API.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AdminApiKeyAttribute : ServiceFilterAttribute
{
    public AdminApiKeyAttribute() : base(typeof(AdminApiKeyFilter)) { }
}

public class AdminApiKeyFilter : IAuthorizationFilter
{
    private readonly IConfiguration _configuration;

    public AdminApiKeyFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("X-Admin-Key", out var key)
            || key != _configuration["Admin:ApiKey"])
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Acceso no autorizado." });
        }
    }
}
