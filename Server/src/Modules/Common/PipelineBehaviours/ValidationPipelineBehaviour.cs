using FluentValidation;
using MediatR;

namespace BlogBackend.Modules.Common.PipelineBehaviours;

public sealed class ValidationPipelineBehaviour<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<ValidationPipelineBehaviour<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        if (!validators.Any()) return await next();
        
        var validationResults = await Task.WhenAll(validators.Select(x => x.ValidateAsync(context, cancellationToken)));

        var invalidResults = validationResults.Where(x => !x.IsValid).SelectMany(x => x.Errors).ToList();
        if (invalidResults.Count != 0)
        {
            throw new ValidationException(invalidResults);
        }
        return await next();
    }
}
