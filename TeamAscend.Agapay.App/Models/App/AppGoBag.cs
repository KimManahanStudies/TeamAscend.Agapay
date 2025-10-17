using SQLite;
using System;
using System.Collections.Generic;

namespace TeamAscend.Agapay.App.Models;

public class AppGoBag
{
    [PrimaryKey]
    [AutoIncrement]
    public int ID { get; set; }

    public int GoPlanID { get; set; }

    public int UserID { get; set; }

    public string Category { get; set; }

    public string Description { get; set; }

    public bool IsDeleted { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}