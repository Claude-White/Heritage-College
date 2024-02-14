package hangman;

import java.io.*;

import linked_data_structures.*;

public class Scoreboard implements Serializable {
	private static DoublyLinkedList<Player> players = new DoublyLinkedList<>();
	private String filename = "save_game.ser";
	
	public int getNumPlayers() {
		return players.getLength();
	} // numPlayers()

	public boolean addPlayer(String username) {
		if (username.isEmpty()) {
			return false;
		}
		else {
			if (players.getLength() == 0) {
				players.add(new Player(username));
				return true;
			}

			int tempPos = 0;
			while (tempPos < players.getLength()) {
				if (players.getElementAt(tempPos).getUsername().compareTo(username) > 0) {
					break;
				}
				tempPos++;
			}
			players.add(new Player(username), tempPos);
		}
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
			if (players.getElementAt(i).getUsername().equalsIgnoreCase(username)) {
				return players.getElementAt(i);
			}
		}
		return players.getElementAt(0);
	} // getPlayer(String)

	public void saveGame() throws IOException {
		FileOutputStream file = new FileOutputStream(filename);
		ObjectOutputStream out = new ObjectOutputStream(file);
		out.writeObject(players);
		out.close();
		file.close();
		System.out.println("Game saved successfully.");
	} // saveGame()

	public DoublyLinkedList<Player> loadGames() throws IOException, ClassNotFoundException {
		FileInputStream file = new FileInputStream(filename);
		ObjectInputStream in = new ObjectInputStream(file);
		players = (DoublyLinkedList<Player>) in.readObject();
		in.close();
		file.close();
		System.out.println("Game loaded successfully.");
		return players;
	} // loadGames()
	
	public void resetPlayerList() {
		players = new DoublyLinkedList<>();
	} // resetPlayerList();
} // Scoreboard class
