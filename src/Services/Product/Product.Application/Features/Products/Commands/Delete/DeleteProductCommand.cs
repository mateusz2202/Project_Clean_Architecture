using MediatR;
using Product.Shared.Wrapper;
using Product.Application.Interfaces.Repositories;

namespace Product.Application.Application.Features.Products.Commands.Delete;

public record DeleteProductCommand(int Id) : IRequest<Result<int>>;

internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<int>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork<int> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Repository<Domain.Entities.Product>().GetByIdAsync(command.Id);
        if (product != null)
        {
            await _unitOfWork.Repository<Domain.Entities.Product>().DeleteAsync(product);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(product.Id, "Product Deleted");
        }
        else
        {
            return await Result<int>.FailAsync("Product Not Found!");
        }
    }
}