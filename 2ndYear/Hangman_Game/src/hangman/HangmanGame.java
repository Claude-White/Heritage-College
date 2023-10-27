package hangman;

import linked_data_structures.*;

public class HangmanGame {

  private static SinglyLinkedList<Character> wordToGuess;
  private SinglyLinkedList<Character> guessedLetters;
  private int wordLength;
  private int numErrors;
  private static Dictionary dictionary;
  private static Player player;

  public HangmanGame(Player user) {
    dictionary = new Dictionary();
    numErrors = 0;
    player = user;
    wordToGuess = new SinglyLinkedList<>();
    guessedLetters = new SinglyLinkedList<>();
    String tempWord = dictionary.getRandomWord();
    for (int i = tempWord.length() - 1; i >= 0; i--) {
      wordToGuess.add(tempWord.charAt(i));
    }
    wordLength = wordToGuess.getLength();
    for (int i = 0; i < wordLength; i++) {
      System.out.print(wordToGuess.getElementAt(i));
    }
    System.out.println();
  } // HangmanGame(Player)

  public HangmanGame() {
    numErrors = 0;
    wordToGuess = new SinglyLinkedList<>();
    guessedLetters = new SinglyLinkedList<>();
    String tempWord = dictionary.getRandomWord();
    for (int i = tempWord.length() - 1; i >= 0; i--) {
      wordToGuess.add(tempWord.charAt(i));
    }
    wordLength = wordToGuess.getLength();
    for (int i = 0; i < wordLength; i++) {
      System.out.print(wordToGuess.getElementAt(i));
    }
    System.out.println();
  } // HangmanGame(Player)

  public Player getPlayer() {
    return player;
  } // getPlayer()

  public boolean hasMoreWords() {
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

  public boolean canGetHint() {
    return numErrors < 5;
  } // canGetHint()

  public char getHint() {
    int rand = (int) (Math.random() * wordToGuess.getLength());
    if(Character.isLetter(wordToGuess.getElementAt(rand))) {
      for (int i = 0; i < guessedLetters.getLength(); i++) {
        if (guessedLetters.getElementAt(i) == wordToGuess.getElementAt(rand)) {
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
      if (!Character.isLetter(wordToGuess.getElementAt(i))) {
        numLetters--;
      }
    }
    return numLetters;
  } // getWordToGuess()

  public int getNumberOfGuessedLetters() {
    return guessedLetters.getLength();
  } // getNumberOfGuessedLetters()

  public String getWordToGuess() {
    StringBuilder tempWord = new StringBuilder();
    for (int i = 0; i < wordLength; i++) {
      tempWord.append(wordToGuess.getElementAt(i));
    }
    return tempWord.toString();
  } // getWordToGuess()

  public void saveGame() {
    // save game to file by serializing this object with the player object
  } // saveGame()

  public boolean checkInput(char input) {
    boolean flag = false;
    for (int i = 0; i < wordToGuess.getLength(); i++) {
      if(wordToGuess.getElementAt(i) == input) {
        guessedLetters.add(input);
        flag = true;
      }
    }
    if (!flag) {
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
      if(Character.isLetter(tempWord.charAt(i))) {
        tempLength++;
      }
    }
    return guessedLetters.getLength() == tempLength;
  } // checkWin()

} // class HangmanGame
