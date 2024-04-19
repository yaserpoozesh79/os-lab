using System.Text.Json;

public static class JsonHelper
{
    private static readonly string FilePath = "persons.json";

    public static void SavePersons(List<Person> persons)
    {
        string jsonString = JsonSerializer.Serialize(persons);
        File.WriteAllText(FilePath, jsonString);
    }

    public static List<Person> LoadPersons()
    {
        if (File.Exists(FilePath))
        {
            string jsonString = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Person>>(jsonString);
        }
        return new List<Person>();
    }
}