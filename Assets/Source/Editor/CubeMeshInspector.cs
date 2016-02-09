using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CubeMesh),true)]
public class CubeMeshInspector : Editor
{

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Build Mesh"))
		{
			((CubeMesh)target).BuildMesh();
		}
	}

}