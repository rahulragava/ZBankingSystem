using SQLite;

namespace ZBMSLibrary.Entities.Model
{
    [Table(nameof(Branch))]
    public class Branch
    {
        //public string Id { get; set; }
        public string Name { get;set; }
        [PrimaryKey]
        public string Ifsc{ get; set; }
    }
}