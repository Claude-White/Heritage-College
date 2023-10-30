package hangman;

import static org.junit.jupiter.api.Assertions.*;

import java.io.File;

import org.junit.jupiter.api.Test;

class DictionaryTest {

	@Test
	void testConstructor() {
		Dictionary dict = new Dictionary();
		assertEquals(35, dict.getNumWords());
		assertEquals(35, dict.getTotalWords());
	}
	
	@Test
	void testGetRandomWord() {
		Dictionary dict = new Dictionary();
		assertEquals(35, dict.getNumWords());
		boolean isGone = true;
		String tempString = dict.getRandomWord();
		for(int i = 0; i < dict.getWords().getLength(); i++) {
			if (dict.getWords().getElementAt(i).equals(tempString)) {
				isGone = false;
			}
		}
		assertTrue(isGone);
		assertEquals(34, dict.getNumWords());
	}
	
	@Test
	void testFileExists() {
		Dictionary dict = new Dictionary();
		assertTrue(new File("word_db.txt").exists());
	}
}
