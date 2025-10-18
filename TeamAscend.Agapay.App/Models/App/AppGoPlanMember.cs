using SQLite;
using System;
using System.Collections.Generic;

namespace TeamAscend.Agapay.App.Models;

public class AppGoPlanMember
{
    [PrimaryKey]
    [AutoIncrement]
    public int ID { get; set; }

    public int GoPlanID { get; set; }

    public int PlanOwnerID { get; set; }

    public int MemberID { get; set; }

    public string PhoneNumber { get; set; }

    public string FamilyMemberRole { get; set; }

    public bool IsDeleted { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}