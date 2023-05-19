using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;

namespace ECA.POC.AntiVirusStorage.Excecao
{
    public static class ExceptionExtensions
    {
        public static DetalhesExcecao ToProblemDetails(this Exception e)
        {
            return new DetalhesExcecao()
            {
                Status = (int)GetErrorCode(e.GetInnermostException()),
                Title = e.GetInnermostException().Message
            };
        }

        private static HttpStatusCode GetErrorCode(Exception e)
        {
            switch (e)
            {
                case ValidationException _:
                    return HttpStatusCode.BadRequest;

                case FormatException _:
                    return HttpStatusCode.BadRequest;

                case AuthenticationException _:
                    return HttpStatusCode.Forbidden;

                case NotImplementedException _:
                    return HttpStatusCode.NotImplemented;

                case NotFoundException _:
                    return HttpStatusCode.NotFound;
                
                case ConflictException _:
                    return HttpStatusCode.Conflict;

                default:
                    return HttpStatusCode.InternalServerError;
            }
        }

        public static Exception GetInnermostException(this Exception e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            while (e.InnerException != null)
            {
                e = e.InnerException;
            }

            return e;
        }
    }
}