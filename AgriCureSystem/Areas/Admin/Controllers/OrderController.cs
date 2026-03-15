using AgriCureSystem.Models;
using AgriCureSystem.Repositories.IRepositories;
using AgriCureSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriCureSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAsync(includes: [o => o.ApplicationUser]);
            return View(orders);
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
        
            var order = await _orderRepository.GetOneAsync(
                e => e.Id == id,
                includes: [
                    e => e.ApplicationUser,
            e => e.OrderItems
                ]);

            if (order is null)
                return NotFound();

            return View(order);
        }

     

        [HttpPost]
        
        public async Task<IActionResult> Shipped(int id)
        {
            var order = await _orderRepository.GetOneAsync(e => e.Id == id);
            if (order != null)
            {
                order.OrderStatus = OrderStatus.shipped; 
                await _orderRepository.CommitAsync();
            }
            return RedirectToAction(nameof(Index)); 
        }

        [HttpPost]
        public async Task<IActionResult> Completed(int id)
        {
            var order = await _orderRepository.GetOneAsync(e => e.Id == id);
            if (order != null)
            {
                order.OrderStatus = OrderStatus.completed;
                await _orderRepository.CommitAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Canceled(int id)
        {

            var order = await _orderRepository.GetOneAsync(e => e.Id == id);
            if (order != null)
            {
                order.OrderStatus = OrderStatus.canceled;
                await _orderRepository.CommitAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
