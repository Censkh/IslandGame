using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour
{

	static Quaternion rotation = Quaternion.identity;
	static int currentId = 0;
	int id;
	Vector3 startPos;
	bool up = true;

	void Start()
	{
		id = currentId++;
		startPos = transform.localPosition;
	}

	void Update()
	{
		if (id != currentId - 1)
		{
			transform.localScale = Vector3.Slerp(transform.localScale,Vector3.zero,8f*Time.deltaTime);
			if (transform.localScale.x < 0.1f)
			{
				Destroy(gameObject);
			}
		}
		else
		{
			transform.rotation = rotation;
			transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 55f);
			rotation = transform.rotation;
			float change = 0.2f;
			float dist = 0.01f;
			float speed = 8f;
			if (up)
			{
				transform.localPosition = Vector3.Slerp(transform.localPosition, startPos + new Vector3(0, change, 0), speed * Time.deltaTime);
				if (transform.localPosition.y > startPos.y + change - dist)
				{
					up = false;
				}
			}
			else
			{
				transform.localPosition = Vector3.Slerp(transform.localPosition, startPos, speed * Time.deltaTime);
				if (transform.localPosition.y < startPos.y + dist)
				{
					up = true;
				}
			}
		}
	}
}
