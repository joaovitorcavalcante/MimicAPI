using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Helpers;
using MimicAPI.V1.Models;
using MimicAPI.V1.Models.DTO;
using MimicAPI.V1.Repositories.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/words")]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository repo;
        private readonly IMapper mapper;
        public WordsController(IWordRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet("", Name = "Index")]
        public ActionResult Index([FromQuery] UrlQueryWord query)
        {
            var words = repo.Index(query);

            if (words.Pagination != null && query.PageNumber > words.Pagination.TotalPages) return NotFound();

            var list = mapper.Map<PaginationList<Word>, PaginationList<WordDTO>>(words);

            foreach (var word in list.Results)
            {
                word.Links = new List<LinkDTO>();
                word.Links.Add(new LinkDTO("self", Url.Link("Show", new { id = word.Id }), "GET"));
                word.Links.Add(new LinkDTO("self", Url.Link("Update", new { id = word.Id }), "PUT"));
                word.Links.Add(new LinkDTO("self", Url.Link("Remove", new { id = word.Id }), "DELETE"));
            }

            list.Links.Add(new LinkDTO("self", Url.Link("Index", query), "GET"));

            if (words.Pagination != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(words.Pagination));

                if (query.PageNumber + 1 <= words.Pagination.TotalPages)
                {
                    var queryString = new UrlQueryWord() { PageNumber = query.PageNumber + 1, RecordsPage = query.RecordsPage, Date = query.Date };

                    list.Links.Add(new LinkDTO("next", Url.Link("Show", queryString), "GET"));
                }

                if (query.PageNumber - 1 > 0)
                {
                    var queryString = new UrlQueryWord() { PageNumber = query.PageNumber - 1, RecordsPage = query.RecordsPage, Date = query.Date };

                    list.Links.Add(new LinkDTO("previous", Url.Link("Show", queryString), "GET"));
                }

            }

            return Ok(list);
        }

        [HttpGet("{id}", Name = "Show")]
        public ActionResult Show(int id)
        {
            var word = repo.Show(id);

            if (word == null) return NotFound();

            WordDTO wordDTO = mapper.Map<Word, WordDTO>(word);

            wordDTO.Links.Add(new LinkDTO("self", Url.Link("Show", new { id = wordDTO.Id }), "GET"));
            wordDTO.Links.Add(new LinkDTO("self", Url.Link("Update", new { id = wordDTO.Id }), "PUT"));
            wordDTO.Links.Add(new LinkDTO("self", Url.Link("Remove", new { id = wordDTO.Id }), "DELETE"));

            return Ok(wordDTO);
        }

        [Route("")]
        [HttpPost]
        public ActionResult Store([FromBody] Word word)
        {

            if (word == null) return BadRequest();

            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            word.Active = true;
            word.Created = DateTime.Now;

            repo.Store(word);

            WordDTO wordDTO = mapper.Map<Word, WordDTO>(word);

            wordDTO.Links.Add(new LinkDTO("self", Url.Link("Show", new { id = wordDTO.Id }), "GET"));

            return Created($"/api/words/{word.Id}", wordDTO);
        }

        [HttpPut("{id}", Name = "Update")]
        public ActionResult Update(int id, [FromBody] Word word)
        {
            var wordExist = repo.Show(id);
            if (wordExist == null) return NotFound();
            if (word == null) return BadRequest();

            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            word.Id = id;
            word.Active = wordExist.Active;
            word.Created = wordExist.Created;
            word.Updated = DateTime.Now;

            repo.Update(word);

            WordDTO wordDTO = mapper.Map<Word, WordDTO>(word);

            wordDTO.Links.Add(new LinkDTO("self", Url.Link("Show", new { id = wordDTO.Id }), "GET"));

            return Ok();
        }

        [HttpDelete("{id}", Name = "Remove")]
        public ActionResult Remove(int id)
        {
            var word = repo.Show(id);

            if (word == null) return NotFound();

            repo.Remove(id);

            return NoContent();
        }
    }
}
