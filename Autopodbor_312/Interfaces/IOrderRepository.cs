using Autopodbor_312.ViewModel;

namespace Autopodbor_312.Interfaces
{
	public interface IOrderRepository
	{
		OrderViewModel CreateOrder(string serviceName);
		void CreateOrder(string userName, string phoneNumber, string email, string comment, string carsBrandsId, string carsBodyTypesId, string carsYearsId, string carsFuelsId, string serviceId, string modelId);
		void CreateCallBackAndAdditionalService(string userName, string phoneNumber, string email, string comment, string serviceName);
	}
}
