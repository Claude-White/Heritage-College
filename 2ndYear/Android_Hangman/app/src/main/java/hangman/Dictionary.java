package hangman;

import android.content.Context;
import android.util.Log;

import java.io.IOException;
import java.io.InputStream;
import java.io.Serializable;
import java.util.Scanner;

import linked_data_structures.SinglyLinkedList;

public class Dictionary implements Serializable {
  private Context context;

  private static final int MIN_WORD_LENGTH = 4;
  private static final int MAX_WORD_LENGTH = 26;
  private static final int MIN_GUESSABLE_CHARS = 3;
  private SinglyLinkedList<String> words;
  private int numWords;
  private int totalWords;

  public Dictionary(Context inputContext) throws IOException {
    context = inputContext;
    words = new SinglyLinkedList<>();
    numWords = 0;
    readFile();
  } // Dictionary()

  public String getRandomWord() {
    int randomWordNum = (int) Math.floor(Math.random() * numWords); // may need changing
    String randomWord = words.getElementAt(randomWordNum);
    words.remove(randomWordNum);
    numWords--;
    return randomWord;
  } // getWord()
  
  public SinglyLinkedList<String> getWords() {
  	return words;
  }

  private void readFile() throws IOException {
    InputStream wordFile = context.getAssets().open("word_db.txt");
    Scanner wordScanner = new Scanner(wordFile);
    while (wordScanner.hasNextLine()) {
      String word = wordScanner.nextLine();
       processWord(word);
    }
    totalWords = numWords;
  } // readFile()

  private void processWord(String word) {
    if (word.length() >= MIN_WORD_LENGTH && word.length() <= MAX_WORD_LENGTH) {
      int numUniqueChars = 0;
      for (int i = 0; i < word.length(); i++) {
        if (word.indexOf(word.charAt(i)) == word.lastIndexOf(word.charAt(i))) {
          numUniqueChars++;
        }
      }
      if (numUniqueChars >= MIN_GUESSABLE_CHARS) {
        words.add(word.trim());
        numWords++;
      }
    }
  } // processWord()

  public int getNumWords() {
    return numWords;
  } // getNumWords()

  public int getTotalWords() {
    return totalWords;
  } // getTotalWords()

}
