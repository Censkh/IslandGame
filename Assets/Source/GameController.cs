using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

	int seed;
	int currentPlanet = 0;

	void OnGUI()
	{
		Sun sun = GameObject.FindObjectOfType<Sun>();
		if (GUI.Button(new Rect(12, 12, 90, 25), "Reset Sun"))
		{
			FindObjectOfType<CameraBoom>().transform.parent = null;
			GameObject gameObject = sun == null ? new GameObject("Sun") : sun.gameObject;
			sun = gameObject.GetComponent<Sun>();
			if (sun == null) sun = gameObject.AddComponent<Sun>();
			sun.seed = seed;
			sun.Init();
		}
		int r;
		int.TryParse(GUI.TextField(new Rect(112, 12, 100, 25), seed + ""), out r);
		if (r != 0) seed = r;
		if (sun != null)
		{
			int c = (int)GUI.HorizontalSlider(new Rect(12, 60, 200, 25), (float)currentPlanet, 0f, (float)sun.islands.Count - 1, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb);
			if (c != currentPlanet)
			{
				currentPlanet = c;
				GameObject.FindObjectOfType<CameraBoom>().attachedObject = sun.islands[currentPlanet].gameObject;
			}
		}
	}
}
