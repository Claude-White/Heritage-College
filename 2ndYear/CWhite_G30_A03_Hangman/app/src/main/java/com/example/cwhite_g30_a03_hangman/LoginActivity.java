package com.example.cwhite_g30_a03_hangman;

import androidx.appcompat.app.ActionBar;
import androidx.appcompat.app.AppCompatActivity;
import androidx.constraintlayout.widget.ConstraintLayout;

import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;
import android.widget.Toolbar;
public class LoginActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        ConstraintLayout loginLayout = findViewById(R.id.LoginLayout);
        Spinner savedPlayers = findViewById(R.id.savedPlayers);
        EditText fldNewPlayer = findViewById(R.id.fldNewPlayer);
        findViewById(R.id.btnNewPlayer).setOnClickListener(v -> {
            savedPlayers.setVisibility(Spinner.GONE);
            fldNewPlayer.setVisibility(EditText.VISIBLE);
        });
        findViewById(R.id.btnSavedPlayers).setOnClickListener(v -> {
            fldNewPlayer.setVisibility(EditText.GONE);
            savedPlayers.setVisibility(Spinner.VISIBLE);
        });
        findViewById(R.id.btnStart).setOnClickListener(v -> {
            if (savedPlayers.getSelectedItem() == null) {
                Intent gameIntent = new Intent(this, HangmanGameActivity.class);
                startActivity(gameIntent);
            } else if (fldNewPlayer.getText().toString().length() > 0) {
                Intent gameIntent = new Intent(this, HangmanGameActivity.class);
                startActivity(gameIntent);
            } else {
                Toast.makeText(this, "Please select a player", Toast.LENGTH_SHORT).show();
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

        String[] options = {"Players"};
        ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_item, options);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        savedPlayers.setAdapter(adapter);
    }

}