using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Windows.Controls.Primitives;
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
            SolidColorBrush sidebarBrush = new SolidColorBrush(Colors.WhiteSmoke);
            SolidColorBrush mainBodyBrush = new SolidColorBrush(Colors.MidnightBlue);

            // Apply the WhiteSmoke color to Sidebar1 with animation
            Grid sidebar = this.FindName("Sidebar1") as Grid;
            if (sidebar != null)
                AnimateBackgroundColor(sidebar, sidebar.Background, sidebarBrush);

            // Apply the MidnightBlue color to mainBody with animation
            Grid mainBody = this.FindName("mainBody") as Grid;
            if (mainBody != null)
                AnimateBackgroundColor(mainBody, mainBody.Background, mainBodyBrush);

            // Change menu buttons to WhiteSmoke
            ChangeMenuButtonsColor(Colors.WhiteSmoke, Colors.MidnightBlue);
        }

        // theme switch toggle on
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            nightTheme = true;
            SolidColorBrush sidebarBrush = new SolidColorBrush(Colors.MidnightBlue);
            SolidColorBrush mainBodyBrush = new SolidColorBrush(Colors.WhiteSmoke);

            // Apply the MidnightBlue color to Sidebar1 with animation
            Grid sidebar = this.FindName("Sidebar1") as Grid;
            if (sidebar != null)
                AnimateBackgroundColor(sidebar, sidebar.Background, sidebarBrush);

            // Apply the WhiteSmoke color to mainBody with animation
            Grid mainBody = this.FindName("mainBody") as Grid;
            if (mainBody != null)
                AnimateBackgroundColor(mainBody, mainBody.Background, mainBodyBrush);

            // Change menu buttons to MidnightBlue
            ChangeMenuButtonsColor(Colors.MidnightBlue, Colors.WhiteSmoke);
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
            if (Sidebar1 == null)
            {
                Debug.WriteLine("Sidebar1 is null");
                return;
            }

            foreach (UIElement element in Sidebar1.Children)
            {
                if (element is Button button && button.Tag != null && button.Tag.ToString() == "menuButton")
                {
                    // Ensure the button's background is not null before accessing its color
                    if (button.Background == null)
                        button.Background = new SolidColorBrush(Colors.Transparent); // Set a default color if null

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
                    button.Background = new SolidColorBrush(Colors.MidnightBlue); // Dark theme color
                else
                    button.Background = new SolidColorBrush(Colors.WhiteSmoke); // Light theme color
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
                        if (frameworkElement.Tag.ToString() == "hardwareScan")
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



        }



        private void buttonListDevice(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string localIP = GetLocalIPAddress();
            if (string.IsNullOrEmpty(localIP))
            {
                listBox1.Items.Add("Failed to obtain local IP address.");
                return;
            }

            string subnetMask = "255.255.255.0"; // Common subnet mask
            string baseIP = CalculateBaseIP(localIP, subnetMask);

            List<Task> pingTasks = new List<Task>();

            // Adjust the range based on your actual subnet mask
            for (int i = 1; i <= 254; i++) // Scanning the whole subnet
            {
                string ip = $"{baseIP}.{i}";
                pingTasks.Add(PingAndLogAsync(ip));
            }

            Task.WhenAll(pingTasks).ContinueWith(tasks =>
            {
                Dispatcher.Invoke(() =>
                {
                    listBox1.Items.Add("Ping scan complete.");
                });
            });
        }

        private async Task PingAndLogAsync(string ipAddress)
        {
            using (Ping pingSender = new Ping()) // Create a new Ping instance for each task
            {
                try
                {
                    PingReply reply = await pingSender.SendPingAsync(ipAddress, 1000); // 1000 ms timeout
                    if (reply.Status == IPStatus.Success)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            listBox1.Items.Add($"Active IP: {ipAddress} - Response Time: {reply.RoundtripTime}ms");
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            listBox1.Items.Add($"No response from: {ipAddress} - Status: {reply.Status}");
                        });
                    }
                }
                catch (PingException ex) // More specific exception type for ping failures
                {
                    Dispatcher.Invoke(() =>
                    {
                        listBox1.Items.Add($"Ping exception for {ipAddress}: {ex.Message}");
                    });
                    Debug.WriteLine($"Ping exception to {ipAddress}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        listBox1.Items.Add($"General error pinging {ipAddress}: {ex.Message}");
                    });
                    Debug.WriteLine($"General error pinging {ipAddress}: {ex.Message}");
                }
            }
        }

        private string CalculateBaseIP(string ipAddress, string subnetMask)
        {
            byte[] ipBytes = IPAddress.Parse(ipAddress).GetAddressBytes();
            byte[] maskBytes = IPAddress.Parse(subnetMask).GetAddressBytes();
            byte[] baseIPBytes = new byte[ipBytes.Length];

            for (int i = 0; i < baseIPBytes.Length; i++)
            {
                baseIPBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }

            return new IPAddress(baseIPBytes).ToString();
        }
        private async Task<PingReply> PingAsync(string ipAddress, Ping pingSender)
        {
            try
            {
                return await pingSender.SendPingAsync(ipAddress, 1000); // 1000 ms timeout
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ping to {ipAddress} failed: {ex.Message}");
                return null; // Return null if an exception occurs
            }
        }

        private string GetLocalIPAddress()
        {
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530); // Google's DNS server IP
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    return endPoint.Address.ToString();
                }
            }
            catch
            {
                return null; // Return null if an exception occurs
            }
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
                        FileName = "devmgmt.`   msc",
                        UseShellExecute = true
                    });
                }
            }
        }

        private int currentTipIndex = 0;
        private string[] tips = { "Use strong passwords.", "Enable two-factor authentication.", "Regularly update your software." };

        private void ScrollLeft(object sender, RoutedEventArgs e)
        {
            currentTipIndex = (currentTipIndex - 1 + tips.Length) % tips.Length;
            UpdateTipText();
        }

        private void ScrollRight(object sender, RoutedEventArgs e)
        {
            currentTipIndex = (currentTipIndex + 1) % tips.Length;
            UpdateTipText();
        }

        private void UpdateTipText()
        {
            TipText.Text = $"Tip {currentTipIndex + 1}: {tips[currentTipIndex]}";
        }
        private void listFirmware_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
