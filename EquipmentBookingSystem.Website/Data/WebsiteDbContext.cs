using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EquipmentBookingSystem.Website.Models;

namespace EquipmentBookingSystem.Website.Data
{
    public class WebsiteDbContext : DbContext
    {
        public WebsiteDbContext (DbContextOptions<WebsiteDbContext> options)
            : base(options)
        {
        }

        public DbSet<EquipmentBookingSystem.Website.Models.Item> Item { get; set; } = default!;
    }
}
