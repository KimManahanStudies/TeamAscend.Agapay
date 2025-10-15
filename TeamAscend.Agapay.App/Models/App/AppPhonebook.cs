using SQLite;
using System;
using System.Collections.Generic;

namespace TeamAscend.Agapay.App.Models;

public partial class AppPhonebook
{
    [PrimaryKey]
    public int ID { get; set; }

    public string ContactName { get; set; }

    public string ContactNo { get; set; }

    public string Location { get; set; }

    public string BarangayName { get; set; }

    public bool IsDeleted { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}