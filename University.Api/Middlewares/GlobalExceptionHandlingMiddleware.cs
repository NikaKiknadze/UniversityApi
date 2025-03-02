using Newtonsoft.Json;
using System.Net;
using University.Domain.CustomExceptions;
using University.Domain.CustomResponses;

namespace University.Api.Middlewares
{
    public class GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                await HandleException(context, ex, (int)HttpStatusCode.NotFound);
            }
            catch (BadRequestException ex)
            {
                await HandleException(context, ex, (int)HttpStatusCode.BadRequest);
            }
            catch (ConflictException ex)
            {
                await HandleException(context, ex, (int)HttpStatusCode.Conflict);
            }
            catch (NoContentException ex)
            {
                await HandleException(context, ex, (int)HttpStatusCode.NoContent);
            }
            catch(Exception ex)
            {
                await HandleException(context, ex, (int)HttpStatusCode.InternalServerError);
            }

        }

        private async Task HandleException(HttpContext context, Exception ex, int statusCode)
        {
            logger.LogError(ex, ex.Message);

            context.Response.StatusCode = statusCode;

            context.Response.ContentType = "application/json";

            var errorResponse = ApiResponse<object>.ExceptionResult(new {ex.Message, ex.InnerException});

            var jsonErrorResponse = JsonConvert.SerializeObject(errorResponse);

            await context.Response.WriteAsync(jsonErrorResponse);
        }
    }
}
