package hangman;

import static org.junit.jupiter.api.Assertions.*;

import org.junit.jupiter.api.Test;

class PlayerTest {

	@Test
	void testEmptyContructor() {
		Player player1 = new Player();
		assertTrue(player1.getUsername().equals("Unknown"));
		assertTrue(player1.getNumGamesPlayed() == 0);
		assertTrue(player1.getNumGamesWon() == 0);
		assertTrue(player1.getGame() != null);
	}
	
	@Test
	void testContructor() {
		Player player1 = new Player("Bob");
		assertTrue(player1.getUsername().equals("Bob"));
		assertTrue(player1.getNumGamesPlayed() == 0);
		assertTrue(player1.getNumGamesWon() == 0);
		assertTrue(player1.getGame() != null);
	}

	@Test
	void testIncrements() {
		Player player1 = new Player("Bob");
		player1.incrementGamesPlayed();
		assertTrue(player1.getNumGamesPlayed() == 1);
		player1.incrementGamesWon();
		assertTrue(player1.getNumGamesWon() == 1);
	}
}
