using HRBackendExercise.API.Models;
using HRBackendExercise.API.Services;
using Moq;
using Newtonsoft.Json;

namespace HRBackendExercise.API.Tests.Basic
{
	public class Service_CreateShould
	{
		[Fact(DisplayName = "ServiceCreateShouldReturnNotNull")]
		public void ReturnNotNull()
		{
			// Arrange
			var dummyProduct = new Product();
			var sut = new ProductService();

			// Act
			var actual = sut.Create(dummyProduct);

			// Assert
			Assert.NotNull(actual);
		}
	}

	public class Service_GetShould
	{
		[Fact(DisplayName = "ServiceGetShouldReturnNotNull")]
		public void ReturnNotNull_OnValidId()
		{
			// Arrange
			var sut = new ProductService();

			// Act
			var product = sut.Create(new Product());
			var actual = sut.GetById(id: product.Id);

			// Assert
			Assert.NotNull(actual);
		}
	}

	public class Service_GetAllShould
	{
		[Fact(DisplayName = "ServiceGetAllShouldReturnEmptyCollection")]
		public void RetrunEmpty()
		{
			// Arrange
			var sut = new ProductService();

			// Act
			var actual = sut.GetAll();

			// Assert
			Assert.Empty(actual);
		}

		[Fact(DisplayName = "ServiceGetAllShouldAllEntries")]
		public void ReturnAllEntries()
		{
			// Arrange
			var sut = new ProductService();

			// Act
			var createdElements = new List<Product> {
				sut.Create(new Product()),
				sut.Create(new Product()),
				sut.Create(new Product())
			};
			var actual = sut.GetAll();

			// Assert
			Assert.NotEmpty(actual);
			Assert.Equal(expected: 3, actual.Count());
			Assert.All(createdElements, x => Assert.Contains(x, actual));
		}
	}

	public class Service_UpdateShould
	{
		[Fact(DisplayName = "ServiceUpdateShouldUpdateValues")]
		public void UpdateValues()
		{
			// Arrange
			var sut = new ProductService();
			var dummyProduct = new Product
			{
				SKU = "1234567890",
				Description = "This is my description",
				Price = 0.99m
			};

			// Act
			var product = sut.Create(dummyProduct);
			var clonedProduct = JsonConvert.DeserializeObject<Product>(JsonConvert.SerializeObject(product));
			clonedProduct.SKU = "0000000000";
			clonedProduct.Price = 123m;
			clonedProduct.Description = "This is my updated description";
			sut.Update(clonedProduct);
			var actual = sut.GetById(product.Id);

			// Assert
			Assert.NotNull(actual);
			Assert.Equal(expected: clonedProduct.SKU, actual.SKU);
			Assert.Equal(expected: clonedProduct.Price, actual.Price);
			Assert.Equal(expected: clonedProduct.Description, actual.Description);
		}

		[Fact(DisplayName = "ServiceUpdateShouldThrowException")]
		public void ThrowException_OnInvalidId()
		{
			// Arrange
			var sut = new ProductService();
			var dummyProduct = new Product { Id = 500 };

			// Act / Assert
			var exception = Assert.ThrowsAny<Exception>(() => sut.Update(dummyProduct));
			Assert.StartsWith("Invalid entity.", exception.Message);
		}
	}

	public class Service_DeleteShould
	{
		[Fact(DisplayName = "ServiceDeleteShouldRemoveProudct")]
		public void RemoveProduct()
		{
			// Arrange
			var sut = new ProductService();

			// Act
			var productToRemove = sut.Create(new Product());
			sut.Delete(productToRemove);
			var products = sut.GetAll();

			// Assert
			Assert.Empty(products);
		}

		[Fact(DisplayName = "ServiceDeleteShouldDoNothing")]
		public void DoNothing_OnCollectionNotContainsProduct()
		{
			// Arrange
			var invalidProduct = new Product();
			var sut = new ProductService();

			// Act
			var validProduct = sut.Create(new Product());
			sut.Delete(invalidProduct);
			var products = sut.GetAll();

			// Assert
			Assert.Single(products);
			Assert.Contains(validProduct, products);
		}
	}
}
