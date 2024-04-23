using OfficeBite.Core.Models.StaffModels;

namespace OfficeBite.Core.Services.Contracts
{
    public interface IStaffService
    {
        Task<List<StaffAllOrdersViewModel>> AllOrders();

        Task<StaffOrderViewModel?> OrderView(int id, string userId, DateTime date);

        Task<StaffAllOrdersViewModel?> OrderComplete(DateTime selectedDate, string username, string userId, int orderId);
        Task CompleteOrderConfirm(DateTime selectedDate, int orderId, string userId);

    }
}
