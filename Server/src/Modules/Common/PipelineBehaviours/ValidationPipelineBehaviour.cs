using FluentValidation;
using MediatR;

namespace BlogBackend.Modules.Common.PipelineBehaviours;

public sealed class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> _logger;

    public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        if (_validators.Any())
        {
            var validationResults = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            var invalidResults = validationResults.Where(x => !x.IsValid).SelectMany(x => x.Errors).ToList();
            if (invalidResults.Any())
            {
                throw new ValidationException(invalidResults);
            }

        }
        return await next();
    }
}
