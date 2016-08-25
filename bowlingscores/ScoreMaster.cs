using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreMaster {

	// A simple way to print lists on one line for debugging purposes
	static string PrintList (List<int> myList)
	{
		string listAsString = "{";
		foreach (int element in myList) {
			listAsString += element.ToString () + ", ";
		}
		listAsString += "}";
		return listAsString;
	}


	// Returns a list of cumulative scores, like a normal scorecard
	// (Needs to use ScoreFrames(rolls) in order to work
	public static List<int> ScoreCumulative (List<int> rolls) {
		List<int> cumulativeScores = new List<int>();
		int previousSum = 0;

		foreach (int frameScore in ScoreFrames (rolls)) {
			previousSum += frameScore;
			cumulativeScores.Add(previousSum);
		}

		Debug.Log ("CUMULATIVES: " + PrintList (cumulativeScores));

		return cumulativeScores;
	}

	// Ben's very elegant way to get a list of frame-by-frame scores
	public static List<int> ScoreFrames (List<int> rolls) {

		List<int> frames = new List<int>();

		for (int i = 1; i < rolls.Count; i += 2) {
			if (frames.Count == 10) {						// Prevents 11th frame
				break;
			}

			if (rolls[i - 1] + rolls[i] < 10) {				// normal "open" frame
				frames.Add( rolls[i - 1] + rolls[i]);
			}

			if (rolls.Count - i <= 1) {						// Insufficient look-ahead
				break;
			}

			if (rolls[i - 1] == 10) {						// Strike
				frames.Add(10 + rolls[i] + rolls[i + 1]);
				i--;										// Strike frame has just one bowl
			} else if (rolls[i - 1] + rolls[i] == 10) {		// Spare
				frames.Add(10 + rolls[i + 1]);
			}

		}

		return frames;
	}


	// My far, far less elegant solution to getting frame-by-frame scores
	public static List<int> MyScoreFrames (List<int> rolls) {

		List<int> frameList = new List<int>();
		int sumLastFrame = 0;
		int whichFrame = 0;

		Debug.Log("ROLLS: " + PrintList (rolls));

		bool isFirstRollOfFrame = true;

		for (int i = 0; i < rolls.Count; i++) {
			if (isFirstRollOfFrame) {
				whichFrame++;

				if (whichFrame == 10) {
					for (int decrement = rolls.Count - i; decrement > 0; decrement--) {
						sumLastFrame += rolls[rolls.Count - decrement];
					}
					break;
				}

				if (rolls[i] != 10) {
					isFirstRollOfFrame = !isFirstRollOfFrame;
				}
			} else {
				isFirstRollOfFrame = !isFirstRollOfFrame;
			}
		}

		whichFrame = 0;

		int oddRoll = -1;
		bool isOdd = false;
		bool oddUsed = false;

		if (rolls.Count % 2 == 1) {
			isOdd = true;
			oddRoll = rolls[rolls.Count - 1];
			rolls.RemoveAt(rolls.Count - 1);
		}


		for (int i = 0; i < rolls.Count; i++) {
			whichFrame += 1;
			if (whichFrame == 10) {
				frameList.Add(sumLastFrame);
				i = rolls.Count;
				break;
			}

			if (rolls.Count - i <= 1) {
				if (isOdd && !oddUsed) {
					rolls.Add(oddRoll);
					oddUsed = true;
					i--;
					whichFrame--;
				} else {
					i++;
				}
			} else {
				if (rolls[i] == 10) { // If strike
					if (rolls.Count - i > 2) {
						frameList.Add (rolls [i] + rolls [i + 1] + rolls [i + 2]);
					} else {
						if (isOdd && !oddUsed) {
							rolls.Add (oddRoll);
							oddUsed = true;
							frameList.Add (rolls [i] + rolls [i + 1] + rolls [i + 2]);
						}
						whichFrame++;
					}

				} else if (rolls[i] + rolls[i + 1] == 10) { // If spare
					if (rolls.Count - i > 2) {
						frameList.Add (rolls [i] + rolls [i + 1] + rolls [i + 2]);
					} else {
						if (isOdd && !oddUsed) {
							rolls.Add (oddRoll);
							oddUsed = true;
							frameList.Add (rolls [i] + rolls [i + 1] + rolls [i + 2]);
						}
					}
					i++;
				} else { // If neither strike nor spare (this is most frequent)
					frameList.Add(rolls[i] + rolls[i + 1]);
					i++;
				}
			}
		}
		//******************BOTTOM**OF**THING*****************

		Debug.Log ("FRAMELIST: " + PrintList (frameList));
		return frameList;
	}



}
