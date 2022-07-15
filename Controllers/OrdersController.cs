using AutoMapper;
using BigProject.Data;
using BigProject.Data.Entities;
using BigProject.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BigProject.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<OrdersController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<StoreUser> userManager;

        public OrdersController(IDutchRepository repository,
            ILogger<OrdersController> logger,
            IMapper mapper,
            UserManager<StoreUser> userManager)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Order>> Get(bool includeItems = true)
        {
            try
            {
                var username = User.Identity.Name;
                var result = repository.GetAllOrdersByUser(username, includeItems);
                return Ok(mapper.Map<IEnumerable<OrderModel>>(result));
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get all orders: {exception}", ex);
                return BadRequest("Failed to get all orders");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult Get(int id)
        {
            try
            {
                var order = repository.GetOrderById(User.Identity.Name, id);
                if (order != null) return Ok(mapper.Map<OrderModel>(order));
                else return NotFound();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to get order: {exception}", ex);
                return BadRequest("Failed to get order");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrderModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newOrder = mapper.Map<Order>(model);

                    if (newOrder.OrderDate == DateTime.MinValue)
                        newOrder.OrderDate = DateTime.Now;

                    var currentUser = await userManager.FindByNameAsync(User.Identity.Name);
                    newOrder.User = currentUser;

                    repository.AddEntity(newOrder);
                    if (repository.SaveAll())
                    {
                        return Created($"api/orders/{newOrder.Id}", 
                            mapper.Map<OrderModel>(newOrder));
                    }
                }
                else return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to save new order: {exception}", ex);
            }

            return BadRequest("Failed to save new order");
        }
    }
}
