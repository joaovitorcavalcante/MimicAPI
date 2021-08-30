using Microsoft.EntityFrameworkCore;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Database
{
    public class MimicContext : DbContext
    {
        public MimicContext(DbContextOptions<MimicContext> Options) : base(Options)
        {

        }

        public DbSet<Word> Words { get; set; }
    }
}
