using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Repositories.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository repo;
        public WordsController(IWordRepository repo)
        {
            this.repo = repo;
        }

        [Route("")]
        [HttpGet]
        public ActionResult Index([FromQuery] UrlQueryWord query)
        {
            var words = repo.Index(query);

            if (words.Pagination != null && query.PageNumber > words.Pagination.TotalPages) return NotFound();

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(words.Pagination));

            return Ok(words.ToList());
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult Show(int id)
        {
            var word = repo.Show(id);

            if (word == null) return NotFound();

            return Ok(word);
        }

        [Route("")]
        [HttpPost]
        public ActionResult Store([FromBody] Word word)
        {
            // Passed the id by reference
            repo.Store(word);

            return Created($"/api/words/{word.Id}", word);
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, [FromBody] Word word)
        {
            var wordExist = repo.Show(id);
            if (wordExist == null) return NotFound();

            word.Id = id;
            repo.Update(word);

            return Ok();
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Remove(int id)
        {
            var word = repo.Show(id);

            if (word == null) return NotFound();

            repo.Remove(id);

            return NoContent();
        }
    }
}
