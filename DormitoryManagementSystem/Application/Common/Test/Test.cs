using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Test
{
    public class Test
    {
        public class Query : IRequest
        {
            public string Text { get; set; }
        }

        public class Handler : IRequestHandler<Query>
        {
            public Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                return Unit.Task;
            }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                CascadeMode = CascadeMode.StopOnFirstFailure;

                RuleFor(x => x.Text).NotEmpty().Must(x => x.Length > 5).WithMessage("Na zich");
            }
        }
    }
}
