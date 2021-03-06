//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Collision.Sql.Ef
{
    using System;
    using System.Collections.Generic;
    
    public partial class Aircraft
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Aircraft()
        {
            this.Positions = new HashSet<Position>();
        }
    
        public int Id { get; set; }
        public string Carrier { get; set; }
        public string CarrierName { get; set; }
        public string FlightNumber { get; set; }
        public string PlaneType { get; set; }
        public bool IsActive { get; set; }
        public string Iata { get; set; }
        public string Icao { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Position> Positions { get; set; }
    }
}
