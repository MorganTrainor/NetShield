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
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Management;
namespace NetShield
{
    // Main
    public partial class MainWindow : Window
    {
        bool nightTheme = false;

        public MainWindow()
        {
            InitializeComponent();
            SetHomePageVisibleAndEnabled();
            // Simulate pressing the theme toggle button
            ToggleButton_Unchecked(null, new RoutedEventArgs());
        }
        private void SetHomePageVisibleAndEnabled()
        {
            // Iterate over all elements in the document
            foreach (UIElement element in mainBody.Children)
            {
                // Check if the element has a Tag property
                if (element is FrameworkElement frameworkElement && frameworkElement.Tag != null)
                {
                    // Check if the Tag matches 'homepage'
                    if (frameworkElement.Tag.ToString() == "homepage")
                    {
                        // Show the element and make it interactable
                        frameworkElement.Visibility = Visibility.Visible;
                        frameworkElement.IsEnabled = true;
                    }
                    else
                    {
                        // Hide the element and make it non-interactable
                        frameworkElement.Visibility = Visibility.Collapsed;
                        frameworkElement.IsEnabled = false;
                    }
                }
            }
        }
        public void ChangeBackgroundColor(Brush brush)
        {
            this.Background = brush;
        }

        // theme switch toggle off
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            nightTheme = false;
            SolidColorBrush sidebarBrush = new SolidColorBrush(Colors.Plum);
            SolidColorBrush mainBodyBrush = new SolidColorBrush(Colors.Indigo);

            // Apply the Plum color to Sidebar1 with animation
            Grid sidebar = this.FindName("Sidebar1") as Grid;
            if (sidebar != null)
                AnimateBackgroundColor(sidebar, sidebar.Background, sidebarBrush);

            // Apply the Indigo color to mainBody with animation
            Grid mainBody = this.FindName("mainBody") as Grid;
            if (mainBody != null)
                AnimateBackgroundColor(mainBody, mainBody.Background, mainBodyBrush);

            // Change menu buttons to Plum
            ChangeMenuButtonsColor(Colors.Plum, Colors.Indigo);
        }

        // theme switch toggle on
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            nightTheme = true;
            SolidColorBrush sidebarBrush = new SolidColorBrush(Colors.Indigo);
            SolidColorBrush mainBodyBrush = new SolidColorBrush(Colors.Plum);

            // Apply the Indigo color to Sidebar1 with animation
            Grid sidebar = this.FindName("Sidebar1") as Grid;
            if (sidebar != null)
                AnimateBackgroundColor(sidebar, sidebar.Background, sidebarBrush);

            // Apply the Plum color to mainBody with animation
            Grid mainBody = this.FindName("mainBody") as Grid;
            if (mainBody != null)
                AnimateBackgroundColor(mainBody, mainBody.Background, mainBodyBrush);

