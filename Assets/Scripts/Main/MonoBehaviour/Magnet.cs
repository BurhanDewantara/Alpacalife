using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Magnet : MonoBehaviour {

	public delegate void MagnetDelegate (GameObject sender, GameObject magnetObject);
	public event MagnetDelegate OnObjectMagnetized;

	public List<GameObject> magnetObjects;
	public GameObject magnetSourcePoint;
	public float magnetPower = 10;

	void Awake()
	{
		magnetObjects = new List<GameObject> ();
	}

	public void AddMagnetObject(GameObject obj)
	{
		if(!magnetObjects.Contains(obj))
			magnetObjects.Add (obj);
	}

	public void ClearMagnetObjects()
	{
		foreach (GameObject obj in magnetObjects) {
			Destroy(obj);
		}
		magnetObjects.Clear ();
	}

	public void RemoveMagnetObject(GameObject magnetObject)
	{
		magnetObjects.Remove (magnetObject);
	}

	void FixedUpdate()
	{
		foreach (GameObject obj in magnetObjects) 
		{
			Vector3 objPosition 	= obj.GetComponent<RectTransform>().position;
			Vector3 objDestination 	= magnetSourcePoint.GetComponent<RectTransform>().position;

			if (Vector3.Distance(objPosition, objDestination) > 1) 
			{
				obj.GetComponent<RectTransform>().position = Vector3.MoveTowards(objPosition,objDestination,magnetPower);
			}
			else{
				obj.GetComponent<RectTransform>().position = objDestination;
				// kaching 
				// and destroy the object;
				if(OnObjectMagnetized !=null)
					OnObjectMagnetized(this.gameObject,obj);
				break;
			}

		}		
	}

	
}
