using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUISound : MonoBehaviour
{
	private static GUISound guiSound;
	public static GUISound Instance
	{
		get
		{
			if (guiSound == null)
			{
				Debug.LogError("GameManager is Null.");
			}
			return guiSound;
		}
	}

/*	void static clickSound()
    {

    }*/
}
