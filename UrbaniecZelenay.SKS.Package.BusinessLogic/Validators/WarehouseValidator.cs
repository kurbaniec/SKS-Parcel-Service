using System.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Validators
{
    public class WarehouseValidator : AbstractValidator<Warehouse>
    {
        public WarehouseValidator()
        {
            RuleFor(w => w.NextHops).NotNull();
            
        }
    }
}