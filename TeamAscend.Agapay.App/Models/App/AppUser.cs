using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamAscend.Agapay.App.Model
{
    public class AppUser
    {
        [PrimaryKey]
        [AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string Username { get; set; }

        [NotNull]
        public string Role { get; set; }

        [NotNull]
        public string Password { get; set; }

        [NotNull]
        public string FirstName { get; set; }

        [NotNull]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [NotNull]
        public string BirthDate { get; set; }

        [NotNull]
        public string EmailAddress { get; set; }

        [NotNull]
        public string ContactNo1 { get; set; }

        public string ContactNo2 { get; set; }

        [NotNull]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [NotNull]
        public string City { get; set; }

        [NotNull]
        public string Region { get; set; }

        [NotNull]
        public string Barangay { get; set; }

        [NotNull]
        public string ZipPostCode { get; set; }

        [NotNull]
        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
