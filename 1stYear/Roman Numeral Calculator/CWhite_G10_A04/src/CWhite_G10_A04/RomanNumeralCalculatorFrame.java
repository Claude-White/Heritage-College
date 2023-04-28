package CWhite_G10_A04;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

public class RomanNumeralCalculatorFrame extends JFrame implements ActionListener {

	private JLabel lblPrompt;
	private JButton btnCalc;
	private JTextField fldNum;
	private JTextArea areaDisplay;
	private JPanel inputPanel;

	File romanFile;
	Scanner input = null;

	RomanNumeral roNum = new RomanNumeral();

	public RomanNumeralCalculatorFrame() {
		setTitle("Roman Numerals");

		inputPanel = new JPanel();

		lblPrompt = new JLabel("Enter filename (if not using romanNumeral.txt):");
		inputPanel.add(lblPrompt);

		fldNum = new JTextField(15);
		inputPanel.add(fldNum);

		btnCalc = new JButton("Calculate Now!");
		btnCalc.addActionListener(this);
		inputPanel.add(btnCalc);

		areaDisplay = new JTextArea(19, 10);
		areaDisplay.setMargin(new Insets(10, 10, 10, 10));
		areaDisplay.setFont(new Font("Monospaced", Font.PLAIN, 12));
		areaDisplay.setEditable(false);

		getContentPane().add(areaDisplay, BorderLayout.SOUTH);
		getContentPane().add(inputPanel, BorderLayout.CENTER);
	} // RomanNumeralCalculatorFrame()

	public void actionPerformed(ActionEvent e) {
		if (e.getSource() == btnCalc) {
			if (fldNum.getText().isEmpty()) {
				romanFile = new File("romanNumeral.txt");
			} // if (fldNum.getText().isEmpty())
			else {
				romanFile = new File(fldNum.getText());
			} // else

			try {
				input = new Scanner(romanFile);
			} // try
			catch (FileNotFoundException f) {
				JOptionPane.showMessageDialog(this, "You must enter a valid path.",
						"Invalid Path", JOptionPane.ERROR_MESSAGE);
			} // catch (FileNotFoundException f)

			areaDisplay.setText("");

			while (input.hasNextLine()) {
				String RomDigit = input.nextLine();
				RomDigit = RomDigit.toUpperCase();
				RomDigit = RomDigit.replaceAll(" ", "");

				if (RomDigit.contains("Q") || RomDigit.contains("W")
						|| RomDigit.contains("E") || RomDigit.contains("R")
						|| RomDigit.contains("T") || RomDigit.contains("Y")
						|| RomDigit.contains("U") || RomDigit.contains("O")
						|| RomDigit.contains("P") || RomDigit.contains("A")
						|| RomDigit.contains("S") || RomDigit.contains("F")
						|| RomDigit.contains("G") || RomDigit.contains("H")
						|| RomDigit.contains("J") || RomDigit.contains("K")
						|| RomDigit.contains("Z") || RomDigit.contains("B")
						|| RomDigit.contains("N")) {
					JOptionPane.showMessageDialog(this, "Invalid input in file",
							"Invalid File", JOptionPane.ERROR_MESSAGE);
					System.exit(-1);
				} // if()
				else {

					String operator = "";
					if (RomDigit.contains("+")) {
						operator = "+";
					} // if()
					else
						if (RomDigit.contains("-")) {
							operator = "-";
						} // else if()
						else
							if (RomDigit.contains("*")) {
								operator = "*";
							} // else if()
							else
								if (RomDigit.contains("/")) {
									operator = "/";
								} // else if()
								else
									if (RomDigit.contains("%")) {
										operator = "%";
									} // else if()
									else {
										areaDisplay.append("Error");
									} // else

					String num1 = (RomDigit.substring(0, RomDigit.indexOf(operator)))
							.trim();
					String num2 = (RomDigit.substring(RomDigit.indexOf(operator) + 1))
							.trim();

					RomanNumeral roNum1 = new RomanNumeral(num1);
					RomanNumeral intNum1 = new RomanNumeral(roNum1.getIntRomNum());

					RomanNumeral roNum2 = new RomanNumeral(num2);
					RomanNumeral intNum2 = new RomanNumeral(roNum2.getIntRomNum());

					if (roNum1.getIntRomNum() < 0 && roNum2.getIntRomNum() < 0) {
						JOptionPane.showMessageDialog(this, "Input is smaller than 0",
								"Invalid Input", JOptionPane.ERROR_MESSAGE);
						System.exit(-1);
					}
					else {

						int intRoAnswer = 0;
						switch (operator) {
						case "+" -> intRoAnswer = roNum1.getIntRomNum()
								+ roNum2.getIntRomNum();
						case "-" -> intRoAnswer = roNum1.getIntRomNum()
								- roNum2.getIntRomNum();
						case "*" -> intRoAnswer = roNum1.getIntRomNum()
								* roNum2.getIntRomNum();
						case "/" -> intRoAnswer = roNum1.getIntRomNum()
								/ roNum2.getIntRomNum();
						case "%" -> intRoAnswer = roNum1.getIntRomNum()
								% roNum2.getIntRomNum();
						} // switch(operator)

						RomanNumeral roNumAnswer = new RomanNumeral(intRoAnswer);

						areaDisplay.append(intNum1.getRomanNumeral() + " ");
						areaDisplay.append("(" + roNum1.getIntRomNum() + ") ");

						areaDisplay.append(operator + " ");

						areaDisplay.append(intNum2.getRomanNumeral() + " ");
						areaDisplay.append("(" + roNum2.getIntRomNum() + ") ");

						areaDisplay.append("= " + roNumAnswer.getRomanNumeral() + " ");
						areaDisplay.append("(" + intRoAnswer + ")\n");
					} // else
				} // else
			} // while(input.hasNextLine())
		} // if (e.getSource() == btnCalc)
	} // actionPerformed(ActionEvent e)

	public static void main(String[] args) {
		RomanNumeralCalculatorFrame frame = new RomanNumeralCalculatorFrame();
		frame.setSize(600, 400);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.setLocationRelativeTo(null);
		frame.setVisible(true);
	} // main(String[])
} // RomanNumeralCalculatorFrame class
