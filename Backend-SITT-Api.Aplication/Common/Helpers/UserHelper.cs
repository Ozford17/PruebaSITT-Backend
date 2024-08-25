using Backend_SITT_Api.Aplication.Contracts.Identity;
using Backend_SITT_Api.Aplication.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Common.Helpers
{
    public static class UserHelper
    {
        public static T GetUserData<T>(this T command, IHeaderDictionary headers, IAuthService? auth = null) where T : AuditProps
        {
            var userName = GetHeader(headers, "UserName");
            var userId = GetHeader(headers, "UserId");
            command.CurrentUserName = userName;
            if (!Guid.TryParse(userId, out Guid result))
                command.CurrentUserId = Guid.Empty;
            else if (auth != null)
            {
                IdentityUser user = (IdentityUser)auth.GetUserById(result).Result;
                command.CurrentUserEmail = user.Email;
            }
            command.CurrentUserId = result;
            return command;
        }

        private static string? GetHeader(IHeaderDictionary headers, string headerName)
        {
            if (!headers.TryGetValue(headerName, out StringValues headerValue) || headerValue.Count() > 1)
                return null;
            return headerValue;
        }
    }
}
