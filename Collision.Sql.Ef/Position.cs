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
    
    public partial class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AircraftId { get; set; }
        public string FlightId { get; set; }
        public string FlightIdentity { get; set; }
        public Nullable<decimal> Latitude1 { get; set; }
        public Nullable<decimal> Longitude1 { get; set; }
        public Nullable<int> Altitude1 { get; set; }
        public Nullable<int> Speed1 { get; set; }
        public Nullable<int> Heading1 { get; set; }
        public Nullable<System.DateTime> UtcTimeStamp1 { get; set; }
        public Nullable<decimal> Latitude2 { get; set; }
        public Nullable<decimal> Longitude2 { get; set; }
        public Nullable<int> Altitude2 { get; set; }
        public Nullable<int> Speed2 { get; set; }
        public Nullable<int> Heading2 { get; set; }
        public Nullable<System.DateTime> UtcTimeStamp2 { get; set; }
        public Nullable<decimal> Latitude3 { get; set; }
        public Nullable<decimal> Longitude3 { get; set; }
        public Nullable<int> Altitude3 { get; set; }
        public Nullable<int> Speed3 { get; set; }
        public Nullable<int> Heading3 { get; set; }
        public Nullable<System.DateTime> UtcTimeStamp3 { get; set; }
        public Nullable<decimal> X1 { get; set; }
        public Nullable<decimal> Y1 { get; set; }
        public Nullable<decimal> Z1 { get; set; }
        public Nullable<decimal> X2 { get; set; }
        public Nullable<decimal> Y2 { get; set; }
        public Nullable<decimal> Z2 { get; set; }
        public Nullable<decimal> X3 { get; set; }
        public Nullable<decimal> Y3 { get; set; }
        public Nullable<decimal> Z3 { get; set; }
        public Nullable<decimal> Radius { get; set; }
        public System.DateTime CreatedAtUtcTimeStamp { get; set; }
        public Nullable<System.DateTime> ModifiedAtUtcTimeStamp { get; set; }
        public Nullable<bool> IsInFlight { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual Aircraft Aircraft { get; set; }
    }
}
