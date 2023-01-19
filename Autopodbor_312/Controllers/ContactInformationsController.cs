using Microsoft.AspNetCore.Mvc;
using Autopodbor_312.Models;
using Microsoft.AspNetCore.Authorization;
using Autopodbor_312.Interfaces;
using System.Threading.Tasks;

namespace Autopodbor_312.Controllers
{
    public class ContactInformationsController : Controller
    {
        private readonly IContactInformationsRepository _contactInformations;

        public ContactInformationsController(IContactInformationsRepository contactInformations)
        {
            _contactInformations = contactInformations;
        }

        public IActionResult Index()
        {
            var contactInformation = _contactInformations.GetFirstContactInformation();
            return View(contactInformation);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit()
        {
            var contactInformation = _contactInformations.GetFirstContactInformation();
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
        public IActionResult Edit(int id, [Bind("Id,Email,PhoneNumber,LinkToInstagram,LinkToTiktok,LinkToYoutube,LinkToWhatsapp,LinkToTelegram")] ContactInformation contactInformation)
        {
            if (id != contactInformation.Id)
            {
                return NotFound();
            }
            _contactInformations.UpdateAndSaveChanges(contactInformation);
            return RedirectToAction("Index");
        }
    }
}
