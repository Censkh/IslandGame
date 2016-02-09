using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Sun))]
public class SunInspector : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Init"))
		{
			((Sun)target).Init();
		}
	}

}