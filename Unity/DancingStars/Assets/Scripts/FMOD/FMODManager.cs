using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Text;
using System.Runtime.InteropServices;

public class FMODManager {

	Dictionary<string, FMODSyncEvent> eventInstances;

	private static FMODManager instance;

	public static FMODManager Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new FMODManager();
			}
			return instance;
		}
	}

	private FMODManager()
	{
		eventInstances = new Dictionary<string, FMODSyncEvent> ();
	}

	public FMODSyncEvent TriggerEvent(string eventPath)
	{
		var evnt = new FMODSyncEvent (eventPath);

		eventInstances.Add (eventPath, evnt);

		return evnt;
	}

	public FMODSyncEvent GetEvent(string eventPath)
	{
		FMODSyncEvent evnt = null;
		eventInstances.TryGetValue (eventPath, out evnt);

		return evnt;
	}

}
