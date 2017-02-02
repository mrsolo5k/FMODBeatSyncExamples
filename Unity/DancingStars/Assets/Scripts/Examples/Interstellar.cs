using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interstellar : MonoBehaviour {

	public Sprite[] prefabs; 

	public Vector3 center;
	public float size;
	public int number = 128;

	private float timer;
	public float timerTreshold = 10.0f;

	public float speed = 1.0f;

	public Dictionary<Sprite, SpritePool> spritePools;

	[FMODUnity.EventRef]
	public string eventPath;

	Dictionary<GameObject, UnityAction> actionForObject = new Dictionary<GameObject, UnityAction>();

	void Awake () {

		spritePools = new Dictionary<Sprite, SpritePool> ();

		for (int i = 0; i < prefabs.Length; i++) {

			var newPool = new SpritePool (prefabs[i], number);

			newPool.SetOnActivate (OnActivate);
			newPool.SetOnDeactivate (OnDeactivate);

			spritePools.Add (prefabs [i], newPool);
		}

	}
		
	void Start() {
		var evnt = FMODManager.Instance.TriggerEvent (eventPath);

		GenerateStars (this.center);

		evnt.RegisterCallback("Marker", () => {
			Camera.main.backgroundColor = Color.white;
		});

		timer = 0.0f;
	}

	void Update () {

		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = Camera.main.orthographicSize * 2;

		Bounds bounds = new Bounds (
			Camera.main.transform.position,
			                new Vector3 (cameraHeight * screenAspect, cameraHeight, 0));

		Camera.main.transform.position += Camera.main.transform.right * Time.deltaTime * speed;

		if (timer <= 0.0f) {
			var oldZ = center.z;

			center = Camera.main.transform.position;

			center.z = oldZ;

			GenerateStars (bounds.center + Camera.main.transform.right * (bounds.extents.x + size));

			timer = timerTreshold;
		}

		Camera.main.backgroundColor = Color.Lerp (Camera.main.backgroundColor, Color.black, Time.deltaTime);

		timer -= Time.deltaTime;
	}

	void GenerateStars(Vector3 center) {

		for (int i = 0; i < number; i++) {
			var whichIdx = Random.Range (0, prefabs.Length);
			var which = prefabs [whichIdx];

			var ovject = (GameObject) spritePools [which].Get ();

			Vector3 position = center + Random.insideUnitSphere * size;

			ovject.transform.position = position;
		}

	}

	void OnActivate(GameObject ovject) {
		var action = GetOnActivateCallback (ovject);
		actionForObject[ovject] = action;

		var evnt = FMODManager.Instance.GetEvent (eventPath);

		if(evnt != null)
			evnt.RegisterCallback(action);
	}

	void OnDeactivate(GameObject ovject) {
		var action = GetOnActivateCallback (ovject);

		var evnt = FMODManager.Instance.GetEvent (eventPath);

		if(evnt != null)
			evnt.UnregisterCallback(action);
	}

	UnityAction GetOnActivateCallback(GameObject ovject)
	{
		return () => {
			if(ovject != null)
				ovject.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
		};
	}

}
