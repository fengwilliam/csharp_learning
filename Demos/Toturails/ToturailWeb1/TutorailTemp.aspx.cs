using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;
using System.Diagnostics;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.IO;

namespace ToturailWeb1
{
    public partial class TutorailTemp : System.Web.UI.Page
    {
        public IEnumerable<IGrouping<string, TutorailChapter>> dicMenu;
        public TutorailChapter chapterContent;
        protected void Page_Load(object sender, EventArgs e)
        {
            //string strPath = System.IO.Directory.GetCurrentDirectory();
            //DirectoryInfo dcInfo = new DirectoryInfo(strPath);
            //foreach (FileInfo file in dcInfo.GetFiles())
            //{
            //    Console.Write(file.FullName);
            //}
            if (!IsPostBack)
            {
                dicMenu = GetTutorailMenuAndContent(out chapterContent);
            }
        }

        public IEnumerable<IGrouping<string, TutorailChapter>> GetTutorailMenuAndContent(out TutorailChapter chapter)
        {
            TutorailChapter tcChapter = null;
            chapter = tcChapter;
            if (Request.QueryString["item"] == ""
                || Request.QueryString["chapter"] == "")
            {
                return null;
            }

            int intItemId;
            int intChapterId;
            if(!int.TryParse(Request.QueryString["item"],out intItemId))
            {
                return null;
            }

            if(!int.TryParse(Request.QueryString["chapter"],out intChapterId))
            {
                return null;
            }

            List<TutorailChapter> chapters;
            Stopwatch sw = new Stopwatch();
            using (var db = new TutorailsDBContext())
            {
                sw.Start();
                chapters = (from c in db.Chapters
                            where c.tutorialitem == intItemId
                            select c
                            ).OrderBy(c=>c.chapter_seq).ToList<TutorailChapter>();
                sw.Stop();
                long t1 = sw.ElapsedMilliseconds;
                //var sqlParameter = new MySqlParameter("@tutorialitem", Request.QueryString["item"]);
                //sw.Start();
                //chapters = db.Chapters.SqlQuery("select * from dt_tutorial_chapter where tutorialitem = @tutorialitem", sqlParameter).ToList();
                //sw.Stop();
                //long t2 = sw.ElapsedMilliseconds;
                if (chapters == null || chapters.Count == 0)
                {
                    return null;
                }

                chapter = (from c in db.Chapters
                           where c.id == intChapterId
                           select c).Single();

                var listChapterGroups = chapters.GroupBy(p => p.chapter_group);

                foreach (var group in listChapterGroups)
                {
                    group.OrderBy(c => c.chapter_seq);
                }

                return listChapterGroups;
                           
            }
        }
    }
}