﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;

namespace RealEstateAuctionManagement.Helpers
{
    public class ResponseHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            await _next(context);
        }
    }

    public class ResultApi
    {
        public string StatusCode { get; set; } = string.Empty;
        public string Messagae { get; set; } = string.Empty;
        public object? Data { get; set; }

    }

    public class ResultManipulator : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            // throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            //run code immediately before and after the execution of action results. They run only when the action method has executed successfully. They are useful for logic that must surround view or formatter execution.
            var result = context.Result as ObjectResult;
            if (result != null)
            {
                var resultObj = result.Value;

                //change this ResultApi with your ApiResponse class
                var resp = new ResultApi
                {
                    StatusCode = context.HttpContext.Response.StatusCode.ToString()
                };

                if (!context.ModelState.IsValid)
                {
                    var validationErrors = context.ModelState
                        .Where(entry => entry.Value.Errors.Any())
                        .ToDictionary(
                            entry => entry.Key,
                            entry => entry.Value.Errors.Select(error => error.ErrorMessage).ToArray()
                        );

                    resp.StatusCode = "400"; // Bad Request
                    resp.Messagae = "Validation failed";
                    resp.Data = validationErrors;

                    // Return the response with validation errors
                    context.Result = new JsonResult(resp, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        Converters = { new JsonStringEnumConverter() }
                    });

                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return;
                }

                if (resultObj is not null)
                    resp.Data = resultObj;

                //you can also change this from System.Text.Json to newtonsoft if you use newtonsoft
                context.Result = new JsonResult(resp, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    Converters = { new JsonStringEnumConverter() }
                });
            }
            
        }

    }
}
