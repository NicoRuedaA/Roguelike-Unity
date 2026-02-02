using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
	public class SpawnRandomRoom : MonoBehaviour
	{
		[SerializeField] private GameObject prefab;

		private void Start()
		{

			Instantiate(prefab, transform.position, transform.rotation, transform);
		}

	}

}

