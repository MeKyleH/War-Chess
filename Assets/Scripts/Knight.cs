using UnityEngine;
using System.Collections;

public class Knight : Chessman {

	public override bool[,] PossibleMove () {
		bool[,] r = new bool[8, 8];

		//Up Left
		KinghtMove(CurrentX -1, CurrentY +2, ref r);

		//Left Up
		KinghtMove(CurrentX -2, CurrentY +1, ref r);

		//Left Down
		KinghtMove(CurrentX -2, CurrentY -1, ref r);

		//Down Left
		KinghtMove(CurrentX -1, CurrentY -2, ref r);

		//Down Right
		KinghtMove(CurrentX +1, CurrentY -2, ref r);

		//Right Down
		KinghtMove(CurrentX +2, CurrentY -1, ref r);

		//Right Up
		KinghtMove(CurrentX +2, CurrentY +1, ref r);

		//Up Right
		KinghtMove(CurrentX +1, CurrentY +2, ref r);

		return r;
	}

	public void KinghtMove(int x, int y, ref bool[,] r) {
		Chessman c;
		if (x >= 0 && x < 8 && y >= 0 && y < 8) {
			c = BoardManager.Instance.Chessmans [x, y];	
			if (c == null) {
				r [x, y] = true;
			} else if (isWhite != c.isWhite) {
				r [x, y] = true;
			}
		}
	}
}
