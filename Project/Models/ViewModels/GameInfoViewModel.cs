namespace Project.Models.ViewModels
{
    public class GameInfoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public int ReleaseYear { get; set; }
        public string ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
