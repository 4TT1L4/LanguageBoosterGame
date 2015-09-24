using LanguageBoosterService;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LanguageBoosterBL
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class LanguageBoosterContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Word> Words { get; set; }
       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Player>().MapToStoredProcedures();
            modelBuilder.Entity<Score>().MapToStoredProcedures();
            modelBuilder.Entity<Word>().MapToStoredProcedures().HasKey(w=>w.Id);
        }

        static string connectionString = "server=127.0.0.1;port=3306;database=languageBooster;uid=User;password=Password";

        static bool IsFirstCall = true;

        public LanguageBoosterContext(string connStr) : base(connStr)
        {
            if (IsFirstCall)
            {
                Database.CreateIfNotExists();
                IsFirstCall = false;
            }
        }
        public LanguageBoosterContext() : this(connectionString)
        {             
        }
    }
}