using AutoMapper;
using BigProject.Data;
using BigProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace BigProject.Controllers;

[Route("api/orders/{orderid}/items")]
public class OrderItemsController : Controller
{
    private readonly IDutchRepository repository;
    private readonly ILogger<OrderItemsController> logger;
    private readonly IMapper mapper;

    public OrderItemsController(IDutchRepository repository,
        ILogger<OrderItemsController> logger,
        IMapper mapper)
    {
        this.repository = repository;
        this.logger = logger;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult Get(int orderId)
    {
        var order = repository.GetOrderById(orderId);
        if (order != null) 
            return Ok(mapper.Map<IEnumerable<OrderItemModel>>(order.Items));
        return NotFound();
    }

    [HttpGet("{id}")]
    public IActionResult Get(int orderId, int id)
    {
        var order = repository.GetOrderById(orderId);
        if (order != null)
        {
            var item = order.Items.Where(i => i.Id == id).FirstOrDefault();
            if(item != null)
                return Ok(mapper.Map<OrderItemModel>(item));
        }
        return NotFound();
    }
}
