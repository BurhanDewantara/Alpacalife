using UnityEngine;
using System.Collections;

namespace Artoncode.Core.Utility {

	public class HUDFPS : MonoBehaviour 
	{
		public  float updateInterval = 0.5F;
		
		private float accum   = 0; // FPS accumulated over the interval
		private int   frames  = 0; // Frames drawn over the interval
		private float timeleft; // Left time for current interval
		private string format;

		void Start()
		{
			timeleft = updateInterval;  
		}

		void OnGUI () {
			timeleft -= Time.deltaTime;
			accum += Time.timeScale/Time.deltaTime;
			++frames;
			
			// Interval ended - update GUI text and start new interval
			if( timeleft <= 0.0 )
			{
				// display two fractional digits (f2 format)
				float fps = accum/frames;
				format = System.String.Format("{0:F2} FPS",fps);

				timeleft = updateInterval;
				accum = 0.0F;
				frames = 0;
			}
			GUI.Label(new Rect(10,10,100,100), format);
		}
	}

}