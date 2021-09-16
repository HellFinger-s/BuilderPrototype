using UnityEngine;
using System;
using UnityEditor;
[CustomEditor(typeof(TileToolStyle))]
[System.Serializable]
public class TileToolStyleEditor: Editor {
	public void OnEnable(){	
		var target_cs = (TileToolStyle)target;
        FindAndSetTileValues();
		if (GUI.changed){
			EditorUtility.SetDirty(target_cs);
		}
	}	
    public override void OnInspectorGUI() {
    	DrawDefaultInspector();
	}
	public void FindAndSetTileValues(){
	// Fills in values that are missing, only happens in editor
		var target_cs = (TileToolStyle)target;
        if(target_cs.style == "" || target_cs.objectName == ""){
			string[] words = target_cs.gameObject.name.Split('_');
			target_cs.style = words[0];
			target_cs.objectName = target_cs.gameObject.name.Replace(target_cs.style, "");
		}
	}
}