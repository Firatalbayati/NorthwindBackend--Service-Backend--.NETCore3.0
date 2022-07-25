using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get Product List
        /// </summary>
        //[Authorize(Roles = "Admin")]
        //[Authorize(Roles = "Product.List")]
        [HttpGet(template: "getlist")]
        public IActionResult GetList()
        {
            var result = _productService.GetList();

            if (result.Success)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// Get Product List By Category Id
        /// </summary>
        [HttpGet(template: "getlistbycategory")]
        public IActionResult GetListByCategory(int categoryId)
        {
            var result = _productService.GetListByCategory(categoryId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Get Product By Id
        /// </summary>
        [HttpGet(template: "getbyid")]
        public IActionResult GetById(int productId)
        {
            var result = _productService.GetById(productId);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Add Product
        /// </summary>
        [HttpPost(template: "add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product); 

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        [HttpPost(template: "delete")]
        public IActionResult Delete(Product product)
        {
            var result = _productService.Delete(product);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Update Product
        /// </summary>
        [HttpPost(template: "update")]
        public IActionResult Update(Product product)
        {
            var result = _productService.Update(product);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}