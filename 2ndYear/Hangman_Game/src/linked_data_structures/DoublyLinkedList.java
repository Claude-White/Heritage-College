package linked_data_structures;

/**
 * Description: A simple singly-linked list introduced in the chapter on
 * Fundamental Data Structures.
 */
public class DoublyLinkedList<E>
{
	private int length; // the number of elements in the linked list
	protected DLNode<E> head; // access point to the linked list
	protected DLNode<E> tail;

	/**
	 * Create an empty <code>SinglyLinkedList</code>.
	 */
	public DoublyLinkedList()
	{
		this.length = 0;
		this.tail = new DLNode<E>(); // tail dummy node
		this.head = new DLNode<E>(null, null, this.tail); // head dummy node
		this.tail.setPredecessor(head);
	}

	/**
	 * Add a new element at the beginning of the linked list.
	 * 
	 * @param e
	 *          the element to add
	 */
	public void add(E e)
	{
		DLNode<E> newnode = new DLNode<E>(e, null, null);
		addAfter(this.head, newnode);
		this.length++;
	}

	/**
	 * Add element <code>e</code> at position <code>p</code> in this singly linked
	 * list
	 * 
	 * @param e
	 *          the element to add
	 * @param p
	 *          position to insert <code>e</code>; must be in the range 0 to
	 *          <code>this.size()</code>.
	 * @throws IndexOutOfBoundsException
	 *           if <code>p</code> is outside the range 0 to
	 *           <code>this.length()</code>.
	 */
	public void add(E e, int p)
	{
		// verify that index p is legal
		if ((p < 0) || (p > this.length))
		{
			throw new IndexOutOfBoundsException("index " + p
					+ " is out of range: 0 to " + this.length);
		}
		DLNode<E> newnode = new DLNode<E>(e, null, null);
		DLNode<E> cursor = this.head;
		for (int i = 0; i < p; i++)
		{
			cursor = (DLNode<E>) cursor.getSuccessor();
		}
		addAfter(cursor, newnode);
		this.length++;
	}

	/**
	 * Return the element stored in the node at position <code>p</code>.
	 * 
	 * @param p
	 *          the position whose element we want
	 * @return the element from position <code>p</code> of this linked list
	 * @throws IndexOutOfBoundsException
	 *           if the index p is outside the range 0 to <code>length - 1</code>.
	 */
	public E getElementAt(int p)
	{
		DLNode<E> node = this.find(p);
		return node.getElement();
	}

	// UTILITY METHODS

	/*
	 * addAfter - add newnode after node p PRECONDITIONS NOT CHECKED! pre: p and
	 * newnode are not null post: newnode is inserted after p
	 */

	/**
	 * Return the length of this singly linked list.
	 * 
	 * @return the number of elements in this singly linked list
	 */
	public int getLength()
	{
		return this.length;
	}

	/*
	 * find - find the first node containing target, return null if target is not
	 * found
	 */

	/**
	 * Remove the node at position <code>p</code> and return its element field.
	 * 
	 * @param p
	 *          the position whose element we are to return
	 * @return the element in position <code>p</code>
	 * @throws IndexOutOfBoundsException
	 *           if <code>p</code> is outside the range 0 to length()</code>
	 */
	public E remove(int p)
	{
		if ((p < 0) || (p >= this.length))
		{
			throw new IndexOutOfBoundsException("index " + p
					+ " is out of range: 0 to " + (this.length - 1));
		}
		DLNode<E> target = find(p);
		E theElement = target.getElement();
		DLNode<E> pred = target.getPredecessor();

		// link target to cursor’s successor
		pred.setSuccessor(target.getSuccessor());
		((DLNode<E>) target.getSuccessor()).setPredecessor(pred);

		// now cleanup after ourselves
		target.setSuccessor(null);
		target.setPredecessor(null);
		target.setElement(null);

		// update the list state
		this.length--;

		return theElement;
	}

	/*
	 * find - find the node at position p throw an exception if the index p is
	 * outside the range 0 to this.length - 1
	 */

	private void addAfter(DLNode<E> p, DLNode<E> newnode)
	{
		newnode.setPredecessor(p);
		newnode.setSuccessor(p.getSuccessor());
		((DLNode<E>) p.getSuccessor()).setPredecessor(newnode);
		p.setSuccessor(newnode);
	}

	private DLNode<E> find(E target)
	{
		DLNode<E> cursor = (DLNode<E>) head.getSuccessor();

		while (cursor != tail)
		{
			if (cursor.getElement().equals(target))
			{
				return cursor; // success
			}
			else
			{
				cursor = (DLNode<E>) cursor.getSuccessor();
			}
		}
		return null; // failure
	}

	private DLNode<E> find(int p)
	{
		if ((p < 0) || (p >= this.length))
		{
			throw new IndexOutOfBoundsException();
		}

		DLNode<E> cursor = null;
		if (p < (length / 2))
		{ // p is in first half
			cursor = (DLNode<E>) head.getSuccessor();
			for (int i = 0; i != p; i++)
			{
				cursor = (DLNode<E>) cursor.getSuccessor();
			}
		}
		else
		{ // p is in second half
			cursor = tail.getPredecessor();
			for (int i = length - 1; i != p; i--)
			{
				cursor = cursor.getPredecessor();
			}
		}
		return cursor;
	} // find()

} // DoublyLinkedList class
