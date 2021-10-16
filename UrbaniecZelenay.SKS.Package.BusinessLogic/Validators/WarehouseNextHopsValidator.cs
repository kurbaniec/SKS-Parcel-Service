using FluentValidation;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Validators
{
    public class WarehouseNextHopsValidator : AbstractValidator<WarehouseNextHops>
    {
        public WarehouseNextHopsValidator()
        {
            RuleFor(w => w.Hop).NotNull();
        }
    }
}