namespace CommonTools.Lib11.DTOs
{
    public interface IHasDTO<T>
        where T : IDocumentDTO
    {
        T DTO  { get; }
    }
}
