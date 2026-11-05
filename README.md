# Data Structures and Algorithms in C#

Four console applications written from first principles in C# / .NET 8: a self-balancing **AVL tree**, a **priority queue**, and **QuickSort**.

## About this project

This is coursework, submitted for **ITDCA2-44 — Data Structures and Algorithms in C#** (BSc IT, Eduvos) in October and November 2025. It was an individual assignment, and the code here is as submitted.

I have published it because the AVL tree was the most interesting thing I had written up to that point, and because the rotation logic is worth keeping somewhere I can find it again.

## Repository layout

| Folder | Contents |
| --- | --- |
| `ITDCA Project/` | Questions 1 and 2 — an AVL tree storing book listings |
| `Question-3/` | Question 3 — a priority queue for a clinic triage system |
| `Question-4/` | Question 4 — QuickSort over hotel listings read from JSON |

Each folder is a standalone console project. `ITDCA Project.sln` opens all three.

## Running it

Requires the .NET 8 SDK.

```bash
dotnet run --project "ITDCA Project"      # AVL book tree
dotnet run --project Question-3           # clinic priority queue
dotnet run --project Question-4           # hotel sorting
```

The first two present a numbered menu.

Question 4 reads a file named `q4_data.json` from its working directory. That file was supplied with the assignment and is not included here, so to run Question 4 you will need to provide one — a JSON array of objects with the fields `id`, `name`, `nightly_rate`, `stars` and `distance_from_airport`.

---

## Questions 1 and 2 — AVL tree of book listings

**The problem.** Store book listings in an AVL tree. A book has a title, an author and a year. The tree is ordered by year: earlier years to the left, later years to the right. The tree must support insertion and deletion, and must rebalance itself. On top of that, provide an in-order traversal, a binary search by year, a lookup of the most recently published book, and a count of the books held.

**What is implemented.** A node holds its value, its height, and references to its parent and both children. Keeping a parent pointer makes rebalancing after an insertion a matter of walking upwards from the new leaf rather than unwinding a recursion, which is the approach I found easier to reason about.

| Operation | Complexity |
| --- | --- |
| Insert | O(log n) to place, see the note below on rebalancing |
| Delete | O(log n) to locate, see the note below |
| Search by year | O(log n) |
| Most recently published | O(log n) — the rightmost node |
| In-order traversal | O(n) |
| Count | O(n) |

### How the balancing works

Every node stores its **height**, defined so that a leaf has height 0 and a null child counts as −1. A node's **balance factor** is the height of its left subtree minus the height of its right subtree, and an AVL tree is valid only while every balance factor is −1, 0 or +1.

When an insertion or deletion pushes a balance factor to ±2, the tree is repaired by **rotation**. There are four cases, decided by comparing the heights of the offending node's grandchildren:

| Case | Repair |
| --- | --- |
| Left-left | one right rotation |
| Right-right | one left rotation |
| Left-right | left-rotate the left child, then right-rotate the node |
| Right-left | right-rotate the right child, then left-rotate the node |

The point of a rotation is that it changes the **height** of a subtree while preserving its **in-order ordering**. That is what makes it a legal repair: the tree gets shorter, and a traversal still produces the same sequence of years.

### Deletion

Deletion uses the **in-order successor** — the smallest year in the right subtree of the node being removed. The successor's value is copied into the node being deleted, and the successor itself is unlinked.

Three cases are handled separately: the node is the only node in the tree; the node has no right child; and the general case where a successor exists.

Where a node has no right child but does have a left child, the implementation copies the left child's value up and drops the link. That looks lossy, and would be in a plain binary search tree, but it is safe here: in a valid AVL tree a node with no right child has a left subtree of height at most 1, so the left child is necessarily a leaf.

---

## Question 3 — clinic priority queue

**The problem.** A clinic wants patients with more urgent conditions to be seen first. Patients are entered into a queue with a triage priority from 0 (most urgent) to 4 (least urgent), and the program announces each patient as they are called.

**What is implemented.** A menu-driven queue built on .NET's `PriorityQueue<string, int>`, with validation on both the patient name and the priority value. The brief asked for the priority queue class to be invoked rather than written from scratch, so this question is about correct use and input handling rather than about implementing a heap.

| Operation | Complexity |
| --- | --- |
| Enqueue | O(log n) |
| Dequeue | O(log n) |
| Peek | O(1) |

---

## Question 4 — QuickSort over hotel listings

**The problem.** Read a JSON file of hotel listings and sort it by one of four metrics — name, nightly rate, star rating, or distance from the airport — chosen by changing a single variable at the top of the program. QuickSort had to be implemented from first principles; using a library sort was not allowed.

**What is implemented.** The JSON is deserialised with `System.Text.Json` into a list of `Hotel` objects. Sorting uses a **Lomuto partition** with the last element as the pivot, recursing on the two halves. The sort metric is read once and used to select which field `CompareTo` is called on, so one implementation serves all four metrics.

| Case | Complexity |
| --- | --- |
| Average | O(n log n) |
| Worst | O(n²) |

The worst case is reachable here: because the pivot is always the last element, data that is already sorted on the chosen metric degrades to quadratic. A random or median-of-three pivot would avoid it. The supplied data set was small enough that it did not matter in practice, but it is a property of this implementation rather than of QuickSort in general.

---

## Known limitations

These are things I know are incomplete, rather than things I have not looked at.

- **Rebalancing after deletion stops too early.** After an insertion, a single rotation always restores the AVL property, so stopping at the first rotation is correct. After a deletion it is not — rebalancing can be required all the way up to the root. The implementation stops at the first rotation in both cases, so some deletions can leave an imbalance higher in the tree.

- **Heights are recalculated across the whole tree after every rotation.** `q1_RecalculateNodeHeights` walks every node, which makes an insertion O(n) in practice rather than the O(log n) the structure is capable of. Only the nodes on the path from the rotation to the root actually need updating.

- **No automated tests.** The tree was verified by hand using the debug printout, which is why the rotation methods still write to the console.

---

## A note on the `q1_` and `q2_` prefixes

The assignment required every class belonging to a question to be prefixed with that question's number — `q1_Book`, `q1_BookTree`, `q2_InOrderTraversal` and so on. They are a marking requirement, not a naming convention I would choose. Outside this constraint the classes would be `Book`, `AvlNode` and `BookTree`, and the question-two methods would simply live on the tree.

## Licence

Published for reference. Please do not submit it as your own work.
