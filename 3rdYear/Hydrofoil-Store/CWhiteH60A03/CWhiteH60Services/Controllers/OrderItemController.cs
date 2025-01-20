using CWhiteH60Services.DAL;
using CWhiteH60Services.Models;
using CWhiteH60Services.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace CWhiteH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderItemController : ControllerBase {
    private readonly IOrderItemRepository<OrderItem> _orderItemRepository;

    public OrderItemController(IOrderItemRepository<OrderItem> orderItemRepository) {
        _orderItemRepository = orderItemRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<OrderItem>>> AllOrderItems() {
        return await _orderItemRepository.Read();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderItem>> FindOrderItem(int id) {
        var orderItem = await _orderItemRepository.Find(id);
        if (orderItem == null) {
            return NotFound();
        }
        return orderItem;
    }
    
    [HttpGet("Order/{id}")]
    public async Task<ActionResult<List<OrderItem>>> FindOrderItemByOrder(int id) {
        var orderItems = await _orderItemRepository.FindByOrderId(id);
        if (orderItems == null) {
            return NotFound();
        }
        return orderItems;
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateOrderItem(OrderItemDto orderItemDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var success = await _orderItemRepository.Create(new OrderItem(orderItemDto));

        if (!success) {
            return BadRequest(ModelState);
        }
        
        return Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrderItem(OrderItemDto orderItemDto) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var success = await _orderItemRepository.Update(new OrderItem(orderItemDto));

        if (!success) {
            return BadRequest(ModelState);
        }
        
        return Ok();
    }
}