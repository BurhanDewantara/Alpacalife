using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Artoncode.Core {

	public class TouchInput {
		public int fingerId;
		public TouchPhase phase;
		public float startTime;
		public List<Vector2> positions = new List<Vector2> ();
		
		public TouchInput (int id, Vector2 position, TouchPhase phase) {
			fingerId = id;
			positions.Add (position);
			startTime = Time.time;
			this.phase = phase;
		}
		
		public Vector2 start {
			get {
				return positions[0];
			}
		}
		
		public Vector2 end {
			get {
				return positions[positions.Count - 1];
			}
		}
		
		public Vector2 travel {
			get {
				if (positions.Count < 2) {
					return Vector2.zero;
				}
				
				return end - start;
			}
		}
		
		public Vector2 deltaPosition {
			get {
				if (positions.Count < 2) {
					return Vector2.zero;
				}
				return positions[positions.Count-1] - positions[positions.Count-2];
			}
		}
		
		public Vector2 normalizedTravel {
			get {
				Vector2 travel = this.travel;
				return travel == Vector2.zero ? Vector2.zero : new Vector2 (travel.x / Screen.width, travel.y / Screen.height);
			}
		}
	}

}