package CWhite_G10_A04;

public class RomanNumeralTest {

	public static void main(String[] args) {
		RomanNumeral roNum1 = new RomanNumeral("MMDCCCLXVI");
		RomanNumeral roNum2 = new RomanNumeral("MDLI");
		RomanNumeral roNum3 = new RomanNumeral("MCCXXVI");
		RomanNumeral roNum4 = new RomanNumeral(783);
		RomanNumeral roNum5 = new RomanNumeral(4843);

		if (roNum1.getIntRomNum() != 2866) {
			System.out.println(
					"roNum1, Converting from Roman Numeral to integers doesn't work");
		}
		if (!roNum1.getRomanNumeral().equals("MMDCCCLXVI")) {
			System.out.println("roNum1, Storing the Roman Numeral doesn't work");
		}

		if (roNum2.getIntRomNum() != 1551) {
			System.out.println(
					"roNum2, Converting from Roman Numeral to integers doesn't work");
		}
		if (!roNum2.getRomanNumeral().equals("MDLI")) {
			System.out.println("roNum2, Storing the Roman Numeral doesn't work");
		}

		if (roNum3.getIntRomNum() != 1226) {
			System.out.println(
					"roNum3, Converting from Roman Numeral to integers doesn't work");
		}
		if (!roNum3.getRomanNumeral().equals("MCCXXVI")) {
			System.out.println("roNum3, Storing the Roman Numeral doesn't work");
		}

		if (roNum4.getIntRomNum() != 783) {
			System.out.println("roNum4, Storing an integer doesn't work");
		}
		if (!roNum4.getRomanNumeral().equals("DCCLXXXIII")) {
			System.out.println(
					"roNum4, Converting from Roman Numeral to integer doesn't work");
		}

		if (roNum5.getIntRomNum() != 4843) {
			System.out.println("roNum5, Storing an integer doesn't work");
		}
		if (!roNum5.getRomanNumeral().equals("MMMMDCCCXXXXIII")) {
			System.out.println(
					"roNum5, Converting from Roman Numeral to integer doesn't work");
		}

		else {
			System.out.println("Your program is good to go!");
		}
	} // main(String[])
} // RomanNumeralTest class
