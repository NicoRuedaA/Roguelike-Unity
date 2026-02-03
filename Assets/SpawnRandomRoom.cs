using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nicorueda
{
	public class SpawnRandomRoom : MonoBehaviour
	{
		[SerializeField] private GameObject[] rooms;
		[SerializeField] bool left = false;
		[SerializeField] bool right = false;
		private void Start()
		{
			if (left)
			{
				Instantiate(rooms[1], transform.position, Quaternion.identity);
			}
			else if (right)
			{
				Debug.Log("se instancia derecha");
				Instantiate(rooms[2], transform.position, Quaternion.identity);
			}
			else
			{
				Instantiate(rooms[0], transform.position, Quaternion.identity);
			}
		}

	}

}

