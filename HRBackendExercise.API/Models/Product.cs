using HRBackendExercise.API.Abstractions;

namespace HRBackendExercise.API.Models
{
	public class Product
	{
		public int Id { get; set; }
        public string? Description { get; set; }
        public string? SKU { get; set; }
        public decimal Price { get; set; }
    }
}
