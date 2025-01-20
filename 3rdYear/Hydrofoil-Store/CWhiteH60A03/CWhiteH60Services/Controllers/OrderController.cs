using CWhiteH60Services.DAL;
using CWhiteH60Services.Models;
using CWhiteH60Services.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CWhiteH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase {
    private readonly IOrderRepository<Order> _orderRepository;

    public OrderController(IOrderRepository<Order> orderRepository) {
        _orderRepository = orderRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> AllOrders() {
        return (await _orderRepository.Read()).Select(o => new OrderDto(o)).ToList();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> FindOrder(int id) {
        var order = await _orderRepository.Find(id);
        if (order == null) {
            return NotFound();
        }
        return order;
    }
    
    [HttpGet("Date/{date}")]
    public async Task<ActionResult<List<OrderDto>>> OrdersByDate(DateTime date) {
        var orders = (await _orderRepository.ReadByDate(date)).Select(o => new OrderDto(o)).ToList();
        if (orders == null) {
            return NotFound();
        }
        return orders;
    }
    
    [HttpGet("Name/{customerName}")]
    public async Task<ActionResult<List<OrderDto>>> OrdersByCustomerName(string customerName) {
        var orders = (await _orderRepository.ReadByCustomer(customerName)).Select(o => new OrderDto(o)).ToList();
        if (orders == null) {
            return NotFound();
        }
        return orders;
    }
    
    [HttpGet("Id/{customerId}")]
    public async Task<ActionResult<List<OrderDto>>> OrdersByCustomerId(int customerId) {
        var orders = (await _orderRepository.ReadByCustomer(customerId)).Select(o => new OrderDto(o)).ToList();;
        if (orders == null) {
            return NotFound();
        }
        return orders;
    }

    [HttpPost]
    public async Task<ActionResult> CreateOrder(OrderDto orderDto) {
        ModelState.Remove("Customer");
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var success = await _orderRepository.Create(new Order(orderDto));

        if (!success) {
            return BadRequest(ModelState);
        }
        
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(OrderDto orderDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        
        var success = await _orderRepository.Update(new Order(orderDto));

        if (!success) {
            return BadRequest(ModelState);
        }
        
        return Ok();
    }
}