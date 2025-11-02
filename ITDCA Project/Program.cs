using Question_1;
using System.Diagnostics.Metrics;
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

                if (leftLeftHeight >= leftRightHeight) //left-left imbalance
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

        public void q2_InOderTraversal()
        {
            if (rootNode == null)
            {
                Console.WriteLine($"No Books to print as tree is empty");
                return;
            }
            else
            {
                q2_InOderTraversalRecursion(rootNode);
            }
        }

        private void q2_InOderTraversalRecursion(q1_Node node)
        {
            if (node == null) //kill switch
            {
                return;
            }
            q2_InOderTraversalRecursion(node.LeftChild);
            Console.WriteLine($"{node.Value.Title}");
            q2_InOderTraversalRecursion(node.RightChild);
        }

        public void q2_SearchBookByYear(int bookYear)
        {
            if (rootNode == null)
            {
                Console.WriteLine($"\n------------There are no books stored in the AVL Tree at this moment------------\n");
                return;
            }

            q1_Node nodeChecked = rootNode;

            while (nodeChecked.Value.Year != bookYear) //loop through books till year match is found or end of tree reached
            {
                if (bookYear <= nodeChecked.Value.Year)
                {
                    if (nodeChecked.LeftChild != null)
                    {
                        nodeChecked = nodeChecked.LeftChild;
                    }
                    else
                    {
                        Console.WriteLine($"\n------------No books stored in AVL Tree match the book year({bookYear}) provided------------\n");
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
                        Console.WriteLine($"\n------------No books stored in AVL Tree match the book year({bookYear}) provided------------\n");
                        return;
                    }
                }
            }
            Console.WriteLine($"\n------------Book match found ({bookYear})------------\n" +
                $"Book year: {nodeChecked.Value.Year}\n" +
                $"Book Title: {nodeChecked.Value.Title}\n" +
                $"Book Author: {nodeChecked.Value.Author}\n");
        }

        public void q2_DisplayMostRecentBook()
        {
            q1_Node mostRecentNode = rootNode; //have to start somewhere

            if (mostRecentNode == null)
            {
                Console.WriteLine($"\n------------There are no books stored in the AVL Tree at this moment------------\n");
                return;
            }
            while (mostRecentNode.RightChild != null)
            {
                mostRecentNode = mostRecentNode.RightChild;
            }
            Console.WriteLine($"\n------------Most Recently Published Book)------------\n" +
                $"{mostRecentNode.Value.Title}, published in {mostRecentNode.Value.Year}.\n");
        }

        public void q2_DisplayNumberOfBooks()
        {
            if (rootNode == null)
            {
                Console.WriteLine($"\n------------Number of Books in AVL Tree------------\n" +
                    $"Number of books = ZERO (no books in AVL tree)\n");
                return;
            }
            else
            {
                int numBooks = q2_DisplayNumberOfBooksRecursion(rootNode);

                Console.WriteLine($"\n------------Number of Books in AVL Tree------------\n" +
                    $"Number of books = {numBooks}\n");
            }
        }

        private int q2_DisplayNumberOfBooksRecursion(q1_Node node)
        {
            if (node == null) //kill switch
            {
                return 0;
            }

            int counter = 0;

            counter += q2_DisplayNumberOfBooksRecursion(node.LeftChild);
            counter += q2_DisplayNumberOfBooksRecursion(node.RightChild);
            return counter + 1;
        }

        public void DebuggingPrintTree()
        {
            Console.WriteLine($"----Printed out AVL Book Tree----");

            q1_Node printNode = rootNode;

            Console.WriteLine($"Root node: {printNode.Value.Year}");

            DebuggingPrintParentPlusChildren(printNode);
        }

        private void DebuggingPrintParentPlusChildren(q1_Node toPrintNode)
        {
            if (toPrintNode == null)
            {
                Console.WriteLine($"No books to print");
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
            DebuggingPrintParentPlusChildren(toPrintNode.LeftChild);
            DebuggingPrintParentPlusChildren(toPrintNode.RightChild);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            q1_Book[] booksAdd = new q1_Book[]
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

            //insert books into book tree one at a time
            foreach (q1_Book book in booksAdd)
            {
                bookTree.q1_InsertBook(book);
            }

            while (true)
            {
                Console.WriteLine($"\n------------Monje's AVL Book Tree------------\n" +
                    $"1. In Order Traversal\n" +
                    $"2. Search for book by year\n" +
                    $"3. Display book most recently published\n" +
                    $"4. Display the number of book stored in tree\n" +
                    $"5. Insert new book into AVL Tree\n" +
                    $"6. Stop Application");

                Console.Write($"Please select a option (1-6): ");
                int userInput = 0;

                try
                {
                    userInput = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception )
                {
                    Console.WriteLine($"\n------------INVALID USER INPUT (enter an integer value from 1 to 6 only)------------");
                    continue;
                }

                if (userInput == 1) //call method to display books in-order(most left to right most book in tree)
                {
                    Console.WriteLine($"\n------------In Order Traversal------------\n");
                    bookTree.q2_InOderTraversal();
                }
                else if (userInput == 2) //call method to search for book that matches the year the user inputted
                {
                    while (true)
                    {
                        Console.WriteLine($"\n------------Search by Year------------\n");
                        Console.Write($"Please input a year value: ");

                        int yearSearch = 0;

                        try //try-catch block if user input was not an int value
                        {
                            yearSearch = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"\n------------INVALID USER INPUT (enter an INTEGER year value)------------");
                            continue;
                        }

                        if (yearSearch < 0) //if value negative also throw error
                        {
                            Console.WriteLine($"\n------------INVALID USER INPUT (enter an POSITIVE year value)------------");
                            continue;
                        }
                        else //call search book method
                        {
                            bookTree.q2_SearchBookByYear(yearSearch);
                            break;
                        }
                    }

                }
                else  if(userInput == 3) //call method to find and display the book with the most recent publish date
                {
                    bookTree.q2_DisplayMostRecentBook();
                }
                else if(userInput == 4) //call method to calaculate and display number of books currently stored in book tree.
                {
                    bookTree.q2_DisplayNumberOfBooks();
                }
                else if (userInput == 5) //call method to insert new book into AVL tree
                {
                    string? titleNewBook = null;
                    string? authorNewBook = null;
                    int yearNewBook = -1;

                    while (true)
                    {

                        Console.Write($"Please enter the Title of the book: ");
                        titleNewBook = Console.ReadLine();

                        if (titleNewBook.Count() == 0)
                        {
                            Console.WriteLine($"\n------------INVALID USER INPUT (enter a title for the book)------------");
                            continue;
                        }
                        else 
                        { 
                            break;
                        }
                    }

                    while (true)
                    {

                        Console.Write($"Please enter the Author of the book: ");
                        authorNewBook = Console.ReadLine();

                        if (authorNewBook.Count() == 0)
                        {
                            Console.WriteLine($"\n------------INVALID USER INPUT (enter a Author for the book)------------");
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (true)
                    {

                        Console.Write($"Please enter the Year of the book: ");

                        try //try-catch block if user input was not an int value
                        {
                            yearNewBook = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"\n------------INVALID USER INPUT (enter an INTEGER year value)------------");
                            continue;
                        }

                        if (yearNewBook <= 0) //user input error, ask for valid non negative user input
                        {
                            Console.WriteLine($"\n------------INVALID USER INPUT (enter a POSITIVE INTEGER year value)------------");
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }

                    q1_Book newBook = new q1_Book(titleNewBook, authorNewBook, yearNewBook);

                    bookTree.q1_InsertBook(newBook); //insert new book into tree

                    Console.WriteLine($"\n------------Book inserted------------\n" +
                        $"Title: {newBook.Title}\n" +
                        $"Author: {newBook.Author}\n" +
                        $"Publish date: {newBook.Year}\n");

                }
                else if(userInput == 6) //option to break out of loop and close program
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"\n------------INVALID USER INPUT (enter an INTEGER value from 1 to 6 only)------------");
                }
            }
        }
    }
}
