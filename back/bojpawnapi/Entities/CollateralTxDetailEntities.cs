using System.ComponentModel.DataAnnotations;
namespace bojpawnapi.Entities
{
    public class CollateralTxDetailEntities
    {
        [Key]
        public int CollateralDetailId {get; set;}
        public int CollateralId {get; set;}
        public int CollateralItemNo {get; set;}
        public string CollateralItemName {get; set;}
        public decimal CollateralPrice {get; set;}

        public CollateralTxEntities CollateralTx {get; set;} 
    }
}