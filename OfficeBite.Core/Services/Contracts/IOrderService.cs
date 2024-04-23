using OfficeBite.Core.Models.OrderModels;

namespace OfficeBite.Core.Services.Contracts
{
    public interface IOrderService
    {
        Task<AddOrderViewModel> AddToOrder();

        Task AddToOrder(int requestMenuNumber);
    }
}
