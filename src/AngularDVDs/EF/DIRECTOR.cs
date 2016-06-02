using System;
using System.Collections.Generic;

namespace AngularDVDs.EF
{
    public partial class DIRECTOR
    {
        public DIRECTOR()
        {
            DVD = new HashSet<DVD>();
        }

        public Guid DIRECTOR_ID { get; set; }
        public string DIRECTOR_NAME { get; set; }
        public DateTime DIRECTOR_ADDMOD_Datetime { get; set; }

        public virtual ICollection<DVD> DVD { get; set; }
    }
}
