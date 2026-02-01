using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BlazorCRUD.Models;

namespace BlazorCRUD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}

