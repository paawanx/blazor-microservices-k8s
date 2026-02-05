using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BlazorCRUD.StudentApi.Models;

namespace BlazorCRUD.StudentApi.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}

