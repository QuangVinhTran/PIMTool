using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIMTool.Core.Constants;
using PIMTool.Core.Enums;
using PIMTool.Core.Exceptions;
using PIMTool.Core.Exceptions.Base;
using PIMTool.Core.Helpers;
using PIMTool.Core.Models;

namespace PIMTool.Controllers.Base;

[ApiController]
public abstract class BaseController : Controller
{
    #region Api Action Result

    #region Success

    public override OkObjectResult Ok(object? value)
    {
        return Success(value);
    }

    protected OkObjectResult Success(object? data, string message = null)
    {
        var successResult = new ApiResponse(true)
        {
            Data = data
        };
        
        var detail = message.IsNotNullNorEmpty() ? message : ApiResultStatus.SUCCESS;
        successResult.AddSuccessMessage(detail);
        return base.Ok(successResult);
    }

    #endregion

    #region Failed

    protected IActionResult ClientError(string messageContent, object data = null)
    {
        var apiResult = new ApiResponse(false)
        {
            Data = data,
            Messages = new List<AppMessage>()
            {
                new AppMessage()
                {
                    Content = messageContent,
                    Type = AppMessageType.Error
                }
            }
        };
        
        return BadRequest(apiResult);
    }

    private IActionResult Error(string messageContent, AppErrorType errorType = AppErrorType.InternalServerError, object? data = null)
    {
        var apiResult = new ApiResponse(false)
        {
            Data = data,
            Messages = new List<AppMessage>()
            {
                new AppMessage()
                {
                    Content = messageContent,
                    Type = AppMessageType.Error
                }
            }
        };

        return errorType switch
        {
            AppErrorType.ClientError => BadRequest(apiResult),
            AppErrorType.BusinessError => StatusCode(StatusCodes.Status409Conflict, apiResult),
            _ => InternalError(messageContent)
        };
    }

    protected IActionResult InternalError(string message)
    {
        return StatusCode(StatusCodes.Status500InternalServerError, message);
    }

    #endregion
    
    #endregion
    
    #region Execute Api Action

    protected async Task<IActionResult> ExecuteApiAsync(Func<Task<ApiActionResult>> apiLogicFuc)
    {
        var methodInfo = string.Empty;
        var startTime = DateTime.UtcNow;
        try
        {
            // var correlationId = HttpContext.Response.Headers["X-Correlation-Id"];
            StringInterpolationHelper.AppendToStart(apiLogicFuc.Method.Name!);
            // StringInterpolationHelper.AppendWithDefaultFormat($"CorrelationId = {correlationId.ToString().ToUpper()}");
            methodInfo = StringInterpolationHelper.BuildAndClear();
            LogHelper.WriteInfo($"[START] [API-Method] - {methodInfo}");

            var apiActionResult = await apiLogicFuc();

            StringInterpolationHelper.AppendToStart("Result of [[");
            StringInterpolationHelper.Append(methodInfo);
            StringInterpolationHelper.Append($"]]. IsSuccess: {apiActionResult.IsSuccess}");
            StringInterpolationHelper.Append(". Detail: ");
            StringInterpolationHelper.Append(apiActionResult.Detail);
            LogHelper.WriteInfo(StringInterpolationHelper.BuildAndClear());
            
            return apiActionResult.IsSuccess ? Success(apiActionResult.Data, apiActionResult.Detail) : ClientError(apiActionResult.Detail, apiActionResult.Data);
        }
        catch (Exception e)
        {
            return e.GetType().IsAssignableTo(typeof(IAppException)) ? Error(e.Message, AppErrorType.BusinessError) : InternalError(e.Message);
        }
        finally
        {
            StringInterpolationHelper.AppendToStart($"[END] - {methodInfo}. ");
            StringInterpolationHelper.Append("Total: ");
            StringInterpolationHelper.Append((DateTime.UtcNow - startTime).Milliseconds.ToString());
            StringInterpolationHelper.Append(" ms.");
            LogHelper.WriteInfo(StringInterpolationHelper.BuildAndClear());
        }
    }
    
    #endregion
}