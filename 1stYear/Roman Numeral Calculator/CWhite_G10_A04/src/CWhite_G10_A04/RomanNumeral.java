package CWhite_G10_A04;

public class RomanNumeral {

	private final String[] romanDigits = { "M", "D", "C", "L", "X", "V", "I" };
	private final int[] romanValues = { 1000, 500, 100, 50, 10, 5, 1 };

	private String romanNumeral;
	private int intRomNum;

	public RomanNumeral() {
		romanNumeral = "";
		intRomNum = 0;
	} // RomanNumeral()

	public RomanNumeral(String rNum) {
		romanNumeral = rNum;
		char[] RomDigitArr = romanNumeral.toCharArray();
		for (Character i : RomDigitArr) {
			romanNumeral = String.valueOf(i);
			intRomNum += convertRomanToInteger();
		} // for()
	} // RomanNumeral(String)

	public RomanNumeral(int iNum) {
		intRomNum = iNum;
	} // RomanNumeral(int)

	public int getIntRomNum() {
		return intRomNum;
	} // getIntRomNum()

	public String getRomanNumeral() {
		return convertIntegerToRoman();
	} // getRomanNumeral()

	private int convertRomanToInteger() {
		int i;
		boolean flag = false;
		for (i = 0; i < romanDigits.length && !flag; i++) {
			if (romanDigits[i].equals(romanNumeral)) {
				flag = true;
			} // if()
		} // for()
		return romanValues[i - 1];
	} // convertToInteger()

	private String convertIntegerToRoman() {
		romanNumeral = "";
		while (intRomNum > 0) {
			if (intRomNum >= 1000) {
				intRomNum -= 1000;
				romanNumeral += "M";
			} // if()
			else
				if (intRomNum >= 500) {
					intRomNum -= 500;
					romanNumeral += "D";
				} // else if()
				else
					if (intRomNum >= 100) {
						intRomNum -= 100;
						romanNumeral += "C";
					} // else if()
					else
						if (intRomNum >= 50) {
							intRomNum -= 50;
							romanNumeral += "L";
						} // else if()
						else
							if (intRomNum >= 10) {
								intRomNum -= 10;
								romanNumeral += "X";
							} // else if()
							else
								if (intRomNum >= 5) {
									intRomNum -= 5;
									romanNumeral += "V";
								} // else if()
								else {
									intRomNum -= 1;
									romanNumeral += "I";
								} // else
		} // while (intRomNum > 0)
		return romanNumeral;
	} // convertToRoman()
} // RomanNumeral class
