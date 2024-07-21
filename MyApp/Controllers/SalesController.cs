using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Data.Entities;
using MyApp.Data.Repository;
using MyApp.DTOs;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        public readonly IRepository<Sales> _salesRepository;
        public SalesController(IRepository<Sales> salesRepository)
        {
            _salesRepository = salesRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SalesDto>> Get()
        {
            var sales = _salesRepository.GetAll().Select(t => new SalesDto
            {
                Id = t.Id,
                ClientId = t.Client.Id,
                ProductId = t.Product.Id
            }).ToList();

            return Ok(sales);
        }

        [HttpGet("{id}")]
        public ActionResult<SalesDto> Get(int id)
        {
            var sale = _salesRepository.Get(id);
            return Ok(sale);
        }        

        [HttpPost]
        public IActionResult Post([FromBody] Sales sales)
        {
            if (sales == null) {
                return BadRequest();
            }
            _salesRepository.Add(sales);
            return CreatedAtAction(nameof(Get), new { id = sales.Id }, sales);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Sales sales)
        {
            if (sales == null) {
                return BadRequest();
            }
            var existingSales = _salesRepository.Get(id);
            if (existingSales == null)
            {
                return NotFound();
            }

            existingSales.Client = sales.Client;
            existingSales.Product = sales.Product;
            _salesRepository.Update(existingSales);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingSales = _salesRepository.Get(id);
            if (existingSales == null)
            {
                return NotFound();
            }
            _salesRepository.Delete(existingSales);
            return NoContent();
        }
    }
}
