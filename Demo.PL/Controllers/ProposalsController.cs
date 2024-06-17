using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class ProposalsController : Controller
    {
        private readonly MvcProjectDbContext _context;
        public ProposalsController(MvcProjectDbContext context)
        {
            _context = context;
        }
        
        // POST: Proposals/SubmitProposal
        //[HttpPost]
        //[ValidateAntiForgeryToken] // Prevents CSRF attacks
        //public async Task<IActionResult> SubmitProposal(int sessionRequestId, string photographerId, string clientId, string proposalText)
        //{
        //    // Creating a new Proposal object with the provided data
        //    var proposal = new Proposal
        //    {
        //        SessionRequestId = sessionRequestId,
        //        PhotographerId = photographerId,
        //        ClientId = clientId,
        //        ProposalText = proposalText,
        //        IsAccepted = false // Assuming the proposal is not accepted initially
        //    };

        //    // Adding the proposal to the DbSet in the DbContext
        //    _context.Proposals.Add(proposal);

        //    // Saving changes asynchronously to the database
        //    await _context.SaveChangesAsync();

        //    // Redirecting to the Details action of the SessionRequests controller
        //    // Passing the sessionRequestId as a route parameter
        //    return RedirectToAction("Details", "SessionRequests", new { id = sessionRequestId });
        //}
    }
}



