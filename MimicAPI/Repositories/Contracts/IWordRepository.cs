using MimicAPI.Helpers;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repositories.Contracts
{
    public interface IWordRepository
    {
        PaginationList<Word> Index(UrlQueryWord query);
        Word Show(int id);
        void Store(Word word);
        void Update(Word word);
        void Remove(int id);
    }
}
