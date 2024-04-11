using INTEX3.Models;

namespace INTEX3.Models
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAllOrders();

        IEnumerable<Order> GetOrdersSortedByDateDescending();

        //IQueryable<Order> Orders { get; }
        //void SaveOrder(Order order);
    }
}
