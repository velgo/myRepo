using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Test.Models
{
    public class DataContext: DbContext
    {
        public DbSet<Sentence> Sentences { get; set; }
    }
}