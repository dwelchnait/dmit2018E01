namespace ChinookSystem.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Album
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Album()
        {
            Tracks = new HashSet<Track>();
        }

        public int AlbumId { get; set; }

        [Required(ErrorMessage ="Album title is required")]
        [StringLength(160,ErrorMessage ="Album title is limited to 160 characters")]
        public string Title { get; set; }

        public int ArtistId { get; set; }

        //the Range validation annotation can check a field for a range of values
        //the minimum and maximum values MUST be constants
        public int ReleaseYear { get; set; }

        [StringLength(50, ErrorMessage = "Album label is limited to 50 characters")]
        public string ReleaseLabel { get; set; }

        public virtual Artist Artist { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Track> Tracks { get; set; }
    }
}
