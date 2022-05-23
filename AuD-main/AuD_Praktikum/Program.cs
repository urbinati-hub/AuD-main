using System;

namespace AuD_Praktikum
{
    class Program
    {
        public enum abstractChoiceName { MultiSetSorted = 1, MultiSetUnsorted, SetSorted, SetUnsorted }
        public enum concreteChoiceName { Array = 1, LinkedList, BinSearchTree, AVLTree, Treap, HashTabSepChain, HashTabQuadProb }
        static void Main(string[] args)
        {
            Console.Write("Herzlich Willkommen!");
            bool restart = false;
            do
            {
                bool error = false;
                if (restart)
                    Console.WriteLine("Willkommen zurück im Hauptmenü!");
                restart = false;

                Console.Write("\nHier können Sie den gewünschten abstrakten Datentyp auswählen:\n" +
                "\n 1. sortiertes Multiset \n 2. unsortiertes Multiset \n 3. sortiertes Set \n 4. unsortiertes Set \n" +
                "\nGeben Sie nun die Nummer des gewünschten Typs ein:  ");

                ConsoleKeyInfo choiceAsCK = Console.ReadKey();
                int abstractChoice = -1;
                if (char.IsDigit(choiceAsCK.KeyChar))
                    abstractChoice = int.Parse(choiceAsCK.KeyChar.ToString());

                if (abstractChoice > 0 && abstractChoice < 5)
                {
                    Console.Clear();
                    Console.Write("Sie haben " + (abstractChoiceName)abstractChoice + " ausgewählt." +
                        "\nHier können Sie nun den gewünschten konkreten Datentyp auswählen:\n" +
                        "\n 1. Array \n 2. verkettete Liste");
                }

                int concreteChoice = -1;
                int concreteChoiceMax = -1;
                switch (abstractChoice)
                {
                    case 1: { concreteChoiceMax = 2; break; }
                    case 2: { concreteChoiceMax = 2; break; }
                    case 3: { Console.Write("\n 3. binärer Suchbaum \n 4. AVL Baum \n 5. Treap"); concreteChoiceMax = 5; break; }
                    case 4: { Console.Write("\n 3. Hashtabelle Chain \n 4. Hashtabelle Prob"); concreteChoiceMax = 4; break; }
                    default:
                        {
                            Console.Write("\n\nIhre Eingabe stimmt nicht mit den Auswahlmöglichkeiten überein.\n" +
                            "Drücken Sie eine beliebige Taste, um zurück zum Hauptmenü zu gelangen.");
                            Console.ReadKey();
                            Console.Clear();
                            error = true; break;
                        }
                }
                if (!error)
                {
                    Console.Write("\n\nGeben Sie nun die Nummer des gewünschten Typs ein:  ");
                    choiceAsCK = Console.ReadKey();
                    if (char.IsDigit(choiceAsCK.KeyChar))
                        concreteChoice = int.Parse(choiceAsCK.KeyChar.ToString());

                    if (concreteChoice > concreteChoiceMax)
                    {
                        Console.Write("\n\nIhre Eingabe stimmt nicht mit den Auswahlmöglichkeiten überein.\n" +
                            "Drücken Sie eine beliebige Taste, um zurück zum Hauptmenü zu gelangen.");
                        Console.ReadKey();
                        Console.Clear();
                        error = true;
                    }
                    if (!error)
                    {
                        if (abstractChoice == 4 && (concreteChoice == 3 || concreteChoice == 4))
                            concreteChoice = concreteChoice + 3;

                        string typeName = "AuD_Praktikum.";
                        if (concreteChoice < 3)
                            typeName += (abstractChoiceName)abstractChoice;
                        typeName += (concreteChoiceName)concreteChoice;


                        var typeNameVar = Type.GetType(typeName);
                        var item = Activator.CreateInstance(typeNameVar) as IDictionary;


                        bool againInnerLoop = true;
                        do
                        {
                            Console.Clear();
                            Console.Write("Sie haben " + (concreteChoiceName)concreteChoice + " (" + (abstractChoiceName)abstractChoice + ") ausgewählt." +
                                "\nHier können Sie flexibel Objekte einfügen, suchen und löschen oder den konkreten Datentyp ausgeben lassen." +
                                "\n\nVerwenden Sie für diese Operationen folgende Syntax, wobei x der Wert des einzufügenden Objektes ist:" +
                                "\n\n insert:x fügt x ein," +
                                "\n search:x sucht nach x," +
                                "\n delete:x löscht x," +
                                "\n print gibt den gesamten Datentyp aus." +
                                "\n\n (restart bringt Sie zurück zum Hauptmenü, exit beendet das Programm)" +
                                "\n\nZur besseren Veranschaulichung wird der gesamte Datentyp nach jedem Aufruf der Einfügen- und Löschen-Funktionen ausgegeben.\n\n\n\n");
                            item.print();
                            string input = Console.ReadLine();
                            string[] inputSplit = input.Split(':');
                            switch (inputSplit[0])
                            {
                                case "insert": { item.insert(Convert.ToInt32(inputSplit[1])); Console.WriteLine(); item.print(); break; }
                                case "search":
                                    {
                                        bool found = item.search(Convert.ToInt32(inputSplit[1]));
                                        if (found)
                                            Console.Write("\nDas Objekt " + inputSplit[1] + " wurde gefunden.\n");
                                        else
                                            Console.Write("\nDas Objekt " + inputSplit[1] + " wurde nicht gefunden.\n");
                                        break;
                                    }
                                case "delete": { item.delete(Convert.ToInt32(inputSplit[1])); Console.WriteLine(); item.print(); break; }
                                case "print": { Console.WriteLine(); item.print(); break; }
                                case "exit": { againInnerLoop = false; restart = false; break; }
                                case "restart": { againInnerLoop = false; restart = true; Console.Clear(); break; }
                                default: { Console.Write("\nIhre Eingabe stimmt nicht mit der vorgegebenen Syntax überein."); break; }
                            }
                            if (againInnerLoop)
                            {
                                Console.Write("\nDrücken Sie die Enter-Taste, um diesen Bildschirm für Ihre nächste Eingabe zu aktualisieren.");
                                Console.ReadKey();
                            }
                        } while (againInnerLoop);
                    }
                    else restart = true;
                }
                else
                    restart = true;
            } while (restart == true);
        }
    }
}
