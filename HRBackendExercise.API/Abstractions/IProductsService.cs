using HRBackendExercise.API.Models;

namespace HRBackendExercise.API.Abstractions
{
	public interface IProductsService
	{
		Product? GetById(int id);
		IEnumerable<Product> GetAll();
		Product Create(Product entity);
		void Update(Product entity);
		void Delete(Product entity);
	}
}
