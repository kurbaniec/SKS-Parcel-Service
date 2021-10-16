using System.Data;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Formatters;
using UrbaniecZelenay.SKS.Package.BusinessLogic.Entities;

namespace UrbaniecZelenay.SKS.Package.BusinessLogic.Validators
{
    public class RecipientValidator : AbstractValidator<Recipient>
    {
        public RecipientValidator()
        {
            RuleFor(r => r.PostalCode)
                .NotEmpty()
                .Matches(@"^A\-\d{4}$")
                .When(r => r.Country is "Austria" or "Österreich");
            
            RuleFor(r => r.Street)
                .NotEmpty()
                .Matches(@"^[A-Za-zßäÄöÖüÜ]+\ [\d][\da-z\/]*$")
                .When(r => r.Country is "Austria" or "Österreich");
            
            RuleFor(r => r.City)
                .NotEmpty()
                .Matches(@"^[A-Z][a-zA-Z0-9äÄöÖüÜß\ \-]+$")
                .When(r => r.Country is "Austria" or "Österreich");
            
            RuleFor(r => r.Name)
                .NotEmpty()
                .Matches(@"^[A-Z][a-zA-Z0-9äÄöÖüÜß\ \-]+$")
                .When(r => r.Country is "Austria" or "Österreich");
        }
    }
}