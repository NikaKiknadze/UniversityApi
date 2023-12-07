namespace UniversityApi.Dtos
{
    public class GetDtosWithCount<T>
    {
        public T? Data { get; set; }
        public int? Count { get; set; }
    }
}
