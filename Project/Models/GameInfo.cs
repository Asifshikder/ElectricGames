namespace Project.Models
{
    public class GameInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public int ReleaseYear { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<GameCharacter> GameCharacters { get; set; }
    }
}
