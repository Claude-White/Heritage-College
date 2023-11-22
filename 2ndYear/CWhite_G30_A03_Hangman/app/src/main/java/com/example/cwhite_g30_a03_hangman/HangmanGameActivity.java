package com.example.cwhite_g30_a03_hangman;

import androidx.annotation.NonNull;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;

import android.annotation.SuppressLint;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;

import com.google.android.material.navigation.NavigationView;

public class HangmanGameActivity extends AppCompatActivity implements View.OnClickListener {

    DrawerLayout drawerLayout;
    NavigationView navigationView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_hangman_game);

        Toolbar toolbar = findViewById(R.id.toolbar);
        toolbar.setTitle("Hangman");
        setSupportActionBar(toolbar);

        navigationView = findViewById(R.id.navView);
        Menu menu = navigationView.getMenu();
        getMenuInflater().inflate(R.menu.main_menu, menu);

        navigationView.setNavigationItemSelectedListener(new NavigationView.OnNavigationItemSelectedListener() {
            @Override
            public boolean onNavigationItemSelected(@NonNull MenuItem item) {
                int id = item.getItemId();
                if (id == R.id.newGameNavMenu) {
                    System.out.println("New Game");
                } else if (id == R.id.rulesNavMenu) {
                    Intent rulesIntent = new Intent(HangmanGameActivity.this, RulesActivity.class);
                    startActivity(rulesIntent);
                } else if (id == R.id.scoreboardNavMenu) {
                    Intent scoreboardIntent = new Intent(HangmanGameActivity.this, ScoreboardActivity.class);
                    startActivity(scoreboardIntent);
                } else if (id == R.id.quitNavMenu) {
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
            Button btn = new Button(this);
            btn.setText((char) (i + 'A') + "");
            btn.setOnClickListener(this);
            btnGroup.addView(btn);
        }

        Button btnBack = findViewById(R.id.btnBack);
        btnBack.setOnClickListener(v -> {
            Intent loginIntent = new Intent(this, LoginActivity.class);
            startActivity(loginIntent);
        });
    }

    @Override
    public void onClick(View view) {

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