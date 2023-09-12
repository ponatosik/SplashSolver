using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesctroyOnTouch : MonoBehaviour
{
	[SerializeField]
	private float mouseDistance = 1f;

	void FixedUpdate()
	{
		if (Input.GetMouseButton(0)) 
		{
			Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if(Vector2.Distance(transform.position, mouseWorldPosition) <= mouseDistance)
			{
				Destroy(gameObject);
			}
		}
	}
}
