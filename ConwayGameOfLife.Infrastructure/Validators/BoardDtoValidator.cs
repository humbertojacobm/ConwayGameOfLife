using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Infrastructure.Validators
{
    using ConwayGameOfLife.DTO;
    using FluentValidation;

    public class BoardDtoValidator : AbstractValidator<BoardDTO>
    {
        public BoardDtoValidator()
        {
            RuleFor(x => x.Width)
                .GreaterThan(0)
                .WithMessage("Width must be greater than 0.");

            RuleFor(x => x.Height)
                .GreaterThan(0)
                .WithMessage("Height must be greater than 0.");
        }
    }

}
