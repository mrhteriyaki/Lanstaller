namespace LanstallerWeb
{
    public class Maintenance
    {

        public static void CleanupTemp()
        {
            foreach (string dir in Directory.GetDirectories("/tmp/"))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                if (directoryInfo.CreationTime < DateTime.Now.AddHours(-1))
                {
                    Directory.Delete(dir, true);
                }
            }
        }

    }
}
