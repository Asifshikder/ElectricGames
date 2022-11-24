using Microsoft.EntityFrameworkCore;

namespace Project.Models
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {


        }
        public virtual DbSet<GameInfo> GameInfos { get; set; }
        public virtual DbSet<GameCharacter> GameCharacters { get; set; }
    }
}
