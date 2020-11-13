using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstantiationController : MonoBehaviour
{
	public float instantiationRange = 10f;
	public GameObject[] objects;


	void Start()
	{
		for (int i = 0; i < objects.Length; i++)
		{
			Vector3 randomSpawnPosition = new Vector3(Random.Range(-instantiationRange, instantiationRange), Random.Range(-instantiationRange, instantiationRange), Random.Range(-instantiationRange, instantiationRange));
			Vector3 randomSpawnRotation = Vector3.up * Random.Range(0, 360);

			GameObject newObject = (GameObject)Instantiate(objects[i], randomSpawnPosition, Quaternion.Euler(randomSpawnRotation));
			newObject.transform.parent = transform;
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
