using Dapper_StoredProcedures.Application.DTOs.Requests;
using Dapper_StoredProcedures.Application.DTOs.Responses;
using Dapper_StoredProcedures.Application.Services.Abtractions;
using Dapper_StoredProcedures.Domain.Entities;
using Dapper_StoredProcedures.Domain.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_StoredProcedures.Application.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> CreateProduct(CreateProductRequest product)
        {
            var exists = await _productRepository.IsProductNameExists(product.Name);
            if (exists)
            {
                throw new InvalidOperationException("Product name already exists.");
            }

            return await _productRepository.InsertProduct(product);
        }



        public Task<IEnumerable<ProductSummaryResponse>> GetProductSummaries(
         int? categoryId = null,
         string? sku = null,
         string sortBy = "SoldDisplay",
         string sortDirection = "DESC")
        {
            return _productRepository.GetProductSummaries(
                categoryId,
                sku,
                sortBy,
                sortDirection
            );
        }

        public Task<IEnumerable<Product>> GetProductsWithCategory()
        {
            return _productRepository.GetProductsWithCategory();
        }
    }
}