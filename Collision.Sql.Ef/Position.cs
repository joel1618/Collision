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
        public decimal Temp1Latitude { get; set; }
        public decimal Temp1Longitude { get; set; }
        public int Temp1Altitude { get; set; }
        public int Temp1Speed { get; set; }
        public Nullable<int> Temp1Heading { get; set; }
        public System.DateTime Temp1UtcTimeStamp { get; set; }
        public Nullable<decimal> Temp2Latitude { get; set; }
        public Nullable<decimal> Temp2Longitude { get; set; }
        public Nullable<int> Temp2Altitude { get; set; }
        public Nullable<int> Temp2Speed { get; set; }
        public Nullable<int> Temp2Heading { get; set; }
        public Nullable<System.DateTime> Temp2UtcTimeStamp { get; set; }
        public Nullable<decimal> X1 { get; set; }
        public Nullable<decimal> Y1 { get; set; }
        public Nullable<int> Z1 { get; set; }
        public Nullable<decimal> X2 { get; set; }
        public Nullable<decimal> Y2 { get; set; }
        public Nullable<int> Z2 { get; set; }
        public Nullable<decimal> X3 { get; set; }
        public Nullable<decimal> Y3 { get; set; }
        public Nullable<int> Z3 { get; set; }
        public Nullable<decimal> X4 { get; set; }
        public Nullable<decimal> Y4 { get; set; }
        public Nullable<int> Z4 { get; set; }
        public Nullable<decimal> X5 { get; set; }
        public Nullable<decimal> Y5 { get; set; }
        public Nullable<int> Z5 { get; set; }
        public Nullable<decimal> X6 { get; set; }
        public Nullable<decimal> Y6 { get; set; }
        public Nullable<int> Z6 { get; set; }
        public Nullable<decimal> X7 { get; set; }
        public Nullable<decimal> Y7 { get; set; }
        public Nullable<int> Z7 { get; set; }
        public Nullable<decimal> X8 { get; set; }
        public Nullable<decimal> Y8 { get; set; }
        public Nullable<int> Z8 { get; set; }
        public System.DateTime CreatedAtUtcTimeStamp { get; set; }
        public Nullable<bool> IsInFlight { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual Aircraft Aircraft { get; set; }
    }
}
