using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Models.ViewModels;
using Project.Services;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Project.Controllers
{
    [Route("[controller]")]
    //[ApiController]
    public class GameCharacterController : ControllerBase
    {
        private ProjectContext context;
        private IFileHandler fileHandler;

        public GameCharacterController(ProjectContext context, IFileHandler fileHandler)
        {
            this.context = context;
            this.fileHandler = fileHandler;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var gamelist = await context.GameCharacters.Include(s=>s.GameInfo).ToListAsync();
            return Ok(gamelist);
        }
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var gameInfo =  context.GameCharacters.Include(s=>s.GameInfo).Where(s=>s.Id ==id).FirstOrDefault();
            return Ok(gameInfo);
        }
        [HttpGet("getbyname/{name}")]
        public IActionResult GetByName(string name)
        {
            var gameInfo = context.GameCharacters.Include(s => s.GameInfo).Where(s => s.Name.Trim().ToLower().Contains(name.Trim().ToLower())).FirstOrDefault();
            return Ok(gameInfo);
        }
        [HttpGet("filter/{query}")]
        public IActionResult Filter(string query)
        {
            if (Regex.IsMatch(query, @"^\d+$"))
            {
                int id = Int32.Parse(query);
                var gameInfo = context.GameCharacters.Include(s => s.GameInfo).Where(s=>s.Id ==id).FirstOrDefault();
                return Ok(gameInfo);
            }
            else
            {
                var gameInfo = context.GameCharacters.Include(s => s.GameInfo).Where(s => s.Name.Trim().ToLower().Contains(query.Trim().ToLower())).FirstOrDefault();
                return Ok(gameInfo);
            }

        }
        [HttpPost("Create")]
        public async Task<IActionResult> Post(GameCharacterViewModel model)
        {
            if (model.CharacterImageFile != null)
            {
                model.CharacterImage = fileHandler.UploadFile(model.CharacterImageFile);
            }
            GameCharacter gameInfo = new GameCharacter()
            {
                CharacterImage = model.CharacterImage,
                Name = model.Name,
                GameInfoId = model.GameInfoId,
            };
            await context.AddAsync(gameInfo);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("Update")]
        public IActionResult Put(GameCharacterViewModel model)
        {
            var dbmodel = context.GameCharacters.Where(s => s.Id == model.Id).FirstOrDefault();
            if (model.CharacterImageFile != null)
            {
                dbmodel.CharacterImage = fileHandler.UpdateFile(dbmodel.CharacterImage, model.CharacterImageFile);
            }

            dbmodel.Name = model.Name;
            dbmodel.GameInfoId = model.GameInfoId;

            context.Entry(dbmodel).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var dbmodel = context.GameCharacters.Where(s => s.Id == id).FirstOrDefault();
            if (!string.IsNullOrEmpty(dbmodel.CharacterImage))
            {
                fileHandler.DeleteFile(dbmodel.CharacterImage);
            }


            context.Entry(dbmodel).State = EntityState.Deleted;
            context.SaveChanges();
            return Ok();
        }
    }
}
