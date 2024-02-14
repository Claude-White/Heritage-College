package hangman;

import static org.junit.jupiter.api.Assertions.*;

import org.junit.jupiter.api.Test;

class HangmanGameTest {

	@Test
	void testConstructorPlayer() {
		HangmanGame game = new HangmanGame(new Player("Bob"));
		assertTrue(game.getDictionary() != null);
		assertEquals(0, game.getNumErrors());
		assertEquals("Bob", game.getPlayer().getUsername());
		assertTrue(game.getNumberOfLetters() > 0);
	}
	
	@Test
	void testConstructorPlayerDict() {
		Dictionary dict = new Dictionary();
		HangmanGame game = new HangmanGame(new Player("Rob"), dict);
		assertTrue(game.getDictionary() == dict);
		assertEquals(0, game.getNumErrors());
		assertEquals("Rob", game.getPlayer().getUsername());
		assertTrue(game.getNumberOfLetters() > 0);
	}
	
	@Test
	void testHint() {
		boolean goodHint = false;
		HangmanGame game = new HangmanGame(new Player("Bob"));
		Character hint = game.getHint();
		for(int i = 0; i < game.getWordToGuessList().getLength(); i++) {
			if (game.getWordToGuessList().getElementAt(i) == hint) {
				goodHint = true;
			}
		}
		assertTrue(goodHint);
	}
	
	@Test
	void testNumOfLetters() {
		HangmanGame game = new HangmanGame(new Player("Bob"));
		int length = game.getWordToGuess().length();
		assertEquals(length, game.getNumberOfLetters());
	}
	
	@Test
	void testCheckInput() {
		boolean isBad = false;
		HangmanGame game = new HangmanGame(new Player("Bob"));
		char[] tempArr = game.getWordToGuess().toCharArray();
		for(int i = 0; i < tempArr.length; i++) {
			if (!game.checkInput(tempArr[i])) {
				isBad = true;
			}
		}
		assertFalse(isBad);
	}
	
	@Test
	void testHasLost() {
		HangmanGame game = new HangmanGame(new Player("Bob"));
		game.setNumErrors(6);
		assertTrue(game.hasLost());
	}
	
	@Test
	void testHasWon() {
		HangmanGame game = new HangmanGame(new Player("Bob"));
		int length = game.getWordToGuess().length();
		game.setNumErrors(3);
		for(int i = 0; i < length; i++) {
			game.checkInput(game.getWordToGuess().charAt(0));
		}
		assertTrue(game.hasWon());
	}

}
