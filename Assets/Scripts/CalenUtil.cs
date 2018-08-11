using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CalenUtil {
	private static System.Random r;

	public static System.Random random{
		get{
			if (r == null){
				r = new System.Random();
			}
			return r;
		}
	}

	public static Vector2 RandomBetweenVectors(Vector2 min, Vector2 max){
		var x = Random.Range(min.x, max.x);
		var y = Random.Range(min.y, max.y);

		return new Vector2(x,y);
	}

	public static bool Coinflip(){
		return Random.Range(0f,1f) > 0.5f;
	}
}
