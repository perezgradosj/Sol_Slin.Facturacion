using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using IOEx;

//using Microsoft.Win32;
using System.IO;

using System.Security.Cryptography;
using System.Management;
using System.Net.NetworkInformation;

namespace Slin.Facturacion.Common.Setup
{
    public class SetupPrivate
    {
        public static string GetMacAddress()
        {
            string macs = "";

            // get network interfaces physical addresses 
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                PhysicalAddress pa = ni.GetPhysicalAddress();
                macs += pa.ToString();
            }
            return macs;
        }

        public static string getVolumenSerialHardDesk()
        {
            //DirectoryInfo currentDir = new DirectoryInfo(Environment.CurrentDirectory);
            //string path = string.Format("win32_logicaldisk.deviceid=\"{0}\"",
            //    currentDir.Root.Name.Replace("\\", ""));
            //ManagementObject disk = new ManagementObject(path);
            //disk.Get();
            //string serial = disk["VolumeSerialNumber"].ToString();
            //return serial;

            string Model = string.Empty;
            string Type = string.Empty;
            string Serial = string.Empty;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                //HardDrive hd = new HardDrive();
                Model = wmi_HD["Model"].ToString();
                Type = wmi_HD["InterfaceType"].ToString();
                Serial = wmi_HD["SerialNumber"].ToString();
                //hdCollection.Add(hd);
            }
            return Serial;


        }

        public static string getBaseBoardMotherSerial()
        {
            //ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            string serie = string.Empty;
            foreach (ManagementObject mo in mos.Get())
            {
                //Console.WriteLine("Serial Number: " + mo.GetPropertyValue("SerialNumber").ToString());
                //Console.WriteLine("Manufacturer: " + mo.GetPropertyValue("Manufacturer").ToString());
                //Console.WriteLine("Product: " + mo.GetPropertyValue("Product").ToString());
                serie = mo.GetPropertyValue("SerialNumber").ToString();
            }
            return serie;
        }

        public static string getProccessInfo()
        {
            string cpuInfo = String.Empty;
            string temp = String.Empty;
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == String.Empty)
                {// only return cpuInfo from first CPU 
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
            }
            return cpuInfo;
        }


        public static string GenerateKey(string hardDesk, string mainboard, string mcId)
        {
            string ID = hardDesk + mainboard + mcId;

            HMACSHA1 hmac = new HMACSHA1();
            hmac.Key = Encoding.ASCII.GetBytes(getBaseBoardMotherSerial());
            hmac.ComputeHash(Encoding.ASCII.GetBytes(ID));

            // convert hash to hex string 
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hmac.Hash.Length; i++)
            {
                sb.Append(hmac.Hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
       
    }
}
