using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITStore.Models
{
    public interface IITStoreEntities : IDisposable
    {
        DbSet<Category> Category { get; set; }
        DbSet<Product> Product { get; set; }
        DbSet<Purchases> Purchases { get; set; }

        Int32 SaveChanges();
    }
}