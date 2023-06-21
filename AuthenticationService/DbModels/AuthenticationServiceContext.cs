using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace AuthenticationService.DbModels
{
    public class AuthenticationServiceContext: DbContext
    {
        public AuthenticationServiceContext(DbContextOptions<AuthenticationServiceContext> options) : base(options)
        {
        }

        public DbSet<AuthenticationUser> AuthenticationUser { get; set; }
    }
}
