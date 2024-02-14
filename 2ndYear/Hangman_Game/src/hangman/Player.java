package hangman;

import java.io.Serializable;

public class Player implements Serializable {

	private String username;
	private int numGamesPlayed;
	private int numGamesWon;
	HangmanGame playerGame;

	public Player() {
		username = "Unknown";
		numGamesPlayed = 0;
		numGamesWon = 0;
		playerGame = new HangmanGame(this);
	}

	public Player(String name) {
		username = name;
		numGamesPlayed = 0;
		numGamesWon = 0;
		playerGame = new HangmanGame(this);
	}

	public String getUsername() {
		return username;
	} // getUsername()

	public void setUsername(String name) {
		username = name;
	} // setUsername(String)

	public int getNumGamesPlayed() {
		return numGamesPlayed;
	} // getNumGamesPlayed()

	public void setNumGamesPlayed(int num) {
		numGamesPlayed = num;
	} // setNumGamesPlayed(int)

	public int getNumGamesWon() {
		return numGamesWon;
	} // getNumGamesWon()

	public void setNumGamesWon(int num) {
		numGamesWon = num;
	} // setNumGamesWon(int)

	public void incrementGamesPlayed() {
		numGamesPlayed++;
	} // incrementGamesPlayed()

	public void incrementGamesWon() {
		numGamesWon++;
	} // incrementGamesWon()

	public HangmanGame setGame(HangmanGame game) {
		playerGame = game;
		return playerGame;
	} // setGame(HangmanGame)

	public HangmanGame getGame() {
		return playerGame;
	} // getGame()
}
