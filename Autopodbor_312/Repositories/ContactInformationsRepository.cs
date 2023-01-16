using Autopodbor_312.Models;
using Autopodbor_312.Interfaces;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

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
            _context.SaveChangesAsync();
        }
    }
}
