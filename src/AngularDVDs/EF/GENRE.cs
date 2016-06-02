using System;
using System.Collections.Generic;

namespace AngularDVDs.EF
{
    public partial class GENRE
    {
        public GENRE()
        {
            DVD = new HashSet<DVD>();
        }

        public Guid GENRE_ID { get; set; }
        public string GENRE_NAME { get; set; }
        public string GENRE_DESC { get; set; }
        public DateTime GENRE_ADDMOD_Datetime { get; set; }

        public virtual ICollection<DVD> DVD { get; set; }
    }
}
