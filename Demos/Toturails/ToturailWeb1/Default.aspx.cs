using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ToturailWeb1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.drpCategory.SelectedIndexChanged += drpCategory_SelectedIndexChanged;
            this.drpTutorail.SelectedIndexChanged += drpTutorail_SelectedIndexChanged;
            this.btnGoto.Click += btnGoto_Click;

            if (!IsPostBack)
            {
                using (var tutoraildb = new TutorailsDBContext())
                {
                    List<TutorailCategory> categorys = tutoraildb.Categorys.ToList<TutorailCategory>();
                    this.drpCategory.DataSource = categorys;
                    this.drpCategory.DataValueField = "id";
                    this.drpCategory.DataTextField = "module_name";
                    this.drpCategory.DataBind();
                    this.drpCategory.SelectedIndex = 0;
                }
            }
        }

        void btnGoto_Click(object sender, EventArgs e)
        {
            Response.Redirect("TutorailTemp.aspx?item="+drpTutorail.SelectedValue+"&chapter=" +drpChapter.SelectedValue);
        }


        void drpTutorail_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<TutorailChapter> chapters;
            int selectValue = int.Parse(drpTutorail.SelectedValue);
            using (var tutoraildb = new TutorailsDBContext())
            {
                chapters = (from i in tutoraildb.Chapters
                             where i.tutorialitem == selectValue
                             select i).ToList<TutorailChapter>();

                if (chapters != null)
                {
                    this.drpChapter.DataSource = chapters;
                    this.drpChapter.DataTextField = "chapter_name";
                    this.drpChapter.DataValueField = "id";
                    this.drpChapter.DataBind();
                }

            }
        }

        void drpCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<TutorailItem> tutorails;
            int selectValue = int.Parse(drpCategory.SelectedValue);
            using (var tutoraildb = new TutorailsDBContext())
            {
                tutorails = (from i in tutoraildb.Items
                             where i.category == selectValue
                             select i).ToList<TutorailItem>();

                if (tutorails != null)
                {
                    this.drpTutorail.DataSource = tutorails;
                    this.drpTutorail.DataTextField = "item_name";
                    this.drpTutorail.DataValueField = "id";
                    this.drpTutorail.DataBind();
                }

            }
        }
    }
}