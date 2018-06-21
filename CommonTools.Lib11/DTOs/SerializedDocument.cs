namespace CommonTools.Lib11.DTOs
{
    public class SerializedDocument : DocumentDTOBase
    {
        public string    DocRefType   { get; set; }
        public int       DocRefId     { get; set; }
        public string    DocRefJson   { get; set; }
    }
}
