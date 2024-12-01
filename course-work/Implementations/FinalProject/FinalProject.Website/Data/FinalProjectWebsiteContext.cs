using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data.Entities;

namespace FinalProject.Website.Data
{
    public class FinalProjectWebsiteContext : DbContext
    {
        public FinalProjectWebsiteContext (DbContextOptions<FinalProjectWebsiteContext> options)
            : base(options)
        {
        }

        public DbSet<Station> Station { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Train> Train { get; set; }
        public DbSet<Timetable> Timetable { get; set; }
    }
}
