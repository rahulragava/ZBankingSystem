using ZBMSLibrary.Entities.Enums;

namespace ZBMSLibrary.Entities.Model
{
    public class Beneficiary
    {
        public string Id { get; set; }
        public string Ifsc { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public BeneficiaryType BeneficiaryType { get; set;}
    }
}