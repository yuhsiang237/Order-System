using OrderSystem.ViewModels;
using System.Linq;
using FluentValidation;
using OrderSystem.Authorization;

namespace OrderSystem.Models.Validator
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateViewModel>
    {
        public UserUpdateValidator(OrderSystemContext context)
        {
            RuleFor(x => x.Name).NotNull().WithMessage("名稱不可為空");
            RuleFor(x => x.Email).NotNull().WithMessage("信箱不可為空");
            RuleFor(x => x).Custom((x, c) =>
            {
                var currentUser = context.Users.
                 Where(x => x.IsDeleted != true).
                 FirstOrDefault(item => item.Id == x.Id);
                // role permission
                if (currentUser.RoleId != x.RoleId)
                {
                    var isRoleModifyPermission = context.Permissions.
                FirstOrDefault(item => item.RoleId == currentUser.RoleId 
                && item.Code == Permissions.Basic_Permission_Modify);
                    if (isRoleModifyPermission == null)
                    {
                        c.AddFailure("RoleId", "沒有權限變更角色");
                    }
                }
            });
        }
       
    }
}

