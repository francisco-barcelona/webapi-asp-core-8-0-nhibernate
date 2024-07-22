using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Data.Entities;
using MyApp.Data.Repository;
using AutoMapper;
using MyApp.DTOs;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IRepository<Client> _clientRepository;
        private readonly IMapper _mapper;
        public ClientsController(IRepository<Client> clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ClientDto>> Get()
        {
            var clients = _clientRepository.GetAllWithEagerLoading(c => c.Sales);
            var clientDtos = _mapper.Map<IEnumerable<ClientDto>>(clients);
            return Ok(clientDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<ClientDto> Get(int id)
        {
            var client = _clientRepository.GetByIdWithEagerLoading(id, c => c.Sales);
            if (client == null)
            {
                return NotFound();
            }
            var clientDto = _mapper.Map<ClientDto>(client);
            return Ok(clientDto);
        }

        [HttpGet("{id:int}/sales")]
        public IEnumerable<Sales> GetSalesByClient(int id)
        {
            var existingClient = _clientRepository.Get(id);
            if (existingClient != null)
            {
                return _clientRepository.Get(id).Sales;
            }
            else
            {
                var emptySales = new List<Sales>();
                return emptySales;
            }
        }

        //[HttpPost]
        //public IActionResult Post([FromBody] Client client)
        //{
        //    _clientRepository.Add(client);
        //    return Ok();
        //}

        [HttpPost]
        public IActionResult Post([FromBody] ClientDto clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = _mapper.Map<Client>(clientDto);
            _clientRepository.Add(client);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            var existingClient = _clientRepository.Get(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.Name = client.Name;
            _clientRepository.Update(existingClient);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingClient = _clientRepository.Get(id);
            if (existingClient == null)
            {
                return NotFound();
            }
            _clientRepository.Delete(existingClient);
            return Ok();
        }
    }
}
