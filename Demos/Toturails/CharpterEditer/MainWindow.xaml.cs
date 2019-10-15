using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CharpterEditer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChapterEdit editer;
        GenarateType genarateType;
        public MainWindow()
        {
            InitializeComponent();
            this.cbCategory.SelectionChanged += cbCategory_SelectionChanged;
            this.cbTutorail.SelectionChanged += cbTutorail_SelectionChanged;
            this.lvChapter.MouseDoubleClick += lvChapter_MouseDoubleClick;
            this.lvChapter.SelectionChanged+=lvChapter_SelectionChanged;
            wbBrowser.Navigated += wbBrowser_Navigated;
            wbLocalBrowser.Navigated += wbLocalBrowser_Navigated;
            InitData();

        }

        void wbLocalBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            SuppressScriptErrors((WebBrowser)sender, true);
        }

        void wbBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            SuppressScriptErrors((WebBrowser)sender, true);
        }

        public void SuppressScriptErrors(WebBrowser wb, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }

        void lvChapter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
        }

        void lvChapter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectChapter = ((ListView)e.Source).SelectedItem as TutorailChapter;

            if (selectChapter == null)
            {
                return;
            }

            wbBrowser.Navigate(new Uri(selectChapter.chapter_url));
            wbLocalBrowser.Navigate(new Uri("http://localhost:25133/TutorailTemp.aspx?item=" + cbTutorail.SelectedValue.ToString() + "&chapter=" + selectChapter.id));
        }

        void cbTutorail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTutorail.SelectedValue == null)
            {
                return;
            }

            List<TutorailChapter> chapters;
            int selectValue = int.Parse(cbTutorail.SelectedValue.ToString());
            using (var tutoraildb = new TutorailsDBContext())
            {
                chapters = (from c in tutoraildb.Chapters
                            where c.tutorialitem == selectValue
                            select c).OrderBy(c=>c.chapter_seq).ToList();

                foreach (TutorailChapter c in chapters)
                {
                    c.chapter_name = c.id +"_"+ c.chapter_name;
                }

                lvChapter.ItemsSource = chapters;
                lvChapter.DisplayMemberPath = "chapter_name";
                lvChapter.SelectedValuePath = "id";
            }
        }

        void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<TutorailItem> tutorails;
            int selectValue = 49;
            if (cbCategory.SelectedValue != null)
            {
                selectValue = int.Parse(cbCategory.SelectedValue.ToString());
            }
            
            using (var tutoraildb = new TutorailsDBContext())
            {
                tutorails = (from i in tutoraildb.Items
                             where i.category == selectValue
                             select i).ToList<TutorailItem>();

                foreach (TutorailItem i in tutorails)
                {
                    i.item_name = i.id + "_" + i.item_name;
                }

                if (tutorails != null)
                {
                    this.cbTutorail.ItemsSource = tutorails;
                    this.cbTutorail.DisplayMemberPath = "item_name";
                    this.cbTutorail.SelectedValuePath = "id";
                    this.cbTutorail.SelectedIndex = 0;
                }

            }
        }

        public void InitData()
        {
            using (var tutoraildb = new TutorailsDBContext())
            {
                List<TutorailCategory> categorys = tutoraildb.Categorys.ToList<TutorailCategory>();

                foreach (TutorailCategory tc in categorys)
                {
                    tc.module_name = tc.id + "_" + tc.module_name;
                }

                this.cbCategory.ItemsSource = categorys;
                this.cbCategory.SelectedValuePath = "id";
                this.cbCategory.DisplayMemberPath = "module_name";
                this.cbCategory.SelectedValue = 49;


            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            var selectChapter = lvChapter.SelectedItem as TutorailChapter;

            if (selectChapter == null)
            {
                return;
            }

            //wbBrowser.Navigate(new Uri(selectChapter.chapter_url));
            wbLocalBrowser.Navigate(new Uri("http://localhost:25133/TutorailTemp.aspx?item=" + cbTutorail.SelectedValue.ToString() + "&chapter=" + selectChapter.id));
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectChapter = lvChapter.SelectedItem as TutorailChapter;
            if (selectChapter == null)
            {
                return;
            }

            editer = new ChapterEdit();
            editer.EditChapterID = selectChapter.id;
            editer.Show();
        }

        private void btnGenarate_Click(object sender, RoutedEventArgs e)
        {
            GenarateHtmFile();
            MessageBox.Show("生成成功");
        }

        public IEnumerable<IGrouping<string, TutorailChapter>> GetTutorailMenuAndContent(int tutorailid, int chapterid, out TutorailChapter chapter)
        {
            TutorailChapter tcChapter = null;
            chapter = tcChapter;
            

            List<TutorailChapter> chapters;

            using (var db = new TutorailsDBContext())
            {
                chapters = (from c in db.Chapters
                            where c.tutorialitem == tutorailid
                            select c
                            ).OrderBy(c => c.chapter_seq).ToList<TutorailChapter>();

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
                           where c.id == chapterid
                           select c).Single();

                var listChapterGroups = chapters.GroupBy(p => p.chapter_group);

                foreach (var group in listChapterGroups)
                {
                    group.OrderBy(c => c.chapter_seq);
                }

                return listChapterGroups;

            }
        }

        public IEnumerable<IGrouping<string, TutorailChapter>> GetTutorailMenuFromLocal(int tutorailid, List<TutorailChapter> chapters, GenarateType gtype)
        {
            IEnumerable<IGrouping<string, TutorailChapter>> result = null;
            if (gtype == GenarateType.Category)
            {
                result = (from c in chapters
                          where c.tutorialitem == tutorailid
                          select c).OrderBy(p => p.chapter_seq).GroupBy(p => p.chapter_group);
            }

            if (gtype == GenarateType.Tutorail)
            {
                result = chapters.OrderBy(p => p.chapter_seq).GroupBy(p => p.chapter_group);
            }

            return result;
        }

        public void GenarateHtmFile()
        { 
            IEnumerable<IGrouping<string, TutorailChapter>> dicMenu;
            TutorailChapter chapterContent;
            List<TutorailChapter> listChapters = null;
            List<TutorailItem> listTutorails = null;
            if (cbTutorail.SelectedValue == ""
                || lvChapter.SelectedValue == "")
            {
                return;
            }

            int intItemId;
            int intChapterId;
            int tutorailCategoryID;
            if (!int.TryParse(cbTutorail.SelectedValue.ToString(), out intItemId))
            {
                return;
            }

            if (!int.TryParse(lvChapter.SelectedValue.ToString(), out intChapterId))
            {
                return;
            }

            tutorailCategoryID = int.Parse(cbCategory.SelectedValue.ToString());

            using (var tutoraildb = new TutorailsDBContext())
            {
                if (cbGenarate.SelectionBoxItem.ToString() == "本类别")
                {
                    listChapters = (from c in tutoraildb.Chapters
                                    join t in tutoraildb.Items on c.tutorialitem equals t.id
                                    where t.category == tutorailCategoryID
                                    select c).ToList<TutorailChapter>();
                    listTutorails = (from t in tutoraildb.Items
                                     where t.category == tutorailCategoryID
                                     select t).ToList();

                    genarateType = GenarateType.Category;
                }

                if (cbGenarate.SelectionBoxItem.ToString() == "本教程")
                {
                    listChapters = (from c in tutoraildb.Chapters
                                    where c.tutorialitem == intItemId
                                    select c).ToList<TutorailChapter>();
                    genarateType = GenarateType.Tutorail;
                }

                if (cbGenarate.SelectionBoxItem.ToString() == "当前页面")
                {
                    listChapters = (from c in tutoraildb.Chapters
                                    where c.id == intChapterId
                                    select c).ToList<TutorailChapter>();
                    genarateType = GenarateType.Page;
                }
            }

            if (genarateType == GenarateType.Category)
            {
                foreach (TutorailItem item in listTutorails)
                {
                    dicMenu = GetTutorailMenuFromLocal(item.id, listChapters, genarateType);
                    List<TutorailChapter> tchapters = (from c in listChapters
                                                       where c.tutorialitem == item.id
                                                       select c).ToList();
                    foreach (TutorailChapter chapter in tchapters)
                    {
                        CreateFile(dicMenu, chapter);
                    }

                }
            }
            else if (genarateType == GenarateType.Tutorail)
            {
                dicMenu = GetTutorailMenuFromLocal(intItemId, listChapters, genarateType);
                foreach (TutorailChapter chapter in listChapters)
                {
                    CreateFile(dicMenu, chapter);
                }
            }
            else
            {
                dicMenu = GetTutorailMenuAndContent(intItemId, intChapterId, out chapterContent);
                if(listChapters.Count > 0)
                {
                    CreateFile(dicMenu,listChapters[0]);
                }
            }


            
        }

        private void btnRedownload_Click(object sender, RoutedEventArgs e)
        {

        }

        public void CreateFile(IEnumerable<IGrouping<string, TutorailChapter>> menus, TutorailChapter chapterContent)
        {
            string htmContent = "<html xmlns=\"http://www.w3.org/1999/xhtml\">"
+ "<head runat=\"server\">"
+ "    <title></title>"
+ "    <link href=\"../Content/style-min.css\" rel=\"stylesheet\" />"
                //+"    <%--<script type=\"text/javascript\" src=\"Scripts/script-min-v4.js?v=2\"></script>--%>"
+ "</head>"
+ "<body>"
+ "    <div class=\"wrapLoader\">"
+ "        <div class=\"imgLoader\">"
+ "            <img src=\"/images/loading-cg.gif\" alt=\"\" width=\"70\" height=\"70\">"
+ "        </div>"
+ "    </div>"
+ "    <div id=\"right_obs\" class=\"display-none\" onclick=\"close_obs_sidenav()\"></div>"
+ "    <header>"
+ "        <div class=\"container\">"
+ "            <h1 class=\"logo\">"
+ "                <a href=\"index.htm\" title=\"哈喽教程\">"
+ "                    哈喽教程"
+ "                </a>"
+ "            </h1>"
+ "            <ul class=\"tp-inline-block pull-right\" id=\"tp-head-icons\">"
+ "                <li>"
+ "                    <div class=\"tp-second-nav tp-display-none tp-pointer\" onclick=\"openNav()\">"
+ "                        <i class=\"fa fa-th-large fa-lg\"></i>"
+ "                    </div>"
+ "                </li>"
+ "            </ul>"
+ "            <button class=\"btn btn-responsive-nav btn-inverse\" data-toggle=\"collapse\" data-target=\".nav-main-collapse\" id=\"pull\" style=\"top: 290px!important\"> <i class=\"icon icon-bars\"></i> </button>"
+ "        </div>"
+ "        <div class=\"sidenav\" id=\"mySidenav\">"
+ "            <div class=\"navbar nav-main\">"
+ "                <div class=\"container\">"
+ "                    <nav class=\"nav-main mega-menu\">"
+ "                        <ul class=\"nav nav-pills nav-main\" id=\"mainMenu\">"
+ "                            <li class=\"dropdown no-sub-menu\">"
+ "                                <!--<div class=\"searchform-popup\">"
+ "                                    <input class=\"header-search-box\" type=\"text\" id=\"search-string\" name=\"q\" placeholder=\"Search your favorite tutorials...\" onfocus=\"if (this.value == 'Search your favorite tutorials...') {this.value = '';}\" onblur=\"if (this.value == '') {this.value = 'Search your favorite tutorials...';}\" autocomplete=\"off\" style=\"\">"
+ "                                    <div class=\"magnifying-glass\"><i class=\"icon-search\"></i> Search </div>"
+ "                                </div>-->"
+ "                            </li>"
+ "                        </ul>"
+ "                    </nav>"
+ "                </div>"
+ "            </div>"
+ "        </div>"
+ "    </header>"
+ "    <div style=\"clear:both;\"></div>"
+ "    <div role=\"main\" class=\"main\">"
+ "        <div class=\"container\">"
+ "            <div class=\"row\">"
+ "                <div class=\"col-md-2\">"
+ "                    <aside class=\"sidebar\">";
            foreach (var group in menus)
            {
                htmContent += "<ul class=\"nav nav-list primary left-menu\">";
                htmContent += "<li class=\"heading\">" + group.Key + "</li>";
                foreach (var chapter in group)
                {
                    if (chapter.original_url == chapterContent.original_url)
                    {
                        htmContent += "<li><a target=\"_top\" href=\"" + chapter.original_url + "\" style=\"background-color: rgb(214, 214, 214);\">" + chapter.chapter_name + "</a></li>";
                    }
                    else
                    {
                        htmContent += "<li><a target=\"_top\" href=\"" + chapter.original_url + "\">" + chapter.chapter_name + "</a></li>";
                    }
                }
                htmContent += "</ul>";
            }
            htmContent += "</aside>"
+ "                </div>"
+ "                <div class=\"row\">"
+ "                    <div class=\"content\">";
            htmContent += chapterContent.charpter_content;
            htmContent += "</div>"
+ "                    <div class=\"row\">"
+ "                        <div class=\"col-md-2\" id=\"rightbar\">"
+ "                            <div class=\"simple-ad\">"
+ "                                主目录"
+ "                            </div>"
+ "                            <div class=\"rightgooglead\">"
+ "                            广告"
+ "                            </div>"
+ "                        </div>"
+ "                    </div>"
+ "                </div>"
+ "            </div>"
+ "        </div>"
+ "        <div class=\"footer-copyright\">"
+ "            <div class=\"container\">"
+ "                <div class=\"row\">"
+ "                </div>"
+ "            </div>"
+ "        </div>"
+ "    </div>"
+ "    <div id=\"privacy-banner\" style=\"display: none;\">"
+ "        <div>"
+ "            <p>"
+ "                We use cookies to provide and improve our services. By using our site, you consent to our Cookies Policy."
+ "                <a id=\"banner-accept\" href=\"javascript:void(0)\">Accept</a>"
+ "                <a id=\"banner-learn\" href=\"/about/about_cookies.htm\" target=\"_blank\">Learn more</a>"
+ "            </p>"
+ "        </div>"
+ "    </div>"
+ "</body>"
+ "</html>";

            string filePath = txtSavePath.Text + "/" + chapterContent.original_url;
            FileStream fs = File.Create(filePath);
            byte[] writeData = System.Text.Encoding.Default.GetBytes(htmContent);
            fs.Write(writeData, 0, writeData.Length);
            fs.Flush();
            fs.Close();
        }

    }
}
