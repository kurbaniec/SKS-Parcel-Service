using FluentValidation;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Validators
{
    public class ParcelValidator : AbstractValidator<Parcel>
    {
        public ParcelValidator()
        {
            RuleFor(p => p.TrackingId)
                .Matches(@"^[A-Z0-9]{9}$")
                .When(p => p != null);
            RuleFor(p => p.Weight).GreaterThan(0);
            RuleFor(p => p.Recipient).NotNull();
            RuleFor(p => p.Sender).NotNull();
            RuleFor(p => p.FutureHops).NotNull();
            RuleFor(p => p.VisitedHops).NotNull();
            RuleFor(p => p.State).NotNull();
        }
    }
}