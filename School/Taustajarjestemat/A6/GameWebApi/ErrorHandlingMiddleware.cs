using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GameWebApi
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //await _next.Invoke(context);
                await _next(context);
            }
            catch (NotFoundException)
            {
                await context.Response.WriteAsync("404");
            }
            catch(ExpectationFilt) 
            {
                await context.Response.WriteAsync("Too low level to add sword. Need to be at least 3 lvl");
            }

        }

    }

}
