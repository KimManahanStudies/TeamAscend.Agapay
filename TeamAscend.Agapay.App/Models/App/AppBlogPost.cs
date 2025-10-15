using SQLite;
using System;
using System.Collections.Generic;

namespace TeamAscend.Agapay.App.Models;

public partial class AppBlogPost
{
    [PrimaryKey]
    public int ID { get; set; }

    public int UserID { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string BlogStatus { get; set; }

    public string BlogType { get; set; }

    public string CoverPhoto { get; set; }

    public bool IsDeleted { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}