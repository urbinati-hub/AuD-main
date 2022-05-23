using System;
namespace AuD_Praktikum
{
    class AVLTreeNode : BinTreeNode
    {
        public AVLTreeNode parent;
        public int height = 0;
        public int balance; 
        public override string ToString()
        {
            return $"{zahl,3}({height}),({balance})";
        }
    }

    /// <summary>
    /// AVL tree, a balanced binary search tree, and derived of BinSearchTree.
    /// Necessary functions have been added to original functions of BinSearchTree.
    /// </summary>
    class AVLTree : BinSearchTree
    {
        /// <summary>
        /// Neue Funktion<see cref="AVLTreeNode"/> gegebenen Parameter verwenden <paramref name="elem"/>
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        protected override BinTreeNode insertNode(int elem)
        {
            AVLTreeNode node = new AVLTreeNode();
            node.zahl = elem;
            node.left = null; node.right = null;
            node.parent = pred as AVLTreeNode;
            node.height = 0;
            node.balance = 0;
            return node;
        }

        /// <summary>
        /// insert element using BinSearchTree.insert_, followed by organising
        /// the tree
        /// </summary>
        /// <param name="elem">value of element that shall be inserted</param>
        /// <returns>true if element has been inserted</returns>
        public override bool insert(int elem)
        {
            AVLTreeNode node = insert_(elem) as AVLTreeNode;
            if (node == null)
                return false;

            organiseTree(node);

            return true;
        }

        /// <summary>
        /// delete element using BinSearchTree.delete_, followed by organising
        /// the tree
        /// </summary>
        /// <param name="elem">value of element that shall be deleted</param>
        /// <returns>true if element was deleted</returns>
        public override bool delete(int elem)
        {
            BinTreeNode binNode = delete_(elem);
            if (binNode == null)
                return false;

            AVLTreeNode temp = (AVLTreeNode)binNode;
            if (temp != null)
            {
                if (temp == root)
                    temp.parent = null;

                if (temp.left != null)
                    ((AVLTreeNode)temp.left).parent = temp;
                if (temp.right != null)
                    ((AVLTreeNode)temp.right).parent = temp;

                temp.height = getHeight(temp);
                temp.balance = getBalance(temp);
                organiseTree(temp);
            }
            return true;
        }

        /// <summary>
        /// determines height of an element in the AVL tree 
        /// </summary>
        /// <param name="node">tree element whose height shall be determined</param>
        /// <returns>height as integer</returns>
        private int getHeight(BinTreeNode node)
        {
            if (node == null)
                return 0;
            else
            {
                if (node.left == null && node.right == null)
                    return 0;

                int heightLeft = 0;
                int heightRight = 0;

                if (node.left != null)
                    heightLeft = ((AVLTreeNode)node.left).height;
                if (node.right != null)
                    heightRight = ((AVLTreeNode)node.right).height;

                return 1 + Math.Max(heightLeft, heightRight);

            }
        }

        /// <summary>
        /// determines balance factor of an element in the AVL tree
        /// </summary>
        /// <param name="node">element whose balance factor shall be determined</param>
        /// <returns>balance factor as integer</returns>
        public int getBalance(BinTreeNode node)
        {
            int heightLeft = 0;
            int heightRight = 0;

            if (node.left != null)
                heightLeft = ((AVLTreeNode)node.left).height + 1;
            if (node.right != null)
                heightRight = ((AVLTreeNode)node.right).height + 1;

            return heightRight - heightLeft;
        }

        /// <summary>
        /// refreshes both height and balance factor for a line from a given
        /// node to the root, returns either the root or the first node with 
        /// either a balance factor of +2/-2 (reorganisation needed) or a height 
        /// factor which is not changing (no further reorganisation needed at all)
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private AVLTreeNode refreshFactors(AVLTreeNode current)
        {
            while (Math.Abs(current.balance) != 2)
            {
                if (current.parent != null)
                    current = current.parent;
                else
                    break;

                current.balance = getBalance(current);

                int height = getHeight(current);
                if (current.height != height)
                    current.height = height;
                else
                    break;
            }
            return current;
        }

        /// <summary>
        /// organises tree by rotating, from current element to the root until
        /// either the root or a node with either balance factor equal 0 or
        /// unchanging height has been reached 
        /// </summary>
        /// <param name="current">element in whose line to the root might be
        /// rotations necessary</param>
        private void organiseTree(AVLTreeNode current)
        {
            current = refreshFactors(current);
            AVLTreeNode nodeLeft = (AVLTreeNode)current.left;
            AVLTreeNode nodeRight = (AVLTreeNode)current.right;

            if (current.balance == -2)
            {
                //LR rotation
                if (nodeLeft.balance == 1)
                {
                    rotateLeft((AVLTreeNode)current.left.right);
                    rotateRight((AVLTreeNode)current.left);
                }
                //R rotation
                else if (nodeLeft.balance == 0 || nodeLeft.balance == -1)
                    rotateRight(nodeLeft);
            }
            else if (current.balance == 2)
            {
                //L rotation
                if (nodeRight.balance == 1 || nodeRight.balance == 0)
                    rotateLeft(nodeRight);
                //RL rotation
                else if (nodeRight.balance == -1)
                {
                    rotateRight((AVLTreeNode)current.right.left);
                    rotateLeft((AVLTreeNode)current.right);
                }
            }
            else
                return;

            organiseTree(current);
        }

        /// <summary>
        /// rotates the element to the left
        /// </summary>
        /// <param name="newRoot">element that shall be rotated to the left</param>
        private void rotateLeft(AVLTreeNode newRoot)
        {
            AVLTreeNode origin = newRoot.parent.parent;
            AVLTreeNode oldRoot = newRoot.parent;
            AVLTreeNode temp = (AVLTreeNode)newRoot.left;
            newRoot.parent.right = temp;
            if (temp != null)
                temp.parent = oldRoot;
            newRoot.left = oldRoot;
            oldRoot.parent = newRoot;
            if (origin != null)
            {
                if (origin.right == newRoot.parent)
                {
                    origin.right = newRoot;
                    newRoot.parent = origin;
                }
                else
                {
                    origin.left = newRoot;
                    newRoot.parent = origin;
                }
            }
            else
            {
                root = newRoot;
                newRoot.parent = null;
            }
            oldRoot.height = getHeight(oldRoot);
            oldRoot.balance = 0;
            //set height of new root to 0 to force a check of its parent in
            //refreshFactors later on
            newRoot.height = 0;
            newRoot.balance = 0;
        }

        /// <summary>
        /// rotates the element to the right
        /// </summary>
        /// <param name="newRoot">element that shall be rotated to the right</param>
        private void rotateRight(AVLTreeNode newRoot)
        {
            AVLTreeNode origin = newRoot.parent.parent;
            AVLTreeNode oldRoot = newRoot.parent;
            AVLTreeNode temp = (AVLTreeNode)newRoot.right;
            newRoot.parent.left = temp;
            if (newRoot.right != null)
                temp.parent = oldRoot;
            newRoot.right = oldRoot;
            oldRoot.parent = newRoot;
            if (origin != null)
            {
                if (origin.right == newRoot.parent)
                {
                    origin.right = newRoot;
                    newRoot.parent = origin;
                }
                else
                {
                    origin.left = newRoot;
                    newRoot.parent = origin;
                }
            }
            else
            {
                root = newRoot;
                newRoot.parent = null;
            }
            oldRoot.height = getHeight(oldRoot);
            oldRoot.balance = 0;
            //set height of new root to 0 to force a check of its parent in
            //refreshFactors later on
            newRoot.height = 0;
            newRoot.balance = 0;
        }
    }
}
