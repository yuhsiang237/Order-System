using OrderSystem.ViewModels;
using System.Linq;
using FluentValidation;

namespace OrderSystem.Models.Validator
{
    public class RoleUpdateValidator : AbstractValidator<RoleUpdateViewModel>
    {
        public RoleUpdateValidator(OrderSystemContext context)
        {


            RuleFor(x => x.Role.Name).NotNull().WithMessage("名稱不可為空");
            RuleFor(x => x).Custom((x, c) =>
            {
                var Role = context.Roles.
                  Where(x => x.IsDeleted != true).
                  FirstOrDefault(item =>
                      (item.Name == x.Role.Name && item.Id != x.Role.Id)
                  );
                if (Role != null)
                {
                    c.AddFailure("Role.Name", "已有相同名稱");
                }
            });
        }
       
    }
}

