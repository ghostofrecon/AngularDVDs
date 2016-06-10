using System;
using System.Collections.Generic;

namespace AngularDVDs.EF
{
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            this.AspNetRoleClaims = new HashSet<AspNetRoleClaims>();
            this.AspNetUserRoles = new HashSet<AspNetUserRoles>();
        }

        public string Id { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}
