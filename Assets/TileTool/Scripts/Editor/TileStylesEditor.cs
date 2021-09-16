using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileStyles))]

[System.Serializable]
public class TileStylesEditor: Editor {
    public override void OnInspectorGUI() {
    	DrawDefaultInspector();
		GUILayout.Label("To drag multiple prefabs to an array:\nLock the inspector by clicking the lock icon\nin the top right corner of the inspector", EditorStyles.miniBoldLabel);
	}
}	