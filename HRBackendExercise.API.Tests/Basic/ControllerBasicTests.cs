using HRBackendExercise.API.Abstractions;
using HRBackendExercise.API.Controllers;
using HRBackendExercise.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HRBackendExercise.API.Tests.Basic
{
	public class Controller_GetShould
	{
		[Fact(DisplayName = "ControllerGetShouldReturnOk")]
		public void ReturnOk_OnNotNullProduct()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(new Product());
			int dummyId = 1;
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Get(dummyId) as ObjectResult;

			// Assert
			Assert.NotNull(result);
			Assert.Equal(expected: 200, result.StatusCode);
			Assert.NotNull(result.Value);
		}
	}

	public class Controller_PostShould
	{
		[Fact(DisplayName = "ControllerPostShouldReturnSuccessStatusCode")]
		public void ReturnSuccessStatusCode()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.Create(It.IsAny<Product>()))
				.Returns(new Product());
			Product dummyProduct = new Product { SKU = "Something", Price = 100m };
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Post(dummyProduct) as ObjectResult;

			// Assert
			Assert.NotNull(result);
			Assert.InRange(result.StatusCode ?? 0, low: 200, high: 299);
			Assert.NotNull(result.Value);
		}

		[Fact(DisplayName = "ControllerPostShouldReturnBadRequestOnNullInput")]
		public void ReturnBadRequest_OnNullInputProduct()
		{
			// Arrange
			Product? dummyProduct = null;
			var productsServiceMock = new Mock<IProductsService>();
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Post(dummyProduct);

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}
	}

	public class Controller_PutShould
	{
		[Fact(DisplayName = "ControllerPutShouldReturnSuccessStatusCode")]
		public void ReturnSuccessStatusCode()
		{
			// Arrange
			var dummyProduct = new Product { Id = 12, SKU = "Something", Price = 200m };
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(dummyProduct.Id))
				.Returns(dummyProduct);
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Put(dummyProduct) as ActionResult;


			// Assert
			// This is a bad practice but is implemented in this way to cover StatusCode() and Ok() calls
			if (result is StatusCodeResult statusCodeResult)
			{
				Assert.InRange(statusCodeResult.StatusCode, low: 200, high: 299);
			}
			else if (result is ObjectResult objectResult)
			{
				Assert.InRange(objectResult?.StatusCode ?? 0, low: 200, high: 299);
			}
			else
			{
				Assert.Fail("Unexpected output.");
			}
		}

		[Fact(DisplayName = "ControllerPutShouldReturnBadRequestOnNullProduct")]
		public void ReturnBadRequest_OnNullInputProduct()
		{
			// Arrange
			Product? dummyProduct = null;
			var productsServiceMock = new Mock<IProductsService>();
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Put(dummyProduct);

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}
	}

	public class Controller_DeleteShould
	{
		[Fact(DisplayName = "ControllerDeleteShouldReturnSuccessStatusCode")]
		public void ReturnSuccessStatusCode()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(new Product());
			int dummyId = 80;
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Delete(dummyId) as ActionResult;

			// Assert
			// This is a bad practice but is implemented in this way to cover StatusCode() and Ok() calls
			if (result is StatusCodeResult statusCodeResult)
			{
				Assert.InRange(statusCodeResult.StatusCode, low: 200, high: 299);
			}
			else if (result is ObjectResult objectResult)
			{
				Assert.InRange(objectResult?.StatusCode ?? 0, low: 200, high: 299);
			}
			else
			{
				Assert.Fail("Unexpected output.");
			}
		}
	}
}
