using System.Reflection;

namespace ActionsExecuter
{
    class MainClass{

        private static readonly string menuHeader = "---------------------------\n"+
            "enter a command to execute:\n";

        public static void Main(String[] args){
            Action[] actions = GetInstancesOfImplementations(typeof(Action));
            string menu = menuHeader;
            int i;
            for(i=0 ; i< actions.Length ; i++)
                menu+= (i+1).ToString()+"- "+actions[i].getTitle()+"\n";
            menu += (i+1).ToString()+"- exit"; 
            while(true){
                Console.WriteLine(menu);
                string? optStr = Console.ReadLine();
                try{
                    int option = int.Parse(optStr);
                    if(option == actions.Length+1){
                        Console.WriteLine("Bye!");
                        break;
                    }
                    actions[option-1].execute();
                }
                catch(Exception){
                    Console.WriteLine("Error: incorrect value!!");
                }
            }
        }

        public static Action[] GetInstancesOfImplementations(Type interfaceType){
            IEnumerable<Type> implementationTypes = GetImplementations(interfaceType);
            List<Action> implementationInstances = [];
            foreach (Type type in implementationTypes){
                Action instance = (Action)Activator.CreateInstance(type);
                implementationInstances.Add(instance);
            }
            return [.. implementationInstances];
        }

        public static Type[] GetImplementations(Type interfaceType){
            List<Type> implementationTypes = [];
            Type[] types = interfaceType.Assembly.GetTypes();
            foreach (Type type in types)
                if (type.IsClass && !type.IsAbstract && interfaceType.IsAssignableFrom(type))
                    implementationTypes.Add(type);
            return [.. implementationTypes];
        }
    }

    

}