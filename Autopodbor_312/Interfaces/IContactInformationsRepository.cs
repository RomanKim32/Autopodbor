using Autopodbor_312.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Autopodbor_312.Interfaces
{
    public interface IContactInformationsRepository
    {
        ContactInformation GetFirstContactInformation();

        void UpdateAndSaveChanges(ContactInformation contactInformation);

    }
}
