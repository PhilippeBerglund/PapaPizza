using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PapaPizza.Data;
using PapaPizza.Models;
using PapaPizza.Models.OrderViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace PapaPizza.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Orders
        public async Task<IActionResult> OrderIndex()
        {
            var session = HttpContext.Session.GetInt32("CartSession");
            var user = await _userManager.GetUserAsync(User);

            var catList = _context.Categories.ToListAsync();
            var cart = _context.Cart
                   .Include(c => c.CartItems)
                   .ThenInclude(ci => ci.Dish)
                   .ThenInclude(d => d.DishIngredients)
                   .ThenInclude(di => di.Ingredient)
                   .Include(cii => cii.CartItems)
                   .ThenInclude(ci => ci.CartItemIngredients)
                   .ThenInclude(cii => cii.Ingredient)
                                   .FirstOrDefault(m => m.CartId == session);

            var newOrder = new OrderViewModel
            {
                Cart = cart,
            };

            if (User.Identity.IsAuthenticated)
            {
                newOrder.UserVM = user;
            }

            return View(newOrder);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderIndex(OrderViewModel model)
        {
               var cartId = HttpContext.Session.GetInt32("CartSession");
               var order = new Order
                {
                    CartId = cartId,
                    User = model.UserVM
                };

                _context.Order.Add(order);
                _context.SaveChanges();

                 HttpContext.Session.Remove("CartSession");

                return View("OrderConfirm", order);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.MyCart)
                .SingleOrDefaultAsync(m => m.id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,ApplicationUserId,CartId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(OrderIndex));
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", order.CartId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.SingleOrDefaultAsync(m => m.id == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", order.CartId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ApplicationUserId,CartId")] Order order)
        {
            if (id != order.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(OrderIndex));
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", order.CartId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.MyCart)
                .SingleOrDefaultAsync(m => m.id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.SingleOrDefaultAsync(m => m.id == id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(OrderIndex));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.id == id);
        }


        [HttpGet]
        public IActionResult GuestLogin()
        {
            var session = HttpContext.Session.GetInt32("CartSession");

            var guest = new ApplicationUser
            {
                FirstName = "",
                LastName = "",
                Street = "",
                City = "",
                PhoneNumber = "",
                CreditCardNumber = "",
            };
            return View(guest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GuestLogin(ApplicationUser guest)
        {
            if (guest == null)
            {
                NotFound();
            }
            var session = HttpContext.Session.GetInt32("CartSession");
            guest = new ApplicationUser
            {
                FirstName = guest.FirstName
            };
            _context.ApplicationUsers.Add(guest);
            _context.SaveChanges();
            return RedirectToAction("OrderIndex", guest);
        }
    }
}
