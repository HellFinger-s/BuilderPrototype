/****************************************
	TileTool.js v1.2
	Copyright 2015 Unluck Software	
 	www.chemicalbliss.com
*****************************************/
#pragma warning disable 0618
using UnityEngine;
using System;
using UnityEditor;

[System.Serializable]
public class TileTool: EditorWindow {
	public Vector3 prevPosition;
	public bool doSnap = false;
	public float snapValue = 0.5f;
	public bool _warned = false;
	public GameObject _replaceObject;
	public GameObject _styleGO;
	public Transform[] _selection;
	public int _autoSnapMax = 50;
	public TileStyles _style;
	public int _tileCycleCounter = 0;
	public int _index = -1;
	public string[] styleNames;
	public TileStyles[] styles;
	public bool _toggleStyle = false;
	public bool _toggleReplace = false;
	public bool _toggleRemove = false;
	public bool _toggleSnap = false;
	public bool _toggleGroup = false;
	public bool _toggleMove = false;	
	public bool _toggleHelpStyles = false;
	public bool _toggleHelpSides = false;
	public bool _toggleHelpMove = false;
	public bool _toggleHelpGroups = false;
	public bool _toggleHelpReplace = false;
	public bool _toggleHelpSnap = false;
	public float _tileSize = 1f;


	[MenuItem("Window/TileTool")]
	public static void ShowWindow() {
		EditorWindow win = EditorWindow.GetWindow(typeof(TileTool));
		win.titleContent = new GUIContent("TileTool");
		win.minSize = new Vector2(200.0f, 300.0f);
		win.maxSize = new Vector2(200.0f, 1000.0f);
	}


	public void ToggleAll() {
		_toggleMove = _toggleStyle = _toggleReplace = _toggleRemove = _toggleSnap = _toggleGroup = true;
	}

	public void OnEnable() {
		RefreshStyles();
	}
	
	public void OnLostFocus() {
		_toggleHelpSnap = _toggleHelpReplace = _toggleHelpGroups = _toggleHelpStyles = _toggleHelpSides = _toggleHelpMove = false;
	}
	
