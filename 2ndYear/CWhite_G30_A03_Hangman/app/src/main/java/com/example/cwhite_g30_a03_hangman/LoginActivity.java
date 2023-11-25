package com.example.cwhite_g30_a03_hangman;

import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;
import androidx.constraintlayout.widget.ConstraintLayout;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Parcelable;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.Toolbar;

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
        findViewById(R.id.btnNewPlayer).setOnClickListener(v -> {
            fldNewPlayer.setVisibility(EditText.VISIBLE);
            btnStart.setVisibility(Button.VISIBLE);
            playersGroup.setVisibility(LinearLayout.GONE);
            playersScroll.setVisibility(ScrollView.GONE);
        });
        findViewById(R.id.btnSavedPlayers).setOnClickListener(v -> {
            addSavedPlayers(playersGroup);
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
                    gameIntent.putExtra("player", scoreboard.getPlayer(username));
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
            playersGroup.addView(noPlayers);
            return;
        }
        else {
            Scoreboard sc = new Scoreboard();
            try {
                players = sc.loadGames(this);
                Log.i("loadedPlayers", players.toString());
                ArrayList<TextView> savedPlayers = new ArrayList<>();
                TextView playersList = new TextView(this);
                playersList.setText("Saved Players");
                savedPlayers.add(playersList);
                for (int i = 1; i < players.getLength() + 1; i++) {
                    TextView btnPlayer = new TextView(this);
                    btnPlayer.setText(players.getElementAt(i).getUsername());
                    savedPlayers.add(btnPlayer);
                }
                for (TextView btnPlayer : savedPlayers) {
                    playersGroup.addView(btnPlayer);
                    btnPlayer.setOnClickListener(v -> {
                        Intent gameIntent = new Intent(this, HangmanGameActivity.class);
                        gameIntent.putExtra("player", players.getElementAt(savedPlayers.indexOf(btnPlayer) + 1));
                        gameIntent.putExtra("samePlayer", true);
                        startActivity(gameIntent);
                    });
                }
            }
            catch (IOException | ClassNotFoundException e) {
                e.printStackTrace();
            }
        }
    } // addSavedPlayers()

}