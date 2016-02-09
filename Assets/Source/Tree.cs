using UnityEngine;

public class Tree : MonoBehaviour
{

	public Island island;
	public TreeMesh leavesMesh;
	public TreeMesh logMesh;

	public void Init()
	{
		RecalculatePosition();
		leavesMesh = CreateTreeMesh(TreeMesh.MeshType.Leaves);
		logMesh = CreateTreeMesh(TreeMesh.MeshType.Log);
		Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		BoxCollider collider = gameObject.AddComponent<BoxCollider>();
		collider.size = collider.size / 15f;
		collider.size = new Vector3(collider.size.x, 1f, collider.size.z);
		collider.center = new Vector3(0, -0.5f, 0);
	}

	TreeMesh CreateTreeMesh(TreeMesh.MeshType type)
	{
		GameObject gameObject = new GameObject(name + "-" + type);
		TreeMesh treeMesh = gameObject.AddComponent<TreeMesh>();
		treeMesh.transform.parent = transform;
		treeMesh.island = island;
		treeMesh.type = type;
		treeMesh.Init();
		return treeMesh;
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