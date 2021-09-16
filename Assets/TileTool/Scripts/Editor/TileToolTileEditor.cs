using UnityEngine;
using System;
using UnityEditor;

[CustomEditor(typeof(TileToolTile))]

[System.Serializable]
public class TileToolTileEditor : Editor {
	public Texture tex;
	public void OnEnable() {
		tex = Resources.Load("TT_SideHelp") as Texture;
		TileToolTile target_cs=(TileToolTile)target;
		target_cs.FindAndSetTileValues();
		TileToolStyle TTS = target_cs.gameObject.GetComponent<TileToolStyle>();

		if (!TTS) {
			target_cs.gameObject.AddComponent<TileToolStyle>();
		}
		if (GUI.changed) {
			EditorUtility.SetDirty(target_cs);
		}
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
		GUILayout.Label(tex, titleStyle);
	}
}