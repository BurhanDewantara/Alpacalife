using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaperContainer : MonoBehaviour {

	private List<GameObject> _papers;

	public List<GameObject> Papers{
		get{
			return _papers;
		}
		set{
			_papers = value;
		}
	}


	void Awake()
	{
		Papers = new List<GameObject> ();
	}

	public void ClearPapers()
	{
		foreach (GameObject paper in Papers) {
			Destroy(paper);
		}
		Papers.Clear ();
	}

	public void RemovePaper(GameObject obj)
	{
		Papers.Remove (obj);
	}

	public void AddPaper(GameObject paper)
	{
		paper.GetComponent<RectTransform>().SetParent(this.GetComponent<RectTransform> (),false);
		paper.GetComponent<RectTransform> ().SetAsFirstSibling ();
		paper.GetComponent<RectTransform> ().position += RandomPoint (-10, 10);
		Papers.Add (paper);
	}

	public void AddPapers(GameObject[] papers)
	{
		foreach (GameObject paper in papers) {
			this.AddPaper(paper);			
		}
	}

	private Vector3 RandomPoint(float min, float max)
	{
		return new Vector3 (Random.Range (min, max), Random.Range (min, max), 0);

	}

	
}
