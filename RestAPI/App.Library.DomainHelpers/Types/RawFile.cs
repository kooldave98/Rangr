namespace App.Library.DomainHelpers.Types
{
    public class RawFile
    {
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public byte[] Buffer { get; set; }

        public RawFile() { }

        public RawFile(string fileName, string mediaType, byte[] buffer)
        {
            FileName = fileName;
            MediaType = mediaType;
            Buffer = buffer;
        }
    }
}
