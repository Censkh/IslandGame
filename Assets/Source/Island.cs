using UnityEngine;
using System;
using System.Collections;

public class Island : MonoBehaviour
{

	public static Color[] colors = new Color[]{
		new Color(1f,0f,0f),
		new Color(12f/255f,80f/255f,1f),
		new Color(0.2f,220f/255f,0.2f),
		new Color(1f,1f,40f/255f),
		Color.cyan,
		new Color(0.9f,0.1f,0.9f),
		new Color(1f,140f/255f,10f/255f),
	};

	public Material material;
	public Material treeMaterial;
	public Material houseMaterial;
	public int seed;
	public int size = 12;
	float colliderTimer;

	Noise _noise;
	public Noise noise { get { return _noise == null ? _noise = new Noise(500, 0.01, seed) : _noise; } set { _noise = value; } }
	System.Random _random;
	public System.Random random { get { return _random == null ? _random = new System.Random(seed) : _random; } set { _random = value; } }
	BoxCollider islandCollider;
	Rigidbody islandRigidbody;

	public void Init()
	{
		SetupMaterial();
		CullComponents();

		CreateIslandMesh(IslandMesh.MeshType.Top);
		CreateIslandMesh(IslandMesh.MeshType.Bottom);
		CreateIslandMesh(IslandMesh.MeshType.Water);

		for (int i = 0; i < random.Next(3) + 1; i++)
		{
			CreateCloud();
		}
		for (int i = 0; i < random.Next(5) + 2; i++)
		{
			CreateTree();
		}
		for (int i = 0; i < random.Next(5) + 2; i++)
		{
			CreateHouse();
		}

		islandCollider = gameObject.GetComponent<BoxCollider>();
		if (islandCollider == null) islandCollider = gameObject.AddComponent<BoxCollider>();
		islandCollider.size = Vector3.one * 30;
		islandCollider.isTrigger = true;
		islandRigidbody = gameObject.GetComponent<Rigidbody>();
		if (islandRigidbody == null) islandRigidbody = gameObject.AddComponent<Rigidbody>();
		islandRigidbody.constraints = RigidbodyConstraints.FreezeAll;
		islandRigidbody.isKinematic = true;
		RecalculatePosition();
	}

	void OnTriggerStay(Collider collider)
	{
		Island island = collider.gameObject.GetComponent<Island>();
		if (island != null)
		{
			RecalculatePosition();
		}
	}

	public void RecalculatePosition()
	{
		transform.position = new Vector3(random.Next(11) - 5, 0, random.Next(11) - 5) * 10f;
		float o = 20f;
		transform.position = new Vector3(transform.position.x + (transform.position.x > 0 ? o : -o), 0, transform.position.z + (transform.position.z > 0 ? o : -o));
	}

	void CreateCloud()
	{
		GameObject obj = new GameObject(name + "-Cloud");
		obj.transform.parent = transform;
		CloudMesh cloud = obj.AddComponent<CloudMesh>();
		cloud.island = this;
		cloud.Init();
	}

	void CreateHouse()
	{
		GameObject obj = new GameObject(name + "-House");
		obj.transform.parent = transform;
		House house = obj.AddComponent<House>();
		house.island = this;
		house.Init();
	}

	void CreateTree()
	{
		GameObject obj = new GameObject(name + "-Tree");
		obj.transform.parent = transform;
		Tree tree = obj.AddComponent<Tree>();
		tree.island = this;
		tree.Init();
	}

	void CullComponents()
	{
		Component component = GetComponent<MeshRenderer>();
		if (component != null) Destroy(component);
		component = GetComponent<MeshFilter>();
		if (component != null) Destroy(component);
	}

	void SetupMaterial()
	{
		int rc;
		Color color = colors[rc = random.Next(colors.Length)] + (new Color(1, 1, 1) * 0.2f);
		int size = 16;
		int res = 4;
		int pres = size / res;
		Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false, true);
		texture.filterMode = FilterMode.Bilinear;
		for (int x = 0; x < res; x++)
		{
			for (int y = 0; y < res; y++)
			{
				float r = (float)(random.NextDouble() - random.NextDouble()) * 0.025f;
				r -= 0.2f;
				Color c = (y < (res / 2) ? new Color(0.5f, 0.3f, 0.3f) + new Color(r, r, r) : color + new Color(r, r, r));
				for (int px = 0; px < pres; px++)
				{
					for (int py = 0; py < pres; py++)
					{
						texture.SetPixel((x * pres) + px, (y * pres) + py, c);
					}
				}
			}
		}
		texture.Apply();
		material = new Material(Resources.Load<Material>("M_Standard"));
		material.SetTexture(0, texture);

		texture = new Texture2D(16, 16);
		texture.filterMode = FilterMode.Bilinear;
		int cr = 0;
		while ((cr = random.Next(colors.Length)) == rc)
		{

		}
		color = colors[cr] + (new Color(1, 1, 1) * 0.2f);
		for (int x = 0; x < res; x++)
		{
			for (int y = 0; y < res; y++)
			{
				float r = (float)(random.NextDouble() - random.NextDouble()) * 0.025f;
				r -= 0.2f;
				Color c = (y < (res / 2) ? new Color(0.5f, 0.3f, 0.3f) + new Color(r, r, r) : color + new Color(r, r, r));
				for (int px = 0; px < pres; px++)
				{
					for (int py = 0; py < pres; py++)
					{
						texture.SetPixel((x * pres) + px, (y * pres) + py, c);
					}
				}
			}
		}
		texture.Apply();
		treeMaterial = new Material(Resources.Load<Material>("M_Standard"));
		treeMaterial.SetTexture(0, texture);

		texture = new Texture2D(16, 16);
		texture.filterMode = FilterMode.Bilinear;
		int cr2 = 0;
		while ((cr2 = random.Next(colors.Length)) == rc || cr2 == cr)
		{

		}
		color = colors[cr2] + (new Color(1, 1, 1) * 0.2f);
		for (int x = 0; x < res; x++)
		{
			for (int y = 0; y < res; y++)
			{
				float r = (float)(random.NextDouble() - random.NextDouble()) * 0.025f;
				r -= 0.2f;
				Color c = (y < (res / 2) ? Color.white : color + new Color(r, r, r));
				for (int px = 0; px < pres; px++)
				{
					for (int py = 0; py < pres; py++)
					{
						texture.SetPixel((x * pres) + px, (y * pres) + py, c);
					}
				}
			}
		}
		texture.Apply();
		houseMaterial = new Material(Resources.Load<Material>("M_Standard"));
		houseMaterial.SetTexture(0, texture);
	}

	IslandMesh CreateIslandMesh(IslandMesh.MeshType type)
	{
		GameObject obj = new GameObject(name + "-" + type);
		obj.transform.parent = transform;
		IslandMesh mesh = obj.AddComponent<IslandMesh>();
		mesh.island = this;
		mesh.type = type;
		mesh.Init();
		mesh.BuildMesh();
		return mesh;
	}
}
