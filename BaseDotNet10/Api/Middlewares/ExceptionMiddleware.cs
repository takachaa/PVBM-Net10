using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Domain.Exceptions;

namespace Api.Middlewares;

/// <summary>
/// グローバル例外ハンドリングミドルウェア
/// </summary>
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        logger.LogWarning(exception, "バリデーションエラー: {Errors}", string.Join(", ", exception.Errors));

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = "バリデーションエラー",
            Detail = string.Join(", ", exception.Errors),
            Instance = context.Request.Path
        };
        problemDetails.Extensions["errors"] = exception.Errors;

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "予期しないエラーが発生しました");

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "サーバーエラー",
            Detail = "予期しないエラーが発生しました。",
            Instance = context.Request.Path
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}
