using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Text;
using System.Runtime.InteropServices;

public class FMODSyncEvent
{

	FMOD.Studio.EventInstance eventInstance;
	FMOD.Studio.EVENT_CALLBACK eventCallback;

	List<UnityAction> beatCallbacks = new List<UnityAction>();
	Dictionary<string, List<UnityAction>> markerCallbacks = new Dictionary<string, List<UnityAction>> ();

	public FMODSyncEvent(string eventPath)
	{
		eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventPath);
		eventCallback = new FMOD.Studio.EVENT_CALLBACK(StudioEventCallback);

		eventInstance.setCallback(eventCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT);
		eventInstance.start();
	}

	public FMOD.RESULT StudioEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameters)
	{
		if (type == FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
		{
			var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameters, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));

			List<UnityAction> toCall;
			var markerName = parameter.name + "";
			var succes = markerCallbacks.TryGetValue (markerName, out toCall);

			if(succes)
			{
				foreach (var action in toCall)
				{
					action ();
					UnityEngine.Debug.Log("Reached marker: " + markerName);
				}
			}
		}
		if (type == FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
		{
			foreach (var action in beatCallbacks) {
				action ();
			}
		}
		return FMOD.RESULT.OK;
	}

	public void RegisterCallback(UnityAction action)
	{
		beatCallbacks.Add (action);
	}

	public void RegisterCallback(string key, UnityAction action)
	{
		if (!markerCallbacks.ContainsKey(key))
		{
			markerCallbacks [key] = new List<UnityAction> ();
		}

		markerCallbacks [key].Add (action);
	}

	public void UnregisterCallback(UnityAction action)
	{
		foreach (var callbacks in markerCallbacks.Values)
		{
			callbacks.Remove (action);
		}
		beatCallbacks.Remove (action);
	}
}
