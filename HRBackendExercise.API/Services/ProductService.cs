using HRBackendExercise.API.Abstractions;
using HRBackendExercise.API.Models;

namespace HRBackendExercise.API.Services
{
	public class ProductService : IProductsService
	{
        private readonly List<Product> _products = new();
        private int _nextId = 1;

        public Product Create(Product product)
		{
            if (product.Price < 0)
            {
                throw new ArgumentException("Price must be greater than 0");
            }

            product.Id = _nextId++;
            _products.Add(product);
            return product;
        }

		public Product? GetById(int id)
		{
            var product = _products.FirstOrDefault(p => p.Id == id);
           
            return product;
        }

		public IEnumerable<Product> GetAll()
		{
            return _products;
		}

		public void Update(Product product)
		{
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Invalid entity. Product not found");
            }

            if (product.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than 0");
            }

            existingProduct.Description = product.Description;
            existingProduct.SKU = product.SKU;
            existingProduct.Price = product.Price;
        }

		public void Delete(Product product)
		{
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                _products.Remove(product);
            }
        }
	}
}