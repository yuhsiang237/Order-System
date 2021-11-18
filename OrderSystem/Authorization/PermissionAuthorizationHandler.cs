using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrderSystem.Authorization
{
    // 權限控制邏輯
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly IServiceProvider _serviceProvider;
        public PermissionAuthorizationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            // check login 
            var accountClaim = context.User.FindFirst(x => x.Type == "Account");
            if (accountClaim == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var requirementPermissions = requirement.Permissions;
            string userAccount = "";

            if (context.User != null)
            {

                //get user permission
                foreach (Claim claim in context.User.Claims)
                {
                    if (claim.Type == "Account")
                    {
                        userAccount = claim.Value;
                        break;
                    }
                }
            }
            List<string> userPermission = null;
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<OrderSystemContext>();

                var user = _context.Users.FirstOrDefault(x => x.Account == userAccount);
                userPermission = (from a in _context.Permissions
                               where a.RoleId == user.RoleId
                               select a.Code
                               ).ToList();
            }
            // check permission   
            bool isExist = false;
            for (int i = 0; i < userPermission.Count(); i++)
            {
                for (int j = 0; j < requirementPermissions.Count(); j++)
                {
                    if (userPermission[i] == requirementPermissions[j])
                    {
                        isExist = true;
                        break;
                    }
                }
            }
            // check success
            if (isExist)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
