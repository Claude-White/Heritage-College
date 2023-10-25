package hangman;

import linked_data_structures.*;

import java.io.FileNotFoundException;
import java.util.Scanner;
import java.io.File;

public class Dictionary {

  private static final int MIN_WORD_LENGTH = 5;
  private static final int MAX_WORD_LENGTH = 16;
  private static final int MIN_GUESSABLE_CHARS = 4;
  private SinglyLinkedList<String> words;
  private int numWords;

  public Dictionary() {
    words = new SinglyLinkedList<>();
    numWords = 0;
    readFile();
  } // Dictionary()

  public String getRandomWord() {
    int randomWordNum = (int) (Math.random() * numWords);
    String randomWord = words.getElementAt(randomWordNum);
    words.remove(randomWordNum);
    return randomWord;
  } // getWord()

  public String getAllWords() {
    StringBuilder allWords = new StringBuilder();
    for (int i = 0; i < numWords; i++) {
      allWords.append(words.getElementAt(i)).append("\n");
    }
    return allWords.toString();
  } // getAllWords()

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
    } catch (FileNotFoundException e) {
      e.printStackTrace();
    }
  } // readFile()

  public int getNumWords() {
    return numWords;
  } // getNumWords()

}
