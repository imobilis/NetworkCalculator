using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NetworkCalculator.Resources;

namespace NetworkCalculator
{
    public partial class MainPage : PhoneApplicationPage
    {
        string[] maskC = new string[7];
        string[] maskB = new string[15];
        string[] maskA = new string[23];
        string[] maskCIDR = new string[32];

        int offsetSubnet = 8;
        int maxBits = 24;
        string ipRange = "";
        int subnet = 0;
        int host = 0;

        int firstOctetMin = 1;
        int firstOctetMax = 126;

        // Constructor
        public MainPage()
        {
            MaskBuilder();
            InitializeComponent();
            netmask.SelectionMode = SelectionMode.Single;
            netmask.ExpansionMode = ExpansionMode.ExpansionAllowed;
        }

        private void CalcSubnet()
        {
            try
            {
                this.subnet = netmask.SelectedIndex;
                this.host = subnet + offsetSubnet;
                int netNumber = Convert.ToInt32(Math.Pow(2, subnet));
                int hostNumber = Convert.ToInt32(Math.Pow(2, (maxBits - subnet))) - 2;

                subnetBits.Text = subnet.ToString();
                hostBits.Text = host.ToString();
                netCount.Text = netNumber.ToString();
                hostCount.Text = hostNumber.ToString();

                byte[] ipParts = new byte[4];
                ipParts[0] = Convert.ToByte(IP0.Text);
                ipParts[1] = Convert.ToByte(IP1.Text);
                ipParts[2] = Convert.ToByte(IP2.Text);
                ipParts[3] = Convert.ToByte(IP3.Text);

                IPAddress ip = new IPAddress(ipParts);
                IPAddress nmask = IPAddress.Parse(netmask.SelectedItem.ToString());
                string netwAdd = GetNetworkAddress(ip, nmask).ToString();
                string broadAdd = GetBroadcastAddress(ip, nmask).ToString();
                netAd.Text = netwAdd;
                broAd.Text = broadAdd;

                string[] networkSplitted = netwAdd.Split('.');
                string[] broadcastSplitted = broadAdd.Split('.');

                int value = int.Parse(networkSplitted[3]);
                value += 1;
                networkSplitted[3] = value.ToString();

                value = int.Parse(broadcastSplitted[3]);
                value -= 1;
                broadcastSplitted[3] = value.ToString();

                netwAdd = string.Join(".", networkSplitted);
                broadAdd = string.Join(".", broadcastSplitted);

                range.Text = netwAdd + " - " + broadAdd;

            }
            catch (Exception e)
            {
               
            }
        }

        public IPAddress GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        public IPAddress GetNetworkAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }

        private void MaskBuilder()
        {
            //C
            maskC[0] = "255.255.255.0";
            maskC[1] = "255.255.255.128";
            maskC[2] = "255.255.255.192";
            maskC[3] = "255.255.255.224";
            maskC[4] = "255.255.255.240";
            maskC[5] = "255.255.255.248";
            maskC[6] = "255.255.255.252";


            //B
            maskB[0] = "255.255.0.0";
            maskB[1] = "255.255.128.0";
            maskB[2] = "255.255.192.0";
            maskB[3] = "255.255.224.0";
            maskB[4] = "255.255.240.0";
            maskB[5] = "255.255.248.0";
            maskB[6] = "255.255.252.0";
            maskB[7] = "255.255.254.0";
            maskB[8] = "255.255.255.0";
            maskB[9] = "255.255.255.128";
            maskB[10] = "255.255.255.192";
            maskB[11] = "255.255.255.224";
            maskB[12] = "255.255.255.240";
            maskB[13] = "255.255.255.248";
            maskB[14] = "255.255.255.252";


            //A
            maskA[0] = "255.0.0.0";
            maskA[1] = "255.128.0.0";
            maskA[2] = "255.192.0.0";
            maskA[3] = "255.224.0.0";
            maskA[4] = "255.240.0.0";
            maskA[5] = "255.248.0.0";
            maskA[6] = "255.252.0.0";
            maskA[7] = "255.254.0.0";
            maskA[8] = "255.255.0.0";
            maskA[9] = "255.255.128.0";
            maskA[10] = "255.255.192.0";
            maskA[11] = "255.255.224.0";
            maskA[12] = "255.255.240.0";
            maskA[13] = "255.255.248.0";
            maskA[14] = "255.255.252.0";
            maskA[15] = "255.255.254.0";
            maskA[16] = "255.255.255.0";
            maskA[17] = "255.255.255.128";
            maskA[18] = "255.255.255.192";
            maskA[19] = "255.255.255.224";
            maskA[20] = "255.255.255.240";
            maskA[21] = "255.255.255.248";
            maskA[22] = "255.255.255.252";

            //CIDR
            maskCIDR[0] = "128.0.0.0";
            maskCIDR[1] = "192.0.0.0";
            maskCIDR[2] = "224.0.0.0";
            maskCIDR[3] = "240.0.0.0";
            maskCIDR[4] = "248.0.0.0";
            maskCIDR[5] = "252.0.0.0";
            maskCIDR[6] = "254.0.0.0";
            maskCIDR[7] = "255.0.0.0";

            maskCIDR[8] = "255.128.0.0";
            maskCIDR[9] = "255.192.0.0";
            maskCIDR[10] = "255.224.0.0";
            maskCIDR[11] = "255.240.0.0";
            maskCIDR[12] = "255.248.0.0";
            maskCIDR[13] = "255.252.0.0";
            maskCIDR[14] = "255.254.0.0";
            maskCIDR[15] = "255.255.0.0";

            maskCIDR[16] = "255.255.128.0";
            maskCIDR[17] = "255.255.192.0";
            maskCIDR[18] = "255.255.224.0";
            maskCIDR[19] = "255.255.240.0";
            maskCIDR[20] = "255.255.248.0";
            maskCIDR[21] = "255.255.252.0";
            maskCIDR[22] = "255.255.254.0";
            maskCIDR[23] = "255.255.255.0";

            maskCIDR[24] = "255.255.255.128";
            maskCIDR[25] = "255.255.255.192";
            maskCIDR[26] = "255.255.255.224";
            maskCIDR[27] = "255.255.255.240";
            maskCIDR[28] = "255.255.255.248";
            maskCIDR[29] = "255.255.255.252";
            maskCIDR[30] = "255.255.255.254";
            maskCIDR[31] = "255.255.255.255";


            this.Resources.Add("maskA", maskA);
            this.Resources.Add("maskB", maskB);
            this.Resources.Add("maskC", maskC);
            this.Resources.Add("maskCIDR", maskCIDR);
        }

