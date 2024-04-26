using System.Collections.Concurrent;
using System.Net;
class Program
{
    static readonly ConcurrentQueue<string> UrlQueue = new();
    static readonly SemaphoreSlim Semaphore = new(0);
    static readonly SemaphoreSlim mutex = new(1);
    static int concurrencyLevel;

    static void Main(string[] args)
    {
        Console.WriteLine("please write the urls in the urls.txt file.");
        Console.ReadKey();
        List<string> urls = ReadUrls("urls.txt");
        Console.WriteLine("max simultaneus downloads? ");
        concurrencyLevel = int.Parse(Console.ReadLine()??"1");
        
        foreach (string url in urls)
            UrlQueue.Enqueue(url);

        Semaphore.Release(concurrencyLevel);
        while(!UrlQueue.IsEmpty){
            Semaphore.Wait();
            Thread newThread = new(DownloadWorker);
            newThread.Start();            
        }
        Console.WriteLine("all done");

        Console.ReadLine();
    }

    static private async void DownloadWorker()
    {
        mutex.Wait();
        if (UrlQueue.TryDequeue(out string url) == false)
            return;
        mutex.Release();
        try
        {
            string fileName = Path.GetFileName(url);
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            await DownloadFileAsync(url, outputPath);
            

            Console.WriteLine($"File '{fileName}' downloaded successfully to '{outputPath}'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading file: {ex.Message}");
        }
        finally{
            Semaphore.Release();
        }
    }

    static private async Task DownloadFileAsync(string url, string outputPath)
    {
        using (WebClient client = new WebClient())
        {
            await client.DownloadFileTaskAsync(new Uri(url), outputPath);
        }
    }

    public static List<string> ReadUrls(string filePath)
    {
        List<string> urls = new List<string>();

        try
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
                urls.Add(line.Trim());
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }

        return urls;
    }
}