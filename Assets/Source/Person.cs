using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Person : MonoBehaviour {

	Arrow arrow { get { return Object.FindObjectOfType<Arrow>();} }

	void Start () {
	
	}

	void Update () {
		if (arrow != null)
		{
			transform.position = Vector3.Slerp(transform.position,arrow.transform.position,2f*Time.deltaTime);
		}
	}

}
