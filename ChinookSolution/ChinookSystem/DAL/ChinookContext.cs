using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region
using ChinookSystem.Data.Entites;
using System.Data.Entity;
#endregion

namespace ChinookSystem.DAL
{
    internal class ChinookContext:DbContext
    {
        public ChinookContext() : base("ChinookDB")
        {

        }

        public DbSet<Artist>  Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
    }
}
