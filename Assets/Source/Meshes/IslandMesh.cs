using UnityEngine;
using System;
using System.Collections.Generic;

public class IslandMesh : CubeMesh
{

	public enum MeshType
	{
		Top, Bottom, Water
	}

	static Material waterMaterial;
	public MeshType type;

	public override void Init()
	{
		base.Init();
		if (type == MeshType.Top)
		{
			Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
		}
		transform.localPosition = Vector3.zero;
	}

	protected override Material GetMaterial()
	{
		return type == MeshType.Water ? (waterMaterial == null ? waterMaterial = Resources.Load<Material>("M_Water") : waterMaterial) : (island == null ? base.GetMaterial() : island.material);
	}

	protected override int GetSize()
	{
		return island.size;
	}

	void OnTriggerStay(Collider collider)
	{
		CloudMesh cloud = collider.GetComponent<CloudMesh>();
		if (cloud != null)
		{
			cloud.RecalculatePosition();
		}
	}

	protected override Collider CreateCollider()
	{
		if (type == MeshType.Water)
		{
			BoxCollider collider = gameObject.AddComponent<BoxCollider>();
			collider.size = new Vector3(GetSize(),1,GetSize());
			return collider;
		}
		return type==MeshType.Top ? base.CreateCollider() : null;
	}

	public override void BuildMesh()
	{
		base.BuildMesh();
		if (type == MeshType.Water)
		{
			transform.localScale = new Vector3(0.9f, 0.2f, 0.9f);
			transform.localPosition -= new Vector3(0, 0.3f, 0);
		}
	}

	protected override Vector2 CalculateUV(Face face, float x, float y)
	{
		Vector2 vector = new Vector2(random.Next(4) + 0.5f, random.Next(2) + 0.5f) / 4f;
		if (type == MeshType.Top)
		{
			vector.y += 0.5f;
		}
		return vector;
	}

	protected override bool FaceCheck(Face face)
	{
		if (type == MeshType.Water && face != Face.Top) return false;
		if (type == MeshType.Bottom && face == Face.Top) return false;
		return true;
	}

	protected override Vector3 TransformPoint(Face face, float x, float y)
	{
		Vector3 vector = base.TransformPoint(face, x, y);
		float dist = 1f - (new Vector3(vector.x, 0, vector.z).magnitude / (GetSize() / 2f));
		if (type == MeshType.Bottom)
		{
			float h = Mathf.Abs(GetHeightValue(vector, 60f)) * -5f;
			vector.y += h * 40f;
			vector.y += 1f;
			h += GetHeightValue(vector, 50f);
			vector = new Vector3(vector.x, vector.y > 0 ? -vector.y * 0.2f : Mathf.Min(0.1f, vector.y) + (h), vector.z);
		}
		else if (type == MeshType.Top)
		{
			dist *= 2f;
			if (vector.y > -0.1)
			{
				vector.y *= 0.2f;
				vector.y += (Mathf.Max(-0.0015f, GetHeightValue(vector, 30f)) * dist) * 600f;
				vector.y += (Mathf.Max(-0.0015f, GetHeightValue(vector, 50f)) * dist) * 300f;
			}
			else
			{
				vector.y *= 0.3f;
				vector.y -= (Mathf.Abs(GetHeightValue(vector, 50f))) * 70f;
			}
			vector *= 1.025f;
		}
		else if (type == MeshType.Water)
		{
			if (vector.y > 0)
			{
				vector.y *= 0.2f;
				vector.y += vector.magnitude - (GetSize() / 4f);
				vector.y += (GetHeightValue(vector, 200f) * (float)dist) * 200f;
				float len = 1.35f;
				vector = new Vector3(vector.x * len, vector.y, vector.z * len);
			}
		}
		return vector;
	}

}
