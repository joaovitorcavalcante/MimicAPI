using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.V1.Models;
using MimicAPI.V1.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly MimicContext DB;

        public WordRepository(MimicContext database)
        {
            DB = database;
        }

        public PaginationList<Word> Index(UrlQueryWord query)
        {
            var list = new PaginationList<Word>();
            var words = DB.Words.AsNoTracking().AsQueryable();

            if (query.Date.HasValue)
            {
                words = words.Where(w => w.Created > query.Date.Value || w.Updated > query.Date.Value);
            }

            if (query.PageNumber.HasValue)
            {
                var recordsQuantity = words.Count();

                words = words.Skip((query.PageNumber.Value - 1) * query.RecordsPage.Value).Take(query.RecordsPage.Value);

                var pagination = new Pagination();

                pagination.PageNumber = query.PageNumber.Value;
                pagination.RecordsPage = query.RecordsPage.Value;
                pagination.TotalRecords = recordsQuantity;
                pagination.TotalPages = (int) Math.Ceiling((double) recordsQuantity / query.RecordsPage.Value);

                list.Pagination = pagination;
            }

            list.Results.AddRange(words.ToList());

            return list;
        }

        public Word Show(int id)
        {
            return DB.Words.AsNoTracking().FirstOrDefault(w => w.Id == id);
            // return DB.Words.Find(id);
        }

        public void Store(Word word)
        {
            DB.Words.Add(word);
            DB.SaveChanges();
        }

        public void Update(Word word)
        {
            DB.Words.Update(word);
            DB.SaveChanges();
        }

        public void Remove(int id)
        {
            var word = Show(id);
            
            word.Active = false;

            DB.Words.Update(word);
            DB.SaveChanges();
        }
    }
}
