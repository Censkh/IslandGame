using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeMesh : MonoBehaviour
{
	public static Vector3[] offsets = new Vector3[]{
		new Vector3(-1,-1,-1),
		new Vector3(1,1,-1),
		new Vector3(-1,-1,-1),
		new Vector3(-1,1,1),
		new Vector3(-1,1,1),
		new Vector3(1,-1,-1),
	};
	public static Vector3[] directions = new Vector3[]{
		Vector3.forward,
		Vector3.back,
		Vector3.left,
		Vector3.right,
		Vector3.up,
		new Vector3(0,0,2),
	};
	static System.Random backupRandom = new System.Random();
	static Noise backupNoise = new Noise(500, 0.01, 1337);
	static Material standardMaterial;

	public Island island;
	public Sun sun;
	protected MeshRenderer meshRenderer { get { return GetComponent<MeshRenderer>(); } }
	protected Collider meshCollider { get { return GetComponent<MeshCollider>(); } }
	protected MeshFilter meshFilter { get { return GetComponent<MeshFilter>(); } }
	protected Noise noise { get { return island != null ? island.noise : (sun == null ? backupNoise : sun.noise); } }
	protected System.Random random { get { return island != null ? island.random : (sun == null ? backupRandom : sun.random); } }
	Vector3 randomOffset;
	int currentLod = -1;
	Dictionary<int, Mesh> lodMap = new Dictionary<int, Mesh>();

	public virtual void Init()
	{
		sun = Object.FindObjectOfType<Sun>();
		CreateCollider();
	}

	protected virtual Collider CreateCollider()
	{
		return gameObject.AddComponent<MeshCollider>();
	}

	protected virtual int GetSize()
	{
		return 0;
	}

	public virtual void BuildMesh()
	{
		BuildMesh(0);
	}

	public virtual void SwithLod(int lod)
	{
		if (!lodMap.ContainsKey(lod))
		{
			BuildMesh(lod);
		}
		else
		{
			meshFilter.sharedMesh = lodMap[lod];
		}
	}

	public virtual void BuildMesh(int lod)
	{
		if (lod != currentLod)
		{
			currentLod = lod;
			if (IsRandomized())
			{
				randomOffset = random.Next(0, 100000) * Vector3.one;
			}
			int size = lod > 0 ? GetSize() / (2 * lod) : GetSize();
			int res = lod > 0 ? (2 * lod) : 1;
			Mesh mesh = new Mesh();
			mesh.name = name + "-Mesh";
			Vector3[] verts = new Vector3[6 * size * size * 6];
			int[] tris = new int[verts.Length];
			Vector2[] uvs = new Vector2[verts.Length];
			int triCount = 0;
			Face face;
			for (int i = 0; i < 6; i++)
			{
				face = (Face)i;
				if (FaceCheck(face))
				{
					for (int x = 0; x < size; x++)
					{
						for (int y = 0; y < size; y++)
						{
							int ox = x;
							int oy = y;
							x *= res;
							y *= res;
							Vector2 uv = CalculateUV(face, x, y);
							AddVertex(face, x, y, triCount, verts, tris, uvs, uv);
							AddVertex(face, x, y + res, triCount + 1, verts, tris, uvs, uv);
							AddVertex(face, x + res, y + res, triCount + 2, verts, tris, uvs, uv);

							AddVertex(face, x + res, y + res, triCount + 3, verts, tris, uvs, uv);
							AddVertex(face, x + res, y, triCount + 4, verts, tris, uvs, uv);
							AddVertex(face, x, y, triCount + 5, verts, tris, uvs, uv);
							triCount += 6;
							x = ox;
							y = oy;
						}
					}
				}
			}
			mesh.vertices = verts;
			mesh.triangles = tris;
			mesh.uv = uvs;
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			mesh.Optimize();
			meshFilter.sharedMesh = mesh;
			meshRenderer.sharedMaterial = GetMaterial();
			if (lod == 0 && meshCollider != null && meshCollider is MeshCollider) ((MeshCollider)meshCollider).sharedMesh = mesh;
			lodMap[lod] = mesh;
		}
	}

	protected virtual Material GetMaterial()
	{
		return standardMaterial == null ? standardMaterial = Resources.Load<Material>("M_Standard") : standardMaterial;
	}

	protected virtual Vector2 CalculateUV(Face face, float x, float y)
	{
		return Vector2.zero;
	}

	protected virtual void AddVertex(Face face, float x, float y, int triCount, Vector3[] verts, int[] tris, Vector2[] uvs, Vector2 uv)
	{
		verts[triCount] = TransformPoint(face, x, y);
		tris[triCount] = triCount;
		uvs[triCount] = uv;
	}

	protected virtual bool IsAutoLod()
	{
		return true;
	}

	protected virtual Vector3 TransformPoint(Face face, float x, float y)
	{
		Vector3 vector = new Vector3(x, 0, y);
		transform.eulerAngles = CubeMesh.directions[(int)face] * 90;
		vector = transform.TransformDirection(vector);
		vector += CubeMesh.offsets[(int)face] * (GetSize() / 2f);
		if (IsNormalized()) vector = vector.normalized * (GetSize() / 2f);
		transform.eulerAngles = Vector3.zero;
		return vector;
	}

	protected virtual bool IsRandomized()
	{
		return false;
	}

	protected virtual bool FaceCheck(Face face)
	{
		return true;
	}

	protected virtual bool IsNormalized()
	{
		return true;
	}

	protected float GetHeightValue(Vector3 vector, float freq)
	{
		vector *= freq;
		vector += randomOffset;
		return (float)noise.GetNoise((int)vector.x, (int)vector.y, (int)vector.z);
	}
}
