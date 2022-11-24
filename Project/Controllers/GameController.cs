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
    public class GameController : ControllerBase
    {
        private ProjectContext context;
        private IFileHandler fileHandler;

        public GameController(ProjectContext context, IFileHandler fileHandler)
        {
            this.context = context;
            this.fileHandler = fileHandler;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var gamelist = await context.GameInfos.ToListAsync();
            return Ok(gamelist);
        }
        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var gameInfo = await context.GameInfos.FindAsync(id);
            return Ok(gameInfo);
        }
        [HttpGet("getbyname/{name}")]
        public IActionResult GetByName(string name)
        {
            var gameInfo = context.GameInfos.Where(s => s.Title.Trim().ToLower().Contains(name.Trim().ToLower())).FirstOrDefault();
            return Ok(gameInfo);
        }
        [HttpGet("filter/{query}")]
        public IActionResult Filter(string query)
        {
            if (Regex.IsMatch(query, @"^\d+$"))
            {
                int id = Int32.Parse(query);
                var gameInfo = context.GameInfos.Find(id);
                return Ok(gameInfo);
            }
            else
            {
                var gameInfo = context.GameInfos.Where(s => s.Title.Trim().ToLower().Contains(query.Trim().ToLower())).FirstOrDefault();
                return Ok(gameInfo);
            }

        }
        [HttpPost("Create")]
        public async Task<IActionResult> Post(GameInfoViewModel model)
        {
            if (model.ImageFile != null)
            {
                model.ImagePath = fileHandler.UploadFile(model.ImageFile);
            }
            GameInfo gameInfo = new GameInfo()
            {
                ImagePath = model.ImagePath,
                Title = model.Title,
                Platform = model.Platform,
                ReleaseYear = model.ReleaseYear,
            };
            await context.AddAsync(gameInfo);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("Update")]
        public IActionResult Put(GameInfoViewModel model)
        {
            var dbmodel = context.GameInfos.Where(s => s.Id == model.Id).FirstOrDefault();
            if (model.ImageFile != null)
            {
                dbmodel.ImagePath = fileHandler.UpdateFile(dbmodel.ImagePath, model.ImageFile);
            }

            dbmodel.Title = model.Title;
            dbmodel.Platform = model.Platform;
            dbmodel.ReleaseYear = model.ReleaseYear;

            context.Entry(dbmodel).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var dbmodel = context.GameInfos.Where(s => s.Id == id).FirstOrDefault();
            if (!string.IsNullOrEmpty(dbmodel.ImagePath))
            {
                fileHandler.DeleteFile(dbmodel.ImagePath);
            }


            context.Entry(dbmodel).State = EntityState.Deleted;
            context.SaveChanges();
            return Ok();
        }
    }
}
