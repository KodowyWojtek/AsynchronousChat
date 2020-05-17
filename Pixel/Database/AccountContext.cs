using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pixel.Models;

namespace Pixel.Database
{
    public class AccountContext : IdentityDbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {

        }   
        public DbSet<UsersModel> UsersModel { get; set; }
        public DbSet<MessageModel> MessageModel { get; set; }
    }
}
