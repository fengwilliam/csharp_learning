using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToturailWeb1
{
    public class TutorailsDBContext : DbContext
    {
        public TutorailsDBContext()
            : base("TutorailContext")
        { 
        
        }
        public DbSet<TutorailCategory> Categorys { get; set; }
        public DbSet<TutorailItem> Items { get; set; }
        public DbSet<TutorailChapter> Chapters { get; set; }
    }

    [Table("dt_category")]
    public class TutorailCategory
    {
        public int id { get; set; }
        public string module_name { get; set; }
        public string module_desc { get; set; }
        public string module_url { get; set; }
        public string original_url { get; set; }
        public int? parentid { get; set; }

    }

    [Table("dt_tutorial_item")]
    public class TutorailItem
    {
        public int id { get; set; }
        public string item_name { get; set; }
        public int category { get; set; }
        public string item_desc { get; set; }
        public DateTime? create_date { get; set; }
        public DateTime? modify_date { get; set; }
        public string item_src_url { get; set; }
        public string item_img { get; set; }
        public string item_ico { get; set; }
        public string item_header_img { get; set; }
        public string original_url { get; set; }
        public string item_group { get; set; }
        public int? item_status { get; set; }
        public int? chapter_count { get; set; }

    }

    [Table("dt_tutorial_chapter")]
    public class TutorailChapter
    { 
        public int id{get;set;}
        public int tutorialitem{get;set;}
        public string chapter_name{get;set;}
        public DateTime? create_date{get;set;}
        public DateTime? modify_date{get;set;}
        public int? chapter_seq{get;set;}
        public string chapter_group{get;set;}
        public string chapter_url{get;set;}
        public string charpter_content{get;set;}
        public string charpter_content_en{get;set;}
        public string content_lan{get;set;}
        public string original_url{get;set;}

    }
}