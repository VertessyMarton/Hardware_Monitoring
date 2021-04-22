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
using System.Management;
using Microsoft.Win32;
using System.Web;
using System.Diagnostics;

namespace HardwareMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0);
            timer.Start();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            CPU();
            GPU();
            RAM();
            Motherboard();
            Storage();
            Software();
        }

        private void CPU()
        {
            System.Management.ManagementClass vmi = new System.Management.ManagementClass("Win32_Processor");
            var providers = vmi.GetInstances();

            foreach (var provider in providers)
            {
                string cpuName = provider["Name"].ToString();
                string cpuStatus = provider["Status"].ToString();
                int cpuClock = Convert.ToInt32(provider["CurrentClockSpeed"]);
                int cpuMag = Convert.ToInt32(provider["NumberofCores"]);
                int cpuThread = Convert.ToInt32(provider["ThreadCount"]);

                cpunev.Text = "Name:" + " " + cpuName.ToString();
                cpustatus.Text = "Status:" + "    " + cpuStatus.ToString() + " " + "MHz";
                cpuorajel.Text = "Core Clock:" + " " + cpuClock.ToString();
                cpumag.Text = "Number of Core:" + "  " + cpuMag.ToString();
                cpuszal.Text = "Number of Thread:" + "  " + cpuThread.ToString();
            }
        }

        private void GPU()
        {
            System.Management.ManagementClass vmi = new System.Management.ManagementClass("Win32_VideoController");
            var providers = vmi.GetInstances();

            foreach (var provider in providers)
            {
                string gpuName = provider["Name"].ToString();
                string gpuStatus = provider["Status"].ToString();
                string gpuClock = provider["DeviceID"].ToString();
                string gpuDriver = provider["DriverVersion"].ToString();


                gpunev.Text = "Name:" + "   " + gpuName.ToString();
                gpustatus.Text = "Status:" + "   " + gpuStatus.ToString();
                gpuid.Text = "Devide ID:" + "   " + gpuClock.ToString();
                gpudriver.Text = "Driver:" + "  " + gpuDriver.ToString();
            }
        }

        private void RAM()
        {
            System.Management.ManagementClass vmi = new System.Management.ManagementClass("Win32_PhysicalMemory");
            var providers = vmi.GetInstances();

            foreach (var provider in providers)
            {
                string ramName = provider["Name"].ToString();
                long ramCapa = Convert.ToInt64(provider["Capacity"]);
                int ramSpeed = Convert.ToInt32(provider["Speed"]);

                ramnev.Text = "Type: " + "  " + ramName.ToString();
                ramkapacitas.Text = "Capaticy: " + "  " + ramCapa.ToString() + "  " + "byte";
                ramolajel.Text = "Speed:" + "  " + ramSpeed.ToString() + "  " + "MHz";
            }
        }

        private void Motherboard()
        {
            System.Management.ManagementClass vmi = new System.Management.ManagementClass("Win32_BaseBoard");
            var providers = vmi.GetInstances();

            foreach (var provider in providers)
            {
                string boardModel = provider["Product"].ToString();
                string boardManufact = provider["Manufacturer"].ToString();
                string boardStatus = provider["Status"].ToString();

                mbtipus.Text = "Type:" + "  " + boardModel.ToString();
                mbgyarto.Text = "Manufacturer:" + "  " + boardManufact.ToString();
                mbstatus.Text = "Status:" + "  " + boardStatus.ToString();
            }
        }

        private void Storage()
        {
            System.Management.ManagementClass vmi = new System.Management.ManagementClass("Win32_DiskDrive");
            var providers = vmi.GetInstances();

            foreach (var provider in providers)
            {
                string DiskName = provider["Model"].ToString();
                long DiskSize = Convert.ToInt64(provider["Size"]);
                int Partition = Convert.ToInt32(provider["Partitions"]);
                string DiskStatus = provider["Status"].ToString();

                diskmodel.Text = "Model:" + "  " + DiskName.ToString();
                disksize.Text = "Size:" + "  " + DiskSize.ToString() + "  " + "byte";
                diskpart.Text = "Partition:" + "  " + Partition.ToString();
                diskstatus.Text = "Status:" + "  " + DiskStatus.ToString();
            }
        }

        private void Software()
        {
            List<Software> soft = new List<Software>();
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
			using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
			{
				foreach (string subkey_name in key.GetSubKeyNames())
				{
					using (RegistryKey subkey = key.OpenSubKey(subkey_name))
					{
						if (subkey.GetValue("DisplayName") != null)
						{
							soft.Add(new Software
							{
								Program = (string) subkey.GetValue("DisplayName"),
								Version = (string) subkey.GetValue("DisplayVersion"),
								Date = (string) subkey.GetValue("InstallDate"),
								Publisher = (string) subkey.GetValue("Publisher"),
							});
						}
					}
				}
			}
			Programs.ItemsSource = soft;
        }
    }

}
