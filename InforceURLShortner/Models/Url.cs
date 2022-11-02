using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InforceURLShortner.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string FullURL { get; set; }
        public string ShortURL { get; set; }
        public string AuthorLogin { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
