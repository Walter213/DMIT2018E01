using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Data.Entites;
using System.ComponentModel;
using ChinookSystem.Data.POCOs;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class ArtistController
    {
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
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

        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public List<SelectionList> List_ArtistNames()
        //{
        //    using (var context = new ChinookContext())
        //    {
        //        var results = from x in context.Artists
        //                      orderby x.Name
        //                      select new SelectionList
        //                      {
        //                          IDValueField = x.ArtistId,
        //                          DisplayText = x.Name
        //                      };
        //        return results.ToList();
        //    }
        //}
    }
}
