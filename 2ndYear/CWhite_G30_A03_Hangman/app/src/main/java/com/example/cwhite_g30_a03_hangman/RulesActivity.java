package com.example.cwhite_g30_a03_hangman;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.widget.TextView;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.ObjectInputStream;
import java.util.Scanner;

import linked_data_structures.*;

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