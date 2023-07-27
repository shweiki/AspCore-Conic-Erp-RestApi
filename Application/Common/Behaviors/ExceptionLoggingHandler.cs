using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class ExceptionLoggingHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
         where TRequest : IRequest<TResponse>
         where TException : Exception
{
    private readonly ILogger<ExceptionLoggingHandler<TRequest, TResponse, TException>> _logger;

    public ExceptionLoggingHandler(ILogger<ExceptionLoggingHandler<TRequest, TResponse, TException>> logger)
    {
        _logger = logger;
    }

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Something went wrong while handling request of type {@requestType}", typeof(TRequest));

        // TODO: when we want to show the user somethig went wrong, we need to expand this with something like
        // a ResponseBase where we wrap the actual response and return an indication whether the call was successful or not.
        //state.SetHandled(default!);

        return Task.CompletedTask;
    }
}
