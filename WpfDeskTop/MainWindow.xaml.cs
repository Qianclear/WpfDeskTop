using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDeskTop
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public DeskWindow deskwindow = null;//创建DeskWindow
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StopWindow_Click(object sender, RoutedEventArgs e)
        {
            deskwindow.Close();
            deskwindow = null;
        }

        private void ChoiceDeskBg_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //添加筛选措施
            openFileDialog.Filter = "(视频)|*.mp4;*.mkv;*.avi";
            openFileDialog.Title = "选择视频";
            bool? v = openFileDialog.ShowDialog();
            string filename = "";//文件名
            //判断是否选择了文件
            if (v == true) { filename = openFileDialog.FileName; }
            else { return; }

            if(deskwindow == null)
            {
                //先创建对象
                deskwindow = new DeskWindow();
                //添加组件
                //获取全部屏幕据
                Screen[] allScreens = System.Windows.Forms.Screen.AllScreens;
                List<Screen> screens = new List<Screen>(allScreens);
                //将屏幕按照X坐标进行排序
                screens.Sort(delegate(System.Windows.Forms.Screen s1,System.Windows.Forms.Screen s2)
                {
                    return s1.Bounds.X.CompareTo(s2.Bounds.X);
                });

                //按顺序添加组件
                int offset = 0; //初始化偏移量为0
                foreach (Screen screen in screens)
                {
                    deskwindow.AddDisplayComponent(offset, screen.Bounds.Width, screen.Bounds.Height);
                    offset += screen.Bounds.Width;
                }
                deskwindow.Width = offset;
                deskwindow.Height = SystemParameters.PrimaryScreenHeight;
                deskwindow.PlayDeskBg(filename);
                deskwindow.Show();
            }
            else
            {
                deskwindow.PlayDeskBg(filename);
            }
            Tools.SetDeskTop(deskwindow);
        }
    }
}
