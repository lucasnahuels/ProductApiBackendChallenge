using HRBackendExercise.API.Abstractions;
using HRBackendExercise.API.Controllers;
using HRBackendExercise.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HRBackendExercise.API.Tests.Hidden
{
	public class ControllerTests
	{
		[Fact(DisplayName = "ControllerGetShouldImplementService")]
		public void Get_ShouldCallGetByIdFromService()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(new Product());
			int dummyId = 1;
			var sut = GetServiceUnderTest(productsServiceMock.Object);

			// Act
			var result = sut.Get(dummyId);

			// Assert
			productsServiceMock.Verify(x => x.GetById(dummyId), Times.Once);
		}

		[Fact(DisplayName = "ControllerGetInternalServerErrorShouldHaveMessageProp")]
		public void Get_ResponseShouldHasMessageProperty_OnInternalServerErrorScenario()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Throws(new Exception("This is my dummy exception"));
			int dummyId = 1;
			var sut = GetServiceUnderTest(productsServiceMock.Object);

			// Act
			var result = sut.Get(dummyId);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.NotNull(objectResult?.Value);
			var properties = objectResult.Value.GetType().GetProperties();
			Assert.Single(properties);
			Assert.Equal(expected: "message", properties.FirstOrDefault()?.Name);
		}

		[Fact(DisplayName = "ControllerGetShouldBeHttpDecorated")]
		public void Get_ShouldHasAccordinglyDecorator()
		{
			var attributes = typeof(ProductsController)
				.GetMethod(nameof(ProductsController.Get))
				?.CustomAttributes;

			Assert.NotNull(attributes);
			Assert.Contains(attributes, x => x.AttributeType == typeof(HttpGetAttribute));
		}

		[Fact(DisplayName = "ControllerGetShouldReturnNotFound")]
		public void Get_ShouldReturnNotFound_OnNullProduct()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns((Product?)null);
			int dummyId = 1;
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Get(dummyId);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact(DisplayName = "ControllerGetShouldReturnInternalServerError")]
		public void Get_ShouldReturnInternalServerError_OnUnhandledException()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Throws(new Exception("This is my dummy exception"));
			int dummyId = 1;
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Get(dummyId);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(expected: 500, objectResult.StatusCode);
			Assert.NotNull(objectResult.Value);
		}

		[Fact(DisplayName = "ControllerPostShouldImplementService")]
		public void Post_ShouldCallCreateFromService()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.Create(It.IsAny<Product>()))
				.Returns(new Product());
			var dummyProduct = new Product { SKU = "Something", Price = 19m };
			var sut = GetServiceUnderTest(productsServiceMock.Object);

			// Act
			var result = sut.Post(dummyProduct);

			// Assert
			productsServiceMock.Verify(x => x.Create(dummyProduct), Times.Once);
		}

		[Fact(DisplayName = "ControllerPostInternalServerErrorShouldHaveMessageProp")]
		public void Post_ResponseShouldHasMessageProperty_OnInternalServerErrorScenario()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.Create(It.IsAny<Product>()))
				.Throws(new Exception("This is my dummy exception"));
			Product dummyProduct = new Product { SKU = "Something", Price = 100m };
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Post(dummyProduct);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.NotNull(objectResult?.Value);
			var properties = objectResult.Value.GetType().GetProperties();
			Assert.Single(properties);
			Assert.Equal(expected: "message", properties.FirstOrDefault()?.Name);
		}

		[Fact(DisplayName = "ControllerPostShouldBeHttpDecorated")]
		public void Post_ShouldHasAccordinglyDecorator()
		{
			var attributes = typeof(ProductsController)
				.GetMethod(nameof(ProductsController.Post))
				?.CustomAttributes;

			Assert.NotNull(attributes);
			Assert.Contains(attributes, x => x.AttributeType == typeof(HttpPostAttribute));
		}

		[Fact(DisplayName = "ControllerPostShouldReturnCreated")]
		public void Post_ShouldReturnCreatedStatusCode()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.Create(It.IsAny<Product>()))
				.Returns(new Product());
			Product dummyProduct = new Product { SKU = "Something", Price = 100m };
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Post(dummyProduct);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(expected: 201, objectResult.StatusCode);
		}

		[Fact(DisplayName = "ControllerPostShouldReturnInternalServerError")]
		public void Post_ShouldReturnInternalServerError_OnUnhandledException()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.Create(It.IsAny<Product>()))
				.Throws(new Exception("This is my dummy exception"));
			Product dummyProduct = new Product { SKU = "Something", Price = 100m };
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Post(dummyProduct);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(expected: 500, objectResult.StatusCode);
			Assert.NotNull(objectResult.Value);
		}

		[Fact(DisplayName = "ControllerPostShouldReturnBadRequestOnNullSKU")]
		public void Post_ShouldReturnBadRequest_OnNullSKU()
		{
			// Arrange
			var dummyProduct = new Product { SKU = null };
			var productsServiceMock = new Mock<IProductsService>();
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Post(dummyProduct);

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}

		[Fact(DisplayName = "ControllerPostShouldReturnBadRequestOnZeroPrice")]
		public void Post_ShouldReturnBadRequest_OnPriceEqualsToZero()
		{
			// Arrange
			var dummyProduct = new Product { SKU = "Something", Price = 0m };
			var productsServiceMock = new Mock<IProductsService>();
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Post(dummyProduct);

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}

		[Fact(DisplayName = "ControllerPutShouldImplementService")]
		public void Put_ShouldCallUpdateFromService()
		{
			// Arrange
			var dummyProduct = new Product { Id = 12, SKU = "Something", Price = 200m };
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(dummyProduct.Id))
				.Returns(dummyProduct);
			var sut = GetServiceUnderTest(productsServiceMock.Object);

			// Act
			var result = sut.Put(dummyProduct);

			// Assert
			productsServiceMock.Verify(x => x.GetById(dummyProduct.Id), Times.Once);
			productsServiceMock.Verify(x => x.Update(dummyProduct), Times.Once);
		}

		[Fact(DisplayName = "ControllerPutInternalServerErrorShouldHaveMessageProp")]
		public void Put_ResponseShouldHasMessageProperty_OnInternalServerErrorScenario()
		{
			// Arrange
			var dummyProduct = new Product { Id = 13, SKU = "Something", Price = 200m };
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Throws(new Exception("This is my dummy exception."));
			productsServiceMock
				.Setup(x => x.Update(It.IsAny<Product>()))
				.Throws(new Exception("This is my dummy exception."));
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Put(dummyProduct);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.NotNull(objectResult?.Value);
			var properties = objectResult.Value.GetType().GetProperties();
			Assert.Single(properties);
			Assert.Equal(expected: "message", properties.FirstOrDefault()?.Name);
		}

		[Fact(DisplayName = "ControllerPutShouldBeHttpDecorated")]
		public void Put_ShouldHasAccordinglyDecorator()
		{
			var attributes = typeof(ProductsController)
				.GetMethod(nameof(ProductsController.Put))
				?.CustomAttributes;

			Assert.NotNull(attributes);
			Assert.Contains(attributes, x => x.AttributeType == typeof(HttpPutAttribute));
		}

		[Fact(DisplayName = "ControllerPutShouldReturnNoContent")]
		public void Put_ShouldReturnNoContent()
		{
			// Arrange
			var dummyProduct = new Product { Id = 12, SKU = "Something", Price = 200m };
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(dummyProduct.Id))
				.Returns(dummyProduct);
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Put(dummyProduct) as StatusCodeResult;

			// Assert
			Assert.NotNull(result);
			Assert.Equal(expected: 204, result.StatusCode);
		}

		[Fact(DisplayName = "ControllerPutShouldReturnNotFound")]
		public void Put_ShouldReturnNotFound_OnNullProduct()
		{
			// Arrange
			var dummyProduct = new Product { Id = 13, SKU = "Something", Price = 200m };
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns((Product?)null);
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Put(dummyProduct);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact(DisplayName = "ControllerPutShouldReturnInternalServerError")]
		public void Put_ShouldReturnInternalServerError_OnUnhandledException()
		{
			// Arrange
			var dummyProduct = new Product { Id = 13, SKU = "Something", Price = 200m };
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Throws(new Exception("This is my dummy exception."));
			productsServiceMock
				.Setup(x => x.Update(It.IsAny<Product>()))
				.Throws(new Exception("This is my dummy exception."));
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Put(dummyProduct);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(expected: 500, objectResult.StatusCode);
			Assert.NotNull(objectResult.Value);
		}

		[Fact(DisplayName = "ControllerPutShouldReturnBadRequestOnNullSKU")]
		public void Put_ShouldReturnBadRequest_OnNullSKU()
		{
			// Arrange
			var dummyProduct = new Product { Id = 12, SKU = null, Price = 200m };
			var productsServiceMock = new Mock<IProductsService>();
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Put(dummyProduct);

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}

		[Fact(DisplayName = "ControllerPutShouldReturnBadRequestOnZeroPrice")]
		public void Put_ShouldReturnBadRequest_OnPriceEqualsToZero()
		{
			// Arrange
			var dummyProduct = new Product { Id = 12, SKU = "Something", Price = 0m };
			var productsServiceMock = new Mock<IProductsService>();
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Put(dummyProduct);

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}

		[Fact(DisplayName = "ControllerPutShouldReturnBadRequestOnZeroId")]
		public void Put_ShouldReturnBadRequest_OnIdEqualsToZero()
		{
			// Arrange
			var dummyProduct = new Product { Id = 0, SKU = "Something", Price = 200m };
			var productsServiceMock = new Mock<IProductsService>();
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Put(dummyProduct);

			// Assert
			Assert.IsType<BadRequestResult>(result);
		}

		[Fact(DisplayName = "ControllerDeleteShouldImplementService")]
		public void Delete_ShouldCallDeleteFromService()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			var dummyProduct = new Product();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(dummyProduct);
			int dummyId = 80;
			var sut = GetServiceUnderTest(productsServiceMock.Object);

			// Act
			var result = sut.Delete(dummyId);

			// Assert
			productsServiceMock.Verify(x => x.GetById(dummyId), Times.Once);
			productsServiceMock.Verify(x => x.Delete(dummyProduct), Times.Once);
		}

		[Fact(DisplayName = "ControllerDeleteInternalServerErrorShouldHaveMessageProp")]
		public void Delete_ResponseShouldHasMessageProperty_OnInternalServerErrorScenario()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Throws(new Exception("This is my dummy exception"));
			productsServiceMock
				.Setup(x => x.Delete(It.IsAny<Product>()))
				.Throws(new Exception("This is my dummy exception"));
			int dummyId = 55;
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Delete(dummyId);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.NotNull(objectResult?.Value);
			var properties = objectResult.Value.GetType().GetProperties();
			Assert.Single(properties);
			Assert.Equal(expected: "message", properties.FirstOrDefault()?.Name);
		}

		[Fact(DisplayName = "ControllerDeleteShouldBeHttpDecorated")]
		public void Delete_ShouldHasAccordinglyDecorator()
		{
			var attributes = typeof(ProductsController)
				.GetMethod(nameof(ProductsController.Delete))
				?.CustomAttributes;

			Assert.NotNull(attributes);
			Assert.Contains(attributes, x => x.AttributeType == typeof(HttpDeleteAttribute));
		}

		[Fact(DisplayName = "ControllerDeleteShouldReturnNoContent")]
		public void Delete_ShouldReturnNoContent()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns(new Product());
			int dummyId = 80;
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Delete(dummyId) as StatusCodeResult;

			// Assert
			Assert.NotNull(result);
			Assert.Equal(expected: 204, result.StatusCode);
		}

		[Fact(DisplayName = "ControllerDeleteShouldReturnNotFound")]
		public void Delete_ShouldReturnNotFound_OnNullProduct()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Returns((Product?)null);
			productsServiceMock
				.Setup(x => x.Delete(It.IsAny<Product>()))
				.Throws(new Exception("This is my dummy exception"));
			int dummyId = 60;
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Delete(dummyId);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact(DisplayName = "ControllerDeleteShouldReturnInternalServerError")]
		public void Delete_ShouldReturnInternalServerError_OnUnhandledException()
		{
			// Arrange
			var productsServiceMock = new Mock<IProductsService>();
			productsServiceMock
				.Setup(x => x.GetById(It.IsAny<int>()))
				.Throws(new Exception("This is my dummy exception"));
			productsServiceMock
				.Setup(x => x.Delete(It.IsAny<Product>()))
				.Throws(new Exception("This is my dummy exception"));
			int dummyId = 55;
			var sut = new ProductsController(productsServiceMock.Object);

			// Act
			var result = sut.Delete(dummyId);

			// Assert
			var objectResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(expected: 500, objectResult.StatusCode);
			Assert.NotNull(objectResult.Value);
		}

		private ProductsController GetServiceUnderTest(IProductsService productsService)
		{
			return new ProductsController(productsService);
		}
	}
}
