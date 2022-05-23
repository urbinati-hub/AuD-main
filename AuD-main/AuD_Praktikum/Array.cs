using System;

namespace AuD_Praktikum
{

    abstract class Array
    {
        public int SIZE = 10;       //maximale länge des Arrays
        public int[] myArray;

        public Array()
        {
            myArray = new int[SIZE] ;       //hier wird das Array mit der Länge SIZE erzeugt
        }

        public void print()         //gibt das Array auf der Konsole aus
        {
            for (int i = 0; i < SIZE; i++)
            {
                    Console.WriteLine(myArray[i]);
            }
 
        }
    }

    class SetSortedArray : MultiSetSortedArray, ISetSorted      
    {
        public override bool insert(int elem)       //fügt ein Element x ins sortierte Array (Set) ein 
        {
            int a = _search_(elem, false);      //ruft Hilfsfunktion auf --> Einfügeposition wird ermittelt
                                                    //false wegen Set
            
            if(a == -1)         //falls Element x schon vorhanden, wird es nicht nochmal eingefügt
            {
                return false;       //wurde nicht eingefügt
            }


            for (int j = SIZE - 1; j > a; j--)      //verschiebt die Elemente im Array um eine Position nach rechts
            {
                myArray[j] = myArray[j - 1];
            }
            
            myArray[a] = elem;      //fügt Element an ermittelter Position ein

            return true;        //wurde eingefügt

        }
    }

    class SetUnsortedArray : MultiSetUnsortedArray, ISetUnsorted
    {
        public override bool insert(int elem)       //fügt ein Element x ins unsortierte Array (Set) ein
        {

            if (search(elem))       //falls Element x schon im Array vorhanden wird es nicht nochmal eingefügt
                return false;       //Element wurde nicht eingefügt

            else
                return base.insert(elem);       //falls Element x nicht im Array vorhanden
                                                //--> insert Methode von MultiSetUnsortetArray wird aufgerufen
        }


    }

    class MultiSetSortedArray : Array, IMultiSetSorted
    {
        
        public virtual bool insert(int elem)        //fügt ein Element x ins sortierte Array (MultiSet) ein
        {
           
                int a = _search_(elem, true);   //ruft Hilfsfunktion auf --> Einfügeposition wird ermittelt
                                                    //true wegen MultiSet


            for (int j = SIZE-1; j > a; j--)        //verschiebt die Elemente im Array bis zur Einfügeposition
            {                                    //um eine Position nach rechts
                myArray[j] = myArray[j-1];
            }
                
                myArray[a] = elem;      //Element x wird an ermittelter Einfügeposition eingefügt

                return true;        //Element x wurde eingefügt
            
        }

        

        public bool search(int elem)        //search Methode gibt zurück, ob gesuchtes Element im Array vorhanden ist
        {

            int i = _search_(elem, true);     //Position im Array an der sich das Element x befindet, wird zurückgegeben

            if (i > -1)         //falls Position zurück gegeben wird, also Element im Array gefunden wurde,
                return true;    //wird true als "gefunden" zurückgegeben
            else
                return false;   //falls das Element nicht gefunden wurde, wird "false" als "nicht gefunden" zurückgegeben
        }

       
        protected int _search_(int elem, bool multiset)     //Hilfsmethode für insert() (binäre Suche)
        {                                                       //der boolean gibt an ob es sich um ein MultiSet (true)
                                                                //oder um ein Set (false) handelt 
            int l = 0;              //linkes Ende des Arrays bzw Suchbereichs
            int r = SIZE - 1;       //rechtes Ende des Arrays bzw des Suchbereichs
            int i = 0;              //Positionszeiger bzw Mitte des Suchbereichs

            if(multiset==false && myArray[i]==elem)     //falls es sich um ein Array vom Typ Set handelt
            {                                           //und das erste Element im Array schon das übergebene Element ist,
                return -1;                              //wird -1 zurückgegeben, um zu signalisieren, dass das Element
            }                                           //schon vorhanden ist --> nur relevant bei insert()

            while( l < r )
            {
                
                i = ((l + r) / 2);      //Mitte des Suchbereichs wird ermittelt

                if( myArray[i] == 0 || myArray[i] > elem )      //falls das Array in der Mitte des Suchbereichs noch nicht
                {                                               //voll ist oder das Element x kleiner ist als das Element 
                                                                //in der Mitte des Suchbereichs, wird das rechte Ende
                    r = i - 1;                                  //auf Positionszeiger - 1 gesetzt
                }

                else                //falls das Element x größer ist als das Element in der Mitte des Suchbereichs
                {                   //wird das linke Ende auf Positionszeiger + 1 gesetzt
                    l = i + 1;
                }

                if( myArray[i] == elem )        //falls das Element in der Mitte des Suchbereichs das gleich groß wie 
                {                               //das Element x ist, wird für das MultiSet die aktuelle Position übergeben
                    if (multiset)
                    {
                        return i;
                    }

                    else                        //für das Set ein -1 für "Element schon vorhanden"
                    {
                        return -1;
                    }
                    
                }

                if( r == l )        //falls das Element nicht im Array gefunden wurde, wird hier die Einfügeposition ermittelt
                {
                    while( myArray[i] > elem )
                    {
                        i--;

                        if( i < 0)
                        {
                            i++;
                            break;
                        }
                    }

                    if(myArray[i] == 0)     //falls Array nicht voll
                    {
                        i--;
                    }

                    while( myArray[i] < elem && myArray[i] != 0 )
                    {
                        i++;

                        if( i == SIZE)
                        {
                            i--;
                            break;
                        }
                    }

                    return i;       //gibt Einfügeposition zurück
                }
                
   
            } 

            return i;       //gibt Einfügeposition zurück
            
        }

