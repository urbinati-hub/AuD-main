using System;

namespace AuD_Praktikum
{
    abstract class Hash : ISetUnsorted      // abstrakte Klasse mit Konstruktoren
    {
        public int tabGroeße = 11;  // Für quadratische Sondierung Tabellengröße in Form von p = 4*k+3
        public HashElement[] hashTab;

        public Hash()                      // Konstruktor 1, falls keine TabGröße gewünscht, Standardgröße 12
        {
            hashTab = new HashElement[tabGroeße];
        }
        /*public Hash(int gewuenschteGroeße) // Konstruktor 2, falls TabGröße gewünscht
        {
            tabGroeße = gewuenschteGroeße;
            hashTab = new HashElement[tabGroeße];
        }*/
        public abstract bool search(int elem);   // abstrakte Methoden aus ISetUnsorted bzw IDictionary
        public abstract bool insert(int elem);
        public abstract bool delete(int elem);
        public void print()
        {
            HashElement laufvariable;
            for (int i = 0; i < tabGroeße; i++)
            {
                if (hashTab[i] == null)
                {
                    Console.WriteLine("/");
                }
                else
                {
                    Console.Write($"{hashTab[i].element} ");
                    laufvariable = hashTab[i];
                    while (laufvariable.nachfolger != null)
                    {
                        Console.Write($"-> {laufvariable.nachfolger.element} ");
                        laufvariable = laufvariable.nachfolger;
                    }
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
        }
    }

    class HashElement                    // Klasse für Hashelemente, v.a. für Verkettung wichtig -> einfache Liste
    {
        public int element { get; set; }
        public HashElement nachfolger { get; set; }

        public HashElement(int elem)
        {
            element = elem;
            nachfolger = null;
        }
    }

    class HashTabSepChain : Hash        // Klasse für separate Verkettung
    {
        public HashTabSepChain() : base() { }                        // Konstruktoren aus class Hash
        //public HashTabSepChain(int tabGroeße) : base(tabGroeße) { }

        public override bool insert(int elem)                // Einfügemethode
        {
            if (search(elem) == true)
            {
                Console.WriteLine($"{elem} existiert bereits!");
                return false;
            }
            HashElement einzufügen = new HashElement(elem);
            int pos = getVertikalePos(elem);
            HashElement eingefügeStelle = hashTab[pos];
            if (eingefügeStelle == null)
            {
                hashTab[pos] = einzufügen;
                return true;
            }
            else
            {
                while (eingefügeStelle.nachfolger != null)
                {
                    eingefügeStelle = eingefügeStelle.nachfolger;
                }
                eingefügeStelle.nachfolger = einzufügen;
                return true;
            }
        }

        public override bool search(int elem)            // Suchmethode
        {
            var (_, aktuelles) = getHorizontalePos(elem);

            if (aktuelles == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override bool delete(int elem)         // Löschmethode
        {
            int pos = getVertikalePos(elem);
            var (vorgänger, aktuelles) = getHorizontalePos(elem);
            if (aktuelles == null)
            {
                Console.WriteLine($"{elem} existiert nicht!");
                return false;
            }
            if (vorgänger == null)
            {
                hashTab[pos] = aktuelles.nachfolger;
                return true;
            }
            vorgänger.nachfolger = aktuelles.nachfolger;
            return true;
        }

        public int getVertikalePos(int elem)      // Methode zum bestimmen der "vertikalen" Position, also der Position in der Hash Tabelle
        {
            int umrechner = elem;
            while (umrechner < 0)
            {
                umrechner += tabGroeße;
            }
            int pos = umrechner % tabGroeße;
            return pos;
        }

        public (HashElement vorgänger, HashElement aktuelles) getHorizontalePos(int elem)   // Methode zum Bestimmen der "horzontalen" Position, also der Position in der verketteten Liste
        {
            int pos = getVertikalePos(elem);
            HashElement gesuchtes = hashTab[pos];
            HashElement vorgänger = null;

            while (gesuchtes != null)
            {
                if (gesuchtes.element == elem)
                {
                    return (vorgänger, gesuchtes);
                }
                else
                {
                    vorgänger = gesuchtes;
                    gesuchtes = gesuchtes.nachfolger;
                }
            }
            return (null, null);       // Element existiert nicht in Hashtabelle
        }

    }

    class HashTabQuadProb : Hash      // Klasse für quadratische Sondierung
    {
        public HashTabQuadProb() : base() { }                     // Konstruktoren aus class Hash
        //public HashTabQuadProb(int tabGroeße) : base(tabGroeße) { }

        public override bool insert(int elem)         // Einfügemethode
        {
            if (search(elem) == true)
            {
                Console.WriteLine($"{elem} existiert bereits!");
                return false;
            }
            HashElement einzufügen = new HashElement(elem);
            int i = 0;
            int pos;
            int abbruch = -1;
            while (abbruch < tabGroeße)
            {
                pos = getHorizontalePosPlus(elem, i);
                if (hashTab[pos] == null)
                {
                    hashTab[pos] = einzufügen;
                    //Console.WriteLine($"{elem} wurde eingefügt!");
                    return true;
                }
                abbruch++;

                pos = getHorizontalePosMinus(elem, i);
                if (hashTab[pos] == null)
                {
                    hashTab[pos] = einzufügen;
                    //Console.WriteLine($"{elem} wurde eingefügt!");
                    return true;
                }
                abbruch++;
                i++;

            }
            Console.WriteLine($"{elem} konnte nicht eingefügt werden, da die Hashtabelle bereits voll ist!");
            return false;

        }

        public override bool search(int elem)      // Suchmethode
        {
            int i = 0;
            int pos;
            int abbruch = -1;
            while (abbruch < tabGroeße)
            {
                pos = getHorizontalePosPlus(elem, i);
                if (hashTab[pos] != null)
                {
                    if (hashTab[pos].element == elem)
                    {
                        return true;
                    }
                }
                abbruch++;
                pos = getHorizontalePosMinus(elem, i);
                if (hashTab[pos] != null)
                {
                    if (hashTab[pos].element == elem)
                    {
                        return true;
                    }
                }
                abbruch++;
                i++;
            }
            return false;
        }

        public override bool delete(int elem)     // Löschmethode
        {
            int i = 0;
            int pos;
            int abbruch = -1;
            while (abbruch < tabGroeße)
            {
                pos = getHorizontalePosPlus(elem, i);
                if (hashTab[pos] != null)
                {
                    if (hashTab[pos].element == elem)
                    {
                        hashTab[pos] = null;
                        return true;
                    }
                }
                abbruch++;
                pos = getHorizontalePosMinus(elem, i);
                if (hashTab[pos] != null)
                {
                    if (hashTab[pos].element == elem)
                    {
                        hashTab[pos] = null;
                        return true;
                    }
                }
                abbruch++;
                i++;
            }
            Console.WriteLine($"{elem} existiert nicht!");
            return false;
        }

        public int getHorizontalePosPlus(int elem, int i)   // Methode für Hashfunktion mit quadratischer Sondierung, Teil mit Addition
        {
            int umrechner = elem;
            int pos;

            while (umrechner < 0)
            {
                umrechner = umrechner + tabGroeße;
            }
            pos = (umrechner + (i * i)) % tabGroeße;
            return pos;
        }

        public int getHorizontalePosMinus(int elem, int i)  // Methode für Hashfunktion mit quadratischer Sondierung, Teil mit Subtraktion
        {
            int umrechner = elem;
            int pos;

            while (umrechner < 0)
            {
                umrechner = umrechner + tabGroeße;
            }
            umrechner += (tabGroeße-1)*tabGroeße/2;                    // Nicht schön, aber funktioniert! Für alle Tabellengrößen in Form m=4*k+3
            pos = (umrechner - (i * i)) % tabGroeße;
            return pos;
        }
    }
}
//Negative Zahlen bei separater Verkettung noch anschauen
//Verknüpfung aus Main fehlt noch
