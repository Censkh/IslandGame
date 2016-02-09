using UnityEngine;
using System.Collections;

public class CloudMesh : CubeMesh
{

	Vector3 startScale;
	Vector3 vector;
	bool scaleUp;
	int bounce;
	float scaleSpeed;
	float startSpeed;

	public override void Init()
	{
		base.Init();
		transform.localPosition = Vector3.zero;
		RecalculatePosition();
		BuildMesh();
		startScale = transform.localScale = new Vector3(0.45f + ((float)random.NextDouble() * 0.1f), 0.45f + ((float)random.NextDouble() * 0.1f), 1f) * 0.4f;
		scaleSpeed = startSpeed = 0.5f+(float)(random.NextDouble()*0.5f);
	}

	protected override int GetSize()
	{
		return 4;
	}

	public void RecalculatePosition()
	{
		transform.localPosition = new Vector3((float)(random.NextDouble() - 0.5) * (island.size / 1.5f), 6.5f + ((float)random.NextDouble()*2f), (float)(random.NextDouble() - 0.5) * (island.size / 1.5f));
	}

	void OnMouseDown()
	{
		scaleSpeed=startSpeed*35;
	}

	public override void BuildMesh()
	{
		base.BuildMesh();
		transform.Rotate(new Vector3(0, random.Next(360), 0));
	}

	protected override Collider CreateCollider()
	{
		BoxCollider collider = gameObject.AddComponent<BoxCollider>();
		collider.size*=5f;
		collider.isTrigger = true;
		return collider;
	}

	protected override Vector3 TransformPoint(Face face, float x, float y)
	{
		Vector3 vector = base.TransformPoint(face,x,y);
		vector -= vector.normalized * GetHeightValue(vector, 80f) * 100f;
		return vector;
	}

	protected override bool IsRandomized()
	{
		return true;
	}

	void Update()
	{
		scaleSpeed = Mathf.Lerp(scaleSpeed,startSpeed,6f*Time.deltaTime);
		float scale = .1f;
		if (scaleUp)
		{
			transform.localScale = Vector3.Slerp(transform.localScale, startScale * (1f+scale), scaleSpeed * Time.deltaTime);
			if (transform.localScale.x > startScale.x * (1f+scale-0.01f))
				scaleUp = false;
		}
		else
		{
			transform.localScale = Vector3.Slerp(transform.localScale, startScale * (1f - scale), scaleSpeed * Time.deltaTime);
			if (transform.localScale.x < startScale.x *(1f-scale+0.01f))
				scaleUp = true;
		}
	}
}
