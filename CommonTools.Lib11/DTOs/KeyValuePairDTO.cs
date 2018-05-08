using System;

namespace CommonTools.Lib11.DTOs
{
    public class KeyValuePairDTO : IDocumentDTO
    {
        public KeyValuePairDTO()
        {
        }


        public KeyValuePairDTO(string key, string value)
        {
            Name = key;
            Value = value;
        }


        public int       Id         { get; set; }
        public string    Author     { get; set; }
        public DateTime  Timestamp  { get; set; }
        public string    Remarks    { get; set; }

        public string    Name       { get; set; }
        public string    Value      { get; set; }
    }
}
