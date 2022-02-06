using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api.Models.Constants
{
    public static class HtmlStatusCode
    {
        public const int SUCCESSFUL_OK = 200;
        public const int SUCCESSFUL_CREATED = 201;
        public const int SUCCESSFUL_NO_CONTENT = 204;
        public const int CLIENT_ERROR_NOT_FOUND = 404;
        public const int CLIENT_ERROR_NOT_ACCEPTABLE = 406;
        public const int CLIENT_ERROR_CONFLICT = 409;
        public const int SERVER_ERROR_INTERNAL = 500;
        public const int SERVER_ERROR_NO_IMPLEMENTED = 501;
        public const int SERVER_ERROR_NETWORK_AUTH_REQUIRED = 501;
    }
}
