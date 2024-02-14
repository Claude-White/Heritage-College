package com.example.android_hangman;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.widget.TextView;

import java.io.IOException;
import java.io.InputStream;
import java.util.Scanner;

public class RulesActivity extends AppCompatActivity {

  @Override
  protected void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);
    setContentView(R.layout.activity_rules);
    TextView txtRules = findViewById(R.id.areaDisplayRules);
    String rules;
    try {
      InputStream rulesFile = getAssets().open("rules.txt");
      Scanner readRules = new Scanner(rulesFile);
      while (readRules.hasNextLine()) {
        rules = readRules.nextLine();
        txtRules.append(rules + "\n");
      }
    }
    catch (IOException e) {
      e.printStackTrace();
    }
  }
}