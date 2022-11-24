namespace Project.Models.ViewModels
{
    public class GameCharacterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GameInfoId { get; set; }
        public virtual GameInfo GameInfo { get; set; }

        public string CharacterImage { get; set; }
        public IFormFile CharacterImageFile { get; set; }
    }
}
