using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class LocalAppDb : DbContext
    {
        public DbSet<Prefecture> prefecture { get; set; }
        public DbSet<Student> students { get; set; }
        public string connectionString2 = "";
        //Other Data Sets....
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
            string dbhost = Regex.Match(connection, @"Data Source=(.+?);").Groups[1].Value;
            string server = dbhost.Split(':')[0].ToString();
            string port = dbhost.Split(':')[1].ToString();
            string dbname = Regex.Match(connection, @"Database=(.+?);").Groups[1].Value;
            string dbusername = Regex.Match(connection, @"User Id=(.+?);").Groups[1].Value;
            string dbpassword = Regex.Match(connection, @"Password=(.+?)$").Groups[1].Value;
            connectionString2 = $@"server={server};userid={dbusername};password={dbpassword};database={dbname};port={port};pooling = false; convert zero datetime=True;";
            optionsBuilder.UseMySQL(connectionString2);
        }
    }

    [Table("student")]
    public class Student
    {
        [Key]
        public int id { get; set; }
    }

    [Table("prefecture_mst")]
    public class Prefecture
    {
        [Key]
        public int prefecture_no { get; set; }
        public string prefecture_name { get; set; }
        public string prefecture_kana { get; set; }
        public DateTime regist_date { get; set; }
        public DateTime update_date { get; set; }
        public bool del_flg { get; set; }
    }

}
