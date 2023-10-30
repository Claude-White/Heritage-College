package hangman;

import linked_data_structures.*;

import java.io.FileNotFoundException;
import java.io.Serializable;
import java.util.Scanner;
import java.io.File;

public class Dictionary implements Serializable {

  private static final int MIN_WORD_LENGTH = 5;
  private static final int MAX_WORD_LENGTH = 16;
  private static final int MIN_GUESSABLE_CHARS = 4;
  private SinglyLinkedList<String> words;
  private int numWords;
  private int totalWords;

  public Dictionary() {
    words = new SinglyLinkedList<>();
    numWords = 0;
    readFile();
  } // Dictionary()

  public String getRandomWord() {
    int randomWordNum = (int) (Math.random() * numWords);
    String randomWord = words.getElementAt(randomWordNum);
    words.remove(randomWordNum);
    numWords--;
    return randomWord;
  } // getWord()
  
  public SinglyLinkedList<String> getWords() {
  	return words;
  }

  private void readFile() {
    boolean flag = true;
    File wordFile = new File("word_db.txt");
    try {
      Scanner wordScanner = new Scanner(wordFile);
      while (wordScanner.hasNextLine()) {
        String word = wordScanner.nextLine();
         if (word.length() >= MIN_WORD_LENGTH && word.length() <= MAX_WORD_LENGTH) {
           int numUniqueChars = 0;
           for (int i = 0; i < word.length(); i++) {
             if (word.indexOf(word.charAt(i)) == word.lastIndexOf(word.charAt(i))) {
               numUniqueChars++;
             }
           }
           if (numUniqueChars >= MIN_GUESSABLE_CHARS) {
             char[] tempWord = word.toUpperCase().trim().toCharArray();
             for (char c: tempWord) {
                if (Character.isDigit(c)) {
                  flag = false;
                }
             }
             if (flag) {
                words.add(word.toUpperCase().trim());
                numWords++;
             }
           }
         }
      }
      totalWords = numWords;
    } catch (FileNotFoundException e) {
      e.printStackTrace();
    }
  } // readFile()

  public int getNumWords() {
    return numWords;
  } // getNumWords()

  public int getTotalWords() {
    return totalWords;
  } // getTotalWords()

}
