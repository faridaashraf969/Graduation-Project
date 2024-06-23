using Microsoft.Extensions.Configuration;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Demo.PL.Helpers;
using Microsoft.Extensions.Options;

namespace Demo.PL.Controllers
{
    public class CommentsController : Controller
    {

        private readonly MvcProjectDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly StripeSettings _stripeSettings;

        public CommentsController(MvcProjectDbContext context, UserManager<ApplicationUser> userManager , IConfiguration configuration , IOptions<StripeSettings> stripeSettings)
        {
            _context = context;
            _userManager = userManager;
            this._configuration = configuration;
        }
        // Action to display all comments for a specific SessionBid
        public async Task<IActionResult> Index(int sessionBidId)
        {
            if (sessionBidId == 0)
            {
                // Log the invalid sessionBidId or handle accordingly
                return NotFound();
            }

            var sessionBid = await _context.SessionBids
                                           .Include(sb => sb.Comments)
                                           .ThenInclude(c => c.User)
                                           .FirstOrDefaultAsync(sb => sb.Id == sessionBidId);

            if (sessionBid == null)
            {
                // Log that the sessionBid was not found
                return NotFound();
            }

            return View(sessionBid);
        }
        // // // /////////////////////Indexcommentphotographer
        public async Task<IActionResult> Indexcommentphotographer(int sessionBidId)
        {
            if (sessionBidId == 0)
            {
                // Log the invalid sessionBidId or handle accordingly
                return NotFound();
            }

            var sessionBid = await _context.SessionBids
                                           .Include(sb => sb.Comments)
                                           .ThenInclude(c => c.User)
                                           .FirstOrDefaultAsync(sb => sb.Id == sessionBidId);

            if (sessionBid == null)
            {
                // Log that the sessionBid was not found
                return NotFound();
            }

            return View(sessionBid);
        }


        // GET: Comments/Create
        public IActionResult Create(int sessionBidId)
        {
            ViewBag.SessionBidId = sessionBidId;
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,SessionBidId,Price")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                var sessionBidExists = _context.SessionBids.Any(sb => sb.Id == comment.SessionBidId);
                if (!sessionBidExists)
                {
                    ModelState.AddModelError("", "Invalid SessionBidId.");
                    ViewBag.SessionBidId = comment.SessionBidId;
                    return View(comment);
                }

                comment.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                comment.CreatedAt = DateTime.Now;  // Set the CreatedAt property
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Indexcommentphotographer), new { sessionBidId = comment.SessionBidId });
            }
            ViewBag.SessionBidId = comment.SessionBidId;
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            // Populate the ViewBag with SessionBids for the dropdown
            ViewBag.SessionBidId = new SelectList(await _context.SessionBids.ToListAsync(), "Id", "Description", comment.SessionBidId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,SessionBidId,Price,UserId")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Indexcommentphotographer), new { sessionBidId = comment.SessionBidId });
            }

            // Repopulate the SessionBidId in case of an error
            ViewBag.SessionBidId = new SelectList(await _context.SessionBids.ToListAsync(), "Id", "Description", comment.SessionBidId);
            return View(comment);
        }

        // Only one CommentExists method should be present
        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.User)
                .Include(c => c.SessionBid)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Indexcommentphotographer), new { sessionBidId = comment.SessionBidId });
        }
        /// ///////////////////////////////////////////////////////
        
        ///////////////////////////////////////////////////////////////////
       
        public async Task<IActionResult> ConfirmProposal(int Id)
        {
            var comment = await _context.Comments.FindAsync(Id);

            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

            // Redirect to the payment action or a view where the client can make the payment
            return RedirectToAction(nameof(MakePayment), new
            {
                sessionBidId = comment.SessionBidId,
                amount = comment.Price,
                proposalId = comment.Id
            });
            }

        [HttpGet]
        public IActionResult MakePayment(int sessionBidId, decimal amount, int proposalId)
        {
            var paymentModel = new PaymentViewModel
            {
                SessionBidId = sessionBidId,
                proposalId = proposalId,
                Amount = amount + amount * 0.12m,
                PublishableKey = _configuration["Stripe:PublishableKey"]
            };

            return View(paymentModel);
        }

        //[HttpPost]
        //public async Task<IActionResult> MakePayment(PaymentViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var options = new ChargeCreateOptions
        //    {
        //        Amount = (long)(model.Amount * 100), // Amount in cents
        //        Currency = "Egp",
        //        Description = $"Payment for SessionBid {model.SessionBidId}",
        //        Source = model.StripeToken,
        //    };

        //    var service = new ChargeService();
        //    Charge charge;
        //    try
        //    {
        //        charge = await service.CreateAsync(options);
        //    }
        //    catch (StripeException ex)
        //    {
        //        // Log the exception or handle accordingly
        //        ModelState.AddModelError(string.Empty, $"Payment failed: {ex.Message}");
        //        return View(model);
        //    }
        //    if (charge.Status == "succeeded")
        //    {
        //        var payment = new Payment
        //        {
        //            SessionBidId = model.SessionBidId,
        //            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Assuming you're using identity
        //            Amount = model.Amount,
        //            PaymentDate = DateTime.Now
        //        };

        //        _context.Payments.Add(payment);
        //        await _context.SaveChangesAsync();

        //        // Redirect to a success page or the session bid details
        //        return RedirectToAction(nameof(Index), new { sessionBidId = model.SessionBidId });
        //    }
        //    else
        //    {
        //        // Handle failure
        //        ModelState.AddModelError(string.Empty, "Payment failed");
        //        return View(model);
        //    }


        //}

        [HttpPost]
        public async Task<IActionResult> MakePayment(PaymentViewModel model)
        {

            
            if (model.SessionBidId != null)
            {
                var payment = new Payment
                {
                    SessionBidId = model.SessionBidId,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Assuming you're using identity
                    Amount = model.Amount,
                    PaymentDate = DateTime.Now
                };


                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                // Redirect to a success page or the session bid details
                return RedirectToAction(nameof(ConfirmPayment), new { /*sessionBidId = model.SessionBidId ,*/ proposalId = model.proposalId });
            }
            else
            {
                var options = new ChargeCreateOptions
                {
                    Amount = (long)(model.Amount * 100), // Amount in cents
                    Currency = "Egp",
                    Description = $"Payment for SessionBid {model.SessionBidId}",
                    Source = model.StripeToken,
                };

                var service = new ChargeService();
                Charge charge;
                try
                {
                    charge = await service.CreateAsync(options);
                }
                catch (StripeException ex)
                {
                    // Log the exception or handle accordingly
                    ModelState.AddModelError(string.Empty, $"Payment failed: {ex.Message}");
                    return View(model);
                }
            }

            return RedirectToAction(nameof(Index), new { sessionBidId = model.SessionBidId });
        }

        
        public async Task<IActionResult> ConfirmPayment(int proposalId)
        {
            var proposal = await _context.Comments
                                         .Include(p => p.SessionBid).ThenInclude(p=>p.Photographer).Include(c=>c.User)
                                         .FirstOrDefaultAsync(p => p.Id == proposalId);

            

            if (proposal == null)
            {
                return NotFound();
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            var viewModel = new ConfirmationViewModel
            {
                SessionBid = proposal.SessionBid,
                Proposal = proposal
            };

            return View( viewModel);
        }
    }
     
   
}

