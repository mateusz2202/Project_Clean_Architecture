using Product.Domain.Entities;

namespace Product.Application.Specifications;

public class BrandFilterSpecification : HeroSpecification<Brand>
{
    public BrandFilterSpecification(string searchString)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
        }
        else
        {
            Criteria = p => true;
        }
    }
}
