using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models.Data
{
    public class CDbContext :DbContext
    {

        public CDbContext(DbContextOptions<CDbContext> Options) : base(Options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Student> Students { get; set; }
        public DbSet<Studentmodel> Studentmodels { get; set; }


    }
}
