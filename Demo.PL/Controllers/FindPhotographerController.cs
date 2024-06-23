using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class FindPhotographerController : Controller
    {
        private readonly MvcProjectDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FindPhotographerController(MvcProjectDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // Action for finding a photographer
        public IActionResult FindPhotographer()
        {
            return View();
        }
        // // ///////////////////Photography freelance
        public IActionResult Photographyfreelance()
        {
            return View();
        }

        // GET: New session bid form
        [HttpGet]
        public IActionResult NewSessionBid()
        {
            return View();
        }

        // POST: New session bid
        [HttpPost]
        public async Task<IActionResult> NewSessionBid(SessionBid sessionBid)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                sessionBid.ClientId = user.Id;
                _context.SessionBids.Add(sessionBid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ShowSessionBids));
            }
            return View(sessionBid);
        }

        // Action to show session bids
        public async Task<IActionResult> ShowSessionBids() //for clients 
        {
            var user = await _userManager.GetUserAsync(User);
            var bids = _context.SessionBids
            .Include(s => s.Photographer)
            .Include(s => s.Client)
            .Include(s => s.Comments)
            .ThenInclude(c => c.User)
            .Where(s => s.ClientId == user.Id)
                .ToList();
            return View(bids);
        }
        //////////////////////////////////
        public IActionResult AllSessionsBids() //for photographers 
        {
            var sessionsBids = _context.SessionBids.Include(s=>s.Client).ToList();
            return View(sessionsBids);
        }
    }
}
