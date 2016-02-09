using UnityEngine;
using System.Collections.Generic;

public class HouseMesh : CubeMesh
{

	public enum MeshType
	{
		Base, Roof
	}

	Vector3 startScale;
	Vector3 vector;
	public MeshType type;

	public override void Init()
	{
		base.Init();
		transform.localPosition = type == MeshType.Base ? Vector3.zero : new Vector3(0, 0.16f, 0);
		BuildMesh();
	}

	protected override bool IsAutoLod()
	{
		return false;
	}

	protected override Material GetMaterial()
	{
		return island != null ? island.houseMaterial : base.GetMaterial();
	}

	protected override int GetSize()
	{
		return 2;
	}

	protected override Collider CreateCollider()
	{
		return null;
	}

	protected override bool FaceCheck(Face face)
	{
		if (type == MeshType.Base) return face != Face.Bottom && face != Face.Top;
		return face != Face.Bottom;
	}

	protected override Vector3 TransformPoint(Face face, float x, float y)
	{
		Vector3 vector = base.TransformPoint(face, x, y);
		if (type == MeshType.Base)
		{
			vector *= 0.3f;
			if (vector.y > 0)
			{
				vector.y += vector.x == 0 ? 0.1f : -0.3f;
			}
			else
			{
				vector.y*=7f;
			}
			float height = GetHeightValue(vector, 50f) * 6f;
			vector += (new Vector3(1, 0, 1) * height);
			vector.y *= 0.462f;
		}
		else
		{
			vector.y += 1f;
			if (Mathf.Abs(vector.x) > 0.1f)
			{
				vector.y -= 4f;
			}
			vector *= 0.45f;
			vector.y *= 0.1245f;
		}
		return vector;
	}

	public override void BuildMesh()
	{
		base.BuildMesh();
	}

	protected override Vector2 CalculateUV(Face face, float x, float y)
	{
		Vector2 vector = new Vector2(random.Next(4) + 0.5f, random.Next(2) + 0.5f) / 4f;
		if (type == MeshType.Roof)
		{
			vector.y += 0.5f;
		}
		return vector;
	}

	protected override bool IsNormalized()
	{
		return false;
	}

	protected override bool IsRandomized()
	{
		return true;
	}

}
