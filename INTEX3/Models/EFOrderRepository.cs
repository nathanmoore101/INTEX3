
using INTEX3.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace INTEX3.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private Intex2Context _context;
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

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.FirstOrDefault(o => o.TransactionId == orderId);
        }

        public void UpdateOrder(Order order)
        {
            var existingOrder = _context.Orders.FirstOrDefault(o => o.TransactionId == order.TransactionId);

            if (existingOrder != null)
            {
                existingOrder.CustomerId = order.CustomerId;
                existingOrder.Date = order.Date;
                existingOrder.DayOfWeek = order.DayOfWeek;
                existingOrder.Time = order.Time;
                existingOrder.EntryMode = order.EntryMode;
                existingOrder.Amount = order.Amount;
                existingOrder.TypeOfTransaction = order.TypeOfTransaction;
                existingOrder.CountryOfTransaction = order.CountryOfTransaction;
                existingOrder.ShippingAddress = order.ShippingAddress;
                existingOrder.Bank = order.Bank;
                existingOrder.TypeOfCard = order.TypeOfCard;
                existingOrder.Fraud = order.Fraud;

                _context.SaveChanges();
            }
        }
        public Order CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }


    }
}

