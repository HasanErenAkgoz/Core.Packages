using Core.Packages.Application.Repositories;
using MediatR;

namespace Core.Packages.Application.Features;

public abstract class BaseCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly IUnitOfWork UnitOfWork;

    protected BaseCommandHandler(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await UnitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await HandleCommand(request, cancellationToken);
            await UnitOfWork.CommitTransactionAsync(cancellationToken);
            return response;
        }
        catch
        {
            await UnitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    protected abstract Task<TResponse> HandleCommand(TRequest request, CancellationToken cancellationToken);
} 