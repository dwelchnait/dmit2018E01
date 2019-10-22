using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Data.POCOs;
using ChinookSystem.DAL;
using ChinookSystem.Data.Entities;
using System.ComponentModel;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class ArtistController
    {
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Artist> Artist_List()
        {
            using (var context = new ChinookContext())
            {
                return context.Artists.ToList();
            }
        }

        public Artist Artist_Get(int artistid)
        {
            using (var context = new ChinookContext())
            {
                return context.Artists.Find(artistid);
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
