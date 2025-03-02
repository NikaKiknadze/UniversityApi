namespace University.Domain.Models
{
    public class GetDtoWithCount<T>
    {
        public required T Data { get; set; }
        public int Count { get; set; }
    }
}
