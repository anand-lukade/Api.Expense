using ExpenseApi.Models;
using System.Data.Entity;

namespace ExpenseApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext():base("name=ExpensesDb")
        {

        }
        public DbSet<Entry> Entries { get; set; }       
        public DbSet<User> users { get; set; }
    }
}