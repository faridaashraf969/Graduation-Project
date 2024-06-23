using Demo.DAL.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Demo.DAL.Entities;
using System.Linq;


namespace Demo.PL.Controllers
{
    //[Authorize]
    //public class MessagesController : Controller
    //{
    //    private readonly MvcProjectDbContext _context;

    //    public MessagesController(MvcProjectDbContext context)
    //    {
    //        _context = context;
    //    }
    //    // GET: Messages/Chat/{userId}
    //    public async Task<IActionResult> Chat(string userId)
    //    {
    //        var currentUserId = User.Identity.Name;
    //        var messages = await _context.Messages
    //                                     .Include(m => m.Sender)
    //                                     .Include(m => m.Receiver)
    //                                     .Where(m => (m.SenderId == currentUserId && m.ReceiverId == userId) ||
    //                                                 (m.SenderId == userId && m.ReceiverId == currentUserId))
    //                                     .OrderBy(m => m.SentAt)
    //                                     .ToListAsync();
    //        ViewData["ChatWith"] = userId;
    //        return View(messages);
    //    }

    //    // POST: Messages/Send
    //    [HttpPost]
    //   [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Send([Bind("ReceiverId,Content")] Message message)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            message.SenderId = User.Identity.Name;
    //            message.SentAt = DateTime.Now;
    //            _context.Add(message);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction("Chat", new { userId = message.ReceiverId });
    //        }
    //        return View("Chat", new { userId = message.ReceiverId });
    //    }
    //}
}

