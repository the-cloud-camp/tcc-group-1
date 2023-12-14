using System.ComponentModel.DataAnnotations;
namespace bojpawnapi.DTO
{
    public class CollateralTxDetailDTO
    {
        [Key]
        public int CollateralDetailId {get; set;}
        public int CollateralId {get; set;}
        public int CollateralItemNo {get; set;}
        public string CollateralItemName {get; set;}

        public decimal CollateralPrice {get; set;}
    }
}