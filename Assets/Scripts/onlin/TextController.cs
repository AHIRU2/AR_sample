using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
	void Start()
	{
		transform.localScale = new Vector3(-1, 1, 1);
	}

	void Update()
	{
		transform.LookAt(Camera.main.transform);
	}
}
