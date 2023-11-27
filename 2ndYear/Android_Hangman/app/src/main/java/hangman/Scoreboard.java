package hangman;

import android.content.Context;
import android.util.Log;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;

import linked_data_structures.DoublyLinkedList;

public class Scoreboard implements Serializable {
	private static DoublyLinkedList<Player> players = new DoublyLinkedList<>();
	private String filename = "save_game.ser";
	private int postion = 0;
	
	public int getNumPlayers() {
		return players.getLength();
	} // numPlayers()

	public boolean addPlayer(String username, Context context) throws IOException {
		if (username.isEmpty()) {
			return false;
		}
		else {
			if (players.getLength() == 0) {
				players.add(new Player(username, context));
				return true;
			}

			int tempPos = 0;
			while (tempPos < players.getLength()) {
				if (players.getElementAt(tempPos).getUsername().compareTo(username) > 0) {
					break;
				}
				tempPos++;
			}
			players.add(new Player(username, context), tempPos);
			postion = tempPos;
		}
		return true;
	} // addPlayer(Player)

	public int getPostion() {
		return postion;
	} // getPostion()

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
		return null; // players.getElementAt(0)
	} // getPlayer(String)

	public Player getPlayer(int index) {
		return players.getElementAt(index);
	} // getPlayer(int)

	public void saveGame(Context context) throws IOException {
		File output = new File(context.getFilesDir(), filename);
		if (!output.exists()) {
			output.createNewFile();
		}
		FileOutputStream fileOut = new FileOutputStream(output);
		ObjectOutputStream out = new ObjectOutputStream(fileOut);
		out.writeObject(players);
		out.close();
		fileOut.close();
		Log.i("Saved", "Game saved successfully.");
	} // saveGame()

	public DoublyLinkedList<Player> loadGames(Context context) throws IOException, ClassNotFoundException {
		File input = new File(context.getFilesDir(), filename);
		if (!input.exists()) {
			input.createNewFile();
		}
		FileInputStream fileIn = new FileInputStream(input);
		ObjectInputStream in = new ObjectInputStream(fileIn);
		players = (DoublyLinkedList<Player>) in.readObject();
		in.close();
		fileIn.close();
		Log.i("Loaded", "Game loaded successfully.");
		return players;
	} // loadGames()
	
	public void resetPlayerList() {
		players = new DoublyLinkedList<>();
	} // resetPlayerList();
} // Scoreboard class
