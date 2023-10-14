using Microsoft.EntityFrameworkCore;
using Product.Application.Interfaces.Repositories;

namespace Product.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IRepositoryAsync<Domain.Entities.Product, int> _repository;

    public ProductRepository(IRepositoryAsync<Domain.Entities.Product, int> repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsBrandUsed(int brandId)
    {
        return await _repository.Entities.AnyAsync(b => b.BrandId == brandId);
    }
}
