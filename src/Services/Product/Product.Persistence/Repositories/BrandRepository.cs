using Product.Application.Interfaces.Repositories;
using Product.Domain.Entities;

namespace Product.Persistence.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly IRepositoryAsync<Brand, int> _repository;

    public BrandRepository(IRepositoryAsync<Brand, int> repository)
    {
        _repository = repository;
    }
}