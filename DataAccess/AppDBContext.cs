using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListORama.Models;
using Microsoft.EntityFrameworkCore;

namespace ListORama.DataAccess
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<User> users { get; set; }
        public DbSet<UserCred> userCred { get; set; }
        public DbSet<UserGroup> groups { get; set; }

        public DbSet<UserGroupMember> groupMemberships { get; set; }

        public DbSet<ShoppingList> shoppingList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingList>()
                .HasKey(o => new { o.listID, o.listItems });
        }

        public DbSet<UserList> lists { get; set; }
        public DbSet<UserListItem> listItems { get; set; }
        public DbSet<UserListUserMap> listUserMap { get; set; }

        public DbSet<ListGroup> listgroups { get; set; }
        public DbSet<ListGroupUser> listgroupusers { get; set; }
    }
}
