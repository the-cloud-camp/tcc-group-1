using System.ComponentModel.DataAnnotations;
namespace bojpawnapi.Entities
{
    public class CollateralTxEntities
    {
        [Key]
        public int CollateralId {get; set;}
        public String CollateralCode {get; set;}
        public string Store {get; set;}

        public int PrevCollateralId {get; set;} //เอาไว้ใช้กรณีต่อดอกเบี้ย
        public int CustomerId {get; set;}
        public CustomerEntities Customer {get; set;}

        public decimal LoanAmt {get; set;}

        public DateTime StartDate {get; set;}

        public DateTime EndDate {get; set;}

        public decimal Interest {get; set;}

        public DateTime PaidDate {get; set;}

        public int EmployeeId {get; set;}
        public EmployeeEntities Employee {get; set;}

        public string StatusCode {get; set;}
        
        public ICollection<CollateralTxDetailEntities> CollateralDetaills { get; set; }

    }
}