        public bool delete(int elem)        //Methode zum Löschen eines bestimmten Elements x
        {
            int i = _search_(elem, true); //_search_(elem);         //Position des zu löschenden Elements wird gesucht

            if (i == -1)        //falls Element nicht im Array gefunden wurde, wird false zurück gegeben bzw "löschen fehlgeschlagen"
            {
                return false;
            }

            for (int j = i;  j < SIZE-1; j++)       //falls das Element gefunden wurde, werden ab der Position dieses
            {                                       //Elements alle anderen Elemente um eine Position nach links gerückt
                myArray[j] = myArray[j + 1];
            }

            myArray[SIZE - 1] = 0;      //das Element in der letzten Position wird wieder auf Null gesetzt

            return true;        //das Element wurde erfolgreich gelöscht

        }
    }


    class MultiSetUnsortedArray : Array, IMultiSetUnsorted
    {
        public virtual bool insert(int elem)        //Methode zum Einfügen in das Array
        {
            for (int i = 0; i < SIZE; i++)      //Array wird durchlaufen und an der ersten leeren Stelle wird das 
            {                                   //Element x eingefügt.
                if (myArray[i] == 0)
                {
                    myArray[i] = elem;
                    return true;
                }
            }

            return false;       //falls das Array schon voll ist, wird nichtsmehr eingefügt
        }

        public bool search(int elem)        //Methode zur Überprüfung, ob Element x in Array vorhanden ist
        {

            int i = _search_(elem);     //Position des Elements x im Array wird ermittelt

            if (i > -1)         //falls eine Position gefunden wurde, wird true bzw "gefunden" zurückgegeben
                return true;
            else                //falls keine Position gefunden wurde, wird false bzw "nicht gefunden" zurückgegeben
                return false;
        }

        private int _search_(int elem)      //Hilfsmethode fürs Suchen --> Position im Array wird zurückgegeben
        {
            int i = 0;

            while (myArray[i] != elem)      //Array wird durchlaufen und auf Gleichheit mit dem gesuchten Element x geprüft
            {
                i++;

                if (i == SIZE)
                    return -1;      //Element nicht gefunden --> Rückgabewert -1
            }

            return i;       //Position wird zurückgegeben
        }

        public bool delete(int elem)        //Methode zum Löschen
        {
            int i = _search_(elem);         //Position des zu löschenden Elements wird ermittelt

            if (i == -1)            //falls Element x nicht gefunden wurde, wird false bzw "nicht gefunden" zurückgegeben
                return false;

            int j = 0;

            
            while(myArray[j]!=0)        //die Position des letzten Elements im Array wird ermittelt, da dieses Element
            {                           //an die Position des Elements x kommt, welches gelöscht wird
                j++;

                if (j == SIZE)
                    break;
            }

            j--;

            myArray[i] = myArray[j];        //letztes Element wird auf die Position des zu löschenden Elements x verschoben

            myArray[j] = 0;         //letzte Position wird wieder auf Null gesetzt
            
            return true;        //Element x wurde erfolgreich gelöscht

        }
    }
}