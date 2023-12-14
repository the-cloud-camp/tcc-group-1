using AutoMapper;
using bojpawnapi.DTO;
using bojpawnapi.Entities;

namespace bojpawnapi.DTO
{
    public class AppMapperProfile:Profile
    {
        public AppMapperProfile()
        {
            //.ReverseMap(); 
            //for bidirectional mapping ไม่ใส่ จะ Error https://stackoverflow.com/questions/62083715/net-core-automapper-missing-type-map-configuration-or-unsupported-mapping
            
            CreateMap<CollateralTxDTO, CollateralTxEntities>().ReverseMap();
            CreateMap<CollateralTxDetailDTO, CollateralTxDetailEntities>().ReverseMap();

            CreateMap<CustomerDTO, CustomerEntities>().ReverseMap();
            CreateMap<EmployeeDTO, EmployeeEntities>().ReverseMap();


            CreateMap<CollateralTxDTO, CollateralTxDTO>();
            CreateMap<CollateralTxDetailDTO, CollateralTxDetailDTO>();
        }
    }   
}