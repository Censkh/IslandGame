using UnityEngine;
using System;
using System.Collections.Generic;

public class Sun : MonoBehaviour
{

	public int seed;
	Noise _noise;
	public Noise noise { get { return _noise == null ? _noise = new Noise(500, 0.01, seed) : _noise; } set { _noise = value; } }
	System.Random _random;
	public System.Random random { get { return _random == null ? _random = new System.Random(seed) : _random; } set { _random = value; } }
	public List<Island> islands = new List<Island>();
	int islandCount = 0;
	SunMesh _sunMesh;
	SunMesh sunMesh { get { return _sunMesh == null ? sunMesh = GetComponentInChildren<SunMesh>() : _sunMesh; } set { _sunMesh = value; } }

	public void Init()
	{
		islandCount = 0;
		transform.localPosition = Vector3.zero;
		if (islands.Count > 0)
		{
			foreach (Island island in islands)
			{
				if (island != null)
				{
					CameraBoom boom = island.GetComponentInChildren<CameraBoom>();
					if (boom != null)
					{
						boom.transform.parent = null;
					}
					DestroyImmediate(island.gameObject);
				}
			}
			islands.Clear();
		}
		int r = random.Next(5) + 2;
		for (int i = 0; i < r; i++)
		{
			islands.Add(CreateIsland());
		}
		FindObjectOfType<CameraBoom>().attachedObject = islands[0].gameObject;
		if (sunMesh != null) { DestroyImmediate(sunMesh.gameObject); }
		sunMesh = CreateMesh();
		LineRenderer line = GetComponent<LineRenderer>();
		int vertCount = 1;
		float radius = 110f;
		float x;
		float y;
		float h= -2f;
		for (float i = 0; i < 2 * Mathf.PI; i += 0.05f)
		{
			x = radius * Mathf.Cos(i);
			y = radius * Mathf.Sin(i);
			line.SetVertexCount(vertCount++);
			line.SetPosition(vertCount - 2, new Vector3(x, h, y));
		}
		line.SetVertexCount(vertCount++);
		x = radius * Mathf.Cos(0);
		y = radius * Mathf.Sin(0);
		line.SetPosition(vertCount - 2, new Vector3(x, h, y));
	}

	SunMesh CreateMesh()
	{
		GameObject obj = new GameObject(name + "-Mesh");
		obj.transform.parent = transform;
		SunMesh sunMesh = obj.AddComponent<SunMesh>();
		obj.transform.localScale = Vector3.one * 4;
		obj.transform.localPosition = Vector3.zero;
		sunMesh.Init();
		return sunMesh;
	}

	Island CreateIsland()
	{
		GameObject obj = new GameObject("Island-" + name + "-" + islandCount++);
		obj.transform.parent = transform;
		Island island = obj.AddComponent<Island>();
		island.seed = random.Next(10000000);
		island.Init();
		return island;
	}

	void Update()
	{
		transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 6f);
	}
}
