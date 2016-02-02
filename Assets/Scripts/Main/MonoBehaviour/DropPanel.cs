using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropPanel : MonoBehaviour {

	public List<SOColor> colorTargets = new List<SOColor>();
	public bool isTrashPanel = false;

	public void AddColorTarget(List<SOColor> target)
	{
		colorTargets.AddRange (target);
	}

	public void AddColorTarget(SOColor target)
	{
		if(!colorTargets.Contains(target))
			colorTargets.Add (target);
	}

	public bool IsColorExist(SOColor target)
	{
		return colorTargets.Contains (target);
	}
}
