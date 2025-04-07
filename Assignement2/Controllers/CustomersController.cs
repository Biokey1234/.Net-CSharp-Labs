using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using Assignment2.Models.ViewModels;

namespace Assignment2.Controllers
{
    public class CustomersController : Controller
    {
        private readonly DealsFinderDbContext _context;

        public CustomersController(DealsFinderDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,BirthDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,BirthDate")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Customers/EditSubscriptions/5
        public async Task<IActionResult> EditSubscriptions(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Subscriptions)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            var allServices = await _context.FoodDeliveryServices.ToListAsync();

            var subscribedServiceIds = customer.Subscriptions.Select(s => s.ServiceId).ToHashSet();

            var viewModel = new CustomerSubscriptionViewModel
            {
                Customer = customer,
                Subscriptions = allServices
                    .OrderBy(s => !subscribedServiceIds.Contains(s.Id)) // unsubscribed first
                    .ThenBy(s => s.Title) //alphabetical 
                    .Select(s => new FoodDeliveryServiceSubscriptionViewModel
                    {
                        FoodDeliveryServiceId = s.Id,
                        Title = s.Title,
                        IsSubscribed = subscribedServiceIds.Contains(s.Id)
                    })
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: Customers/AddSubscription
        [HttpPost]
        public async Task<IActionResult> AddSubscription(int customerId, string serviceId)
        {
            if (!_context.Subscriptions.Any(s => s.CustomerId == customerId && s.ServiceId == serviceId))
            {
                _context.Subscriptions.Add(new Subscription
                {
                    CustomerId = customerId,
                    ServiceId = serviceId
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(EditSubscriptions), new { id = customerId });
        }

        // POST: Customers/RemoveSubscription
        [HttpPost]
        public async Task<IActionResult> RemoveSubscription(int customerId, string serviceId)
        {
            var sub = await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.CustomerId == customerId && s.ServiceId == serviceId);

            if (sub != null)
            {
                _context.Subscriptions.Remove(sub);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(EditSubscriptions), new { id = customerId });
        }
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
