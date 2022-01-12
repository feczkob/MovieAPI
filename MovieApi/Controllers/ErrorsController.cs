using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieApi.Controllers
{
    public class ErrorsController : ControllerBase
    {
        private readonly ILogger<ErrorsController> _logger;

        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger;
        }

        [Route("error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public MyErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error;
            var code = 500;
            var message = "Internal Server Error";

            _logger.LogError(exception, "Something went wrong");

            if(exception is InvalidOperationException)
            {
                code = 409;
                message = "Operation cannot be executed in the current state of the application.";
            }
            else if (exception is ArgumentException)
            {
                code = 400;
                message = "Something is wrong the the parameters provided.";
            }
            else if (exception is HttpResponseException httpEx)
            {
                code = httpEx.Status;
                message = httpEx.Message;
            }

            Response.StatusCode = code;

            return new MyErrorResponse(message);
        }
    }

    public class MyErrorResponse
    {
        public string Message { get; init; }

        public MyErrorResponse(string message)
        {
            Message = message;
        }
    }

    public class HttpResponseException : Exception
    {
        public HttpResponseException(string message) : base(message) { }

        public HttpResponseException(string message, int status) : base(message) 
        {
            Status = status;
        }

        public int Status { get; set; } = 500;
    }
}
