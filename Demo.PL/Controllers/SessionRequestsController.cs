using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Demo.DAL.Contexts;


namespace Demo.PL.Controllers
{
    [Authorize]
    public class SessionRequestsController : Controller
    {
        private readonly MvcProjectDbContext _context;

        public SessionRequestsController(MvcProjectDbContext context)
        {
            _context = context;
        }
        // GET: SessionRequests
        public IActionResult Index()
        {
            var sessionRequests = _context.SessionRequests.ToList();
            return View(sessionRequests);
        }

        // GET: SessionRequests/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessionRequest = _context.SessionRequests
                .Include(sr => sr.Proposals)
                .Include(sr => sr.Photographer)
                .Include(sr => sr.Client)
                .FirstOrDefault(sr => sr.Id == id);

            if (sessionRequest == null)
            {
                return NotFound();
            }

            return View(sessionRequest);
        }

        // GET: SessionRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SessionRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SessionRequest sessionRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sessionRequest);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(sessionRequest);
        }

        // POST: SessionRequests/AcceptProposal/{proposalId}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptProposal(int proposalId)
        {
            var proposal = await _context.Proposals.FindAsync(proposalId);

            if (proposal == null)
            {
                return NotFound();
            }

            // Mark the proposal as accepted
            proposal.IsAccepted = true;

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to the details page of the session request associated with the proposal
            return RedirectToAction("Details", "SessionRequests", new { id = proposal.SessionRequestId });
        }
    }

    
    
    
}

       

           
