using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CharpterEditer
{
    /// <summary>
    /// Interaction logic for ChapterEdit.xaml
    /// </summary>
    public partial class ChapterEdit : Window
    {
        public int EditChapterID { get; set; }
        public ChapterEdit()
        {
            InitializeComponent();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (txtSource.Text == "" || txtReplace.Text == "")
            {
                MessageBox.Show("请输入正确的值");
                return;
            }

            using (var tutorailDB = new TutorailsDBContext())
            {
                var paraSource = new MySqlParameter("@source", txtSource.Text);
                var paraReplace = new MySqlParameter("@replace", txtReplace.Text);
                var id = new MySqlParameter("@id", EditChapterID);
                var result = tutorailDB.Database.ExecuteSqlCommand(@"update dt_tutorial_chapter
                        set charpter_content = replace(charpter_content, @source, @replace)
                        where id = @id;",paraSource,paraReplace,id);
                
                if (result != 0)
                {
                    this.Close();
                }
            }
        }
    }
}
