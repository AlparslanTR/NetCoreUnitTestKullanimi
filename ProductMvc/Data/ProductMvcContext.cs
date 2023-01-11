using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductMvc.Models;

namespace ProductMvc.Data
{
    public class ProductMvcContext : DbContext
    {
        public ProductMvcContext (DbContextOptions<ProductMvcContext> options)
            : base(options)
        {
        }

        public DbSet<ProductMvc.Models.Product> Product { get; set; } = default!;
    }
}
