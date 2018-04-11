using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Cutscene))]

public class CutsceneEditor : Editor {

	Cutscene cutscene;
	SerializedProperty actionsProp;

	private static bool showActions = true;

	void OnEnable()
	{
		cutscene = (Cutscene)target;
		actionsProp = serializedObject.FindProperty("actions");
	}

	public override void OnInspectorGUI()
	{
		//EditorGUILayout.PropertyField(actionsProp);

		showActions = EditorGUILayout.Foldout(showActions, "Actions");
		if (showActions){
			int size = actionsProp.arraySize;
			for(int i = 0; i < size; i++){
				DrawActionInspector(cutscene.actions[i], i);
			}

			if(GUILayout.Button("Add Action"))
			{
				AddAction();
			}
		}

		if(GUI.changed)
		{
			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(target);
		}
	}

	void DrawActionInspector(SceneAction action, int index){
		if (action != null){
			action.gameObject.name = "Action " + index + ": " + action.actionType.ToString();
			SerializedObject serObj = new SerializedObject (action);
		
			SerializedProperty numProp = serObj.FindProperty ("value");

			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("X", GUILayout.Width (20))) {
				actionsProp.MoveArrayElement (index, cutscene.actions.Length - 1);
				actionsProp.arraySize--;
				DestroyImmediate (action.gameObject);
				return;
			}
			// Title of action
			EditorGUILayout.LabelField(action.gameObject.name, GUILayout.Width (180));
			// action type
			action.actionType = (SceneAction.Type)EditorGUILayout.EnumPopup(action.actionType);

			EditorGUILayout.EndHorizontal();

			// Draw the stats
			DrawInfo(action);
		}

	}

	void AddAction(){
		GameObject actionObject = new GameObject();
		actionObject.transform.parent = cutscene.transform;
		actionObject.transform.localPosition = Vector3.zero;
		SceneAction newAction = actionObject.AddComponent<SceneAction>();
				
		actionsProp.InsertArrayElementAtIndex(actionsProp.arraySize);
		actionsProp.GetArrayElementAtIndex(actionsProp.arraySize - 1).objectReferenceValue = newAction;
	}

	void DrawInfo_Dialogue(SceneAction action){
		
	}

	void DrawInfo(SceneAction action){
		if (action.actionType == SceneAction.Type.DialogueLine){
			action.dialogueLine = EditorGUILayout.TextArea(action.dialogueLine, GUILayout.ExpandHeight(true), GUILayout.MaxHeight(60));
		}
		else if (action.actionType == SceneAction.Type.Wait){
			action.waitTime = EditorGUILayout.FloatField("Wait Time", action.waitTime);
		}
		else if (action.actionType == SceneAction.Type.SetCameraTarget){
			action.cameraTarget = EditorGUILayout.Vector3Field("Camera Target", action.cameraTarget);
		}
	}

}
