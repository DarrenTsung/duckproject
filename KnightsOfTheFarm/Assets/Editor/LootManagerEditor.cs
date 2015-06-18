using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(LootManager))]
public class LootManagerEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		LootManager myScript = (LootManager)target;
		if(GUILayout.Button("Load LootDatabase"))
		{
			myScript.LoadDatabase();
		}
	}
}
