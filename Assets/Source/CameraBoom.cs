using UnityEngine;

public class CameraBoom : MonoBehaviour
{
	
	public GameObject attachedObject;
	Vector3 rotation;

	void Start()
	{
		rotation = transform.localEulerAngles;
	}

	void Update()
	{
		if (attachedObject!=null && (transform.parent==null || !transform.parent.Equals(attachedObject.transform)))
		{
			transform.parent = attachedObject.transform;
		}
		if (transform.parent!=null) transform.position = Vector3.Lerp(transform.position, transform.parent.position, 2f * Time.deltaTime);
		rotation += new Vector3(0,Input.GetAxis("Horizontal"),0)*Time.deltaTime*-55f;
		transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(rotation), 5f * Time.deltaTime);
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] hits;
			hits = Physics.RaycastAll(ray);
			foreach (RaycastHit hit in hits)
			{
				IslandMesh mesh = hit.collider.GetComponent<IslandMesh>();
				if (mesh!=null&&mesh.type!=IslandMesh.MeshType.Water) {
					GameObject obj = (GameObject)Instantiate(Resources.Load<GameObject>("P_Arrow"), hit.point + (Vector3.up * 0.3f), Quaternion.Euler(Vector3.down));
					obj.transform.parent = mesh.island.transform;
				}
			}
		}
	}

}
