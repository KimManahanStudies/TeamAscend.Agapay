using SQLite;
using System;
using System.Collections.Generic;

namespace TeamAscend.Agapay.App.Models;

public partial class AppMapLocation
{
    [PrimaryKey]
    public int ID { get; set; }

    public int UserID { get; set; }

    public string Name { get; set; }

    public string MapCoordinates { get; set; }

    public string Address { get; set; }

    public string LocationType { get; set; }

    public string Description { get; set; }

    public string ExtraDetails { get; set; }

    public bool IsDeleted { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}