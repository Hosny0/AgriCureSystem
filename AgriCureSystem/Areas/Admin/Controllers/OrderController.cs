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
            var orders = await _orderRepository.GetAsync();
            return View(orders); 
        }

 
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetOneAsync(e => e.Id == id, includes: [equals => equals.ApplicationUser]);

            if (order is null)
                return NotFound();

            return View(order);
        }
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var order = await _orderRepository.GetOneAsync(e => e.Id == id);

            if (order is not null)
            {
                return View(order);
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Order order)
        {
            if (!ModelState.IsValid)
            {
                return View(order); 
            }
            _orderRepository.Edit(order);

            TempData["success-notification"] = "Update Order Successfully";

            await _orderRepository.CommitAsync();

            return RedirectToAction(nameof(Index));
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
