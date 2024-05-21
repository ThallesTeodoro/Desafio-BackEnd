using FluentValidation;

namespace DesafioBackEnd.Application.Bikes.List;

public class ListBikeQueryValidator : AbstractValidator<ListBikeQuery>
{
    public ListBikeQueryValidator()
    {
        RuleFor(c => c.Page)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(c => c.PageSize)
            .NotEmpty()
            .GreaterThan(0);
    }
}

