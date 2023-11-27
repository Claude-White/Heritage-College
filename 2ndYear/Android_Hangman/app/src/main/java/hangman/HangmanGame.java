package hangman;

import android.content.Context;
import android.util.Log;

import java.io.IOException;
import java.io.Serializable;
import java.util.regex.Pattern;

import linked_data_structures.SinglyLinkedList;

public class HangmanGame implements Serializable {

	private SinglyLinkedList<Character> wordToGuess;
	private SinglyLinkedList<Character> correctlyGuessedLetters;
	private SinglyLinkedList<Character> wronglyGuessedLetters;
	private int wordLength;
	private int numErrors;
	private Dictionary dictionary;
	private Player player;

	public HangmanGame(Player user, Context context) throws IOException {
		dictionary = new Dictionary(context);
		numErrors = 0;
		player = user;
		wordToGuess = new SinglyLinkedList<>();
		correctlyGuessedLetters = new SinglyLinkedList<>();
		wronglyGuessedLetters = new SinglyLinkedList<>();
		String tempWord = dictionary.getRandomWord();
		for (int i = tempWord.length() - 1; i >= 0; i--) {
			wordToGuess.add(tempWord.charAt(i));
		}
		wordLength = wordToGuess.getLength();
		player.setGame(this);
	} // HangmanGame(Player)

	public HangmanGame(Player gamePlayer, Dictionary dict) {
		dictionary = dict;
		numErrors = 0;
		player = gamePlayer;
		wordToGuess = new SinglyLinkedList<>();
		correctlyGuessedLetters = new SinglyLinkedList<>();
		wronglyGuessedLetters = new SinglyLinkedList<>();
		String tempWord = dictionary.getRandomWord();
		for (int i = tempWord.length() - 1; i >= 0; i--) {
			wordToGuess.add(tempWord.charAt(i));
		}
		wordLength = wordToGuess.getLength();
		player.setGame(this);
	} // HangmanGame(Player)

	public Dictionary getDictionary() {
		return dictionary;
	} // getDictionary()

	public Player getPlayer() {
		return player;
	} // getPlayer()

	public boolean noMoreWords() {
		return dictionary.getNumWords() < 1;
	} // hasMoreWords()

	public int getNumWords() {
		return dictionary.getNumWords();
	} // hasMoreWords()

	public int getTotalWords() {
		return dictionary.getTotalWords();
	} // hasMoreWords()

	public int getNumErrors() {
		return numErrors;
	} // getNumErrors()
	
	public void setNumErrors(int num) {
		numErrors = num;
	}

	public boolean canGetHint() {
		return numErrors < 5;
	} // canGetHint()

	public char getHint() { // may needs adjustment
		int rand = (int) (Math.random() * wordToGuess.getLength());
		if (Pattern.matches("[a-zA-Z]", wordToGuess.getElementAt(rand) + "")) {
			for (int i = 0; i < correctlyGuessedLetters.getLength(); i++) {
				if (Character.toUpperCase(correctlyGuessedLetters.getElementAt(i)) == Character.toUpperCase(wordToGuess.getElementAt(rand))) {
					return getHint();
				}
			}
			numErrors++;
			return wordToGuess.getElementAt(rand);
		}
		else {
			return getHint();
		}
	} // getHint()

	public int getNumberOfLetters() {
		int numLetters = wordLength;
		for (int i = 0; i < wordToGuess.getLength(); i++) {
			if (!Pattern.matches("[a-zA-Z]", wordToGuess.getElementAt(i) + "")) {
				numLetters--;
			}
		}
		return numLetters;
	} // getNumberOfLetters()

	public SinglyLinkedList<Character> getCorrectlyGuessedLettersList() {
		return correctlyGuessedLetters;
	} // getGuessedLetters()

	public void resetCorrectlyGuessedLetters() {
		correctlyGuessedLetters = new SinglyLinkedList<>();
	} // setCorrectlyGuessedLetters()

	public SinglyLinkedList<Character> getWronglyGuessedLettersList() {
		return wronglyGuessedLetters;
	} // getGuessedLetters()

	public int getNumberOfGuessedLetters() {
		return correctlyGuessedLetters.getLength();
	} // getNumberOfGuessedLetters()

	public String getWordToGuess() {
		StringBuilder tempWord = new StringBuilder();
		for (int i = 0; i < wordLength; i++) {
			tempWord.append(wordToGuess.getElementAt(i));
		}
		return tempWord.toString();
	} // getWordToGuess()

	public SinglyLinkedList<Character> getWordToGuessList() {
		return wordToGuess;
	} // getWordToGuessList()

	public boolean checkInput(char input) {
		boolean flag = false;
		for (int i = 0; i < wordToGuess.getLength(); i++) {
			if (Character.toUpperCase(wordToGuess.getElementAt(i)) == Character.toUpperCase(input)) {
				correctlyGuessedLetters.add(input);
				flag = true;
			}
		}
		if (!flag) {
			wronglyGuessedLetters.add(input);
			numErrors++;
			return false;
		}
		return true;
	} // checkInput(char)

	public boolean hasLost() {
		return numErrors >= 6;
	} // checkLoss()

	public boolean hasWon() {
		String tempWord = getWordToGuess();
		int tempLength = 0;
		for (int i = 0; i < wordLength; i++) {
			if (Pattern.matches("[a-zA-Z]", tempWord.charAt(i) + "")) {
				tempLength++;
			}
		}
		return correctlyGuessedLetters.getLength() == tempLength;
	} // checkWin()

	public char getLetterAt(int index) {
		return wordToGuess.getElementAt(index);
	} // getLetterAt(int)

} // class HangmanGame
