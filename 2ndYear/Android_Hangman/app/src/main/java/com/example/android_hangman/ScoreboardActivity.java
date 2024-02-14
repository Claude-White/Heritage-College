package com.example.android_hangman;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.widget.TextView;

import hangman.Player;
import hangman.Scoreboard;
import linked_data_structures.DoublyLinkedList;

public class ScoreboardActivity extends AppCompatActivity {

  @Override
  protected void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);
    setContentView(R.layout.activity_scoreboard);
    TextView lblScoreboard = findViewById(R.id.lblScoreboard);
    Scoreboard scoreboard = new Scoreboard();
    DoublyLinkedList<Player> players = scoreboard.getPlayers();
    lblScoreboard.setText("Username        Games Won      Games Played\n");
    lblScoreboard.append ("-------------   ------------   ------------\n");
    for (int i = 0; i < players.getLength(); i++) {
      lblScoreboard.append(String.format("%-16s%-15s%-12s\n", players.getElementAt(i).getUsername(), players.getElementAt(i).getNumGamesWon(), players.getElementAt(i).getNumGamesPlayed()));
    }
  }
}