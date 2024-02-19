using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Vlc.DotNet.Forms;

namespace WpfDeskTop
{
    /// <summary>
    /// DeskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DeskWindow : Window
    {
        //由于可能存在多个屏幕，故可能需要多次创建
        public List<VlcControl> controlsList = new List<VlcControl>();
        public List<WindowsFormsHost> windowsFormsList = new List<WindowsFormsHost>();

        public string[] options = new string[]
        {
            "--input-repeat=65535"
        };

        public DeskWindow()
        {
            InitializeComponent();
        }

        //添加组件  参数：偏移量，宽，高
        public void AddDisplayComponent(int offset, int width, int height)
        {
            DockPanel dockPanel = new DockPanel();//添加dockPannel组件
            dockPanel.Margin = new Thickness(offset, 0, 0, 0); //属性设置
            dockPanel.HorizontalAlignment = HorizontalAlignment.Left;
            dockPanel.Visibility = Visibility.Collapsed;
            dockPanel.Width = width;
            dockPanel.Height = height;

            WindowsFormsHost windowsFormsHost = new WindowsFormsHost();
            VlcControl vlcControl = new VlcControl();
            //初始化VLC
            DirectoryInfo directoryInfo = new DirectoryInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"libvlc",IntPtr.Size == 4?"win-x86":"win-x64"));
            vlcControl.BeginInit();
            vlcControl.VlcLibDirectory = directoryInfo;//指定初始化库的位置
            //字符串列表的形式存储参数，故在上面
            vlcControl.VlcMediaplayerOptions = options;
            vlcControl.EndInit();

            //组件放置嵌套，一层一层
            windowsFormsHost.Child = vlcControl;
            dockPanel.Children.Add(windowsFormsHost);
            rootGird.Children.Add(dockPanel);

            //组件创建完毕之后加入列表
            controlsList.Add(vlcControl);
            windowsFormsList.Add(windowsFormsHost);
        }

        //播放
        public void PlayDeskBg(string furl)
        {
            //由于之前设置的组件是不可见的，故第一步是先显示出来
            //先找到所有DockPanel然后遍历显示
            IEnumerable<DockPanel> enumerable = rootGird.Children.OfType<DockPanel>();
            enumerable.ToList().ForEach((item) => { item.Visibility = Visibility.Visible; });
            foreach(VlcControl vlc in controlsList)
            {
                vlc.Audio.Volume = 0;
                vlc.Play(new Uri( furl));
            }
        }

        //释放资源
        private void Window_Closed(object sender, EventArgs e)
        {
            foreach(VlcControl vlc in controlsList) {  vlc.Dispose(); }
            foreach(WindowsFormsHost host in windowsFormsList) {  host.Dispose(); }
        }
    }
}
