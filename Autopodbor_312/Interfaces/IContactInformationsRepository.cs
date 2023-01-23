using Autopodbor_312.Models;

namespace Autopodbor_312.Interfaces
{
    public interface IContactInformationsRepository
    {
        ContactInformation GetFirstContactInformation();

        void UpdateAndSaveChanges(ContactInformation contactInformation);

    }
}
