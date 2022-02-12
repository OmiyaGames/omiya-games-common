using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmiyaGames.Common.Runtime.Tests
{
	public class TestSerializables : MonoBehaviour
	{
		[SerializeField]
		SerializableHashSet<string> hashSet = new SerializableHashSet<string>();
		[SerializeField]
		SerializableListSet<string> listSet = new SerializableListSet<string>();
		[SerializeField]
		RandomList<string> randomList = new RandomList<string>();

		void Start()
		{
			Debug.Log("=> Logging HashSet", this);
			foreach (var item in hashSet)
			{
				Debug.Log(item, this);
			}

			Debug.Log("=> Logging ListSet", this);
			foreach (var item in listSet)
			{
				Debug.Log(item, this);
			}

			Debug.Log("=> Logging RandomList", this);
			foreach (var item in randomList)
			{
				Debug.Log(item, this);
			}
		}
	}
}
