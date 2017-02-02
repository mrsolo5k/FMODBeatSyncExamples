using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : ObjectPool<GameObject, GameObject> {

	public override void SetActive(GameObject ovject, bool active) {

		ovject.SetActive (active);

	}

	public override GameObject Instantiate(GameObject prefab) {

		return GameObject.Instantiate (prefab);

	}
		
	public GameObjectPool(GameObject prefab, int number) : base(prefab, number) {

	}
}