            // Change menu buttons to Indigo
            ChangeMenuButtonsColor(Colors.Indigo, Colors.Plum);
        }

        // Helper method to animate background color
        private void AnimateBackgroundColor(UIElement element, Brush fromBrush, Brush toBrush)
        {
            // Default color if fromBrush is null
            Color defaultFromColor = Colors.Transparent;

            if (fromBrush == null)
            {
                fromBrush = new SolidColorBrush(defaultFromColor);
            }

            if (element is Control control)
            {
                var colorAnimation = new ColorAnimation
                {
                    From = ((SolidColorBrush)fromBrush).Color,
                    To = ((SolidColorBrush)toBrush).Color,
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                var brush = new SolidColorBrush(((SolidColorBrush)fromBrush).Color);
                brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                control.Background = brush;
            }
            else if (element is Panel panel)
            {
                var colorAnimation = new ColorAnimation
                {
                    From = ((SolidColorBrush)fromBrush).Color,
                    To = ((SolidColorBrush)toBrush).Color,
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                var brush = new SolidColorBrush(((SolidColorBrush)fromBrush).Color);
                brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                panel.Background = brush;
            }
            else if (element is Border border)
            {
                var colorAnimation = new ColorAnimation
                {
                    From = ((SolidColorBrush)fromBrush).Color,
                    To = ((SolidColorBrush)toBrush).Color,
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                var brush = new SolidColorBrush(((SolidColorBrush)fromBrush).Color);
                brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                border.Background = brush;
            }
            // Add more cases as necessary for other types that have a Background property
        }

        // Helper method to change menu buttons color
        private void ChangeMenuButtonsColor(Color backgroundColor, Color foregroundColor)
        {
            foreach (UIElement element in Sidebar1.Children)
            {
                if (element is Button button && button.Tag != null && button.Tag.ToString() == "menuButton")
                {
                    // Create new SolidColorBrushes for each button to avoid freezing issues
                    SolidColorBrush backgroundBrush = new SolidColorBrush(((SolidColorBrush)button.Background).Color);
                    SolidColorBrush foregroundBrush = new SolidColorBrush(foregroundColor);

                    // Apply foreground color
                    button.Foreground = foregroundBrush;

                    // Adding a simple fade transition for the color change
                    var anim = new ColorAnimation
                    {
                        From = ((SolidColorBrush)button.Background).Color,
                        To = backgroundColor,
                        Duration = TimeSpan.FromSeconds(0.3)
                    };

                    // Apply the animation to a new brush and set it as the button's background
                    backgroundBrush.BeginAnimation(SolidColorBrush.ColorProperty, anim);
                    button.Background = backgroundBrush;
                }
            }
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
            else if (obj is Grid grid && grid.Name == "Sidebar1") // Check if the object is the Sidebar1 Grid
            {
                // Apply the desired color to the Sidebar1 Grid
                grid.Background = brush;
            }

            int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                ApplyBrushToElements(child, brush, isLightMode);
            }
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            // Change properties when mouse enters the button
            Button button = sender as Button;
            if (button != null)
            {
                if (nightTheme)
                    button.Background = new SolidColorBrush(Colors.LightGray); // Light color for dark theme
                else
                    button.Background = new SolidColorBrush(Colors.DarkGray); // Dark color for light theme
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            // Reset properties when mouse leaves the button
            Button button = sender as Button;
            if (button != null)
            {
                if (nightTheme)
                    button.Background = new SolidColorBrush(Colors.Indigo); // Dark theme color
                else
                    button.Background = new SolidColorBrush(Colors.Plum); // Light theme color
            }
        }

        private void button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Revert the background color of the button to its original color when released
            Button clickedButton = (Button)sender;
            clickedButton.Background = SystemColors.ControlBrush; // Or any other color you want to revert to
        }

        private void buttonClick1(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Check if the button's tag is 'networkScan'
            if (clickedButton.Tag != null && clickedButton.Tag.ToString() == "menuButton")
            {
                // Iterate over all elements in the document
                foreach (UIElement element in mainBody.Children)
                {
                    // Check if the element has a Tag property
                    if (element is FrameworkElement frameworkElement && frameworkElement.Tag != null)
                    {
                        // Check if the Tag matches 'networkScan'
                        if (frameworkElement.Tag.ToString() == "homepage")
                        {
                            // Show the element and make it interactable
                            frameworkElement.Visibility = Visibility.Visible;
                            frameworkElement.IsEnabled = true;
                        }
                        else
                        {
                            // Hide the element and make it non-interactable
                            frameworkElement.Visibility = Visibility.Collapsed;
                            frameworkElement.IsEnabled = false;
                        }
                    }
                }
            }

            // Perform other actions as needed
            testTextBlock.Text = "Button 1 clicked!";
        }
        private void buttonClick2(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Check if the button's tag is 'networkScan'
            if (clickedButton.Tag != null && clickedButton.Tag.ToString() == "menuButton")
            {
                // Iterate over all elements in the document
                foreach (UIElement element in mainBody.Children)
                {
                    // Check if the element has a Tag property
                    if (element is FrameworkElement frameworkElement && frameworkElement.Tag != null)
                    {
                        // Check if the Tag matches 'networkScan'
                        if (frameworkElement.Tag.ToString() == "networkScan")
                        {
                            // Show the element and make it interactable
                            frameworkElement.Visibility = Visibility.Visible;
                            frameworkElement.IsEnabled = true;
                        }
                        else
                        {
                            // Hide the element and make it non-interactable
                            frameworkElement.Visibility = Visibility.Collapsed;
                            frameworkElement.IsEnabled = false;
                        }
                    }
                }
            }

            // Perform other actions as needed
            testTextBlock.Text = "Button 2 clicked!";
        }
        private void buttonClick3(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Check if the button's tag is 'networkScan'
            if (clickedButton.Tag != null && clickedButton.Tag.ToString() == "menuButton")
            {
                // Iterate over all elements in the document
                foreach (UIElement element in mainBody.Children)
                {
                    // Check if the element has a Tag property
                    if (element is FrameworkElement frameworkElement && frameworkElement.Tag != null)
                    {
                        // Check if the Tag matches 'networkScan'
                        if (frameworkElement.Tag.ToString() == "portScan")
                        {
                            // Show the element and make it interactable
                            frameworkElement.Visibility = Visibility.Visible;
                            frameworkElement.IsEnabled = true;
                        }
                        else
                        {
                            // Hide the element and make it non-interactable
                            frameworkElement.Visibility = Visibility.Collapsed;
                            frameworkElement.IsEnabled = false;
                        }
                    }
                }
            }

            // Perform other actions as needed
            testTextBlock.Text = "Button 3 clicked!";
        }
        private void buttonClick4(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Check if the button's tag is 'networkScan'
            if (clickedButton.Tag != null && clickedButton.Tag.ToString() == "menuButton")
            {
                // Iterate over all elements in the document
                foreach (UIElement element in mainBody.Children)
                {
                    // Check if the element has a Tag property
                    if (element is FrameworkElement frameworkElement && frameworkElement.Tag != null)
                    {
                        // Check if the Tag matches 'networkScan'
                        if (frameworkElement.Tag.ToString() == "tipsTricks")
                        {
                            // Show the element and make it interactable
                            frameworkElement.Visibility = Visibility.Visible;
                            frameworkElement.IsEnabled = true;
                        }
                        else
                        {
                            // Hide the element and make it non-interactable
                            frameworkElement.Visibility = Visibility.Collapsed;
                            frameworkElement.IsEnabled = false;
                        }
                    }
                }
            }

            // Perform other actions as needed
            testTextBlock.Text = "Button 4 clicked!";
        }
        private void buttonClick5(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Check if the button's tag is 'networkScan'
            if (clickedButton.Tag != null && clickedButton.Tag.ToString() == "menuButton")
            {
                // Iterate over all elements in the document
                foreach (UIElement element in mainBody.Children)
                {
                    // Check if the element has a Tag property
                    if (element is FrameworkElement frameworkElement && frameworkElement.Tag != null)
                    {
                        // Check if the Tag matches 'networkScan'
                        if (frameworkElement.Tag.ToString() == "settings")
                        {
                            // Show the element and make it interactable
                            frameworkElement.Visibility = Visibility.Visible;
                            frameworkElement.IsEnabled = true;
                        }
                        else
                        {
                            // Hide the element and make it non-interactable
                            frameworkElement.Visibility = Visibility.Collapsed;
                            frameworkElement.IsEnabled = false;
                        }
                    }
                }
            }

            // Perform other actions as needed
            testTextBlock.Text = "Button 5 clicked!";
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

        private void firmwareButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear the ListBox
            listFirmware.Items.Clear();

            // Get the firmware versions
            HashSet<string> firmwareVersions = GetFirmwareVersions();

            // Add each firmware version to the ListBox
            foreach (string version in firmwareVersions)
            {
                listFirmware.Items.Add(version);
            }
        }

        private HashSet<string> GetFirmwareVersions()
        {
            HashSet<string> firmwareVersions = new HashSet<string>();

            // Query WMI for BIOS information
            ManagementObjectSearcher biosSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            foreach (ManagementObject queryObj in biosSearcher.Get())
            {
                firmwareVersions.Add($"BIOS Version: {queryObj["Version"]}");
            }

            // Query WMI for Keyboard information
            ManagementObjectSearcher keyboardSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Keyboard");
            foreach (ManagementObject queryObj in keyboardSearcher.Get())
            {
                firmwareVersions.Add($"Keyboard: {queryObj["Description"]}");
            }

            // Query WMI for PointingDevice (Mouse) information
            ManagementObjectSearcher mouseSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PointingDevice");
            foreach (ManagementObject queryObj in mouseSearcher.Get())
            {
                firmwareVersions.Add($"Mouse: {queryObj["Description"]}");
            }

            // Query WMI for DiskDrive information
            ManagementObjectSearcher driveSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject queryObj in driveSearcher.Get())
            {
                firmwareVersions.Add($"Drive: {queryObj["Model"]}");
            }

            return firmwareVersions;
        }
        private void listFirmware_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Get the selected item
            string selectedItem = listFirmware.SelectedItem as string;

            if (selectedItem != null)
            {
                // Check if the selected item is a BIOS entry
                if (selectedItem.StartsWith("BIOS Version:"))
                {
                    // Extract the BIOS version from the selected item
                    string biosVersion = selectedItem.Substring("BIOS Version: ".Length);

                    // Open a web browser and search for how to update the specific BIOS version
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = $"https://www.google.com/search?q=how+to+update+BIOS+version+{biosVersion}",
                        UseShellExecute = true
                    });
                }
                else
                {
                    // Open the Device Manager
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "devmgmt.msc",
                        UseShellExecute = true
                    });
                }
            }
        }
    }
}
