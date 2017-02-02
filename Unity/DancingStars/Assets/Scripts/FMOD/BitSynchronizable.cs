using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BitSynchronizable : MonoBehaviour {

	void Awake() {
		//FMODManager.Instance.RegisterCallback (BaseOnBit);
	}

	public void BaseOnBit() {
		OnBit ();
	}
		
	public abstract void OnBit ();
}
