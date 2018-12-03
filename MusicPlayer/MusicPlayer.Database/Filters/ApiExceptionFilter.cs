using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using MusicPlayer.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Core.Services.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;

namespace MusicPlayer.Service.Filters
{
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        private ILogger _logger;

        public ApiExceptionAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is ValidationException)
            {
                var exception = filterContext.Exception as ValidationException;
                filterContext.HttpContext.Response.StatusCode = 400;
                filterContext.Result = new JsonResult(new ValidationResultModel(exception));
                return;
            }

            var exceptionId = Guid.NewGuid();
            filterContext.HttpContext.Response.StatusCode = 500;
            var sb = new StringBuilder();
            sb.AppendLine("ErrorId: " + exceptionId);
            sb.AppendLine(filterContext.HttpContext.Request.GetDisplayUrl());
            sb.AppendLine();
            //filterContext.HttpContext.Request..Values.ForEach(parameter => sb.Append($"{parameter.Key} = {parameter.Value}").AppendLine());
            sb.AppendLine();
            sb.Append(filterContext.Exception);

            _logger.Error(sb.ToString());

            filterContext.Result = new ContentResult() { Content = exceptionId.ToString() };
        }
    }
}
