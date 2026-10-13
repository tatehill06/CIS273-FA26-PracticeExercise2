using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace PracticeExercise2;

public class LinkedListNode<T>
{
   public T Data {get; set;}

   public LinkedListNode<T> Next {get; set;}

   public LinkedListNode(T data = default, LinkedListNode<T> next = null)
    {
        Data = data;
        Next = next;
    }

    public override string ToString()
    {
        return Data is null? "null": Data.ToString();
    }
}

public class LinkedList<T> : IList<T>
{
    private LinkedListNode<T>? Head {get; set;}

    private LinkedListNode<T>? Tail {get; set;}

    public LinkedList()
    {
        Head = null;
        Tail = null;
    }
   
    // TODO
    /// <summary>
    /// Returns first element in list, null if empty.
    /// </summary>
    public T? First => IsEmpty? default : Head.Data;

    // TODO
    /// <summary>
    /// Returns last element in list, null if empty.
    /// </summary>
    public T? Last => IsEmpty? default : Tail.Data;

    // TODO
    /// <summary>
    /// Returns true if list is has no elements; false otherwise.
    /// </summary>
    public bool IsEmpty => Head is null;

    /// <summary>
    /// Number of elements in list.
    /// </summary>
    public int Length
    {
        get;
        private set;
    }

    // TODO 
    /// <summary>
    /// Adds given item to end of list.
    /// </summary>
    /// <param name="item">item to add to the list</param>
    public void Append(T item)
    {
        var newNode = new LinkedListNode<T>(item);

        //if empty
        if (IsEmpty)
        {
            Head = newNode;
            Tail = newNode;
        }

        else
        {
            //1+ nodes in list
            Tail.Next = newNode;
            Tail = newNode;
        }
        Length++;
    }

    /// <summary>
    /// Remove all elements from list
    /// </summary>
    public void Clear()
    {
        Head = null;
        Tail = null;
        Length = 0;
    }

    /// <summary>
    /// Checks if the list contains the given value.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>true if item is in list; false otherwise</returns>
    public bool Contains(T item)
    {
        for(var currentNode=Head; currentNode!=null; currentNode = currentNode.Next)
        {
            if(currentNode.Data.Equals(item))
            {
                return true;
            }
        }
        return false;
    }

    // TODO
    /// <summary>
    /// Find index of first element with matching value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Index of first element with value; -1 if element is not found</returns>
    public int FirstIndexOf(T item)
    {
        int index = 0;
        for(var currentNode=Head; currentNode!=null; currentNode = currentNode.Next)
        {
            if(currentNode.Data.Equals(item))
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    /// <summary>
    /// Return the element at the given index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>The element at the given index.</returns>
    public T Get(int index)
    {
        if (index < 0 || index >= Length)
        {
            throw new IndexOutOfRangeException();
        }

        int i = 0;
        for(var currentNode=Head; currentNode!=null; currentNode = currentNode.Next)
        {
            if(i == index)
            {
                return currentNode.Data;
            }
            i++;
        }
        throw new Exception();
    }

    // TODO
    /// <summary>
    /// Insert new value after first instance of existing value.
    /// If existingValue is not in list, then add new value to end of list.
    /// </summary>
    /// <param name="newValue"></param>
    /// <param name="existingValue"></param>
    public void InsertAfter(T newItem, T existingItem)
    {
        var newNode = new LinkedListNode<T>(newItem);
        for(var currentNode=Head; currentNode!=null; currentNode = currentNode.Next)
        {  
            if(currentNode.Data.Equals(existingItem))
            {
                newNode.Next = currentNode.Next;
                currentNode.Next = newNode;

                if (newNode.Next == null)
                {
                    Tail = newNode;
                }

                Length++;
                return;
            }
        }
        Append(newItem);
    }

    // TODO
    /// <summary>
    /// Insert value at given index 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    public void InsertAt(T item, int index)
    {
        var newNode = new LinkedListNode<T>(item);
        if (index < 0 || index > Length)
        {
            throw new IndexOutOfRangeException();
        }

        if (index == 0)
        {
            newNode.Next = Head;
            Head = newNode;

            if(Tail == null)
            {
                Tail = newNode;
            }
            Length++;
            return;
        }
        int i = 0;
        for(var currentNode=Head; currentNode!=null; currentNode = currentNode.Next)
        {
            if(i == index - 1)
            {
                newNode.Next = currentNode.Next;
                currentNode.Next = newNode;
                if (newNode.Next == null)
                {
                    Tail = newNode;
                }
                
                Length++;
                return;
            }
            i++;
        }
    }
        
    // TODO
    /// <summary>
    /// Add value to beginning of list
    /// </summary>
    /// <param name="value"></param>
    public void Prepend(T item)
    {
        var newNode = new LinkedListNode<T>(item);

        if (IsEmpty)
        {
            Head = newNode;
            Tail = newNode;
        }
        else
        {
            newNode.Next = Head;
            Head = newNode;
        }
        Length++;
    }

    // TODO
    /// <summary>
    /// Remove first item with given value
    /// </summary>
    /// <param name="value">value of item to be removed</param>
    public void Remove(T item)
    {
        //empty list
        if (IsEmpty)
            return;

        //item is head
        if (Head.Data.Equals(item))
        {
            //one node list
            if(Head == Tail)
            {
                Tail = null;
            }
            Head = Head.Next;
            Length--;
            return;
        }

        //item is not in head
        var currentNode = Head;
        while(currentNode != null)
        {
            var successorNode = currentNode.Next;

            if(successorNode == null)
            {
                return;
            }

            if (successorNode.Data.Equals(item))
            {
                if(successorNode == Tail)
                {
                    Tail = currentNode;
                }
                currentNode.Next = successorNode.Next;
                Length--;
                return;
            }
            currentNode = currentNode.Next;
        }
    }

    // TODO
    /// <summary>
    /// Removed item at specififed index.
    /// </summary>
    /// <param name="index"></param>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Length)
        {
            throw new IndexOutOfRangeException();
        }
        if (index == 0)
        {
            if(Head == Tail)
            {
                Tail = null;
            }
            Head = Head.Next;
            Length--;
            return;
        }
        
        int i = 0;
        var currentNode = Head;
        
        while(currentNode != null)
        {
            var successorNode = currentNode.Next;
            if(successorNode == null)
            {
                return;
            }

            if (i == index - 1)
            {
                if(successorNode == Tail)
                {
                    Tail = currentNode;
                }
                currentNode.Next = successorNode.Next;
                Length--;
                return;
            }
            currentNode = currentNode.Next;
            i++;
        }
    }

    // TODO
    /// <summary>
    /// Return a new copy of list in reverse order
    /// </summary>
    /// <returns></returns>
    public IList<T> Reverse()
    {
        var newLinkedList = new LinkedList<T>();
    
        for(var currentNode=Head; currentNode!=null; currentNode = currentNode.Next)
        {
            newLinkedList.Prepend(currentNode.Data);
        }
        return newLinkedList;
    }

    private LinkedListNode<T> GetNode(T item)
    {
        return null;
    }
    
    public override string ToString()
    {
        string str = "[ ";
        var currentNode = Head;
        while (currentNode != null)
        {
            str += currentNode.Data.ToString();

            if (currentNode != Tail)
            {
                str += ", ";
            }
            currentNode = currentNode.Next;
        }
        str += "]";
        return str;
    }
}
