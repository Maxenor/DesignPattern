namespace Bibliotheque
{
    public interface IOrderBuilder
    {
        IOrderBuilder SetCustomer(Customer customer);
        IOrderBuilder SetDeliveryAddress(string address);
        IOrderBuilder AddBook(Book book, int quantity);
        Order Build();
    }
    public class OrderBuilder : IOrderBuilder
    {
        private Order _order;

        public OrderBuilder()
        {
            _order = new Order();
        }

        public IOrderBuilder SetCustomer(Customer customer)
        {
            _order.Customer = customer;
            return this;
        }

        public IOrderBuilder SetDeliveryAddress(string address)
        {
            _order.DeliveryAddress = address;
            return this;
        }

        public IOrderBuilder AddBook(Book book, int quantity)
        {
            _order.AddBook(book, quantity);
            return this;
        }

        public Order Build()
        {
            return _order;
        }
    }


    public class OrderDirector
    {
        public Order ConstructOrder(IOrderBuilder builder, Customer customer, string address, List<(Book, int)> bookList)
        {
            builder.SetCustomer(customer);
            builder.SetDeliveryAddress(address);
            foreach (var (book, quantity) in bookList)
            {
                builder.AddBook(book, quantity);
            }
            return builder.Build();
        }
    }
}
