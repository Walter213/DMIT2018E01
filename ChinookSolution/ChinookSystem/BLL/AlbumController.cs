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
    public class AlbumController
    {
        public List<Album> Album_List()
        {
            using (var context = new ChinookContext())
            {
                return context.Albums.ToList();
            }
        }

        public Artist Album_Get(int Albumid)
        {
            using (var context = new ChinookContext())
            {
                return context.Artists.Find(Albumid);
            }
        }
    }
}
