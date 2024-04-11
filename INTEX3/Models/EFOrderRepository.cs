
using INTEX3.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace INTEX3.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private Intex2Context _context; // UPDATE WITH CONTEXT FILE
        public EFOrderRepository(Intex2Context temp) {
            _context = temp;
        }
        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }
        public IEnumerable<Order> GetOrdersSortedByDateDescending()
        {
            return _context.Orders.OrderByDescending(o => o.Date).ToList();
        }
//        public IQueryable<Order> Orders => _context.Orders
//                            .Include(o => o.Lines)
//                            .ThenInclude(l => l.Product);

//        public void SaveOrder(Order order)
//        {
//            _context.AttachRange(order.Lines.Select(l => l.Product));
//            if (order.TransactionId == 0)
//            {
//                _context.Orders.Add(order);
//            }
//            _context.SaveChanges();
    }
}

