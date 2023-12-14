using System.ComponentModel.DataAnnotations;
namespace bojpawnapi.DTO
{
    public class CollateralTxDTO
    {
        [Key]
        public int CollateralId {get; set;}
        public String CollateralCode {get; set;}
        public string Store {get; set;}

        public int PrevCollateralId {get; set;} //เอาไว้ใช้กรณีต่อดอกเบี้ย

        public int CustomerId {get; set;}

        public decimal LoanAmt {get; set;}

        public DateTime StartDate {get; set;}

        public DateTime EndDate {get; set;}

        public decimal Interest {get; set;}

        public DateTime PaidDate {get; set;}

        public int EmployeeId {get; set;}

        public string StatusCode {get; set;}
        
        public ICollection<CollateralTxDetailDTO> CollateralDetaills { get; set; }
    }
}