
namespace App
{
    class main{
        static void Main(string[] args){
            using (var monitor = new DiskMonitor())
            {
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}