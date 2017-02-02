using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePool : ObjectPool<Sprite, GameObject> {

	public override void SetActive(GameObject sprite, bool active) {

		sprite.SetActive (active);

	}
		
	public override GameObject Instantiate(Sprite prefab) {

		var ovject = new GameObject ();

		ovject.AddComponent<SpriteRenderer> ();
		ovject.GetComponent<SpriteRenderer> ().sprite = prefab;

		return ovject;

	}
		
	public SpritePool(Sprite sprite, int number) : base(sprite, number) {
		
	}

}
