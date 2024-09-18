// See https://aka.ms/new-console-template for more information
using LanstallerShared;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

LanstallerServer.ConnectionString = "Data Source=192.168.88.3,1433;Initial Catalog=lanstaller;Integrated Security=true";

Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fffff"));

List<SoftwareInfo> testList = SoftwareInfo.LoadSoftware();

Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fffff"));

//JsonConvert.SerializeObject(testList);

//Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fffff"));