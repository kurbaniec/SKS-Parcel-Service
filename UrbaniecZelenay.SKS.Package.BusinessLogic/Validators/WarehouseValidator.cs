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
            RuleFor(w => w.LocationCoordinates).NotNull();
            RuleFor(h => h.Code)
                .NotEmpty()
                .Matches(@"^[A-Z]{4}\d{1,4}$");
            RuleFor(h => h.Description)
                .NotEmpty()
                .Matches(@"^[a-zA-Z0-9ßäÄöÖüÜ\ \-]+$");
        }
    }
}