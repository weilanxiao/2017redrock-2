using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace _2017_redrock2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static SQLiteAsyncConnection MyConnection { get; set; }
        List<Student> list = new List<Student>();
        public int _switch = 0;
        public MainPage()
        {
            this.InitializeComponent();

            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState00";
                if (e.NewSize.Width > 600)
                {
                    _switch = 1;
                    state = "VisualState01";
                }else if (e.NewSize.Width<=600&&_switch==1)
                {
                    state = "VisualState02";
                }
                VisualStateManager.GoToState(this, state, true);
            };
            First();
        }
        public async void First()
        {
            MyConnection = new SQLiteAsyncConnection("Students.db");
            await MyConnection.CreateTableAsync<Student>();
            var query = await(MyConnection.Table<Student>().Where(v => v._ID >= 1)).ToListAsync();
            list = new List<Student>(query);
            SQLList.ItemsSource = list;
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            Student stu = new Student();
            stu.ID = StudentID.Text;
            stu.Name = Name.Text;
            try
            {
                stu.Age = Convert.ToInt32(Age.Text);
            }
            catch
            {
                stu.Age = 0;
            }
            GetSQL(stu);
            ShowContent(stu);
        }

        public void ShowContent(Student stu)
        {
            name.Text = "名字：" + stu.Name;
            iD.Text = "学号：" + stu.ID;
            age.Text = "年龄：" + Convert.ToString(stu.Age);
        }


        public async void GetSQL(Student stu)
        {
            MyConnection = new SQLiteAsyncConnection("Students.db");
            await MyConnection.CreateTableAsync<Student>();
            await MyConnection.InsertAsync(stu);
            var query = await (MyConnection.Table<Student>().Where(v => v._ID >= 1)).ToListAsync();
            list = new List<Student>(query);
            SQLList.ItemsSource = list;
        }

        private void SQLList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var stu = (Student)e.ClickedItem;
            ShowContent(stu);
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MyConnection = new SQLiteAsyncConnection("Students.db");
            await MyConnection.CreateTableAsync<Student>();
            var query = await (MyConnection.Table<Student>().Where(v => v._ID >= 1)).ToListAsync();
            list = new List<Student>(query);
            SQLList.ItemsSource = list;
            name.Text = "名字：";
            iD.Text = "学号：";
            age.Text = "年龄：";
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            int i = SQLList.SelectedIndex;
            MyConnection = new SQLiteAsyncConnection("Students.db");
            await MyConnection.CreateTableAsync<Student>();
            var query = await (MyConnection.Table<Student>().Where(v => v._ID >= 1)).ToListAsync();            
            await MyConnection.DeleteAsync(query[i]);
            query = await (MyConnection.Table<Student>().Where(v => v._ID >= 1)).ToListAsync();
            list = new List<Student>(query);
            SQLList.ItemsSource = list;
            name.Text = "名字：";
            iD.Text = "学号：";
            age.Text = "年龄：";
        }

        private async void Search_Click(object sender, RoutedEventArgs e)//搜索方法怎么写？
        {
            //MyConnection = new SQLiteAsyncConnection("Students.db");
            //await MyConnection.CreateTableAsync<Student>();
            //var e1 = name.Text;
            //var e2 = iD.Text;
            //var e3 = age.Text;
            //if (e1 != null)
            //{
            //    var query = await (MyConnection.Table<Student>().Where(v => v.Name.Equals(name)).OrderBy(p => p._ID)).ToListAsync();
            //    list = new List<Student>(query);
            //    SQLList.ItemsSource = list;
            //}
        }
    }

    public class Student
    {
        [PrimaryKey,AutoIncrement]
        public int _ID { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public int Age { get; set; }
    }
}
