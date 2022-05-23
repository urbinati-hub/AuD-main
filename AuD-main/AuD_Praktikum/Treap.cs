using System;

namespace AuD_Praktikum
{
    class TreapNode : BinTreeNode
    {
        public int priority;
        public TreapNode parent;

        public TreapNode(int priority)
        {
            this.priority = priority;
        }

        public override string ToString()
        {
            return $"{zahl}({priority})";
        }

    }
    class Treap : BinSearchTree
    {
        private Random random;


        public Treap()
        {
            root = null;
            // für die Priorität
            random = new Random();

        }

        /// <summary>
        /// Neue Funktion<see cref="TreapNode"/> gegebenen Parameter verwenden <paramref name="elem"/>
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        protected override BinTreeNode insertNode(int elem)
        {
            TreapNode a = new TreapNode(random.Next(0, 50)); 
            a.zahl = elem;
            a.left = null; a.right = null;
            // Parent Knoten initialisieren 
            a.parent = pred as TreapNode;
            return a;
        }


        public override bool insert(int elem)
        {
            // Neu casten
            TreapNode a = insert_(elem) as TreapNode;
            // eingefügtes Element nicht null
            if (a != null)
            {
                while (a != root && a.priority < a.parent.priority)
                {
                    // Parent und Grandparent initialisieren
                    TreapNode parent = a.parent;
                    TreapNode grandparent = a.parent.parent;
                    // checken, ob grandparent vorhanden
                    if (grandparent != null)
                    {
                        // wenn konten parent von n links steht
                        if (grandparent.left == a.parent)
                        {
                            grandparent.left = a;
                        }
                        // rechts
                        else
                        {
                            grandparent.right = a;
                        }
                    }
                    // Nach Priorität sortieren
                    if (a == parent.left)
                        RRot(a);
                    else
                        LRot(a);

                    a.parent = grandparent;
                    // Parent ersetzen
                    parent.parent = a;

                    // wenn Element die neue Wurzel
                    if (a.parent == null)
                    {
                        root = a;
                    }
                }
                return true;
            }
            return false;
        }

        // Rechtsrotation, Element wird rechts nach oben rotiert
        private TreapNode RRot(TreapNode n)
        {
            // Parent und Grandparent initialisieren
            TreapNode parent = n.parent;
            TreapNode right = (TreapNode)n.right; // behalte n->right
            
            // n links
            parent.left = right;
            if (right != null)
                right.parent = parent;
            n.right = parent;

            return n;
        }

        // Linksrotation, Element wird links nach oben rotiert
        private TreapNode LRot(TreapNode n)
        {
            // Parent initialisieren
            TreapNode parent = n.parent;
            TreapNode left = (TreapNode)n.left; // behalte n->left

            // n rechts
            parent.right = left;
            if (left != null)
                left.parent = parent;
            n.left = parent;

            return n;
        }

        public override bool delete(int elem)
        {
            TreapNode a = searchNode(elem) as TreapNode;
            if (a != null)
            {
                // Verwendung von DownHeap um Element nach unten "sickern" zu lassen
                //DownHeap(a);
                
                while (!(a.right == null && a.left == null))
                {
                    TreapNode left = a.left as TreapNode;
                    TreapNode right = a.right as TreapNode;
                    TreapNode next;

                    if (a.right == null)
                    {
                        next = RRot(left);
                    }
                    else if (a.left == null)
                    {
                        next = LRot(right);
                    }

                    else if (left.priority < right.priority)
                    {
                        next = RRot(left);
                    }
                    else
                    {
                       next = LRot(right);
                    }

                //Vorgänger zeigt auf Nachfolger
                if (a.parent != null)
                    {
                        if (a.parent.left == a)
                        {
                            // Parent zeigt auf nächstes Element
                            a.parent.left = next;
                        }
                        else
                        {
                            a.parent.right = next;
                        }
                    }

                    TreapNode grandParent = a.parent;
                    a.parent = next;
                    if (next != null)
                    {
                        next.parent = grandParent;
                        if (next.parent == null)
                        {
                            root = next;
                        }
                    }
                }

                // wenn nur noch ein Element vorhanden ist, dann ist dieses die Wurzel
                if (a.parent == null)
                {
                    root = null;
                }
                else
                {
                    // Entweder rechts oder links die Verbindung löschen
                    if (a == a.parent.left)
                    {
                        a.parent.left = null;
                    }
                    else
                    {
                        a.parent.right = null;
                    }
                }
                return true;
            }
            return false;
        }
    }



}