        private void IPAddress_fieldChange(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            if (box.Text == "")
            {
                box.Text = "0";
            }
            
            if (int.Parse(box.Text) > 255)
            {
                box.Text = "255";
            }

            int val = int.Parse(box.Text);
            box.Text = val.ToString();
            box.SelectionStart = box.Text.Length;
            CalcSubnet();
        }

        private void classSelect(object sender, RoutedEventArgs e)
        {
            PivotItem rad = ((PivotItem)((Pivot)sender).SelectedItem);
            string className = rad.Name;
            if (className == "ClA")
            {
                try
                {
                    IP0.Text = "10";
                    IP1.Text = "0";
                    IP2.Text = "0";
                    IP3.Text = "1";
                    offsetSubnet = 8;
                    maxBits = 24;
                    netmask.ItemsSource = maskA;
                    netmask.SelectedIndex = 0;
                    firstOctetMin = 1;
                    firstOctetMax = 126;
                }
                catch (Exception eA)
                {

                }
            }
            if (className == "ClB")
            {
                try
                {
                    IP0.Text = "172";
                    IP1.Text = "16";
                    IP2.Text = "0";
                    IP3.Text = "1";
                    offsetSubnet = 16;
                    maxBits = 16;
                    netmask.ItemsSource = maskB;
                    netmask.SelectedIndex = 0;
                    firstOctetMin = 127;
                    firstOctetMax = 191;
                }
                catch (Exception eB)
                {

                }
            }
            if (className == "ClC")
            {
                try
                {
                    IP0.Text = "192";
                    IP1.Text = "168";
                    IP2.Text = "0";
                    IP3.Text = "1";
                    offsetSubnet = 24;
                    maxBits = 8;
                    netmask.ItemsSource = maskC;
                    netmask.SelectedIndex = 0;
                    firstOctetMin = 192;
                    firstOctetMax = 223;
                }
                catch (Exception eC)
                {

                }
            }
            if (className == "ClM")
            {
                try
                {
                    IP0.Text = "224";
                    IP1.Text = "0";
                    IP2.Text = "0";
                    IP3.Text = "1";
                    offsetSubnet = 8;
                    maxBits = 24;
                    netmask.ItemsSource = maskA;
                    netmask.SelectedIndex = 0;
                    firstOctetMin = 224;
                    firstOctetMax = 239;
                }
                catch (Exception eM)
                {

                }
            }
            if (className == "ClO")
            {
                try
                {
                    IP0.Text = "240";
                    IP1.Text = "0";
                    IP2.Text = "0";
                    IP3.Text = "1";
                    offsetSubnet = 8;
                    maxBits = 24;
                    netmask.ItemsSource = maskA;
                    netmask.SelectedIndex = 0;
                    firstOctetMin = 240;
                    firstOctetMax = 255;
                }
                catch (Exception eO)
                {

                }
            }
            CalcSubnet();
        }

        private void netmaskSelected(object sender, SelectionChangedEventArgs e)
        {
            CalcSubnet();
        }

        private void IP0_TextChanged_1(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            if (box.Text == "")
            {
                box.Text = "1";
            }

            if (int.Parse(box.Text) < firstOctetMin)
            {
                box.Text = firstOctetMin.ToString();
            }
            if (int.Parse(box.Text) > firstOctetMax)
            {
                box.Text = firstOctetMax.ToString();
            }

            int val = int.Parse(box.Text);
            box.Text = val.ToString();
            box.SelectionStart = box.Text.Length;
            CalcSubnet();
        }
    }
}