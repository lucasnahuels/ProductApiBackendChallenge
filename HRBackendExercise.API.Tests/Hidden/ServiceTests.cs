using HRBackendExercise.API.Models;
using HRBackendExercise.API.Services;
using Moq;

namespace HRBackendExercise.API.Tests.Hidden
{
	public class ServiceTests
	{
		[Fact(DisplayName = "ServiceCreateShouldAssignIncrementalIds")]
		public void Create_ShouldAssignIncrementalIds()
		{
			// Arrange
			var sut = new ProductService();

			// Act
			var product1 = sut.Create(new Product());
			var product2 = sut.Create(new Product());
			var product3 = sut.Create(new Product());

			// Assert
			Assert.Equal(expected: 1, product1.Id);
			Assert.Equal(expected: 2, product2.Id);
			Assert.Equal(expected: 3, product3.Id);
		}

		[Fact(DisplayName = "ServiceCreateShouldIgnoreInputId")]
		public void Create_ShouldIgnoreInputModelId()
		{
			// Arrange
			var dummyId = 500;
			var dummyProduct = new Product { Id = dummyId };
			var sut = new ProductService();

			// Act
			var actual = sut.Create(dummyProduct);

			// Assert
			Assert.NotEqual(expected: dummyId, actual.Id);
		}

		[Fact(DisplayName = "ServiceCreateShouldCreateARecord")]
		public void Create_ShouldAddARecord()
		{
			// Arrange
			var dummyProduct = new Product();
			var sut = new ProductService();

			// Act
			var product = sut.Create(dummyProduct);
			var entities = sut.GetAll();

			// Assert
			Assert.NotEmpty(entities);
			Assert.Contains(product, entities);
		}

		[Fact(DisplayName = "ServiceGetShouldReturnNullOnEmptyCollection")]
		public void Get_ShouldReturnNull_OnEmptyCollection()
		{
			// Arrange
			var sut = new ProductService();

			// Act
			var actual = sut.GetById(It.IsAny<int>());

			// Assert
			Assert.Null(actual);
		}

		[Fact(DisplayName = "ServiceGetShouldReturnNullOnInvalidId")]
		public void Get_ShouldReturnNull_OnInvalidId()
		{
			// Arrange
			var sut = new ProductService();

			// Act
			var product = sut.Create(new Product());
			var actual = sut.GetById(id: product.Id + 1);

			// Assert
			Assert.Null(actual);
		}

		[Fact(DisplayName = "ServiceUpdateShouldThrowArgumentBasedException")]
		public void Update_ShouldThrowArgumentBasedException_OnInvalidId()
		{
			// Arrange
			var sut = new ProductService();
			var dummyProduct = new Product { Id = 500 };

			// Act / Assert
			var exception = Assert.ThrowsAny<Exception>(() => sut.Update(dummyProduct));
			Assert.StartsWith("Invalid entity.", exception.Message);
		}
	}
}
