using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Data.POCOs;
#endregion

namespace ChinookSystem.Data.DTOs
{
    public class AlbumDTO
    {
        public string AlbumTitle { get; set; }
        public string AlbumArtist { get; set; }
        public int TrackCount { get; set; }
        public int PlayTime { get; set; }
        public List<TrackPOCO> AlbumTracks { get; set; }
    }
}
