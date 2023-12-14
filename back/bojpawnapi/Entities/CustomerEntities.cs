using System.ComponentModel.DataAnnotations;
namespace bojpawnapi.Entities
{
    public class CustomerEntities : PersonBaseEntities
    {
        [Key]
        public int CustomerId {get; set;}
        public ICollection<CollateralTxEntities> CollateralTxls { get; set; }
    }
}