using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MusicPlayer.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {


            var builder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = "Data Source =.\\SQLEXPRESS; Initial Catalog = MusicPlayer; Persist Security Info = False; Integrated Security = True;";
            builder.UseSqlServer(connectionString);

            return new DataContext(builder.Options);
        }
    }
}
