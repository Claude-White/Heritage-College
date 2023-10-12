package hangman;

import java.io.Serializable;
import linked_data_structures.*;

public class Scoreboard implements Serializable {
  private static DoublyLinkedList<Player> players;
  private static int numPlayers;

  public Scoreboard() {
    players = new DoublyLinkedList<>();
    numPlayers = 0;
  } // Scoreboard()

  public void addPlayer(String username) {
    players.add(new Player(username));
    numPlayers++;
  } // addPlayer(Player)

  public void gamePlayed(String username, boolean winOrLose) {

  } // gamePlayed(String, boolean)

  public Player getNextPlayer() {
    return players.getElementAt(0);
  } // getNextPlayer()

  public Player getPlayer(String username) { // or at index?
    return players.getElementAt(0);
  } // getPlayer(String)

  public void sortScoreboard() {

  } // sortScoreboard()

}
