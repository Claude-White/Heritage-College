package com.example.android_hangman;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.constraintlayout.widget.ConstraintLayout;

import android.annotation.SuppressLint;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;

import java.io.File;
import java.io.IOException;
import java.io.Serializable;
import java.util.ArrayList;

import hangman.Player;
import hangman.Scoreboard;
import linked_data_structures.*;

public class LoginActivity extends AppCompatActivity implements Serializable {

  private DoublyLinkedList<Player> players;


  @Override
  protected void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);
    setContentView(R.layout.activity_login);
    Button btnStart = findViewById(R.id.btnStart);
    ConstraintLayout loginLayout = findViewById(R.id.LoginLayout);
    ScrollView playersScroll = findViewById(R.id.playersScroll);
    LinearLayout playersGroup = findViewById(R.id.playersGroup);
    EditText fldNewPlayer = findViewById(R.id.fldNewPlayer);
    addSavedPlayers(playersGroup);
    findViewById(R.id.btnNewPlayer).setOnClickListener(v -> {
      fldNewPlayer.setVisibility(EditText.VISIBLE);
      btnStart.setVisibility(Button.VISIBLE);
      playersGroup.setVisibility(LinearLayout.GONE);
      playersScroll.setVisibility(ScrollView.GONE);
    });
    findViewById(R.id.btnSavedPlayers).setOnClickListener(v -> {
      fldNewPlayer.setVisibility(EditText.GONE);
      btnStart.setVisibility(Button.GONE);
      playersGroup.setVisibility(LinearLayout.VISIBLE);
      playersScroll.setVisibility(ScrollView.VISIBLE);
    });
    btnStart.setOnClickListener(v -> {
      String username = fldNewPlayer.getText().toString().trim();
      Scoreboard scoreboard = new Scoreboard();
      try {
        if (scoreboard.addPlayer(username, this)) {
          Intent gameIntent = new Intent(this, HangmanGameActivity.class);
          gameIntent.putExtra("indexOfPlayer", scoreboard.getPostion());
          gameIntent.putExtra("samePlayer", false);
          startActivity(gameIntent);
        }
        else {
          Toast.makeText(this, "Please select a player", Toast.LENGTH_SHORT).show();
        }
      }
      catch (IOException e) {
        e.printStackTrace();
      }
    });
    findViewById(R.id.btnRules).setOnClickListener(v -> {
      Intent rulesIntent = new Intent(this, RulesActivity.class);
      startActivity(rulesIntent);
    });
    findViewById(R.id.btnScoreboard).setOnClickListener(v -> {
      Intent scoreboardIntent = new Intent(this, ScoreboardActivity.class);
      startActivity(scoreboardIntent);
    });
  } // onCreate()

  @SuppressLint("SetTextI18n")
  private void addSavedPlayers(LinearLayout playersGroup) {
    File file = new File(getFilesDir(), "./save_game.ser");
    if (!file.exists() || file.length() == 0) {
      TextView noPlayers = new TextView(this);
      noPlayers.setText("No saved players");
      noPlayers.setGravity(Gravity.CENTER);
      playersGroup.addView(noPlayers);
      return;
    }
    else {
      Scoreboard sc = new Scoreboard();
      try {
        players = sc.loadGames(this);
        ArrayList<View> savedPlayers = new ArrayList<>();
        TextView playersList = new TextView(this);
        playersList.setText("Saved Players");
        playersList.setGravity(Gravity.CENTER);
        savedPlayers.add(playersList);
        for (int i = 0; i < players.getLength(); i++) {
          Button btnPlayer = new Button(this);
          btnPlayer.setText(players.getElementAt(i).getUsername());
          savedPlayers.add(btnPlayer);
        }
        for (int i = 0; i < savedPlayers.size(); i++) {
          if (savedPlayers.get(i) instanceof Button) {
            Button btnTemp = (Button) savedPlayers.get(i);
            btnTemp.setId(i);
            btnTemp.setOnClickListener(v -> {
              Intent gameIntent = new Intent(this, HangmanGameActivity.class);
              gameIntent.putExtra("indexOfPlayer", savedPlayers.indexOf(btnTemp) - 1);
              gameIntent.putExtra("samePlayer", false);
              startActivity(gameIntent);
            });
            playersGroup.addView(btnTemp);
          }
          else {
            TextView txtTemp = (TextView) savedPlayers.get(i);
            playersGroup.addView(txtTemp);
          }

        }

      }
      catch (IOException | ClassNotFoundException e) {
        e.printStackTrace();
      }
    }
  } // addSavedPlayers()

}