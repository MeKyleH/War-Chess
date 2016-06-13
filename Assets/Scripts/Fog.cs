using UnityEngine;
using System.Collections;

public class Fog : MonoBehaviour {

	public int CurrentX{ set; get; }
	public int CurrentY{ set; get; }

	public void SetPosition(int x, int y) {
		CurrentX = x;
		CurrentY = y;
	}
}
