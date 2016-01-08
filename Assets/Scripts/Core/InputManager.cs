#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_FLASH
#define RECORD_MOUSE_INPUT
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Artoncode.Core {
	public class InputManager : SingletonMonoBehaviour<InputManager> {

		const int kMouseButtonID = 0;
		public bool touchEnabled;
		public bool isOnConsole = false;
		public List <GameObject> receivers;
		public float maxDuration = 10f;
		private Dictionary<int, TouchInput> trackedTouches = new Dictionary<int, TouchInput> ();

		void Start () {
			touchEnabled = true;
		}

		void Update () {
			record ();
			sendTouches ();
			cleanUp ();
		}

		public void disableTouch () {
			touchEnabled = false;
			if (trackedTouches.Keys.Count == 0)
				return;
			int [] keys = trackedTouches.Keys.ToArray ();
			for (int i=0; i<keys.Length; i++) {
				trackedTouches [keys [i]].phase = TouchPhase.Ended;
			}
		}

		public void enableTouch () {
			touchEnabled = true;
		}

		private void sendTouches () {
			if (trackedTouches.Keys.Count == 0)
				return;
			TouchInput [] touches = (from touch in trackedTouches.Values
			                        orderby touch.fingerId ascending
			                        select touch
			                        ).ToArray ();
			foreach (GameObject receiver in receivers.ToArray()) {
				MonoBehaviour[] components = receiver.GetComponents<MonoBehaviour> ();
				foreach (MonoBehaviour component in components) {
					if (component is IInputManagerDelegate) {
						if (component.enabled) {
							IInputManagerDelegate d = component as IInputManagerDelegate;
							d.touchStateChanged (touches);
						}
					}
				}
			}
		}
		
		private void cleanUp () {
			if (trackedTouches.Keys.Count == 0)
				return;
			int [] keys = trackedTouches.Keys.ToArray ();
			for (int i=0; i<keys.Length; i++) {
				TouchInput touch = trackedTouches [keys [i]];
				if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended) {
					trackedTouches.Remove (touch.fingerId);
				}
			}
		}
		
		private void record () {
			if (!touchEnabled)
				return;

			mouseRecord ();
			
			foreach (Touch touch in Input.touches) {
				if (trackedTouches.ContainsKey (touch.fingerId)) {
					if (maxDuration > 0 && Time.time - trackedTouches [touch.fingerId].startTime > maxDuration) {
						trackedTouches [touch.fingerId].phase = TouchPhase.Ended;
					} else {
						switch (touch.phase) {
						case TouchPhase.Moved:
							trackedTouches [touch.fingerId].positions.Add (touch.position);
							break;
						}
						trackedTouches [touch.fingerId].phase = touch.phase;
					}
				} else if (touch.phase == TouchPhase.Began) {
					trackedTouches [touch.fingerId] = new TouchInput (touch.fingerId, touch.position, TouchPhase.Began);
				}
			}
		}
		
		[System.Diagnostics.Conditional("RECORD_MOUSE_INPUT")]
		private void mouseRecord () {
			if (trackedTouches.ContainsKey (kMouseButtonID)) {
				if (maxDuration > 0 && Time.time - trackedTouches [kMouseButtonID].startTime > maxDuration) {
					trackedTouches [kMouseButtonID].phase = TouchPhase.Ended;
				} else {
					TouchInput trackedTouch = trackedTouches [kMouseButtonID];
					
					// TouchPhase.Ended
					if (Input.GetMouseButtonUp (0)) {
						trackedTouches [kMouseButtonID].phase = TouchPhase.Ended;
					}
					// TouchPhase.Moved
					else if (System.Math.Abs (Input.mousePosition.x - trackedTouch.end.x) > 0.01
						|| System.Math.Abs (Input.mousePosition.y - trackedTouch.end.y) > 0.01) {
						Vector2 position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
						trackedTouches [kMouseButtonID].positions.Add (position);
						trackedTouches [kMouseButtonID].phase = TouchPhase.Moved;
					} else {
						trackedTouches [kMouseButtonID].phase = TouchPhase.Stationary;
					}
				}
			} else if (Input.GetMouseButtonDown (0)) {
				Vector2 position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				trackedTouches [kMouseButtonID] = new TouchInput (kMouseButtonID, position, TouchPhase.Began);
			}
		}
	}

}
