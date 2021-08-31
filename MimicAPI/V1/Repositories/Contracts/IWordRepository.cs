using MimicAPI.Helpers;
using MimicAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Repositories.Contracts
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
