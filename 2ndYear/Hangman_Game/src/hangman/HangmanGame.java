package hangman;

import linked_data_structures.*;

public class HangmanGame {

  private static SinglyLinkedList<Character> remainingLetters;
  private SinglyLinkedList<Character> guessedLetters;
  private String wordToGuess;
  private int wordLength;
  private int numErrors;
  private Dictionary dictionary;
  private Player player;

  public HangmanGame(Player user) {
    dictionary = new Dictionary();
    wordToGuess = dictionary.getRandomWord();
    wordLength = wordToGuess.length();
    numErrors = 0;
    player = user;
    remainingLetters = new SinglyLinkedList<>();
    guessedLetters = new SinglyLinkedList<>();
    for (int i = wordLength - 1; i >= 0; i--) {
      remainingLetters.add(wordToGuess.charAt(i));
    }
  } // HangmanGame(Player)

  public void saveGame() {
    // save game to file by serializing this object with the player object
  } // saveGame()

  public boolean checkInput(char input) {
    if (remainingLetters.find(input) != null) {
      guessedLetters.add(input);
      remainingLetters.remove(input);
      return true;
    }
    else {
      numErrors++;
      return false;
    }
  } // checkInput(char)

} // class HangmanGame
