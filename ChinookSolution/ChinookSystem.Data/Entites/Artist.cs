using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Addition Namespaces
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
#endregion

namespace ChinookSystem.Data.Entites
{
    [Table("Artists")]
    public class Artist
    {
        /*  POCOs stands for Plain Ordinary Common Object
            DTOs (Data Transfer Object) : That has POCOs and structure
            Entities: definition to map a database entity (table): that contrain native basic datatype elements  */

        [Key]       /* Compund Key [Key, Column(Order = n)] then their is [Key, DatabaseGenerated(DatabaseGeneratedOption.3optionsareavaliable)]  */

        public int ArtistId { get; set; }

        private string _Name;

        public string Name  /* for reminder, this is incase it is null */
        {
            get
            {
                return _Name;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _Name = null;
                }
                else
                {
                    _Name = value;
                }//eof
            }
        }//eop

        public virtual ICollection<Album> Albums { get; set; } 
    }//eoc
}//eon

/* Tracks, MediaTypes, Genres */

/* LINQPad 5 poop
 
    from x in Albums
    orderby x.Artist.Name
    select new
    {
        ArtistName = x.ArtistName
        Title = x.Title
        Year = x.ReleaseYear
    }

*/