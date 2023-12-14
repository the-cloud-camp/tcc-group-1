using bojpawnapi.DTO;
namespace bojpawnapi.Service
{
    public interface ICollateralTxService
    {
        Task<CollateralTxDTO> GetCollateralTxByIdAsync(int id);
        Task<IEnumerable<CollateralTxDTO>> GetCollateralTxsAsync();
        Task<CollateralTxDTO> AddCollateralTxAsync(CollateralTxDTO collateralTxDTO);
        Task<bool> UpdateCollateralTxAsync(CollateralTxDTO collateralTxDTO);
        Task<bool> DeleteCollateralTxAsync(int id);

        //Business
        Task<CollateralTxDTO> AddPawnCollateralTxAsync(CollateralTxDTO pCollateralPayload);
        Task<CollateralTxDTO> AddRolloverCollateralTxAsync(CollateralTxDTO pCollateralPayload);
        Task<bool> AddRedeemCollateralTxAsync(CollateralTxDTO pCollateralPayload);
        
    }
}