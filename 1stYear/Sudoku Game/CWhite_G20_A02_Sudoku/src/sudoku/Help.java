package sudoku;

import javax.swing.JPanel;

import javax.swing.JTextPane;
import javax.swing.text.SimpleAttributeSet;
import javax.swing.text.StyleConstants;
import javax.swing.text.StyledDocument;
import java.awt.BorderLayout;
import javax.swing.DropMode;
import javax.swing.JTextArea;
import java.awt.GridBagLayout;
import java.awt.GridBagConstraints;

/*
 * This is a panel for getting help on how to play sudoku.
 * 
 * Author: Claude White
 */

public class Help extends JPanel {

	public Help() {
		setBounds(5, 5, 400, 110);
		GridBagLayout gridBagLayout = new GridBagLayout();
		gridBagLayout.columnWidths = new int[]{400, 0};
		gridBagLayout.rowHeights = new int[]{110, 0};
		gridBagLayout.columnWeights = new double[]{1.0, Double.MIN_VALUE};
		gridBagLayout.rowWeights = new double[]{1.0, Double.MIN_VALUE};
		setLayout(gridBagLayout);
		JTextArea displayArea = new JTextArea();
		displayArea.setEditable(false);
		displayArea.setLineWrap(true);
		displayArea.setWrapStyleWord(true);
		displayArea.setText("The rules for sudoku are simple. A 9×9 square must be filled in with numbers from 1-9 with no repeated numbers in each line, horizontally or vertically. To challenge you more, there are 3×3 squares marked out in the grid, and each of these squares can’t have any repeat numbers either.");
		GridBagConstraints gbc_displayArea = new GridBagConstraints();
		gbc_displayArea.fill = GridBagConstraints.BOTH;
		gbc_displayArea.gridx = 0;
		gbc_displayArea.gridy = 0;
		add(displayArea, gbc_displayArea);

	}
}
