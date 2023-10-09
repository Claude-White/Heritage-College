package linked_data_structures;
/**
 *   The structure of a node in the simple doubly linked
 *   list introduced in the chapter on Fundamental Data Structures.
 */
public class DLNode<E> extends SLNode<E> {
  private DLNode<E> predecessor; // link to the predecessor

  /**
   * Constructor. Create an empty <code>DLNode</code> object.
   */
  public DLNode() {
    super();
    this.predecessor = null;
  }

  /**
   * Constructor. Create a <code>DLNode</code> object that stores
   * <code>theElement</code>, whose predecessor is
   * <code>predecessor</code> and whose successor is
   * <code>successor</code>.
   * @param theElement the element to be stored in this node
   * @param thePredecessor  this node's predecessor
   * @param theSuccessor  this node's successor
   */
  public DLNode( E theElement, DLNode<E> thePredecessor,
                 DLNode<E> theSuccessor ) {
    super( theElement, theSuccessor );
    this.predecessor = thePredecessor;
  }

  /**
   * Return the predecessor to this node.
   * @return this node's predecessor
   */
  public DLNode<E> getPredecessor() {
    return this.predecessor;
  }

  /**
   * Set the predecessor to this node in the list.
   * @param thePredecessor  this node's new predecessor
   */
  public void setPredecessor( DLNode<E> thePredecessor ) {
    this.predecessor = thePredecessor;
  }
}
