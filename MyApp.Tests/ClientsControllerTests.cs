using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyApp.Controllers;
using MyApp.Data.Entities;
using MyApp.Data.Repository;
using MyApp.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Tests
{
    public class ClientsControllerTests
    {
        private readonly ClientsController _clientController;
        private readonly Mock<IRepository<Client>> _mockRepo;
        private readonly IMapper _mapper;

        public ClientsControllerTests()
        {
            // Setup AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Client, ClientDto>()
                    .ForMember(dest => dest.Sales, opt => opt.MapFrom(src => src.Sales));
                cfg.CreateMap<Sales, SalesDto>();
            });
            _mapper = config.CreateMapper();

            // Initialize mock repository
            _mockRepo = new Mock<IRepository<Client>>();

            // Initialize controller
            _clientController = new ClientsController(_mockRepo.Object, _mapper);
        }

        [Fact]
        public void Get_ReturnsAllClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client { Id = 1, Name = "Client1" },
                new Client { Id = 2, Name = "Client2" }
            };
            _mockRepo.Setup(repo => repo.GetAllWithEagerLoading(It.IsAny<System.Linq.Expressions.Expression<System.Func<Client, object>>[]>())).Returns(clients);

            // Act
            var result = _clientController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnClients = Assert.IsType<List<ClientDto>>(okResult.Value);
            Assert.Equal(2, returnClients.Count);
            Assert.NotEqual(3,returnClients.Count);
        }

        [Fact]
        public void Get_ReturnsClientWithSales()
        {
            // Arrange
            var sales = new List<Sales>
            {
                new Sales { Id = 1, Product = new Product { Id = 1, Name = "Product1", Price = 10 } },
                new Sales { Id = 2, Product = new Product { Id = 2, Name = "Product2", Price = 20 } }
            };

            var client = new Client
            {
                Id = 1,
                Name = "Client1",
                Sales = sales
            };

            _mockRepo.Setup(repo => repo.GetByIdWithEagerLoading(It.IsAny<int>(), It.IsAny<Expression<Func<Client, object>>>()))
                     .Returns(client);

            // Act
            var result = _clientController.Get(1);
            var okResult = result.Result as OkObjectResult;
            var clientDto = okResult?.Value as ClientDto;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(clientDto);
            Assert.Equal(client.Id, clientDto.Id);
            Assert.Equal(client.Name, clientDto.Name);
            Assert.Equal(client.Sales.Count, clientDto.Sales.Count());
        }


    }
}
