package hangman;

import static org.junit.jupiter.api.Assertions.*;

import java.io.File;
import java.io.IOException;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

class ScoreboardTest {
	
	Scoreboard sc;
	
	@BeforeEach
	public void setupTest() {
		sc = new Scoreboard();
	}
	
	@Test
	void testAddPlayer() {
		sc.resetPlayerList();
		sc.addPlayer("Bob");
		assertEquals("Bob", sc.getPlayer("Bob").getUsername());
	}
	
	@Test
	void testAddMultiplePlayer() {
		sc.resetPlayerList();
		sc.addPlayer("Bob");
		sc.addPlayer("George");
		assertEquals(2, sc.getNumPlayers());
	}
	
	@Test
	void testLoss() {
		sc.resetPlayerList();
		sc.addPlayer("Bobby");
		sc.gamePlayed("Bobby", false);
		assertEquals(0, sc.getPlayer("Bobby").getNumGamesWon());
		assertEquals(1, sc.getPlayer("Bobby").getNumGamesPlayed());
	}
	
	@Test
	void testWin() {
		sc.resetPlayerList();
		sc.addPlayer("Bob");
		sc.gamePlayed("Bob", true);
		assertEquals(1, sc.getPlayer("Bob").getNumGamesWon());
		assertEquals(1, sc.getPlayer("Bob").getNumGamesPlayed());
	}
	
	@Test
	void testGamePlayedPlayerNotFound() {
		sc.resetPlayerList();
		sc.addPlayer("Bob");
		sc.gamePlayed("Rob", true);
		assertEquals(1, sc.getPlayer("Rob").getNumGamesWon());
		assertEquals(1, sc.getPlayer("Rob").getNumGamesPlayed());
	}
	
	@Test
	void saveGame() {
		sc.resetPlayerList();
		try {
			sc.saveGame();
			assertTrue(new File("save_game.ser").exists());
		}
		catch (IOException e) {
			e.printStackTrace();
		}
	}
}
