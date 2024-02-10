using Microsoft.EntityFrameworkCore;

namespace Customer.BusinessLogic
{
    public class CustomerDb : DbContext
    {
        public CustomerDb(DbContextOptions<CustomerDb> options)
        : base(options) { }

        public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();

        //private static List<Customer> customers = new List<Customer>()
        //{
        //    new Customer() {  Id = 1, FirstName="John", LastName="Doe", EmailAddress="jd@gmail.com", Age=30, Address="123 Main Street Texas"},
        //    new Customer() {  Id = 2, FirstName="Jack", LastName="Smith", EmailAddress="js@gmail.com", Age=30, Address="234 Main Street Florida"},
        //    new Customer() {  Id = 3, FirstName="George", LastName="Mathews", EmailAddress="gm@gmail.com", Age=40, Address="123 Main Street California"},
        //    new Customer() {  Id = 4, FirstName="John", LastName="Doe", EmailAddress="jd@gmail.com", Age=50, Address="123 Main Street Newyork"}
        //};

        //public static List<Customer> GetCustomers()
        //{
        //    return customers;
        //}

        //public static Customer? GetCustomer(int id)
        //{
        //    return customers.SingleOrDefault(c => c.Id == id);
        //}

        //public static Customer CreateCustomer(Customer customer)
        //{
        //    customers.Add(customer);
        //    return customer;
        //}

        //public static Customer UpdateCustomer(Customer customer)
        //{
        //    customers = customers.Select(c =>
        //    {
        //        if (c.Id == customer.Id)
        //        {
        //            c.FirstName = customer.FirstName;
        //            c.LastName = customer.LastName;
        //            c.EmailAddress = customer.EmailAddress;
        //            c.Age = customer.Age;
        //            c.Address = customer.Address;
        //        }
        //        return customer;
        //    }).ToList();
        //    return customer;
        //}

        //public static void RemoveCustomer(int id)
        //{
        //    customers = customers.FindAll(c => c.Id != id).ToList();
        //}
    }
}