	public void OnGUI() {
		GUIStyle _miniButtonStyle = null;
		_miniButtonStyle = new GUIStyle(GUI.skin.button);
		_miniButtonStyle.fixedWidth = 24.0f;
		_miniButtonStyle.fixedHeight = 24.0f;
		_miniButtonStyle.fontSize = 12;
		_miniButtonStyle.margin = new RectOffset(3, 3, 3, 3);		
		GUIStyle _mediumButtonStyle = null;
		_mediumButtonStyle = new GUIStyle(GUI.skin.button);
		_mediumButtonStyle.fixedWidth = 81.0f;
		_mediumButtonStyle.fixedHeight = 24.0f;
		_mediumButtonStyle.fontSize = 9;
		_mediumButtonStyle.margin = new RectOffset(3, 3, 3, 3);	
		GUIStyle _bigButtonStyle = null;
		_bigButtonStyle = new GUIStyle(GUI.skin.button);
		_bigButtonStyle.fixedWidth = 174.0f;
		_bigButtonStyle.fixedHeight = 24.0f;
		_bigButtonStyle.fontSize = 9;
		_bigButtonStyle.margin = new RectOffset(3, 3, 3, 3);	
		GUIStyle _mainBox = null;
		_mainBox = new GUIStyle(GUI.skin.customStyles[0]);
		_mainBox.fixedWidth = 200.0f;	
		GUIStyle _helpStyle = null;
		_helpStyle = new GUIStyle(GUI.skin.label);
		_helpStyle.fontSize = 9;		
		Color32 _color1 = new Color32((byte)100, (byte)255, (byte)255, (byte)255);
		Color32 _color2 = new Color32((byte)250, (byte)255, (byte)255, (byte)255);
		Color32 _color4 = new Color32((byte)255, (byte)255, (byte)200, (byte)255);
		Color32 _color3 = new Color32((byte)255, (byte)255, (byte)100, (byte)255);	
		GUILayout.BeginVertical(_mainBox);
		EditorGUILayout.Space();
		if (_toggleStyle) 	GUI.color = _color1;
		else 				GUI.color = _color2;
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Tile Styles", EditorStyles.toolbarButton, GUILayout.Width(180.0f))) {
			_toggleStyle = !_toggleStyle;
			_toggleHelpStyles = false;
		}	
		GUI.color = _color3;
		if (GUILayout.Button("?", EditorStyles.toolbarButton, GUILayout.Width(20.0f))) {
			_toggleHelpStyles = !_toggleHelpStyles;
			_toggleStyle = true;
		}
		EditorGUILayout.EndHorizontal();	
		if (_toggleStyle) {
			EditorGUILayout.Space();
			if(_toggleHelpStyles){
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.TextArea("Replace style of tiles and objects \nCycle trough objects in seleced style \nRotate tiles", _helpStyle);
				EditorGUILayout.EndVertical();
			}
			GUI.color = _color4;
			if (_index >= 0) {
				_index = EditorGUILayout.Popup(_index, styleNames);
				_style = styles[_index];
			}		
			GUI.color = _color3;		
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Replace All", _bigButtonStyle)) {
				ReplaceStyles("All");
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.color = _color4;
			if (GUILayout.Button("Tiles",	_mediumButtonStyle)) {
				ReplaceStyles("Tile");
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Objects",	_mediumButtonStyle)) {
				ReplaceStyles("Object");
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUI.color = _color3;
			if (GUILayout.Button("Prev Tile", EditorStyles.miniButtonLeft)) {
				CycleGameObjects(false);
			}
			GUI.color = _color4;
			if (GUILayout.Button("Rotate", EditorStyles.miniButtonMid)) {
				RotateGameObjects();
			}
			GUI.color = _color3;
			if (GUILayout.Button("Next Tile", EditorStyles.miniButtonRight)) {
				CycleGameObjects(true);
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Space();
		if (_toggleMove) 	GUI.color = _color1;
		else 				GUI.color = _color2;
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Move & Duplicate", EditorStyles.toolbarButton, GUILayout.Width(180.0f))) {
			_toggleMove = !_toggleMove;
			_toggleHelpMove = false;
		}
		GUI.color = _color3;
		if (GUILayout.Button("?", EditorStyles.toolbarButton, GUILayout.Width(20.0f))) {
			_toggleHelpMove = !_toggleHelpMove;
			_toggleMove = true;
		}
		EditorGUILayout.EndHorizontal();
		if (_toggleMove) {
			EditorGUILayout.Space();
			if(_toggleHelpMove){
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.TextArea("Blue arrows moves tiles \nYellow arrows duplicates tiles", _helpStyle);
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.BeginVertical();
			GUI.color = _color2;
			_tileSize = EditorGUILayout.FloatField("TileSize", _tileSize);
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			

			if (GUILayout.Button("△", _miniButtonStyle)) {
				Move(new Vector3(0.0f, 0.0f, -_tileSize), false);
			}
			GUI.color = _color1;
			Event e = Event.current;
			if (GUILayout.Button("▲", _miniButtonStyle))  {
				Move(new Vector3(0.0f, _tileSize, 0.0f), false);
			}
			GUI.color = _color2;
			if (GUILayout.Button("▽", _miniButtonStyle)) {
				Move(new Vector3(0.0f, 0.0f, _tileSize), false);
			}
			GUILayout.FlexibleSpace();
			GUI.color = _color4;
			if (GUILayout.Button("△", _miniButtonStyle)) {
				Move(new Vector3(0.0f, 0.0f, -_tileSize), true);
			}
			GUI.color = _color3;
			if (GUILayout.Button("▲", _miniButtonStyle) || e.character == 'k') {
				Move(new Vector3(0.0f, _tileSize, 0.0f), true);
			}
			GUI.color = _color4;
			if (GUILayout.Button("▽", _miniButtonStyle)) {
				Move(new Vector3(0.0f, 0.0f, _tileSize), true);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.color = _color1;
			if (GUILayout.Button("◄", _miniButtonStyle)) {
				Move(new Vector3(_tileSize, 0.0f, 0.0f), false);
			}
			if (GUILayout.Button("▼", _miniButtonStyle)) {
				Move(new Vector3(0.0f, -_tileSize, 0.0f), false);
			}
			if (GUILayout.Button("►", _miniButtonStyle)) {
				Move(new Vector3(-_tileSize, 0.0f, 0.0f), false);
			}
			GUILayout.FlexibleSpace();
			GUI.color = _color3;
			if (GUILayout.Button("◄", _miniButtonStyle)) {
				Move(new Vector3(_tileSize, 0.0f, 0.0f), true);
			}
			if (GUILayout.Button("▼", _miniButtonStyle)) {
				Move(new Vector3(0.0f, -_tileSize, 0.0f), true);
			}
			if (GUILayout.Button("►", _miniButtonStyle)) {
				Move(new Vector3(-_tileSize, 0.0f, 0.0f), true);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			
		}
		EditorGUILayout.Space();
		if (_toggleRemove) 	GUI.color = _color1;
		else 				GUI.color = _color2;
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Remove Hidden Sides", EditorStyles.toolbarButton, GUILayout.Width(180.0f))) {
			_toggleRemove = !_toggleRemove;
			_toggleHelpSides = false;
		}
		GUI.color = _color3;
		if (GUILayout.Button("?", EditorStyles.toolbarButton, GUILayout.Width(20.0f))) {
			_toggleHelpSides = !_toggleHelpSides;
			_toggleRemove = true;
		}
		EditorGUILayout.EndHorizontal();
		if (_toggleRemove) {			
			EditorGUILayout.Space();
			if(_toggleHelpSides){
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.TextArea("Edit meshes to remove geometry \n\nArrows manually remove side \nAuto Destroy remove blocked sides\nReset Sides revert to prefab", _helpStyle);
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.BeginVertical();
			GUI.color = _color1;
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			if (GUILayout.Button("△", _miniButtonStyle)) {
				removey("back");
			}
			GUI.color = _color2;
			if (GUILayout.Button("▲", _miniButtonStyle)) {
				removey("up");
			}
			GUI.color = _color1;
			if (GUILayout.Button("▽", _miniButtonStyle)) {
				removey("front");
			}
			GUILayout.FlexibleSpace();
			GUI.color = _color3;
			if (GUILayout.Button("Auto Destroy", _mediumButtonStyle)) {
				if (Selection.activeTransform.GetComponent(typeof(TileToolGroup)) != null) {
					Transform[] glist = null;
					glist = new Transform[Selection.activeTransform.childCount];
					Transform o = Selection.activeTransform;
					for(int k = 0; k < Selection.activeTransform.childCount; k++) {
						glist[k] = o.GetChild(k);
					}
					_selection = glist;
				} else {
					_selection = Selection.transforms; //Add selection to array
				}
				Undo.RecordObjects(_selection, "TileTool: Auto Destroy Sides");
				for(int j = 0; j < _selection.Length; j++) {
					float pp = (float)j;
					EditorUtility.DisplayProgressBar("TileTool: Auto Destroy Sides", "", (pp + _selection.Length) / (_selection.Length * 2));
					AutoRemoveSides(_selection[j].gameObject);
				}
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.color = _color2;
			if (GUILayout.Button("◄", _miniButtonStyle)) {
				removey("left");
			}
			if (GUILayout.Button("▼", _miniButtonStyle)) {
				removey("down");
			}
			if (GUILayout.Button("►", _miniButtonStyle)) {
				removey("right");
			}
			GUILayout.FlexibleSpace();
			GUI.color = _color4;	
			if (GUILayout.Button("Reset Sides", _mediumButtonStyle)) {
				ResetToPrefab();
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
			GUI.color = Color.white;

		}
		EditorGUILayout.Space();
		if (_toggleGroup) 	GUI.color = _color1;
		else 				GUI.color = _color2;
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Grouping", EditorStyles.toolbarButton, GUILayout.Width(180.0f))) {
			_toggleGroup = !_toggleGroup;
			_toggleHelpGroups = false;
		}
		GUI.color = _color3;
		if (GUILayout.Button("?", EditorStyles.toolbarButton, GUILayout.Width(20.0f))) {
			_toggleHelpGroups = !_toggleHelpGroups;
			_toggleGroup = true;
		}
		EditorGUILayout.EndHorizontal();	
		if (_toggleGroup) {
			EditorGUILayout.Space();
			if(_toggleHelpGroups){
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.TextArea("Group selected tiles or objects", _helpStyle);
				EditorGUILayout.EndVertical();
			}		
			GUI.color = _color3;
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Group", _mediumButtonStyle)) {
				Group();
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("UnGroup", _mediumButtonStyle)) {
				UnGroup();
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		//	EditorGUILayout.LabelField("Undo not supported for grouping", EditorStyles.miniLabel);
		}
		EditorGUILayout.Space();
		if (_toggleReplace) 	GUI.color = _color1;
		else 				GUI.color = _color2;		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Replace GameObjects", EditorStyles.toolbarButton, GUILayout.Width(180.0f))) {
			_toggleReplace = !_toggleReplace;
			_toggleHelpReplace = false;
		}
		GUI.color = _color3;
		if (GUILayout.Button("?", EditorStyles.toolbarButton, GUILayout.Width(20.0f))) {
			_toggleHelpReplace = !_toggleHelpReplace;
			_toggleReplace = true;
		}
		EditorGUILayout.EndHorizontal();
		if (_toggleReplace) {
			EditorGUILayout.Space();
			if(_toggleHelpReplace){
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.TextArea("Replace selected objects in scene\nwith prefab", _helpStyle);
				EditorGUILayout.EndVertical();
			}
			GUI.color = _color4;	
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			_replaceObject = (GameObject)EditorGUILayout.ObjectField(_replaceObject, typeof(GameObject), false,  GUILayout.Width(81.0f));
			GUI.color = _color3;
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Replace", _mediumButtonStyle)) {
				if (_replaceObject != null) {
					ReplaceGameObjects();
				} else {
					Debug.LogError("No Prefab assigned");
				}
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Space();
		if (_toggleSnap) 	GUI.color = _color1;
		else 				GUI.color = _color2;		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Snapping", EditorStyles.toolbarButton, GUILayout.Width(180.0f))) {
			_toggleSnap = !_toggleSnap;
			_toggleHelpSnap = false;
		}
		GUI.color = _color3;
		if (GUILayout.Button("?", EditorStyles.toolbarButton, GUILayout.Width(20.0f))) {
			_toggleHelpSnap = !_toggleHelpSnap;
			_toggleSnap = true;
		}
		EditorGUILayout.EndHorizontal();
		if (_toggleSnap) {
			EditorGUILayout.Space();
			if(_toggleHelpSnap){
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.TextArea("Snap gameobject position\nAutosnap only works with tiles", _helpStyle);
				EditorGUILayout.EndVertical();
			}
			GUI.color = _color4;	
			if (Selection.transforms.Length < _autoSnapMax) doSnap = EditorGUILayout.Toggle("Autosnap", doSnap);
			else doSnap = EditorGUILayout.Toggle("Autosnap (disabled)", doSnap);
			snapValue = EditorGUILayout.FloatField("Value", snapValue);
			_autoSnapMax = EditorGUILayout.IntField("Max Autosnap", _autoSnapMax);
			EditorGUILayout.Space();
			GUI.color = _color3;
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Snap", _bigButtonStyle)) {
				snap(false);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();		
		}
		//EditorGUILayout.LabelField("Selected Objects: " + Selection.transforms.Length, EditorStyles.miniLabel);
		GUILayout.EndVertical();
	}

	public void OnInspectorUpdate() {
		Repaint();
		EditorUtility.ClearProgressBar();
		if (_index < 0) {
			this.RefreshStyles();
		}
	}
	
	public void RefreshStyles() {
		Resources.LoadAll("");
		//styles = (TileStyles[])new Resources.FindObjectsOfTypeAll(typeof(TileStyles));

		styles = Resources.FindObjectsOfTypeAll(typeof(TileStyles)) as TileStyles[];


		styleNames = new String[styles.Length];
		for(int i = 0; i < styleNames.Length; i++) {
			styleNames[i] = styles[i].name;
			_index = 0;
		}
	}
	
	public void AutoRemoveSidesInit(GameObject gameObj) {
		TileToolTile TTT = gameObj.GetComponent<TileToolTile>();
		if (TTT != null) {
			TTT.Init();
		}
	}

	public void AutoRemoveSides(GameObject gameObj) {
		TileToolTile TTT = gameObj.GetComponent<TileToolTile>();
		if (TTT != null) {
			Undo.RegisterCompleteObjectUndo(gameObj, "TileTool: Auto Remove Side");
			TTT.DetectTiles();
		}
	}

	public void ManualRemoveSide(Transform t,string side) {
		TileToolTile TTT = (TileToolTile)t.GetComponent(typeof(TileToolTile));
		if (TTT != null) {
			Undo.RegisterCompleteObjectUndo(t.gameObject, "TileTool: Manual Remove Side "+side);
			TTT.RemoveSide((MeshFilter)t.Find("Model").GetComponent(typeof(MeshFilter)), side, t.gameObject);
		}
	}

	public void SetActiveColliders(GameObject go,bool a) {
		Collider[] colliders = null;
		colliders = go.GetComponentsInChildren<Collider>();
		foreach(Collider collider in colliders) {
			collider.enabled = a;
		}
	}

	public GameObject[] CombineVector3Arrays(GameObject[] array1,GameObject[] array2) {
		GameObject[] array3 = new GameObject[array1.Length + array2.Length];
		System.Array.Copy(array1, array3, array1.Length);
		System.Array.Copy(array2, 0, array3, array1.Length, array2.Length);
		return array3;
	}

	public void ReplaceStyles(string type) {
		if (Selection.transforms.Length > 0 && (_style != null)) { //Check if style is assigned
			GameObject[] styleObjects = null;
			if (type == "Tile") styleObjects = _style._tiles; //CombineVector3Arrays(_style._tiles, _style._objects);
			else if (type == "Object") styleObjects = _style._objects;
			else if (type == "All") styleObjects = CombineVector3Arrays(_style._tiles, _style._objects);
			if (Selection.activeTransform.GetComponent(typeof(TileToolGroup)) != null) {
				Transform[] glist = null;
				glist = new Transform[Selection.activeTransform.childCount];
				Transform o = Selection.activeTransform;
				for(int i = 0; i < Selection.activeTransform.childCount; i++) {
					glist[i] = o.GetChild(i);
				}
				_selection = glist;
			} else {
				_selection = Selection.transforms; //Add selection to array
			}
			//     var olist:GameObject[];
			//   olist = new GameObject[_selection.length];
			for(int i = 0; i < _selection.Length; i++) {
				float p = (float)i;
				EditorUtility.DisplayProgressBar("TileTool: Replacing GameObjects", "Any removed sides will be reset", p / _selection.Length);
				//olist[i] = _selection[i].gameObject;
				TileToolStyle TTO = (TileToolStyle)_selection[i].GetComponent(typeof(TileToolStyle));
				if ((TTO != null) && TTO.objectName != "") {
					string s = TTO.objectName;
					for(int j = 0; j < styleObjects.Length; j++) {
						TileToolStyle newTTO = styleObjects[j].GetComponent<TileToolStyle>();
						if (newTTO != null) {
							string newObjectTile = newTTO.objectName;
							if (s == newObjectTile) {
								GameObject newObject = null;
								newObject = (GameObject)PrefabUtility.InstantiatePrefab(styleObjects[j]);
								Transform newT = newObject.transform;
								newT.position = _selection[i].position;
								newT.rotation = _selection[i].rotation;
								newT.localScale = _selection[i].localScale;
								//              olist[i] = newObject;
								newT.parent = _selection[i].transform.parent;
								Undo.RegisterCreatedObjectUndo(newObject, "TileTool: Replace Styles");
								Undo.DestroyObjectImmediate(_selection[i].gameObject);
								break;
							}
						}
					}
				}
			}
			//if(!Selection.activeTransform.GetComponent(TileToolGroup)){
			//      	Selection.objects = olist;
			//}
		}
	}

	public void ReplaceGameObjects() {
		_selection = Selection.transforms;
		for(int i = 0; i < _selection.Length; i++) {
			GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(_replaceObject);
			Transform newT = newObject.transform;
			newT.position = _selection[i].position;
			newT.rotation = _selection[i].rotation;
			newT.localScale = _selection[i].localScale;
			newT.parent = _selection[i].transform.parent;
			Undo.RegisterCreatedObjectUndo(newObject, "TileTool: Replace Styles");
			Undo.DestroyObjectImmediate(_selection[i].gameObject);
		}
	}

	public void RotateGameObjects() {
		_selection = Selection.transforms;
		Undo.RecordObjects(_selection, "TileTool: Rotate");
		for(int i = 0; i < _selection.Length; i++) {
			_selection[i].Rotate(_selection[i].transform.up * 90);
		}
	}

	public void CycleGameObjects(bool next) {
		if (_style != null) {
			_selection = Selection.transforms;
			if (next) this._tileCycleCounter++;
			else this._tileCycleCounter--;
			if (_tileCycleCounter >= _style._tiles.Length) _tileCycleCounter = 0;
			else if (_tileCycleCounter < 0) _tileCycleCounter = _style._tiles.Length - 1;
			GameObject[] s = null;
			s = new GameObject[_selection.Length];
			for(int i = 0; i < _selection.Length; i++) {
				TileToolTile TTT = (TileToolTile)_selection[i].GetComponent(typeof(TileToolTile));
				if (TTT != null) {
					GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(_style._tiles[_tileCycleCounter]);
					Transform newT = newObject.transform;
					newT.position = _selection[i].position;
					newT.rotation = _selection[i].rotation;
					newT.localScale = _selection[i].localScale;
					newT.parent = _selection[i].transform.parent;
					Undo.RegisterCreatedObjectUndo(newObject, "TileTool: Cycle Tiles");
					Undo.DestroyObjectImmediate(_selection[i].gameObject);
					s[i] = newObject;
				}
			}
			Selection.objects = s;
		} else {
			Debug.Log("No Style");
		}
	}

	public void Update() {
		if (Selection.transforms.Length < _autoSnapMax && Selection.transforms.Length > 0 && !EditorApplication.isPlaying && doSnap && Selection.transforms[0].position != prevPosition) snap(true);
	}

	public void snap(bool onlyTiles) {
		_selection = Selection.transforms;
		try {
			for(int i = 0; i < Selection.transforms.Length; i++) {
				TileToolTile TTT = (TileToolTile)_selection[i].GetComponent(typeof(TileToolTile));
				if (onlyTiles && (TTT != null) || !onlyTiles) {
					if (!onlyTiles) {
						Undo.RecordObjects(_selection, "TileTool: Snapping");
					}
					Vector3 t = Selection.transforms[i].transform.position;
					t.x = round(t.x);
					t.y = round(t.y);
					t.z = round(t.z);
					Selection.transforms[i].transform.position = t;
				}
			}
			prevPosition = Selection.transforms[0].position;
		} catch (System.Exception e) {
			Debug.LogError("Nothing to move.  " + e);
		}
	}

	public float round(float input) {
		float snappedValue = 0.0f;
		snappedValue = snapValue * Mathf.Round((input / snapValue));
		return (snappedValue);
	}

	public void Group() {
		_selection = Selection.transforms;
		int sl = _selection.Length;
		if (_selection.Length > 0) {
			GameObject newGo = new GameObject();
			newGo.name = "_TileTool Group";
			newGo.AddComponent<TileToolGroup>();
			for(int i = 0; i < _selection.Length; i++) {
				float p = (float)i;
				EditorUtility.DisplayProgressBar("TileTool: Grouping" + p + "/" + sl, "", p / sl);
				if ((_selection[i].transform.parent == null) || (_selection[i].transform.parent.GetComponent(typeof(TileToolGroup)) != null)) {
					_selection[i].gameObject.transform.parent = newGo.transform;
				}
			}
			if (newGo.transform.childCount == 0) {
				DestroyImmediate(newGo);
				Debug.LogWarning("Group Failed: Objects already grouped");
			}
		}
	}

	public void UnGroup() {
		_selection = Selection.transforms;
		int sl = _selection.Length;
		if (_selection.Length > 0) {
			for(int i = 0; i < _selection.Length; i++) {
				if ((_selection[i].transform.parent != null) && (_selection[i].transform.parent.GetComponent(typeof(TileToolGroup)) != null)) {
					float p = (float)i;
					EditorUtility.DisplayProgressBar("TileTool: UnGrouping " + p + "/" + sl, "", p / sl);
					_selection[i].gameObject.transform.parent = null;
				}
			}
		}
	}

	public void ResetToPrefab() {
		if (Selection.activeTransform.GetComponent(typeof(TileToolGroup)) != null) {
			Transform[] glist = null;
			glist = new Transform[Selection.activeTransform.childCount];
			Transform o = Selection.activeTransform;
			for(int k = 0; k < Selection.activeTransform.childCount; k++) {
				glist[k] = o.GetChild(k);
			}
			_selection = glist;
		} else {
			_selection = Selection.transforms; //Add selection to array
		}
		Undo.RecordObjects(_selection, "TileTool: Reset To Prefab");
		int sl = _selection.Length;
		if (_selection.Length > 0) {
			for(int i = 0; i < _selection.Length; i++) {
				float p = (float)i;
				EditorUtility.DisplayProgressBar("TileTool: Tile Reset", "", p / sl);
				TileToolTile TTT = (TileToolTile)_selection[i].GetComponent(typeof(TileToolTile));
				if (TTT != null) {
					Undo.RegisterCompleteObjectUndo(_selection[i].gameObject, "TileTool: Prefab Reset");
					Vector3 scale = _selection[i].localScale;
					PrefabUtility.RevertPrefabInstance(_selection[i].gameObject);
					_selection[i].localScale = scale;
				}
			}
		}
	}

	public void removey(string side) {
		if (Selection.activeTransform.GetComponent(typeof(TileToolGroup)) != null) {
			Transform[] glist = null;
			glist = new Transform[Selection.activeTransform.childCount];
			Transform o = Selection.activeTransform;
			for(int k = 0; k < Selection.activeTransform.childCount; k++) {
				glist[k] = o.GetChild(k);
			}
			_selection = glist;
		} else {
			_selection = Selection.transforms; //Add selection to array
		}
		for(int i = 0; i < _selection.Length; i++) {
			float p = (float)i;
			EditorUtility.DisplayProgressBar("TileTool: Removing Sides", "", p / _selection.Length);
			ManualRemoveSide(_selection[i].transform, side);
		}
	}
	
	public void Move(Vector3 dir,bool dupe) {	
		if(Selection.transforms.Length > 0){
			TileToolGroup TTG = (TileToolGroup)Selection.activeTransform.GetComponent(typeof(TileToolGroup));
			if (TTG != null) {
				Transform[] glist = null;
				glist = new Transform[Selection.activeTransform.childCount];
				Transform o = Selection.activeTransform;
				for(int k = 0; k < Selection.activeTransform.childCount; k++) {
					glist[k] = o.GetChild(k);
				}
				_selection = glist;
			} else {
				_selection = Selection.transforms;
			}
			GameObject[] newSelection = new GameObject[_selection.Length];
			for(int i = 0; i < _selection.Length; i++) {
				TileToolTile TTT = (TileToolTile)_selection[i].GetComponent(typeof(TileToolTile));			
				if (TTT != null) {	
					if (dupe) {
						UnityEngine.Object dupePrefab = PrefabUtility.GetPrefabParent(_selection[i].gameObject);
						GameObject dupeTarget = (GameObject)PrefabUtility.InstantiatePrefab(dupePrefab as GameObject) as GameObject;
						dupeTarget.transform.position = _selection[i].transform.position + dir;
						dupeTarget.transform.rotation = _selection[i].transform.rotation;
						dupeTarget.transform.parent = _selection[i].transform.parent;
						newSelection[i] = dupeTarget.gameObject;
						Undo.RegisterCreatedObjectUndo(dupeTarget.gameObject, "TileTool: Move & Duplicate");
					}else{
						Undo.RegisterCompleteObjectUndo(_selection[i].gameObject, "TileTool: Move & Duplicate");
						_selection[i].transform.position += dir;
					}
				}
			}
			if(dupe)
				Selection.objects = newSelection;	
		}
	}
}