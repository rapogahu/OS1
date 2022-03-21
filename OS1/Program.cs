using System;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using System.IO.Compression;

namespace OS_practice_1
{
    class PLACEnTIME
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public short Day { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string pathtxt;
            First();
            Second();
            Third();
            Fourth();
            Fifth();
            Sixth();
        }
        static void First()
        {
            Console.WriteLine("1.Print information to the console about logical drives, names, volume label, size and file system types.");

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in allDrives)
            {
                Console.WriteLine("Drive " + drive.Name);
                Console.WriteLine("  Drive type: " + drive.DriveType);
                if (drive.IsReady == true)
                {
                    Console.WriteLine("  Volume label: " + drive.VolumeLabel);
                    Console.WriteLine("  File system: " + drive.DriveFormat);
                    Console.WriteLine("  Available space: " + drive.AvailableFreeSpace + " bytes");
                    Console.WriteLine("  Total available space: " + drive.TotalFreeSpace + " bytes");
                    Console.WriteLine("  Total size of drive: " + drive.TotalSize + " bytes");
                }
            }
        }
        static void Second()
        {
            Console.WriteLine("\n2.Working with files.");

            Console.WriteLine("Enter .txt file name: ");
            string path = Console.ReadLine() + ".txt";

            Console.WriteLine("\nEnter a string:");
            string str = Console.ReadLine();

            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write(str);
            }

            Console.WriteLine("Extracting a line from a file:");
            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

            FileInfo fileInf = new FileInfo(path);

            Console.WriteLine();
        }
        static void Third()
        {
            Console.WriteLine("3.Working with format JSON.");
            string path = "JSONfile.json";
            PLACEnTIME rf = new PLACEnTIME { Latitude = 55.755800, Longitude = 37.617683, Day = 25, Month = "December", Year = 1992 };
            string json = JsonSerializer.Serialize<PLACEnTIME>(rf);

            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.Write(json);
            }

            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

            Console.WriteLine();
        }
        static void Fourth()
        {
            Console.WriteLine("4.Working with format XML.");
            string path = "people.xml";
            XDocument xdoc = new XDocument();

            XElement first = new XElement("person");

            Console.Write("Enter a name: ");
            string name = Console.ReadLine();
            XAttribute firstName = new XAttribute("name", name);
            Console.Write("Enter a name of a company: ");
            string company = Console.ReadLine();
            XElement firstCompany = new XElement("company", company);
            Console.Write("Enter age: ");
            int age = Int32.Parse(Console.ReadLine());
            XElement firstAge = new XElement("age", age);

            first.Add(firstName);
            first.Add(firstCompany);
            first.Add(firstAge);

            XElement people = new XElement("people");
            people.Add(first);
            xdoc.Add(people);
            xdoc.Save(path);

            Console.WriteLine("\nYour XML file:");
            using (StreamReader sr = new StreamReader(path))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

        }
        static void Fifth()
        {
            Console.WriteLine("5.Creating a zip archive, adding a file, output the size of the archive.");
            string zipPath = "ZIPfile.zip";
            using (FileStream file = new FileStream(zipPath, FileMode.OpenOrCreate))
            {
                using ZipArchive zipfile = new ZipArchive(file, ZipArchiveMode.Update);

                string path = "";
                while (!File.Exists(path))
                {
                    Console.Write("Enter file path to add to archive: ");
                    path = Console.ReadLine();
                }

                zipfile.CreateEntryFromFile(path, path.Substring(path.LastIndexOf('\\') + 1));

                zipfile.ExtractToDirectory("fromZIPfile");

                path = "fromZIPfile\\" + path.Substring(path.LastIndexOf('\\') + 1);
                FileInfo fileInf = new FileInfo(path);
                Console.WriteLine("File name: {0}\nTime of creation: {1:F}\nSize: {2} byte", fileInf.Name, fileInf.CreationTime, fileInf.Length);
            }

            Console.WriteLine();
        }

        static void Sixth()
        {
            Console.WriteLine("6. Choose file to delete:\n1. JSON \n2. XML\n3. ZIP archive\n");
            int choice = Int32.Parse(Console.ReadLine());
            if (choice == 1)
            {
                File.Delete("JSONfile.json");
                Console.WriteLine("JSON file was succesfully deleted.");
            }

            if (choice == 2)
            {
                File.Delete("people.xml");
                Console.WriteLine("XML file was succesfully deleted.");
            }
            if (choice == 3)
            {
                File.Delete("ZIPfile.zip");
                Directory.Delete("fromZIPfile", true);
                Console.WriteLine("ZIP archive was succesfully deleted.");
            }
        }
    }
}