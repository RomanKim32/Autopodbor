using Autopodbor_312.Interfaces;
using Autopodbor_312.Models;
using System.Linq;

namespace Autopodbor_312.Repositories
{
    public class ContactInformationsRepository : IContactInformationsRepository
    {
        private readonly AutopodborContext _context;


        public ContactInformationsRepository(AutopodborContext autopodborContext)
        {
            _context = autopodborContext;
        }

        public ContactInformation GetFirstContactInformation()
        {
            var contactInformation = _context.ContactInformation.First();
            return contactInformation;
        }

        public void UpdateAndSaveChanges(ContactInformation contactInformation)
        {
            _context.Update(contactInformation);
            _context.SaveChanges();
        }
    }
}
