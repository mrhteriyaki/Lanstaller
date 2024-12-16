// See https://aka.ms/new-console-template for more information
using LanstallerShared;
using Newtonsoft.Json;
using System.IO;

LanstallerServer.ConnectionString = "Data Source=192.168.88.3,1433;Initial Catalog=lanstaller;Integrated Security=true;TrustServerCertificate=True";

//Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fffff"));

//List<SoftwareInfo> testList = SoftwareInfo.LoadSoftware();

//JsonConvert.SerializeObject(testList);

//Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fffff"));

File.Create("C:\\Install\\testfile.txt");