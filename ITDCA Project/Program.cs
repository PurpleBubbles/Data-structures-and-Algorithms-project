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

        public q1_Node(q1_Book value, q1_Node? parent, q1_Node? leftChild, q1_Node? rightChild)
        {
            Value = value;
            Parent = parent;
            LeftChild = leftChild;
            RightChild = rightChild;
        }
    }

    public class q1_BookTree
    {
        private q1_Node rootNode;
        public void q1_InsertBook(q1_Book book)
        {
            q1_Node newNode = new q1_Node(book, null, null, null);

            if (rootNode == null)
            {
                rootNode = newNode;
            }
            else
            {
                q1_Node tempNode = rootNode;

                while (true)
                {
                    if (newNode.Value.Year <= tempNode.Value.Year) //if book year value of new node is less or equal to book year value of temparary node
                    {
                        if (tempNode.LeftChild == null) // tempNode's left child is null then we at end and can store new node as left child
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
        }

        public void q1_DeleteBook()
        {

        }

        private void q1_BalanceBookTree()
        {

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
            Console.WriteLine($"\nParent: {toPrintNode.Value.Year}");

            if (toPrintNode.LeftChild == null && toPrintNode.RightChild == null)
            {
                Console.Write($"Left Child of {toPrintNode.Value.Year}: NULL ------- Right Child of {toPrintNode.Value.Year}: NUll");
            }
            else if (toPrintNode.LeftChild == null && toPrintNode.RightChild != null)
            {
                Console.Write($"Left Child of {toPrintNode.Value.Year}: NULL ------- Right Child of {toPrintNode.Value.Year}: {toPrintNode.RightChild.Value.Year}\n");
            }
            else if (toPrintNode.LeftChild != null && toPrintNode.RightChild == null)
            {
                Console.Write($"Left Child of {toPrintNode.Value.Year}: {toPrintNode.LeftChild.Value.Year} ------- Right Child of {toPrintNode.Value.Year}: NUll");
            }
            else
            {
                Console.Write($"Left Child of {toPrintNode.Value.Year}: {toPrintNode.LeftChild.Value.Year} ------- Right Child of {toPrintNode.Value.Year}: {toPrintNode.RightChild.Value.Year}\n");
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
