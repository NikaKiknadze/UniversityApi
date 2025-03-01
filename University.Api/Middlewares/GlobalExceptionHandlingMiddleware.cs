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
                await HandleException(context, ex, (int)HttpStatusCode.NotFound, "Not Found");
            }
            catch (BadRequestException ex)
            {
                await HandleException(context, ex, (int)HttpStatusCode.BadRequest, "Bad Request");
            }
            catch (ConflictException ex)
            {
                await HandleException(context, ex, (int)HttpStatusCode.Conflict, "Conflict");
            }
            catch (NoContentException ex)
            {
                await HandleException(context, ex, (int)HttpStatusCode.NoContent, "No Content");
            }
            catch(Exception ex)
            {
                await HandleException(context, ex, (int)HttpStatusCode.InternalServerError, "Internal Server Error");
            }

        }

        private async Task HandleException(HttpContext context, Exception ex, int statusCode, string title)
        {
            logger.LogError(ex, ex.Message);

            context.Response.StatusCode = statusCode;

            context.Response.ContentType = "application/json";

            var errorResponse = ApiResponse<object>.ErrorResult(ex.Message);

            var jsonErrorResponse = JsonConvert.SerializeObject(errorResponse);

            await context.Response.WriteAsync(jsonErrorResponse);

            
        }
    }
}
