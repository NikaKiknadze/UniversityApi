using System.Net;
using Newtonsoft.Json;
using University.Domain.CustomExceptions;

namespace University.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
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
        catch (AuthorizationDeniedException ex)
        {
            await HandleException(context, ex, (int)HttpStatusCode.Unauthorized);
        }
        catch(Exception ex)
        {
            await HandleException(context, ex, (int)HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleException(HttpContext context, Exception ex, int statusCode)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = ex.Message,
            innerException = ex.InnerException?.Message,
            status = statusCode
        };

        var json = JsonConvert.SerializeObject(response);
        await context.Response.WriteAsync(json);
    }
}