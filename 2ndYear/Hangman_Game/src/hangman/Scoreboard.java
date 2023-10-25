package hangman;

import java.io.Serializable;
import linked_data_structures.*;

public class Scoreboard implements Serializable {
  private static DoublyLinkedList<Player> players = new DoublyLinkedList<>();
  private static int numPlayers;

  public Scoreboard() {
    numPlayers = players.getLength();
  } // Scoreboard()

  public boolean addPlayer(String username) {
  	if (username.isEmpty()) {
  		return false;
  	}
    players.add(new Player(username));
    return true;
  } // addPlayer(Player)

  public void gamePlayed(String username, boolean hasWon) {
    if (hasWon) {
      getPlayer(username).incrementGamesWon();
    }
    getPlayer(username).incrementGamesPlayed();
  } // gamePlayed(String, boolean)

  public DoublyLinkedList<Player> getPlayers() {
    return players;
  } // getNextPlayer()

  public Player getPlayer(String username) {
    for (int i = 0; i < players.getLength(); i++) {
      if(players.getElementAt(i).getUsername().equals(username)) {
        return players.getElementAt(i);
      }
    }
    return players.getElementAt(0);
  } // getPlayer(String)

//  public void sortScoreboard() {
//    mergeSort(players);
//  } // sortScoreboard()

//  private void mergeSort(DoublyLinkedList<Player> playerList) {
//    int length = playerList.getLength();
//    if (length < 2) {
//      return;
//    }
//    int mid = length / 2;
//    DoublyLinkedList<Player> left = new DoublyLinkedList<>();
//    DoublyLinkedList<Player> right = new DoublyLinkedList<>();
//    for (int i = 0; i < mid; i++) {
//      left.add(playerList.getElementAt(i));
//    }
//    for (int i = mid; i < length; i++) {
//      right.add(playerList.getElementAt(i));
//    }
//    mergeSort(left);
//    mergeSort(right);
//    merge(playerList, left, right);
//  } // mergeSort(DoublyLinkedList)

//  private void merge (DoublyLinkedList<Player> playerList, DoublyLinkedList<Player> left, DoublyLinkedList<Player> right) {
//    int leftLength = left.getLength();
//    int rightLength = right.getLength();
//    int i = 0, j = 0, k = 0;
//
//    while (i < leftLength && j < rightLength) {
//      if (left.getElementAt(i).getUsername().compareToIgnoreCase(right.getElementAt(j).getUsername()) <= 0) {
//        playerList.replace(k, left.getElementAt(i));
//        i++;
//      }
//      else {
//        playerList.replace(k, right.getElementAt(j));
//        j++;
//      }
//      k++;
//    }
//    while (i < leftLength) {
//      playerList.replace(k, left.getElementAt(i));
//      i++;
//      k++;
//    }
//    while (j < rightLength) {
//      playerList.replace(k, right.getElementAt(j));
//      j++;
//      k++;
//    }
//  } // merge(DoublyLinkedList, DoublyLinkedList, DoublyLinkedList)

} // Scoreboard class
