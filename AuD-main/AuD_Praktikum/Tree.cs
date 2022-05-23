using System;
using System.Collections.Generic;
using System.Text;

namespace AuD_Praktikum
{
   
    class BinTreeNode 
    {
        public int zahl;
        public BinTreeNode left;
        public BinTreeNode right;
        public override string ToString()
        {
            return $"{zahl,3}";
        }
    }

    /// <summary>
    /// Representiert eine generic <see cref="BinSearchTree{T}"/> Klasse 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class BinSearchTree : ISetSorted
    {
        protected BinTreeNode root;
        protected BinTreeNode pred; //Ablagevariable für Vorgängerknoten
        protected string dir; //Richtung, in der a von pred aus gesehen hängt

        /// <summary>
        /// offizielle Suchfunktion, enthält searchNode
        /// </summary>
        /// <param name="elem">zu suchendes Element</param>
        /// <returns>true, wenn Element gefunden</returns>
        public bool search(int elem)
        {
            BinTreeNode a = searchNode(elem);
            if (a != null) //a gefunden
                return true;
            else
                return false;

        }

        /// <summary>
        /// Suchfunktion der Klasse, legt pred und dir in Klassenvariable ab
        /// </summary>
        /// <param name="elem">zu suchendes Element</param>
        /// <returns>gefundener Knoten oder null, wenn nicht gefunden</returns>
        protected BinTreeNode searchNode(int elem)
        {
            BinTreeNode a = root;
            if (a == null || a.zahl == elem) //Baum leer oder Wurzel ist gesuchtes Element
            {
                dir = "root";
                pred = null;
            }
            while (a != null) //Baum noch nicht fertig durchlaufen
            {
                if (elem < a.zahl) //gesuchtes Element gehört in den linken Teilbaum
                {
                    pred = a;
                    dir = "left";
                    a = a.left;
                }
                else if (elem > a.zahl) //gesuchtes Element gehört in den rechten Teilbaum
                {
                    pred = a;
                    dir = "right";
                    a = a.right;
                }
                else //gesuchtes Element gefunden, in a gespeichert
                    break;
            }
            return a;
        }

        /// <summary>
        /// offizielle Einfügefunktion, ruft insert_ auf
        /// </summary>
        /// <param name="elem">einzufügendes Element</param>
        /// <returns>true, wenn Element eingefügt wurde</returns>
        public virtual bool insert(int elem)
        {
            BinTreeNode a = insert_(elem);
            if (a == null) //Element war schon enthalten
                return false;
            else
                return true;
        }

        /// <summary>
        /// Hilfseinfügefunktion, enthält searchNode und insertNode
        /// </summary>
        /// <param name="elem">einzufügendes Element</param>
        /// <returns>eingefügter Knoten oder null, wenn Element schon vorhanden</returns>
        protected BinTreeNode insert_(int elem)
        {
            BinTreeNode a = searchNode(elem);
            if (a != null) //Element schon im Baum enthalten --> wird nicht nochmal eingefügt
                return null;
            a = insertNode(elem); //Funktion eines Konstruktors
            if (dir == "root") //Baum leer --> neues Element wird Wurzel
                root = a;
            else if (dir == "left") //neues Element ist linker Nachfolger
                pred.left = a;
            else //neues Element ist rechter Nachfolger
                pred.right = a;
            return a; //eingefügtes Element
        }

        /// <summary>
        /// offizielle Löschfunktion, ruft delete_ auf
        /// </summary>
        /// <param name="elem">zu löschendes Element</param>
        /// <returns>true, wenn Element gelöscht wurde</returns>
        public virtual bool delete(int elem)
        {
            BinTreeNode node = delete_(elem);
            if (node == null) //Element war nicht enthalten
                return false;
            else
                return true;
        }

        /// <summary>
        /// Hilfslöschfunktion, enthält searchNode 
        /// </summary>
        /// <param name="node">zu löschender Knoten</param>
        /// <returns>gelöschter Knoten oder null, wenn Element nicht vorhanden</returns>
        protected BinTreeNode delete_(int elem)
        {
            BinTreeNode a = searchNode(elem);
            BinTreeNode b;
            if (a == null) //Element nicht im Baum enthalten
                return null;
            if (a.right != null && a.left != null) //a hat zwei Nachfolger
            {
                BinTreeNode c;
                b = a;
                if (b.left.right != null) //suche symmetrischen Vorgänger
                {
                    b = b.left;
                    while (b.right.right != null)
                    {
                        b = b.right;
                    }
                }
                if (b == a) //symmetrischer Vorgänger ist linker Nachfolger
                {
                    c = b.left;
                    b.left = c.left;
                }
                else
                {
                    c = b.right;
                    b.right = c.left;
                }
                a.zahl = c.zahl; //setze Wert des symmetrischen Vorgängers an zu löschende Stelle. 
                return b;
            }
            //a hat höchstens einen Nachfolger
            //b speichert den einzigen Nachfolger oder null
            if (a.left == null)
                b = a.right;
            else
                b = a.left;
            if (root == a) //a ist Wurzel
            {
                root = b;
                return root;
            }
            if (dir == "left") //a ist linker Nachfolger eines Knotens
                pred.left = b;
            else //a ist rechter Nachfolger eines Knotens
                pred.right = b;
            return pred;
        }

        /// <summary>
        /// offizielle Print-Funktion
        /// </summary>
        public void print()
        {
            //Baum wird um 90° gedreh und ausgegeben
            treePrint(root, 0, "null");
            Console.WriteLine();
        }

        /// <summary>
        /// Funktion zum Einfügen eines neuen Knotens, sollte in abgeleiteten Klassen überschrieben werden
        /// </summary>
        /// <param name="elem">Wert des Knotens</param>
        /// <returns>eingefügter Knoten</returns>
        protected virtual BinTreeNode insertNode(int elem)
        {
            BinTreeNode a = new BinTreeNode();
            a.zahl = elem;
            a.left = null; a.right = null;
            return a;
        }

        /// <summary>
        /// rekursive Funktion zur Ausgabe eines Baums
        /// </summary>
        /// <param name="a">aktueller Knoten</param>
        /// <param name="level">Höhe im Baum (0: Wurzel)</param>
        protected void treePrint(BinTreeNode a, int level, string direction)
        {
            if (a != null)
            {
                treePrint(a.right, level + 1, "right"); //rufe treePrint für rechten Nachfolger mit höherem Level auf
                indentPrint(a, level, direction);
                treePrint(a.left, level + 1, "left"); //rufe treePrint für linken Nachfolger mit höherem Level auf
            }
        }

        /// <summary>
        /// Funktion zur Ausgabe eines Baumknotens, sollte evtl. in abgeleiteten Funktionen überschrieben werden
        /// </summary>
        /// <param name="a">Baumknoten</param>
        /// <param name="level">Höhe im Baum</param>
        protected void indentPrint(BinTreeNode a, int level, string direction)
        {
            //rückt den übergebenen Knoten ein, je nachdem, wie hoch das level des Baums ist.
            if (level == 0) //Knoten ist Wurzel
                Console.WriteLine(a);
            else if (level == 1)
                if (direction == "right")
                    Console.WriteLine("    /-- " + a);
                else
                    Console.WriteLine("    \\-- " + a);
            else
            {
                for (int i = 0; i < level - 1; i++)
                {
                    Console.Write("       ");
                }
                if (direction == "right")
                    Console.WriteLine("    /-- " + a);
                else
                    Console.WriteLine("    \\-- " + a);
            }
        }
    }
}
