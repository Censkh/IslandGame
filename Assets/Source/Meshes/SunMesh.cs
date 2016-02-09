using UnityEngine;
using System;
using System.Collections.Generic;

public class SunMesh : CubeMesh
{

	static Material sunMaterial;

	public override void Init()
	{
		base.Init();
		BuildMesh();
	}

	protected override Material GetMaterial()
	{
		return sunMaterial==null ? sunMaterial = Resources.Load<Material>("M_Sun") : sunMaterial;
	}

	protected override bool IsAutoLod()
	{
		return false;
	}

	protected override int GetSize()
	{
		return 3;
	}

	protected override Collider CreateCollider()
	{
		return null;
	}

	protected override Vector3 TransformPoint(Face face, float x, float y)
	{
		Vector3 vector = base.TransformPoint(face, x, y);
		vector -= vector.normalized * GetHeightValue(vector, 80f) * 100f;
		return vector;
	}
}
