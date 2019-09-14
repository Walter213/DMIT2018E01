using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Data.Entites;
using System.ComponentModel;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
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

        [DataObjectMethod(DataObjectMethodType.Insert,false)]

        public List<Album> Album_FindByArtist(int artistid)
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums
                    where x.ArtistId == artistid
                    select x; /* select a row */

                return results.ToList();
            }
        }
    }
}
