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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitProposal(int sessionRequestId, string photographerId, string clientId, string proposalText)
        {
            var proposal = new Proposal
            {
                SessionRequestId = sessionRequestId,
                PhotographerId = photographerId,
                ClientId = clientId,
                ProposalText = proposalText,
                IsAccepted = false
            };
            _context.Proposals.Add(proposal);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "SessionRequests", new { id = sessionRequestId });
        } 
    }

}

