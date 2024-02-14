package com.example.android_hangman;

import androidx.annotation.NonNull;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.core.content.ContextCompat;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;

import android.annotation.SuppressLint;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.BitmapFactory;
import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import com.google.android.material.navigation.NavigationView;

import java.io.IOException;
import java.io.InputStream;
import java.io.Serializable;
import java.util.regex.Pattern;

import hangman.*;
import linked_data_structures.SinglyLinkedList;

public class HangmanGameActivity extends AppCompatActivity implements View.OnClickListener, Serializable {

  private Button btnHint;
  private TextView lblWordToGuess;
  private TextView lblWrongGuessesList;
  private TextView lblUsername;
  private TextView lblLives;
  private ImageView hangmanImg;
  private String tempWord;
  private Button[] btnLetters = new Button[26];
  private DrawerLayout drawerLayout;
  private NavigationView navigationView;
  private Handler handler = new Handler(Looper.getMainLooper());

  private Player player;
  private Scoreboard scoreboard;
  private int indexOfPlayer;

  @SuppressLint("SetTextI18n")
  @Override
  protected void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);
    setContentView(R.layout.activity_main);
    int textColor = ContextCompat.getColor(this, R.color.textColor);
    Toolbar toolbar = findViewById(R.id.toolbar);
    toolbar.setTitle("Hangman");
    toolbar.setTitleTextColor(textColor);
    setSupportActionBar(toolbar);
    Scoreboard sc = new Scoreboard();
    indexOfPlayer = getIntent().getIntExtra("indexOfPlayer", 0);
    player = sc.getPlayer(indexOfPlayer);
    boolean samePlayer = getIntent().getBooleanExtra("samePlayer", false);

    lblWordToGuess = findViewById(R.id.lblWordToGuess);
    lblWrongGuessesList = findViewById(R.id.lblWrongGuessesList);
    lblUsername = findViewById(R.id.lblUsername);
    lblLives = findViewById(R.id.lblLives);
    hangmanImg = findViewById(R.id.hangmanImg);

    navigationView = findViewById(R.id.navView);
    Menu menu = navigationView.getMenu();
    getMenuInflater().inflate(R.menu.main_menu, menu);

    navigationView.setNavigationItemSelectedListener(new NavigationView.OnNavigationItemSelectedListener() {
      @Override
      public boolean onNavigationItemSelected(@NonNull MenuItem item) {
        int id = item.getItemId();
        if (id == R.id.newGameNavMenu) {
          setupNewGame(player, true);
        } else if (id == R.id.rulesNavMenu) {
          Intent rulesIntent = new Intent(HangmanGameActivity.this, RulesActivity.class);
          startActivity(rulesIntent);
        } else if (id == R.id.scoreboardNavMenu) {
          Intent scoreboardIntent = new Intent(HangmanGameActivity.this, ScoreboardActivity.class);
          startActivity(scoreboardIntent);
        } else if (id == R.id.quitNavMenu) {
          finishAffinity();
          System.exit(0);
        }
        drawerLayout.closeDrawer(GravityCompat.START);
        return true;
      }
    });

    drawerLayout = findViewById(R.id.drawerLayout);
    ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar,
        R.string.open_navigation_drawer, R.string.close_navigation_drawer);
    drawerLayout.addDrawerListener(toggle);
    toggle.syncState();

    LinearLayout btnGroup = findViewById(R.id.btnGroup);
    for (int i = 0; i < 26; i++) {
      btnLetters[i] = new Button(this);
      btnLetters[i].setText((char) (i + 'A') + "");
      btnLetters[i].setOnClickListener(this);
      btnGroup.addView(btnLetters[i]);
    }

    Button btnBack = findViewById(R.id.btnBack);
    btnBack.setOnClickListener(v -> {
      Intent loginIntent = new Intent(this, LoginActivity.class);
      startActivity(loginIntent);
    });

    btnHint = findViewById(R.id.btnHint);
    btnHint.setOnClickListener(v -> {
      char hint = player.getGame().getHint();
      for (Button btnLetter : btnLetters) {
        if (Character.toUpperCase(btnLetter.getText().charAt(0)) == Character.toUpperCase(hint)) {
          updateHangman();
          btnLetter.performClick();
          break;
        }
      }
      if (!player.getGame().canGetHint()) {
        btnHint.setEnabled(false);
      }
    });
    setupNewGame(player, samePlayer);
  }

  private void updateHangman() {
    try {
      InputStream originalImage = getAssets().open((6 - player.getGame().getNumErrors()) + "Lives.png");
      hangmanImg.setImageBitmap(BitmapFactory.decodeStream(originalImage));
    }
    catch (IOException e1) {
      AlertDialog.Builder alertLoss = new AlertDialog.Builder(this);
      alertLoss.setTitle("Image not found")
          .setMessage("Hangman image could not be found.")
          .setPositiveButton("OK", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int which) {
              // Handle positive button click
            }
          })
          .show();
    }
  }

  @SuppressLint("SetTextI18n")
  private void setupNewGame(Player player, boolean samePlayer) {
    clearAll();
    scoreboard = new Scoreboard();
    if (scoreboard.getPlayer(indexOfPlayer).getGame().getNumWords() == 0) {
      AlertDialog.Builder alertNoMoreWords = new AlertDialog.Builder(this);
      alertNoMoreWords.setTitle("No more words")
          .setMessage("No more words for this player.")
          .show();
      handler.postDelayed(new Runnable() {
        @Override
        public void run() {
          finish();
        }
      }, 2000);
    }
    else {
    if (samePlayer) {
      player.setGame(new HangmanGame(player, player.getGame().getDictionary()));
      this.setTitle("Hangman - Game " + (player.getGame().getTotalWords() - player.getGame().getNumWords()) + "/" + player.getGame().getTotalWords());
      updateHangman();
      lblUsername.setText("Username: " + player.getUsername());
      lblLives.setText("Lives: " + (6 - player.getGame().getNumErrors()));
      tempWord = player.getGame().getWordToGuess();
      for (int i = 0; i < tempWord.length(); i++) {
        if (Pattern.matches("[a-zA-Z]", tempWord.charAt(i) + "")) {
          lblWordToGuess.append("_");
        } else {
          lblWordToGuess.append(tempWord.charAt(i) + "");
        }
        lblWordToGuess.append(" "); // made changes might be wrong
      }
    } else {
      if (!player.getGame().canGetHint()) {
        btnHint.setEnabled(false);
      }
      // call incorrectGuess() for each incorrect guess, maybe .getNumErrors()
      for (int i = 0; i < player.getGame().getWronglyGuessedLettersList().getLength(); i++) {
        incorrectGuess(btnLetters[player.getGame().getWronglyGuessedLettersList().getElementAt(i) - 'A']);
      }
      setTitle("Hangman - Game " + (player.getGame().getTotalWords() - player.getGame().getNumWords()) + "/" + player.getGame().getTotalWords());
      updateHangman();
      lblUsername.setText(player.getUsername());
      lblLives.setText("Lives: " + (6 - player.getGame().getNumErrors()));
      tempWord = player.getGame().getWordToGuess();

      for (int i = 0; i < tempWord.length(); i++) {
        if (Pattern.matches("[a-zA-Z]", tempWord.charAt(i) + "")) {
          lblWordToGuess.append("_");
        } else {
          lblWordToGuess.append(tempWord.charAt(i) + "");
        }
        lblWordToGuess.append(" "); // made changes might be wrong
      }
      SinglyLinkedList<Character> tempLetters = player.getGame().getCorrectlyGuessedLettersList();
      player.getGame().resetCorrectlyGuessedLetters();
      for (Button btnLetter : btnLetters) {
        for (int i = 0; i < tempLetters.getLength(); i++) {
          if (Character.toUpperCase(btnLetter.getText().charAt(0)) == Character.toUpperCase(tempLetters.getElementAt(i))) {
            btnLetter.performClick();
          }
        }
      }
    }
    try {
      scoreboard.saveGame(this);
    } catch (IOException e) {
      AlertDialog.Builder alertGameNotSaved = new AlertDialog.Builder(this);
      alertGameNotSaved.setTitle("Fail to Save")
          .setMessage("Failed to save game.")
          .show();
      handler.postDelayed(new Runnable() {
        @Override
        public void run() {
          finish();
        }
      }, 3000);
      e.printStackTrace();
    }
  }
  }

  private void clearAll() {
    int defaultButtonColor = ContextCompat.getColor(this, R.color.defaultButtonColor);
    lblWordToGuess.setText("");
    lblWrongGuessesList.setText("");
    for (Button btnLetter : btnLetters) {
      btnLetter.setBackgroundColor(defaultButtonColor);
      btnLetter.setEnabled(true);
      btnHint.setEnabled(true);
    }
  } // clearAll()

  private void correctGuess(Button btnLetter) {
    int correctGuessColor = ContextCompat.getColor(this, R.color.correctButtonColor);
    btnLetter.setBackgroundColor(correctGuessColor);
    btnLetter.setEnabled(false);
    StringBuilder tempLblWordToGuess;
    for (int i = 0; i < tempWord.length(); i++) {
      tempLblWordToGuess = new StringBuilder(lblWordToGuess.getText());
      if (Character.toUpperCase(tempWord.charAt(i)) == Character.toUpperCase(btnLetter.getText().charAt(0))) {
        tempLblWordToGuess.setCharAt(i * 2, player.getGame().getLetterAt(i));
        lblWordToGuess.setText(tempLblWordToGuess.toString());
      }
    }
  } // correctGuess()

  private void incorrectGuess(Button btnLetter) {
    int incorrectGuessColor = ContextCompat.getColor(this, R.color.incorrectGuessColor);
    updateHangman();
    lblWrongGuessesList.append(btnLetter.getText().charAt(0) + " "); // made changes might be wrong
    btnLetter.setBackgroundColor(incorrectGuessColor);
    btnLetter.setEnabled(false);
  } // incorrectGuess()

  @SuppressLint("SetTextI18n")
  @Override
  public void onClick(View view) {
    Button btn = (Button) view;
    for (Button btnLetter : btnLetters) {
      if (btn == btnLetter) {
        char letter = btnLetter.getText().charAt(0);
        if (player.getGame().checkInput(letter)) {
          correctGuess(btnLetter);
        }
        else {
          incorrectGuess(btnLetter);
        }


        try {
          scoreboard.saveGame(this);
        } catch (IOException e) {
          AlertDialog.Builder alertGameNotSaved = new AlertDialog.Builder(this);
          alertGameNotSaved.setTitle("Fail to Save")
              .setMessage("Failed to save game.")
              .show();
          handler.postDelayed(new Runnable() {
            @Override
            public void run() {
              finish();
            }
          }, 3000);
          e.printStackTrace();
        }

        lblLives.setText("Lives: " + (6 - player.getGame().getNumErrors()));
        if (player.getGame().hasLost()) {
          AlertDialog.Builder alertLoss = new AlertDialog.Builder(this);
          alertLoss.setTitle("Loss")
              .setMessage("You lose. Word was: " + tempWord)
              .setPositiveButton("OK", new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int which) {
                  // Handle positive button click
                }
              })
              .show();
          if (player.getGame().noMoreWords()) {
            AlertDialog.Builder alertOutOfWords = new AlertDialog.Builder(this);
            alertOutOfWords.setTitle("Out of Words")
                .setMessage("There are no words left.")
                .show();
            handler.postDelayed(new Runnable() {
              @Override
              public void run() {
                finish();
              }
            }, 3000);
          }
          scoreboard.gamePlayed(player.getGame().getPlayer().getUsername(), false);
          setupNewGame(player.getGame().getPlayer(), true);
        }
        if (player.getGame().hasWon()) {
          AlertDialog.Builder alertWin = new AlertDialog.Builder(this);
          alertWin.setTitle("Win")
              .setMessage("You win")
              .setPositiveButton("OK", new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int which) {
                  // Handle positive button click
                }
              })
              .show();
          //JOptionPane.showMessageDialog(new JPanel(), "You win");
          if (player.getGame().noMoreWords()) {
            AlertDialog.Builder alertOutOfWords = new AlertDialog.Builder(this);
            alertOutOfWords.setTitle("Out of Words")
                .setMessage("There are no words left.")
                .show();
            handler.postDelayed(new Runnable() {
              @Override
              public void run() {
                finish();
              }
            }, 3000);
          }
          scoreboard.gamePlayed(player.getGame().getPlayer().getUsername(), true);
          setupNewGame(player.getGame().getPlayer(), true);
        }
        if (!player.getGame().canGetHint() || player.getGame().getNumberOfLetters() - 1 == player.getGame().getNumberOfGuessedLetters()) {
          btnHint.setEnabled(false);
        }
      } // if (btn == btnLetter)
    }
  }

  @Override
  public boolean onOptionsItemSelected(MenuItem item) {
    int id = item.getItemId();
    if (id == android.R.id.home) {
      if (drawerLayout.isDrawerOpen(GravityCompat.START)) {
        drawerLayout.closeDrawer(GravityCompat.START);
      } else {
        drawerLayout.openDrawer(GravityCompat.START);
      }
      return true;
    }
    return super.onOptionsItemSelected(item);
  }

}