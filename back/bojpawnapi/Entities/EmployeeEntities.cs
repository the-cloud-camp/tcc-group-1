using System.ComponentModel.DataAnnotations;
namespace bojpawnapi.Entities
{
    public class EmployeeEntities : PersonBaseEntities
    {
        [Key]
        public int EmployeeId {get; set;}
        public ICollection<CollateralTxEntities> CollateralTxls { get; set; }
    }
}