using UnityEngine;
using System.Collections;

public class Pawn : Chessman {

	public override bool[,] PossibleMove() {
		bool[,] r = new bool[8,8];

		Chessman c, c2;
		int[] e = BoardManager.Instance.EnPassantMove;

		//White team move
		if (isWhite) {
			//DIAGONAL LEFT
			//enpassant
			if (e [0] == CurrentX - 1 && e [1] == CurrentY + 1) {
				r [CurrentX - 1, CurrentY + 1] = true;
			}
			if (CurrentX != 0 && CurrentY != 7) {
				c = BoardManager.Instance.Chessmans [CurrentX - 1, CurrentY + 1];
				if (c != null && !c.isWhite) {
					r [CurrentX - 1, CurrentY + 1] = true;
				}
			}
			//DIAGONAL RIGHT
			//enpassant
			if (e [0] == CurrentX + 1 && e [1] == CurrentY + 1) {
				r [CurrentX + 1, CurrentY + 1] = true;
			}
			if (CurrentX != 7 && CurrentY != 7) {
				c = BoardManager.Instance.Chessmans [CurrentX + 1, CurrentY + 1];
				if (c != null && !c.isWhite) {
					r [CurrentX + 1, CurrentY + 1] = true;
				}
			}
			//MIDDLE
			if (CurrentY != 7) {
				c = BoardManager.Instance.Chessmans [CurrentX, CurrentY + 1];
				if (c == null)
					r [CurrentX, CurrentY + 1] = true;
			}
			//MIDDLE ON FIRST MOVE
			if (CurrentY == 1) {
				c = BoardManager.Instance.Chessmans [CurrentX, CurrentY + 1];
				c2 = BoardManager.Instance.Chessmans [CurrentX, CurrentY + 2];
				if (c == null && c2 == null) {
					r [CurrentX, CurrentY + 2] = true;
				}
			}


		//Black team move
		} 
		else {
			//DIAGONAL LEFT
			//enpassant
			if (e [0] == CurrentX - 1 && e [1] == CurrentY - 1) {
				r [CurrentX - 1, CurrentY - 1] = true;
			}
			if (CurrentX != 0 && CurrentY != 0) {
				c = BoardManager.Instance.Chessmans [CurrentX - 1, CurrentY - 1];
				if (c != null && c.isWhite) {
					r [CurrentX - 1, CurrentY - 1] = true;
				}
			}
			//DIAGONAL RIGHT
			//enpassant
			if (e [0] == CurrentX + 1 && e [1] == CurrentY - 1) {
				r [CurrentX + 1, CurrentY - 1] = true;
			}
			if (CurrentX != 7 && CurrentY != 0) {
				c = BoardManager.Instance.Chessmans [CurrentX + 1, CurrentY - 1];
				if (c != null && c.isWhite) {
					r [CurrentX + 1, CurrentY - 1] = true;
				}
			}
			//MIDDLE
			if (CurrentY != 0) {
				c = BoardManager.Instance.Chessmans [CurrentX, CurrentY - 1];
				if (c == null)
					r [CurrentX, CurrentY - 1] = true;
			}
			//MIDDLE ON FIRST MOVE
			if (CurrentY == 6) {
				c = BoardManager.Instance.Chessmans [CurrentX, CurrentY - 1];
				c2 = BoardManager.Instance.Chessmans [CurrentX, CurrentY - 2];
				if (c == null && c2 == null) {
					r [CurrentX, CurrentY - 2] = true;
				}
			}
		}

		return r;

	}
}
