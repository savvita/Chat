using Chat.DataAccess.UI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Chat.API
{
    public class AppExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                if (context.Exception is InvalidCredentialsException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "invalid-credentials"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                }
                else if (context.Exception is BannedUserException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "user-is-banned"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                }
                else if (context.Exception is LoginConflictException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "login-is-registered"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Conflict
                    };
                }
                else if (context.Exception is ForbiddenException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "forbidden"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden
                    };
                }
                else if (context.Exception is MaxRequestCountException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "max-count-reached"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden
                    };
                }
                else if (context.Exception is AuthorizationException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "authorization-failed"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }
                else if (context.Exception is InternalServerException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "internal-server-error"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }
                else if (context.Exception is UserNotFoundException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "user-not-found"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.NotFound
                    };
                }
                else if (context.Exception is SubscriptionNotFoundException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "subscription-not-found"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.NotFound
                    };
                }
                else if (context.Exception is ArgumentNullException)
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "required-argumets-missed"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.NotFound
                    };
                }
                else
                {
                    context.Result = new ObjectResult(new
                    {
                        code = "internal-server-error"
                    })
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }


                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
