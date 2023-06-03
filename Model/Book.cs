using System.Text.Json.Serialization;

namespace LibraryAPI.Models {
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishedYear {get; set; }
        public int? UserId { get; set; }
    }
}