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
    [Table("Tracks")]
    class Track
    {
        [Key]

        public int TrackId { get; set; }

        public string Name { get; set; }

        private string _Name { get; set; }
        public string ReleaseLabel  /* for reminder, this is incase it is null */
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
                }
            }
        }
    }
}
