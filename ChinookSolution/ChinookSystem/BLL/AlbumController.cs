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
using DMIT2018Common.UserControls;
using ChinookSystem.Data.DTOs;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class AlbumController
    {
        #region Class Variables
        private List<string> reasons = new List<string>();
        #endregion

        #region Queries
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Album> Album_List()
        {
            using (var context = new ChinookContext())
            {
                return context.Albums.ToList();
            }
        }

        public Album Album_Get(int albumid)
        {
            using (var context = new ChinookContext())
            {
                return context.Albums.Find(albumid);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Album> Album_FindByArtist(int artistid)
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums
                              where x.ArtistId == artistid
                              select x;

                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<AlbumsOfArtist> Album_AlbumsOfArtist(string artistname)
        {
            using (var context = new ChinookContext())
            {
                //unlike LinqPad which is Linq to Sql
                //within out application it is Linq to Entities
                var results = from x in context.Albums
                              where x.Artist.Name.Contains(artistname)
                              orderby x.ReleaseYear, x.Title
                              select new AlbumsOfArtist
                              {
                                  Title = x.Title,
                                  ArtistName = x.Artist.Name,
                                  RYear = x.ReleaseYear,
                                  RLabel = x.ReleaseLabel
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<AlbumDTO> Album_AlbumAndTracks()
        {
            using (var context = new ChinookContext())
            {
                var results = from x in context.Albums
                              where x.Tracks.Count() > 25
                              select new AlbumDTO
                              {
                                  AlbumTitle = x.Title,
                                  AlbumArtist = x.Artist.Name,
                                  TrackCount = x.Tracks.Count(),
                                  PlayTime = x.Tracks.Sum(z => z.Milliseconds),
                                  AlbumTracks = (from y in x.Tracks
                                                 select new TrackPOCO
                                                 {
                                                     SongName = y.Name,
                                                     SongGenre = y.Genre.Name,
                                                     SongLength = y.Milliseconds
                                                 }).ToList()

                              };
                return results.ToList();
            }
        }

        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public List<SelectionList> List_AlbumNames()
        //{
        //    using (var context = new ChinookContext())
        //    {
        //        var results = from x in context.Albums
        //                      orderby x.Title
        //                      select new SelectionList
        //                      {
        //                          IDValueField = x.AlbumId,
        //                          DisplayText = x.Title
        //                      };
        //        return results.ToList();
        //    }
        //}
    
    #endregion

    #region Add, Update, Delete
    [DataObjectMethod(DataObjectMethodType.Insert,false)]
        public int Album_Add(Album item)
        {
            using (var context = new ChinookContext())
            {
                if (CheckReleaseYear(item))
                {
                    //item.Title = null; use to test entity validation
                    context.Albums.Add(item);   //staging
                    context.SaveChanges();      //committed
                    return item.AlbumId;        //return new id value
                }
                else
                {
                    throw new BusinessRuleException("Validation Error", reasons);
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update,false)]
        public int Album_Update(Album item)
        {
            using (var context = new ChinookContext())
            {
                if (CheckReleaseYear(item))
                {
                    context.Entry(item).State =
                    System.Data.Entity.EntityState.Modified;
                    return context.SaveChanges();
                }
                else
                {
                    throw new BusinessRuleException("Validation Error", reasons);
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete,false)]
        public int Album_Delete(Album item)
        {
            return Album_Delete(item.AlbumId);
        }
        public int Album_Delete(int albumid)
        {
            using (var context = new ChinookContext())
            {
                var existing = context.Albums.Find(albumid);
                if (existing == null)
                {
                    throw new Exception("Album not on file. Delete unnecessary.");
                }
                else
                {
                    context.Albums.Remove(existing);
                    return context.SaveChanges();
                }
            }
           
        }
        #endregion

        #region Support Methods
        private bool CheckReleaseYear(Album item)
        {
            bool isValid = true;
            int releaseyear;
            if(string.IsNullOrEmpty(item.ReleaseYear.ToString()))
            {
                isValid = false;
                reasons.Add("Release year is required");
            }
            else if (!int.TryParse(item.ReleaseYear.ToString(),out releaseyear))
            {
                isValid = false;
                reasons.Add("Release year is not an number");
            }
            else if (releaseyear < 1950 || releaseyear > DateTime.Today.Year)
            {
                isValid = false;
                reasons.Add(string.Format("Album release year of {0} invalid. Year must be between 1950 and today",
                    releaseyear));
            }
            return isValid;
        }
        #endregion
    }
}
