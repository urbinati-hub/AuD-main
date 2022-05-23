using System;
namespace AuD_Praktikum
{
    abstract class LinkedList : IDictionary
    {
        protected class LElem
        {
            public int elem;

            public LElem next = null;

            public LElem(int elem) { this.elem = elem; }
        }

        protected LElem first = null, last = null, position = null, prevposition = null;

        protected int count; // Anzahl der Elemente in der Liste

        public void print()
        {
            Console.WriteLine($"Anzahl der Elemente: {count}");
            for (LElem tmp = first; tmp != null; tmp = tmp.next)
                Console.WriteLine(tmp.elem);
        }

        // Suche
        public bool search(int elem) // Speichert das jeweils vorderste gefundene Element zwischen
        {
            position = null;
            if (first == null) // Liste ist leer
            {
                return false;
            }
            else if (first.elem.CompareTo(elem) == 0) // Das gesuchte Element befindet sich am Anfang
            {
                position = first;
                return true;
            }
            else // Das gesuchte Element befindet sich mittendrin ODER am Ende
            {
                LElem tmp = first;
                while (tmp.elem.CompareTo(elem) != 0) // Suche gibt Position des jeweils vordersten elem zurück
                {
                    if (tmp.next != null)
                        if (tmp.next.elem.CompareTo(elem) == 0) // Position vor dem gesuchten Element wird zwischengespeichert (nötig für delete)
                            prevposition = tmp;
                    if (tmp.elem.CompareTo(elem) <= 0) // Falls Element nicht vorhanden, wird zusätzlich Stelle des größten kleineren Element gespeichert (nötig für Sorted insert)
                        position = tmp;
                    if (tmp.next == null)
                        break;
                    tmp = tmp.next;
                }
                if (tmp.next != null) // While stoppte obwohl Liste nicht zu Ende, d.h. Element gefunden
                {
                    return true;
                }
                else if (last.elem.CompareTo(elem) == 0) // Das gesuchte Element befindet sich am Ende
                {
                    position = last;
                    return true;
                }
                else // Das gesuchte Element ist nicht vorhanden
                {
                    return false;
                }
            }
        }

        // Löschen
        public bool delete(int elem) // Löscht das jeweils vorderste gefundene Element
        {
            if (search(elem) == true)
            {
                if (first == last) // Nur ein Element
                {
                    first = last = null;
                    count--;
                    return true;
                }
                else // Mindestens zwei Elemente
                {
                    if (first.elem.CompareTo(elem) == 0) // Erstes Element löschen
                    {
                        first = first.next;
                        count--;
                        return true;
                    }
                    else if (prevposition.next != last) // Element mittendrin löschen
                    {
                        prevposition.next = prevposition.next.next;
                        count--;
                        return true;
                    }
                    else // Letztes Element löschen
                    {
                        prevposition.next = null;
                        last = prevposition;
                        count--;
                        return true;
                    }
                }
            }
            else { return false; }
        }

        // Einfügen
        public abstract bool insert(int elem); // Wird Datentyp spezifisch implementiert, da nicht bei allen gleich


    }

    // Insgesamt vier geforderte Klassen für verkettete Listen

    class MultiSetSortedLinkedList : LinkedList
    {
        public override bool insert(int elem)
        {
            search(elem);
            _insert_(elem);
            return true;
        }

        protected void _insert_(int elem) // Ausgelagerte Hilfsfunktion, um redundanten Suchaufruf in erbender Klasse zu vermeiden
        {
            LElem nelem = new LElem(elem);

            if (first == null) // Liste leer
            {
                first = last = nelem;
                count++;
            }
            else if (first == last) // Liste hat nur ein Element
            {
                if (elem > last.elem) // Add End
                {
                    last.next = nelem;
                    last = last.next;
                    count++;
                }
                else // (elem <= last.elem) Add Front
                {
                    nelem.next = first;
                    first = nelem;
                    count++;
                }
            }
            else if (elem < first.elem) // Am Anfang einfügen
            {
                nelem.next = first;
                first = nelem;
                count++;
            }
            else if (position != last) // Mittendrin einfügen
            {
                nelem.next = position.next;
                position.next = nelem;
                count++;
            }
            else // Am Ende einfügen
            {
                last.next = nelem;
                last = last.next;
                count++;
            }
        }
    }

    class MultiSetUnsortedLinkedList : LinkedList
    {
        public override bool insert(int elem)
        {
            LElem nelem = new LElem(elem);
            if (first == null) // Liste leer
            {
                first = last = nelem;
                count++;
                return true;
            }
            else // Liste nicht leer, am Ende einfügen
            {
                last.next = nelem;
                last = last.next;
                count++;
                return true;

            }
        }
    }

    class SetSortedLinkedList : MultiSetSortedLinkedList
    {
        public override bool insert(int elem)
        {
            if (search(elem) == false) // Element noch nicht vorhanden
            {
                _insert_(elem);
                return true;
            }
            else // Element bereits vorhanden
                return false;
        }
    }

    class SetUnsortedLinkedList : MultiSetUnsortedLinkedList
    {
        public override bool insert(int elem)
        {
            if (search(elem) == false) // Element noch nicht vorhanden
            {
                base.insert(elem);
                return true;
            }
            else // Element bereits vorhanden
                return false;
        }
    }
}