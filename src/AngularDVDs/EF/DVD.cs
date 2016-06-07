using System;
using System.Collections.Generic;

namespace AngularDVDs.ef
{
    public partial class DVD
    {
        public Guid DVD_ID { get; set; }
        public string DVD_TITLE { get; set; }
        public Guid DVD_DIRECTOR_ID { get; set; }
        public Guid DVD_GENRE_ID { get; set; }
        public int? DVD_RELEASE_YEAR { get; set; }
        public DateTime DVD_ADDMOD_Datetime { get; set; }

        public virtual DIRECTOR DVD_DIRECTOR { get; set; }
        public virtual GENRE DVD_GENRE { get; set; }
    }
}
