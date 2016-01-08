using UnityEngine;
using System.Collections;

namespace Artoncode.Core.Utility {

	[RequireComponent (typeof (BoxCollider))]
	public class BoxGizmos : MonoBehaviour {

		public Color gizmoColour;

		void OnDrawGizmos() {
			Gizmos.color = gizmoColour;
			Gizmos.matrix = Matrix4x4.TRS (transform.position, transform.rotation, transform.lossyScale);
			Gizmos.DrawCube(GetComponent<BoxCollider>().center,GetComponent<BoxCollider>().size);
		}
	}

}