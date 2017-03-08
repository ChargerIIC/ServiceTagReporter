using DellWarranty;
using System;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using System.Windows;

namespace Service_Tag_Reporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            grdWarrantyView.ItemsSource = WarrantyEngine.dataTable.DefaultView;
        }

        #region private methods

        private void fetchServiceTags()
        {
            var tasks = txtServiceTags.Text.Split('\n').Select(str => Task.Factory.StartNew((arg) =>
            {
                str = str.Trim();

                WarrantyEngine.FetchTag(str);

                Dispatcher.BeginInvoke(new queryDelegate(this.UpdateProgressBar), str);
            }, str)).ToList();
            pbarLow.Maximum = tasks.Count;

            Task.WaitAll(tasks.ToArray());
        }

        private void UpdateProgressBar(string serial)
        {
            pbarLow.Value++;
            if (pbarLow.Value != pbarLow.Maximum)
            {
                lblStatus.Content = ("Fetching Service Information...");
            }
            else
            {
                lblStatus.Content = "Finished!";
            }
        }

        #endregion private methods

        #region Gui Events

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            WarrantyEngine.dataTable.Rows.Clear();
            pbarLow.Value = 0;
            fetchServiceTags();
        }

        private delegate void queryDelegate(string serial);

        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            //pulls Serial number from target's WMI and dumps it into the serial box
            string strResults = null;
            string path = @"\\" + txtQueryTarget.Text + "\\root\\cimv2";

            try
            {
                var wmiScope = new ManagementScope(path);
                var wmiOQuery = new ObjectQuery("SELECT SerialNumber FROM Win32_Bios");
                var wmiSearcher = new ManagementObjectSearcher(wmiScope, wmiOQuery);
                //Execute WMI query
                ManagementObjectCollection wmiResults = wmiSearcher.Get();
                foreach (ManagementObject Serial in wmiResults)
                {
                    strResults += Serial["SerialNumber"].ToString();
                }
                txtServiceTags.Text += strResults + "\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error scouting computer: " + ex.Message, "Error Locating Service Tag", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion Gui Events

    }
}
