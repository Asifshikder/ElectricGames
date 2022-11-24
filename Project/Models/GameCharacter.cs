namespace Project.Models
{
    public class GameCharacter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GameInfoId { get; set; }
        public virtual GameInfo GameInfo { get; set; }
        public string CharacterImage { get; set; }
    }
}
