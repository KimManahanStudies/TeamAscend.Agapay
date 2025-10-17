using SQLite;
using System;
using System.Collections.Generic;

namespace TeamAscend.Agapay.App.Models;

public partial class AppPhonebook
{
    [PrimaryKey]
    public int ID { get; set; }

    public string ContactName { get; set; }

    public string ContactNumber { get; set; }

    public string Location { get; set; }

    public string Agency { get; set; }

    public string District { get; set; }

    public string Barangay { get; set; }

    public string BarangayCaptain { get; set; }

    public string EmergencyHotline { get; set; }

    public bool IsDeleted { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}