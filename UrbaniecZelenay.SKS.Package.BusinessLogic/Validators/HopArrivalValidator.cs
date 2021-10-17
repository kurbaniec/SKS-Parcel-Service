using System.Data;
using FluentValidation;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Validators
{
    public class HopArrivalValidator : AbstractValidator<HopArrival>
    {
        public HopArrivalValidator()
        {
            RuleFor(h => h.Code)
                .NotEmpty()
                .Matches(@"^[A-Z]{4}\d{1,4}$");
            RuleFor(h => h.Description)
                .NotEmpty()
                .Matches(@"^[a-zA-Z0-9ßäÄöÖüÜ\ \-]+$");

        }
    }
}