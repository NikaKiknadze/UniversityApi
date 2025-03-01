namespace University.Domain.Models
{
    public class GetDtoWithCount<T>
    {
        public T? Data { get; set; }
        public int? Count { get; set; }
    }
}
