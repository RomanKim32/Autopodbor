using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Autopodbor_312.Models;
using Microsoft.AspNetCore.Authorization;

namespace Autopodbor_312.Controllers
{
    public class ContactInformationsController : Controller
    {
        private readonly AutopodborContext _context;

        public ContactInformationsController(AutopodborContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var contactInformation = _context.ContactInformation.First();
            return View(contactInformation);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit()
        {
            var contactInformation = await _context.ContactInformation.FirstAsync();
            if (contactInformation.Id.ToString() == null)
            {
                return NotFound();
            }

            if (contactInformation == null)
            {
                return NotFound();
            }
            return View(contactInformation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,PhoneNumber,LinkToInstagram,LinkToTiktok,LinkToYoutube,LinkToWhatsapp,LinkToTelegram")] ContactInformation contactInformation)
        {
            if (id != contactInformation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactInformationExists(contactInformation.Id))
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
            return View(contactInformation);
        }

        private bool ContactInformationExists(int id)
        {
            return _context.ContactInformation.Any(e => e.Id == id);
        }
    }
}
