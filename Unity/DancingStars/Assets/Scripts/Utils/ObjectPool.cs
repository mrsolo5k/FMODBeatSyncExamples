using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ObjectPool<T, K> {

	private List<K> storage;
	private List<K> inUse;
	private List<K> available;

	private UnityAction<K> onActivate;
	private UnityAction<K> onDeactivate;

	public K Get() {

		if (available.Count <= 0) {
			K ovject = inUse [0];
			available.Add (ovject);
			inUse.Remove (ovject);
		}

		if (available.Count > 0) {

			K ovject = available[0];
			available.Remove (ovject);
			inUse.Add (ovject);
		
			SetActive (ovject, true);
			onActivate (ovject);

			return ovject;
		}

		return default(K);
	}

	public void Discard(K ovject) {

		SetActive (ovject, false);
		onDeactivate (ovject);

		available.Add (ovject);
	}

	public ObjectPool() {
	}

	public ObjectPool(T prefab, int capacity) {
		storage = new List<K>();
		available = new List<K> ();
		inUse = new List<K> ();

		for (int i = 0; i < capacity; i++) {

			var ovject = Instantiate (prefab);

			SetActive (ovject, false);

			storage.Add (ovject);
			available.Add (ovject);
		}
	}

	public void SetOnActivate(UnityAction<K> onActivate) {
		this.onActivate = onActivate;
	}

	public void SetOnDeactivate(UnityAction<K> onDeactivate) {
		this.onDeactivate = onDeactivate;
	}

	public abstract K Instantiate (T prefab);

	public abstract void SetActive(K ovject, bool active);

	// TODO: resizable capacity
}
