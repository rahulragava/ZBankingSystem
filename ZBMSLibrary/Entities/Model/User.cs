using System;
using SQLite;

namespace ZBMSLibrary.Entities.Model
{
    [Table(nameof(User))]
    public class User
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PAN { get; set; }
        public DateTime LastLoggedOn { get; set; }
    }
}