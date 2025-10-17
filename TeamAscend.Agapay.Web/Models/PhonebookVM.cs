using System;
using System.Collections.Generic;

namespace TeamAscend.Agapay.Web.Models;

public class PhonebookVM:Phonebook
{
    public string DisplayName { get; set; }
    public string DisplayNumber { get; set; }
}

public class PhonebookPageVM
{
    public List<PhonebookVM> Contacts { get; set; }
}