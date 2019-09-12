using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Data.Entites;
#endregion

namespace ChinookSystem.BLL
{
    public class ArtistController
    {
        public List<Artist> Artist_List()
        {
            using (var context = new ChinookContext())
            {
                return context.Artists.ToList();
            }
        }

        public Artist Artist_Get(int Artistid)
        {
            using (var context = new ChinookContext())
            {
                return context.Artists.Find(Artistid);
            }
        }
    }
}
