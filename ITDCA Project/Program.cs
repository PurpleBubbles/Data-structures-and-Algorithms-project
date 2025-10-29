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

        public void q1_DeleteBook(q1_Book deleteBook)
        {
            if (rootNode == null)
            {
                Console.WriteLine($"\n No books stored in AVL Tree at this moment");
                return;
            }

            q1_Node nodeChecked = rootNode;

            while (nodeChecked.Value.Year != deleteBook.Year && nodeChecked.Value.Title != deleteBook.Title && nodeChecked.Value.Author != deleteBook.Author) //loop through books till match is found or end of tree reached
            {
                if (deleteBook.Year <= nodeChecked.Value.Year)
                {
                    if(nodeChecked.LeftChild != null) //value looked for can not be stored in tree
                    {
                        nodeChecked = nodeChecked.LeftChild;
                    }
                    else
                    {
                        Console.WriteLine($"\n No books stored in AVL Tree match the book details provided");
                        return;
                    }
                }
                else
                {
                    if (nodeChecked.RightChild != null) //value looked for can not be stored in tree
                    {
                        nodeChecked = nodeChecked.RightChild;
                    }
                    else
                    {
                        Console.WriteLine($"\n No books stored in AVL Tree match the book details provided");
                        return;
                    }
                }
            }

            //will be selecting the in order succesor when deleting any nodes (smallest year value in right subtree of node being deleted)
            q1_Node nodeToDelete = nodeChecked;
            q1_Node? inOrderSuccessor = nodeChecked.RightChild; //temp value for now

            if (nodeToDelete == rootNode && nodeToDelete.LeftChild == null && nodeToDelete.RightChild == null)
            {
                rootNode = null;
                Console.WriteLine($"\n Root node was deleted it was also the only value stored in the AVL tree");
            }
            else if (nodeToDelete.RightChild == null)
            {
                if (nodeToDelete.LeftChild == null) //node to delete is a leaf i.e. no children
                {
                    if (nodeToDelete.Parent.RightChild == nodeToDelete)
                    {
                        nodeToDelete.Parent.RightChild = null; //make the parent of the node to be deleted point to null
                    }
                    else
                    {
                        nodeToDelete.Parent.LeftChild = null; //make the parent of the node to be deleted point to null
                    }

                    q1_RecalculateNodeHeights(rootNode);
                    q1_BalanceAncestors(nodeToDelete.Parent);
                }
                else //no right children only one left child
                {
                    nodeToDelete.Value = nodeToDelete.LeftChild.Value;
                    nodeToDelete.LeftChild = null;

                    q1_RecalculateNodeHeights(rootNode);
                    q1_BalanceAncestors(nodeToDelete);
                }
            }
            else
            {
                while (inOrderSuccessor.LeftChild != null)
                {
                    inOrderSuccessor = inOrderSuccessor.LeftChild;
                }

                nodeToDelete.Value = inOrderSuccessor.Value;

                if (inOrderSuccessor.RightChild != null) //if left most node has a right child
                {
                    inOrderSuccessor.Parent.LeftChild = inOrderSuccessor.RightChild;
                    inOrderSuccessor.RightChild.Parent = inOrderSuccessor.Parent;
                }
                else 
                {
                    if (inOrderSuccessor.Parent.RightChild == inOrderSuccessor)
                    {
                        inOrderSuccessor.Parent.RightChild = null; //make the parent of the node to be deleted point to null
                    }
                    else
                    {
                        inOrderSuccessor.Parent.LeftChild = null; //make the parent of the node to be deleted point to null
                    }
                }

                q1_RecalculateNodeHeights(rootNode);
                q1_BalanceAncestors(inOrderSuccessor);

            }
        }

        private void q1_BalanceAncestors(q1_Node checkNodeBalance)
        {

            while (checkNodeBalance.Parent != null)
            {
                checkNodeBalance = checkNodeBalance.Parent;

                int leftHeight = -1;
                if (checkNodeBalance.LeftChild != null)
                {
                    leftHeight = checkNodeBalance.LeftChild.Height;
                }

                int rightHeight = -1;
                if (checkNodeBalance.RightChild != null)
                {
                    rightHeight = checkNodeBalance.RightChild.Height;
                }

                if ((leftHeight - rightHeight) >= 2 || (leftHeight - rightHeight) <= -2)
                {
                    //unbalanced make balanced
                    q1_balanceNode(checkNodeBalance);
                    break;
                }
            }
        }
        
        private void q1_balanceNode(q1_Node nodeToBalance)
        {
            int leftHeight = -1;
            if (nodeToBalance.LeftChild != null)
            {
                leftHeight = nodeToBalance.LeftChild.Height;
            }

            int rightHeight = -1;
            if (nodeToBalance.RightChild != null)
            {
                rightHeight = nodeToBalance.RightChild.Height;
            }

            if (leftHeight > rightHeight) //left-? imbalance
            {
                int leftLeftHeight = -1;
                if (nodeToBalance.LeftChild.LeftChild != null)
                {
                    leftLeftHeight = nodeToBalance.LeftChild.LeftChild.Height;
                }

                int leftRightHeight = -1;
                if (nodeToBalance.LeftChild.RightChild != null)
                {
                    leftRightHeight = nodeToBalance.LeftChild.RightChild.Height;
                }

                if (leftLeftHeight > leftRightHeight) //left-left imbalance
                {
                    q1_RightRotate(nodeToBalance);
                }
                else //left-right imbalance
                {
                    q1_LeftRotate(nodeToBalance.LeftChild);
                    q1_RightRotate(nodeToBalance);
                }
            }
            else //right-? imbalance
            {
                int rightLeftHeight = -1;
                if (nodeToBalance.RightChild.LeftChild != null)
                {
                    rightLeftHeight = nodeToBalance.RightChild.LeftChild.Height;
                }

                int rightRightHeight = -1;
                if (nodeToBalance.RightChild.RightChild != null)
                {
                    rightRightHeight = nodeToBalance.RightChild.RightChild.Height;
                }

                if (rightLeftHeight > rightRightHeight) //right-left imbalance
                {
                    q1_RightRotate(nodeToBalance.RightChild);
                    q1_LeftRotate(nodeToBalance);
                }
                else //right-right imbalance
                {
                    q1_LeftRotate(nodeToBalance);
                }
            }

            q1_RecalculateNodeHeights(rootNode);
        }

        private void q1_RecalculateNodeHeights(q1_Node? calculateHeightNode)
        {
            if (calculateHeightNode == null) //recursion termination point
            {
                return;
            }
            q1_RecalculateNodeHeights(calculateHeightNode.LeftChild);
            q1_RecalculateNodeHeights(calculateHeightNode.RightChild);

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

        private void q1_RightRotate(q1_Node nodeToBalance)
        {
            q1_Node imbalanceNode = nodeToBalance;
            q1_Node leftChildImbalanceNode = nodeToBalance.LeftChild;
            q1_Node? rightChildLeftChildImbalanceNode = nodeToBalance.LeftChild.RightChild;
            q1_Node? parentImbalancedNode = nodeToBalance.Parent;

            Console.WriteLine($"\nRotating right: {nodeToBalance.Value.Year}");

            if (imbalanceNode == rootNode)
            {
                rootNode = leftChildImbalanceNode;
                Console.WriteLine("Changing rootnode");
            }

            //Change parent of imbalance node's leftchild
            leftChildImbalanceNode.Parent = imbalanceNode.Parent;
            //Change parent of imbalance node
            imbalanceNode.Parent = leftChildImbalanceNode;
            //Change parent of imbalance node's left child's right child
            if (rightChildLeftChildImbalanceNode != null)//could not have a right child
            {
                rightChildLeftChildImbalanceNode.Parent = imbalanceNode;
            }
            //Change left child of imbalanced node
            imbalanceNode.LeftChild = rightChildLeftChildImbalanceNode;
            //change right child of imbalaced node's left child
            leftChildImbalanceNode.RightChild = imbalanceNode;


            if (parentImbalancedNode == null)
            {
                leftChildImbalanceNode.Parent = null;
                
            }
            else if (parentImbalancedNode.LeftChild == imbalanceNode)
            {
                parentImbalancedNode.LeftChild = leftChildImbalanceNode;
            }
            else if (parentImbalancedNode.RightChild == imbalanceNode)
            {
                parentImbalancedNode.RightChild = leftChildImbalanceNode;
            }
        }

        private void q1_LeftRotate(q1_Node nodeToBalance) 
        {
            q1_Node imbalanceNode = nodeToBalance;
            q1_Node rightChildImbalanceNode = nodeToBalance.RightChild;
            q1_Node? leftChildRightChildImbalanceNode = nodeToBalance.RightChild.LeftChild;
            q1_Node? parentImbalancedNode = nodeToBalance.Parent;

            Console.WriteLine($"\nRotating left: {nodeToBalance.Value.Year}");

            if (imbalanceNode == rootNode)
            {
                rootNode = rightChildImbalanceNode;
                Console.WriteLine("Changing rootnode");
            }

            //Change parent of imbalance node's rightchild
            rightChildImbalanceNode.Parent = imbalanceNode.Parent;
            //Change parent of imbalance node
            imbalanceNode.Parent = rightChildImbalanceNode;
            //Change parent of imbalance node's right child's left child
            if (leftChildRightChildImbalanceNode != null) //could not have a left child
            {
                leftChildRightChildImbalanceNode.Parent = imbalanceNode;
            }
            //Change right child of imbalanced node
            imbalanceNode.RightChild = leftChildRightChildImbalanceNode;
            //change left child of imbalaced node's right child
            rightChildImbalanceNode.LeftChild = imbalanceNode;

            if(parentImbalancedNode == null)
            {
                rightChildImbalanceNode.Parent = null;
            }
            if (parentImbalancedNode.LeftChild == imbalanceNode)
            {
                parentImbalancedNode.LeftChild = rightChildImbalanceNode;
            }

            if(parentImbalancedNode.RightChild == imbalanceNode)
            {
                parentImbalancedNode.RightChild = rightChildImbalanceNode;
            }

            Console.WriteLine("DOne rotating");
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
                bookTree.PrintTree();
                Console.WriteLine("\n_____________________________________");
            }

            q1_Book deleteBook = new q1_Book("The Da Vinci Code", "Dan Brown", 2003);

            bookTree.q1_DeleteBook(deleteBook);

            bookTree.PrintTree();
        }
    }
}
