using UnityEngine;
using System.Collections;

public class TreeMesh : CubeMesh
{

	public enum MeshType
	{
		Log, Leaves
	}

	public MeshType type;

	public override void Init()
	{
		base.Init();
		if (type == MeshType.Leaves)
		{
			transform.localScale = Vector3.one * 0.2f;
			transform.localPosition = Vector3.zero;
		}
		else
		{
			transform.localScale = new Vector3(0.02f, 0.5f, 0.02f);
			transform.localPosition = new Vector3(0f, -0.75f, 0f);
		}
		BuildMesh();
	}

	protected override Material GetMaterial()
	{
		return island != null ? island.treeMaterial : base.GetMaterial();
	}

	protected override int GetSize()
	{
		return type == MeshType.Leaves ? 4 : 3;
	}

	protected override Collider CreateCollider()
	{
		return null;
	}

	protected override bool IsRandomized()
	{
		return true;
	}

	protected override bool FaceCheck(Face face)
	{
		if (type == MeshType.Log)
		{
			return !(face == Face.Top || face == Face.Bottom);
		}
		return true;
	}

	protected override Vector2 CalculateUV(Face face, float x, float y)
	{
		Vector2 vector = new Vector2(random.Next(4) + 0.5f, random.Next(2) + 0.5f) / 4f;
		if (type == MeshType.Leaves)
		{
			vector.y += 0.5f;
		}
		return vector;
	}

	public override void BuildMesh()
	{
		base.BuildMesh();
		transform.Rotate(new Vector3(0, random.Next(360), 0));
	}

	protected override Vector3 TransformPoint(Face face, float x, float y)
	{
		Vector3 vector = base.TransformPoint(face, x, y);
		if (type == MeshType.Leaves) vector -= vector.normalized * GetHeightValue(vector, 80f) * 100f;
		else
		{
			float d = Mathf.Abs(vector.y) * 12f;
			vector -= new Vector3(vector.normalized.x, 0, vector.normalized.z) * (Mathf.Abs(GetHeightValue(vector, 80f) * 200f) + (-d));
		}
		return vector;
	}
}
