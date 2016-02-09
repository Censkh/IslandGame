using UnityEngine;
using System.Collections.Generic;

public class FireMesh : CubeMesh
{

	static Material fireMaterial;
	static List<Mesh> meshes = new List<Mesh>();
	int meshCount = 0;
	float meshTimer;

	void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();
		if (meshes.Count == 0)
		{
			for (int i = 0; i < 25; i++)
			{
				meshFilter.sharedMesh = null;
				BuildMesh();
				meshes.Add(meshFilter.sharedMesh);
			}
		}
		meshCount = random.Next(5);
		UpdateMesh();
		GameObject obj = Instantiate(Resources.Load<GameObject>("P_SmokeEmitter"));
		obj.transform.parent = transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = transform.localScale;
	}

	protected override Material GetMaterial()
	{
		return fireMaterial == null ? fireMaterial = Resources.Load<Material>("M_Fire") : fireMaterial;
	}

	protected override int GetSize()
	{
		return 3;
	}

	void Update()
	{
		meshTimer += Time.deltaTime;
		if (meshTimer > (1f / 5f))
		{
			meshTimer = 0f;
			UpdateMesh();
		}
	}

	void UpdateMesh()
	{
		meshCount++;
		if (meshCount >= meshes.Count)
		{
			meshCount = 0;
		}
		meshFilter.sharedMesh = meshes[meshCount];
	}

	protected override Collider CreateCollider()
	{
		return null;
	}

	protected override bool IsRandomized()
	{
		return true;
	}

	protected override Vector2 CalculateUV(Face face, float x, float y)
	{
		return base.CalculateUV(face, x, y);
	}

	protected override Vector3 TransformPoint(Face face, float x, float y)
	{
		Vector3 vector = base.TransformPoint(face, x, y);
		vector.y += (Mathf.Abs(GetHeightValue(vector, 80f)) * 240f) - 1f;
		vector += -new Vector3(vector.x, 0, vector.z).normalized * 0.5f;
		return vector * 0.4f;
	}
}
