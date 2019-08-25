using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webshop.Data;
using webshop.Models;

namespace webshop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDetails>>> GetProductDetails()
        {
            return await _context.ProductDetails.ToListAsync();
        }

        // GET: api/ProductDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetails>> GetProductDetails(int id)
        {
           var product =  await _context.Products.FindAsync(id);

            ProductDetails productDetails = new ProductDetails(product.Id, product.Name, product.Price,product.Description);
            
            if (productDetails == null)
            {
                return NotFound();
            }

            return productDetails;
        }

        // PUT: api/ProductDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductDetails(int id, ProductDetails productDetails)
        {
            if (id != productDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(productDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductDetails
        [HttpPost]
        public async Task<ActionResult<ProductDetails>> PostProductDetails(ProductDetails productDetails)
        {
            _context.ProductDetails.Add(productDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductDetails", new { id = productDetails.Id }, productDetails);
        }

        // DELETE: api/ProductDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDetails>> DeleteProductDetails(int id)
        {
            var productDetails = await _context.ProductDetails.FindAsync(id);
            if (productDetails == null)
            {
                return NotFound();
            }

            _context.ProductDetails.Remove(productDetails);
            await _context.SaveChangesAsync();

            return productDetails;
        }

        private bool ProductDetailsExists(int id)
        {
            return _context.ProductDetails.Any(e => e.Id == id);
        }
    }
}
