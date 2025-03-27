using HRBackendExercise.API.Abstractions;
using HRBackendExercise.API.Models;
using HRBackendExercise.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace HRBackendExercise.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
        private readonly IProductsService _productService;

        public ProductsController(IProductsService productsService)
		{
            _productService = productsService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
		{
            try
            {
                var product = _productService.GetById(id);
                if (product == null)
                {
                    return NotFound();
                }   
                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Product not found" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the product" });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
		{
            if (product == null)
            {
                return BadRequest();
            }

            try
            {
                var createdProduct = _productService.Create(product);
                if(createdProduct == null)
                {
                    return BadRequest();
                }
                return new ObjectResult(createdProduct) { StatusCode = 201 };
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while creating the product" });
            }
        }


        [HttpPut]
        public IActionResult Put([FromBody] Product product)
		{
            if (product == null || product.Id == 0 || product.SKU == null || product.Price <= 0)
            {
                return BadRequest();
            }

            try
            {
                var existingProduct = _productService.GetById(product.Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                _productService.Update(product);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while updating the product" });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
		{
            try
            {
                var product = _productService.GetById(id);
                if (product == null)
                {
                    return NotFound();
                }
                _productService.Delete(product);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Product not found" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the product" });
            }
        }
	}
}