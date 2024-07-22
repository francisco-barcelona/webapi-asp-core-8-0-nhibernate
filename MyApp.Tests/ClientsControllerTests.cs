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

            /*  
             Explanation of the last line:

            The line _mockRepo.Setup(repo => repo.GetAllWithEagerLoading(It.IsAny<System.Linq.Expressions.Expression<System.Func<Client, object>>[]>())).Returns(clients); is configuring a mock repository to return a predefined list of clients when the GetAllWithEagerLoading method is called. Let's break it down step by step.

            Explanation of the Line
            1. _mockRepo.Setup(...)
            _mockRepo is a mock object created using the Moq framework. The Setup method is used to specify the behavior of the mock object for a particular method call. In this case, we are setting up the behavior for the GetAllWithEagerLoading method.

            2. repo => repo.GetAllWithEagerLoading(...)
            This part is a lambda expression representing the method we want to configure. repo is a placeholder for the mock repository, and repo.GetAllWithEagerLoading(...) specifies the method we are setting up.

            3. It.IsAny<System.Linq.Expressions.Expression<System.Func<Client, object>>[]>())
            It.IsAny<T>() is a Moq method that matches any value of the specified type. Here, System.Linq.Expressions.Expression<System.Func<Client, object>>[] represents an array of expressions that are used to include related entities for eager loading. By using It.IsAny<>, we are saying that this setup should apply regardless of what expressions are passed to the GetAllWithEagerLoading method.

            4. .Returns(clients);
            This specifies the return value for the GetAllWithEagerLoading method when it is called on the mock object. In this case, it returns the predefined list clients.

            Why is this Necessary?
            When testing, you often need to isolate the unit of code being tested from its dependencies. In this case, the ClientsController depends on an IRepository<Client>. By using a mock repository, you can control the behavior of this dependency, making it easier to test the controller in isolation.

            Example in Context
            Here is the full context of how this line is used in the test:

            [Fact]
            public void Get_ReturnsAllClients()
            {
                // Arrange
                var clients = new List<Client>
                {
                    new Client { Id = 1, Name = "Client1" },
                    new Client { Id = 2, Name = "Client2" }
                };

                // Setting up the mock repository to return the clients list when GetAllWithEagerLoading is called
                _mockRepo.Setup(repo => repo.GetAllWithEagerLoading(It.IsAny<System.Linq.Expressions.Expression<System.Func<Client, object>>[]>())).Returns(clients);

                // Act
                var result = _controller.Get();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnClients = Assert.IsType<List<ClientDto>>(okResult.Value);
                Assert.Equal(2, returnClients.Count);
            }

            In this example:

            Arrange: We create a list of clients and set up the mock repository.
            Act: We call the Get method on the ClientsController.
            Assert: We check that the result is an OkObjectResult, and that the returned list of clients has the expected count.
            By setting up the mock repository to return the predefined list, we ensure that the test is predictable and not dependent on any actual database operations.

            Summary
            The line _mockRepo.Setup(repo => repo.GetAllWithEagerLoading(It.IsAny<System.Linq.Expressions.Expression<System.Func<Client, object>>[]>())).Returns(clients); is a way to configure the mock repository to return a specific list of clients when the GetAllWithEagerLoading method is called, regardless of the arguments passed to it. This is useful for isolating the controller logic from the data access layer during testing.
             

            Let's break down the specific part It.IsAny<System.Linq.Expressions.Expression<System.Func<Client, object>>[]>()) in more detail:

            Breaking Down the Expression
            1. It.IsAny<T>()
            Purpose: It.IsAny<T>() is a method provided by the Moq framework. It is used to match any argument of the specified type T in the setup configuration.
            Usage: When setting up a mock method, It.IsAny<T>() tells Moq to accept any value of type T as an argument for that method. This is useful when the exact value of the argument isn't important for the test, or when you want to match calls regardless of the argument's value.
            2. System.Linq.Expressions.Expression<System.Func<Client, object>>
            System.Linq.Expressions.Expression<TDelegate>: This represents a strongly typed lambda expression as data in the form of an expression tree. It allows the lambda expression to be parsed, examined, and modified at runtime.
            System.Func<Client, object>: This is a delegate type that represents a method that takes a Client object as an input and returns an object. This delegate is commonly used for defining lambda expressions that access properties of the Client class.
            3. Combining Expression and Func
            Expression<Func<Client, object>>: This is a lambda expression that takes a Client as input and returns an object. It is used to define how to access a property or navigate a relationship from the Client entity.
            4. Array of Expressions
            Expression<Func<Client, object>>[]: This is an array of lambda expressions. Each expression in the array specifies a navigation property (or related entity) to include in the query for eager loading. Eager loading means loading related entities as part of the initial query to avoid lazy loading exceptions.
            Putting It All Together
            It.IsAny<Expression<Func<Client, object>>[]>())
            Purpose: This part of the setup tells Moq to match any array of lambda expressions that specify which related entities of Client should be eagerly loaded.
            Usage in Setup: By using It.IsAny<Expression<Func<Client, object>>[]>()), you are configuring the mock repository method GetAllWithEagerLoading to respond correctly regardless of which properties or relationships are specified for eager loading.
            Example in Context
            Here is the complete setup line with more context:

            _mockRepo.Setup(repo => repo.GetAllWithEagerLoading(It.IsAny<Expression<Func<Client, object>>[]>())).Returns(clients);
            _mockRepo.Setup(repo => repo.GetAllWithEagerLoading(...)): Configures the mock repository's GetAllWithEagerLoading method.
            It.IsAny<Expression<Func<Client, object>>[]>()): Specifies that this setup should match any call to GetAllWithEagerLoading that passes any array of expressions for eager loading.
            .Returns(clients);
            
            Specifies that when this method is called with any array of expressions, the mock repository should return the predefined list of clients.

            Summary
            The part It.IsAny<Expression<Func<Client, object>>[]>()) in the setup configuration for the mock repository tells Moq to accept any array of lambda expressions that specify which properties or related entities of Client should be eagerly loaded. This makes the setup flexible and ensures that the GetAllWithEagerLoading method on the mock repository will return the predefined list of clients regardless of the specific properties requested for eager loading.

            Purpose of Eager Loading
            Eager loading is used to retrieve related entities as part of the initial database query. This can help avoid the LazyInitializationException that occurs when related entities are accessed outside the context of an active NHibernate session.

            Method to Retrieve Client with Sales
            You have a method GetAllWithEagerLoading that can include related entities (like Sales) when retrieving data. Here’s how it can be used to retrieve a Client with their Sales:

            Repository Method Implementation
            Here is the repository method that allows eager loading of related entities:

            public IEnumerable<T> GetAllWithEagerLoading(params Expression<Func<T, object>>[] includeProperties)
            {
                using var session = _sessionFactory.OpenSession();
                var query = session.Query<T>();

                foreach (var includeProperty in includeProperties)
                {
                    query = query.Fetch(includeProperty);
                }

                return query.ToList();
            }

            Retrieving a Client with Sales in the Controller
            In your ClientsController, you can use this method to retrieve a Client along with their Sales:
                        
            [HttpGet("{id}")]
            public ActionResult<ClientDto> Get(int id)
            {
                var client = _clientRepository.GetAllWithEagerLoading(c => c.Sales)
                                              .FirstOrDefault(c => c.Id == id);
                if (client == null)
                {
                    return NotFound();
                }

                var clientDto = _mapper.Map<ClientDto>(client);
                return Ok(clientDto);
            }

            Explanation of the Code
            Eager Loading with Fetch: query = query.Fetch(includeProperty);
            This line ensures that the specified related entities (in this case, Sales) are loaded as part of the initial query.
            Using Expression for Eager Loading: c => c.Sales
            This lambda expression specifies that the Sales collection should be included when loading Client entities.
            Filtering the Client by Id: .FirstOrDefault(c => c.Id == id);
            After including the related entities, this part filters the Client entities to find the one with the specified Id.
            Mapping to DTO: _mapper.Map<ClientDto>(client)
            This line maps the Client entity to a ClientDto for the API response.
            Complete Example of the Controller Method
            Here is the complete method in your controller with comments:

            [HttpGet("{id}")]
            public ActionResult<ClientDto> Get(int id)
            {
                // Fetch the client with their sales included
                var client = _clientRepository.GetAllWithEagerLoading(c => c.Sales)
                                              .FirstOrDefault(c => c.Id == id);
    
                // Check if the client is not found
                if (client == null)
                {
                    return NotFound();
                }

                // Map the client entity to a DTO
                var clientDto = _mapper.Map<ClientDto>(client);
    
                // Return the DTO
                return Ok(clientDto);
            }

            Summary
            Eager Loading: Use the GetAllWithEagerLoading method with a lambda expression to specify related entities (e.g., Sales) that should be loaded with the Client.
            Avoid LazyInitializationException: By using eager loading, you ensure that related entities are loaded within the context of the initial query, avoiding exceptions related to accessing lazy-loaded properties outside the session scope.
            DTO Mapping: Map the loaded entity to a DTO to return a simplified and appropriate response to the client.

             */

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
