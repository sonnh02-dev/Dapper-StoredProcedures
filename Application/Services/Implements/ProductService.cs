using Dapper_StoredProcedures.Application.Services.Abtractions;
using Dapper_StoredProcedures.Application.Services.Abtractions.Abtractions;
using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Domain.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Application.Services.Implements.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateProduct(Product product)
        {
            var exists = await _repository.IsProductNameExists(product.Name);
            if (exists)
            {
                throw new InvalidOperationException("Product name already exists.");
            }

            return await _repository.InsertProduct(product);
        }

      

        public Task<IEnumerable<Product>> GetProductsWithCategoryAndSold()
        {
            return _repository.GetProductsWithCategoryAndSold();
        }
    }
}
