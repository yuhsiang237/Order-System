using OrderSystem.ViewModels;
using System.Linq;
using FluentValidation;

namespace OrderSystem.Models.Validator
{
    public class RoleCreateValidator : AbstractValidator<RoleCreateViewModel>
    {
        public RoleCreateValidator(OrderSystemContext context)
        {
            RuleFor(x => x.Role.Name).NotNull().WithMessage("名稱不可為空");
            RuleFor(x => x).Custom((x, c) =>
            {
                var Role = context.Roles
                .Where(x => x.IsDeleted != true)
                .FirstOrDefault(item => item.Name == x.Role.Name);
                if (Role != null)
                {
                    c.AddFailure("Role.Name", "已有相同名稱");
                }
            });
        }
       
    }
}

