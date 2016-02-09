using UnityEngine;

public class House : MonoBehaviour
{

	public Island island;
	public HouseMesh roofMesh;
	public HouseMesh baseMesh;

	public void Init()
	{
		RecalculatePosition();
		roofMesh = CreateHouseMesh(HouseMesh.MeshType.Roof);
		baseMesh = CreateHouseMesh(HouseMesh.MeshType.Base);
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		BoxCollider collider = gameObject.AddComponent<BoxCollider>();
		collider.size = new Vector3(0.2f,0.3f,0.2f)*1f;
		collider.center = new Vector3(0, -0.3f, 0);
	}

	HouseMesh CreateHouseMesh(HouseMesh.MeshType type)
	{
		GameObject gameObject = new GameObject(name + "-" + type);
		HouseMesh houseMesh = gameObject.AddComponent<HouseMesh>();
		houseMesh.transform.parent = transform;
		houseMesh.island = island;
		houseMesh.type = type;
		houseMesh.Init();
		return houseMesh;
	}

	public void RecalculatePosition()
	{
		transform.localPosition = new Vector3((float)(island.random.NextDouble() - 0.5) * (island.size / 1.5f), 9, (float)(island.random.NextDouble() - 0.5) * (island.size / 1.5f));
		Ray ray = new Ray(transform.position, Vector3.down);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f))
		{
			IslandMesh islandMesh = hit.collider.gameObject.GetComponent<IslandMesh>();
			if (islandMesh != null)
				if (hit.normal.y < 0.5f || islandMesh.type == IslandMesh.MeshType.Water) RecalculatePosition();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		IslandMesh mesh = collision.collider.GetComponent<IslandMesh>();
		if (mesh != null && mesh.type == IslandMesh.MeshType.Top)
		{
			if (collision.contacts[0].normal.y < 0.5f) RecalculatePosition();
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		IslandMesh mesh = collider.GetComponent<IslandMesh>();
		if (mesh != null && mesh.type == IslandMesh.MeshType.Water)
		{
			RecalculatePosition();
		}
	}

}