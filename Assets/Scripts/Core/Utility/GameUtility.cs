using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Artoncode.Core.Utility {
	public class GameUtility {

		/// <summary>
		/// Increase current to a target value by acceleration overtime.
		/// </summary>
		/// <returns>The towards.</returns>
		/// <param name="current">The current value.</param>
		/// <param name="target">The speed of change per second.</param>
		/// <param name="acceleration">The speed of change per second.</param>
		/// <param name="timeStep">The time used to update the value.</param>
		public static float changeTowards (float current, float target, float acceleration, float timeStep) {
			if (current == target) {
				return current;	
			} else {
				float dir = Mathf.Sign (target - current); // must n be increased or decreased to get closer to target
				current += acceleration * timeStep * dir;
				return (dir == Mathf.Sign (target - current)) ? current : target; // if n has now passed target then return target, otherwise return n
			}
		}

		public static float inchToPixel (float inch) {
			return inch * (Screen.dpi > 0 ? Screen.dpi : 200);
		}

		public static Vector2 cross (Vector2 a, float s) {
			return new Vector2 (s * a.y, -s * a.x);
		}

		public static float cross (Vector2 a, Vector2 b) {
			return a.x * b.y - a.y * b.x;
		}
	}

}