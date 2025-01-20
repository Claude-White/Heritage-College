
namespace HFA.Models
{
    public class Image
    {
        public int ImageId { get; set; }

        public string? Path { get; set; }

        public DateTime DateUploaded { get; set; } = DateTime.Now;

        public Image()
        {
            ImageId = 0;
            Path = string.Empty;
            DateUploaded = DateTime.Now;
        }

        public Image(string path)
        {
            ImageId = 0;
            Path = path;
            DateUploaded = DateTime.Now;
        }

        public static implicit operator Task<object>(Image? v)
        {
            throw new NotImplementedException();
        }
    }
}
