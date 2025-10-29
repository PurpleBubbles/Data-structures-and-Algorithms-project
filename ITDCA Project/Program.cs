using Question_1;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace Question_1
{
    public class q1_Book
    {
        public string Title;
        public string Author;
        public int Year;

       public q1_Book(string title, string author, int year)
        {
            Title = title;
            Author = author;
            Year = year;
        }
    }
    
    public class q1_Node
    {
        public q1_Book Value;
        public q1_Node? Parent; //can be null if node is root node i.e. no parent node
        public q1_Node? LeftChild; //can be null if node does not have a left child
        public q1_Node? RightChild; //can be null if node does not have a right child
        public int Height;

        public q1_Node(q1_Book value, q1_Node? parent, q1_Node? leftChild, q1_Node? rightChild, int height)
        {
            Value = value;
            Parent = parent;
            LeftChild = leftChild;
            RightChild = rightChild;
            Height = height;
        }
    }

    public class q1_BookTree
    {
        private q1_Node? rootNode;
        public void q1_InsertBook(q1_Book book)
        {
            q1_Node newNode = new q1_Node(book, null, null, null, 0);

            if (rootNode == null)
            {
                rootNode = newNode;
                return;
            }
            else
            {
                q1_Node tempNode = rootNode;

                while (true)
                {
                    if (newNode.Value.Year <= tempNode.Value.Year) //if book year value of new node is less or equal to book year value of temparary node
                    {
                        if (tempNode.LeftChild == null) // tempNode's left child is null then we are at end and can store new node as left child
                        {
                            newNode.Parent = tempNode; //set new node's parent to that of temp node
                            tempNode.LeftChild = newNode;
                            break;
                        }
                        else // tempNode's left child not null, make new temp Node equal to left child and check year values again with while loop
                        {
                            tempNode = tempNode.LeftChild;
                        }
                    }
                    else
                    {
                        if (tempNode.RightChild == null) // tempNode's right child is null then we at end and can store new node as right child
                        {
                            newNode.Parent = tempNode; //set new node's parent to that of temp node
                            tempNode.RightChild = newNode;
                            break;
                        }
                        else // tempNode's right child not null, make new temp Node equal to right child and check year values again with while loop
                        {
                            tempNode = tempNode.RightChild;
                        }
                    }
                }
            }
            // change heights of parent nodes after insertion of new leaf node
            q1_Node nodeHeightChange = newNode;

            while (nodeHeightChange.Parent != null)
            {
                nodeHeightChange = nodeHeightChange.Parent;

                int leftHeight = -1; 
                if (nodeHeightChange.LeftChild != null)
                {
                    leftHeight = nodeHeightChange.LeftChild.Height;
                }

                int rightHeight = -1;
                if (nodeHeightChange.RightChild != null)
                {
                    rightHeight = nodeHeightChange.RightChild.Height;
                }

                nodeHeightChange.Height = Math.Max(leftHeight, rightHeight) + 1;
            }

            q1_BalanceAncestors(newNode); //now to check if tree still balanced and if not balance it
        }

        public void q1_DeleteBook()
        {

        }

        private void q1_BalanceAncestors(q1_Node checkNodeBalance)
        {

            while (checkNodeBalance.Parent != null)
            {
                checkNodeBalance = checkNodeBalance.Parent;

                if ((checkNodeBalance.LeftChild.Height - checkNodeBalance.RightChild.Height) >= 2 || (checkNodeBalance.LeftChild.Height - checkNodeBalance.RightChild.Height) <= -2)
                {
                    //unbalanced make balanced
                    balanceNode(checkNodeBalance);
                    break;
                }
            }
        }
        
        private void balanceNode(q1_Node nodeToBalance)
        {
            if (nodeToBalance.LeftChild.Height > nodeToBalance.RightChild.Height) //left-? imbalance
            {
                if (nodeToBalance.LeftChild.LeftChild.Height > nodeToBalance.LeftChild.RightChild.Height) //left-left imbalance
                {
                    RightRotate(nodeToBalance);
                }
                else //left-right imbalance
                {
                    LeftRotate(nodeToBalance.LeftChild);
                    RightRotate(nodeToBalance);
                }
            }
            else //right-? imbalance
            {
                if(nodeToBalance.RightChild.LeftChild.Height > nodeToBalance.RightChild.RightChild.Height) //right-left imbalance
                {
                    RightRotate(nodeToBalance.RightChild);
                    LeftRotate(nodeToBalance);
                }
                else //right-right imbalance
                {
                    LeftRotate(nodeToBalance);
                }
            }

            RecalculateNodeHeights(rootNode);
        }

        private void RecalculateNodeHeights(q1_Node? calculateHeightNode)
        {
            if (calculateHeightNode == null) //recursion termination point
            {
                return;
            }
            RecalculateNodeHeights(calculateHeightNode.LeftChild);
            RecalculateNodeHeights(calculateHeightNode.RightChild);

            int leftHeight = -1;
            if (calculateHeightNode.LeftChild != null)
            {
                leftHeight = calculateHeightNode.LeftChild.Height;
            }

            int rightHeight = -1;
            if (calculateHeightNode.RightChild != null)
            {
                rightHeight = calculateHeightNode.RightChild.Height;
            }

            calculateHeightNode.Height = Math.Max(leftHeight, rightHeight) + 1;

        }

        private void RightRotate(q1_Node nodeToBalance)
        {
            q1_Node imbalanceNode = nodeToBalance;
            q1_Node? leftChildImbalanceNode = nodeToBalance.LeftChild;
            q1_Node? rightChildLeftChildImbalanceNode = nodeToBalance.LeftChild.RightChild;

            //Change parent of imbalance node's leftchild
            leftChildImbalanceNode.Parent = imbalanceNode.Parent;
            //Change parent of imbalance node
            imbalanceNode.Parent = leftChildImbalanceNode;
            //Change parent of imbalance node's left child's right child
            rightChildLeftChildImbalanceNode.Parent = imbalanceNode;
            //Change left child of imbalanced node
            imbalanceNode.LeftChild = rightChildLeftChildImbalanceNode;
            //change right child of imbalaced node's left child
            leftChildImbalanceNode.RightChild = imbalanceNode;

        }

        private void LeftRotate(q1_Node nodeToBalance) 
        {
            q1_Node imbalanceNode = nodeToBalance;
            q1_Node? rightChildImbalanceNode = nodeToBalance.RightChild;
            q1_Node? leftChildRightChildImbalanceNode = nodeToBalance.RightChild.LeftChild;

            //Change parent of imbalance node's rightchild
            rightChildImbalanceNode.Parent = imbalanceNode.Parent;
            //Change parent of imbalance node
            imbalanceNode.Parent = rightChildImbalanceNode;
            //Change parent of imbalance node's right child's left child
            leftChildRightChildImbalanceNode.Parent = imbalanceNode;
            //Change right child of imbalanced node
            imbalanceNode.RightChild = leftChildRightChildImbalanceNode;
            //change left child of imbalaced node's right child
            rightChildImbalanceNode.LeftChild = imbalanceNode;

        }

        public void PrintTree()
        {
            Console.WriteLine($"----Printed out AVL Book Tree----");

            q1_Node printNode = rootNode;

            Console.WriteLine($"Root node: {printNode.Value.Year}");

            PrintParentPlusChildren(printNode);
        }

        private void PrintParentPlusChildren(q1_Node toPrintNode)
        {
            if (toPrintNode == null)
            {
                return;
            }
            Console.WriteLine($"\nParent: {toPrintNode.Value.Year}, height: {toPrintNode.Height}");

            if (toPrintNode.LeftChild == null && toPrintNode.RightChild == null)
            {
                Console.Write($"Left Child of {toPrintNode.Value.Year}: NULL ------- Right Child of {toPrintNode.Value.Year}: NUll");
            }
            else if (toPrintNode.LeftChild == null && toPrintNode.RightChild != null)
            {
                Console.Write($"Left Child of {toPrintNode.Value.Year}: NULL ------- Right Child of {toPrintNode.Value.Year}: {toPrintNode.RightChild.Value.Year}, height: {toPrintNode.RightChild.Height}\n");
            }
            else if (toPrintNode.LeftChild != null && toPrintNode.RightChild == null)
            {
                Console.Write($"Left Child of {toPrintNode.Value.Year}: {toPrintNode.LeftChild.Value.Year}, height: {toPrintNode.LeftChild.Height} ------- Right Child of {toPrintNode.Value.Year}: NUll");
            }
            else
            {
                Console.Write($"Left Child of {toPrintNode.Value.Year}: {toPrintNode.LeftChild.Value.Year}, height: {toPrintNode.LeftChild.Height} ------- Right Child of {toPrintNode.Value.Year}: {toPrintNode.RightChild.Value.Year}, height: {toPrintNode.RightChild.Height}\n");
            }
            PrintParentPlusChildren(toPrintNode.LeftChild);
            PrintParentPlusChildren(toPrintNode.RightChild);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            q1_Book[] treeBooks = new q1_Book[]
            {
                new q1_Book("The Silent Patient", "Alex Michaelides", 2019),
                new q1_Book("The Great Gatsby", "F. Scott Fitzgerald",1925),
                new q1_Book("To Kill a Mockingbird", "Harper Lee", 1960),
                new q1_Book("1984", "George Orwell", 1949),
                new q1_Book("The Catcher in the Rye", "J.D. Salinger", 1951),
                new q1_Book("Pride and Prejudice", "Jane Austen", 1813),
                new q1_Book("Moby-Dick", "Herman Melville", 1851),
                new q1_Book("The Hobbit", "J.R.R. Tolkien", 1937),
                new q1_Book("Brave New World", "Aldous Huxley", 1932),
                new q1_Book("The Book Thief", "Markus Zusak", 2005),
                new q1_Book("The Road", "Cormac McCarthy", 2006),
                new q1_Book("Harry Potter and the Philosopher's Stone", "J.K. Rowling", 1997),
                new q1_Book("The Girl with the Dragon Tattoo", "Stieg Larsson", 2005),
                new q1_Book("The Alchemist", "Paulo Coelho", 1988),
                new q1_Book("The Shining", "Stephen King", 1977),
                new q1_Book("Wuthering Heights", "Emily Brontë", 1847),
                new q1_Book("Catch-22", "Joseph Heller", 1961),
                new q1_Book("The Hunger Games", "Suzanne Collins", 2008),
                new q1_Book("The Da Vinci Code", "Dan Brown", 2003),
                new q1_Book("The Outsiders", "S.E. Hinton", 1967)
            };

            q1_BookTree bookTree = new q1_BookTree(); //instantiate a new object of the booktree class

            //loop through each element in the treeBooks array and call the insert book method for each book element
            foreach (q1_Book book in treeBooks)
            {
                bookTree.q1_InsertBook(book);
            }

            bookTree.PrintTree();

        }
    }
}
