using MediatR;
using Product.Shared.Wrapper;
using Product.Application.Interfaces.Repositories;
using Product.Domain.Entities;
using Product.Shared.Constans;

namespace Product.Application.Application.Features.Brands.Commands.Delete;

public record DeleteBrandCommand(int Id) : IRequest<Result<int>>;


internal class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Result<int>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork<int> _unitOfWork;

    public DeleteBrandCommandHandler(IUnitOfWork<int> unitOfWork, IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;      
    }

    public async Task<Result<int>> Handle(DeleteBrandCommand command, CancellationToken cancellationToken)
    {
        var isBrandUsed = await _productRepository.IsBrandUsed(command.Id);
        if (!isBrandUsed)
        {
            var brand = await _unitOfWork.Repository<Brand>().GetByIdAsync(command.Id);
            if (brand != null)
            {
                await _unitOfWork.Repository<Brand>().DeleteAsync(brand);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBrandsCacheKey);
                return await Result<int>.SuccessAsync(brand.Id, "Brand Deleted");
            }
            else
            {
                return await Result<int>.FailAsync("Brand Not Found!");
            }
        }
        else
        {
            return await Result<int>.FailAsync("Deletion Not Allowed");
        }
    }
}