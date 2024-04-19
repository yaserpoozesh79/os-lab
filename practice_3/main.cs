using System.Text.Json;


class Program
{
    private static List<Person> persons = new();

    static void Main(string[] args)
    {
        persons = JsonHelper.LoadPersons();

        while (true)
        {
            Console.WriteLine(
                "----------------------------------------------------\n"+
                "Enter your Choice:\n"+
                "1-add a person"+
                "\n2-save persons\n"+
                "3-list Persons\n"+
                "4-exit");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AddPerson();
                    break;
                case "2":
                    SavePersons();
                    break;
                case "3":
                    ListPersons();
                    break;
                case "4":
                    Environment.Exit(0);
                    break;    
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    static void ListPersons(){
        foreach (var person in persons)
            Console.WriteLine(
                $"{person.nationalId}- "+
                $"{person.name} "+
                $"{person.emailAddress} "+
                $"{person.birthDate} "+
                $"{person.address}"
            );
    }

    static void AddPerson()
    {
        Console.Write("Enter name: ");
        string name = Console.ReadLine();

        Console.Write("Enter national id: ");
        string nationalId = Console.ReadLine();

        Console.Write("Enter birthdate: ");
        string birthdate = Console.ReadLine();

        Console.Write("Enter email: ");
        string email = Console.ReadLine();

        Console.Write("Enter address: ");
        string address = Console.ReadLine();

        Person person = new()
        {
            name=name, 
            emailAddress=email, 
            address=address,  
            birthDate=birthdate, 
            nationalId=nationalId
        };
        persons.Add(person);
    }

    static void SavePersons()
    {
        JsonHelper.SavePersons(persons);
        Console.WriteLine("Persons saved to persons.json");
    }
}