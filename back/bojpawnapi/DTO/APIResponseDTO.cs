namespace bojpawnapi.DTO
{
    public class APIResponseDTO<T>
    {
        //https://github.com/the-cloud-camp/project-demo/issues/9
        public string Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public T Data { get; set; }

        public APIResponseDTO()
        {
            Timestamp = DateTime.UtcNow;
        }
    } 
}