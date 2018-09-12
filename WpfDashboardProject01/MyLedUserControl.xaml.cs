using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDashboardProject01
{
    /// <summary>
    /// Interaction logic for MyLedUserControl.xaml
    /// </summary>
    public partial class MyLedUserControl : UserControl
    {
        public MyLedUserControl()
        {
            InitializeComponent();

            innerEllipse.Fill = new SolidColorBrush(Colors.Red);
        }

        public void SetOn()
        {
            innerEllipse.Fill = new SolidColorBrush(Colors.Lime);
        }

        public void SetOff()
        {
            innerEllipse.Fill = new SolidColorBrush(Colors.Red);
        }

        public void SetWarning()
        {
            innerEllipse.Fill = new SolidColorBrush(Colors.Yellow);
        }

        public void SetPowerOff()
        {
            innerEllipse.Fill = new SolidColorBrush(Colors.Black);
        }

        public void SetPowerOn()
        {
            innerEllipse.Fill = new SolidColorBrush(Colors.WhiteSmoke);
        }

        public void SetColor(Color c)
        {
            innerEllipse.Fill = new SolidColorBrush(c);
        }
    }
}
