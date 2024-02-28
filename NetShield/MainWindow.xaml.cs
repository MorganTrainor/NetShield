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
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Windows.Media.Animation;

namespace NetShield
{
    // Main
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ChangeBackgroundColor(Brush brush)
        {
            this.Background = brush;
        }

        // theme switch toggle off
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.From = (Color)FindResource("Dark");
            colorAnimation.To = (Color)FindResource("Light");
            colorAnimation.Duration = TimeSpan.FromSeconds(0.3);

            SolidColorBrush brush = new SolidColorBrush();
            brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

            ApplyBrushToElements(this, brush, false);
        }
        // theme switch toggle on
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.From = (Color)FindResource("Light");
            colorAnimation.To = (Color)FindResource("Dark");
            colorAnimation.Duration = TimeSpan.FromSeconds(0.1);

            SolidColorBrush brush = new SolidColorBrush();
            brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

            ApplyBrushToElements(this, brush, true);
        }

        // apply colour to all elements
        private void ApplyBrushToElements(DependencyObject obj, Brush brush, bool isLightMode)
        {
            Color lightColor = (Color)FindResource("Light");
            Color darkColor = (Color)FindResource("Dark");

            if (obj is Panel panel)
            {
                panel.Background = brush;
            }
            else if (obj is Button button)
            {
                button.Background = new SolidColorBrush(isLightMode ? lightColor : darkColor);
                button.Foreground = new SolidColorBrush(isLightMode ? Colors.Black : Colors.White);
            }

            int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                ApplyBrushToElements(child, brush, isLightMode);
            }
        }


        private void buttonClick1(object sender, RoutedEventArgs e)
        {
            testTextBlock.Text = "Button 1 clicked!";
        }
        private void buttonClick2(object sender, RoutedEventArgs e)
        {
            testTextBlock.Text = "Button 2 clicked!";
        }
        private void buttonClick3(object sender, RoutedEventArgs e)
        {
            testTextBlock.Text = "Button 3 clicked!";
        }
        private void buttonClick4(object sender, RoutedEventArgs e)
        {
            testTextBlock.Text = "Button 4 clicked!";
        }
        private void buttonClick5(object sender, RoutedEventArgs e)
        {
            testTextBlock.Text = "Button 5 clicked!";
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            // Change properties when mouse enters the button
            Button button = sender as Button;
            button.Background = Brushes.LightGray;
            button.Foreground = Brushes.Black;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            // Change properties when mouse leaves the button
            Button button = sender as Button;
            button.Background = Brushes.Transparent;
            button.Foreground = Brushes.White;
        }

        private void buttonListDevice(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            List<string> deviceList = new List<string>();

            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up && networkInterface.Supports(NetworkInterfaceComponent.IPv4))
                {
                    IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                    foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            deviceList.Add(ip.Address.ToString());
                        }
                    }
                }
            }

            // Call deviceListBox method to populate the ListBox
            listBox1.ItemsSource = null;
            deviceListBox(deviceList);
        }

        private void deviceListBox(List<string> deviceList)
        {
            foreach (string device in deviceList)
            {
                listBox1.Items.Add(device);
            }
        }
    }

}
