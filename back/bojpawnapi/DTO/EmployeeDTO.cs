using bojpawnapi.DTO;
using bojpawnapi.Entities;
using System.Text.Json.Serialization;

namespace bojpawnapi.DTO
{
    public class EmployeeDTO : PersonBaseEntities 
    {
        public int EmployeeId {get; set;}

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Username {get; set;}
        //https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/ignore-properties?pivots=dotnet-8-0
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Password {get; set;}
    }
}