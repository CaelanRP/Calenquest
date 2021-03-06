﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Cutscene))]

public class CutsceneEditor : Editor {

	Cutscene cutscene;
	SerializedProperty actionsProp;
	SerializedProperty unlockAtEnd;

	private static bool showActions = true;
	

	void OnEnable()
	{
		cutscene = (Cutscene)target;
		actionsProp = serializedObject.FindProperty("actions");
		unlockAtEnd = serializedObject.FindProperty("keepCameraLocked");
	}

	public override void OnInspectorGUI()
	{
		//EditorGUILayout.PropertyField(actionsProp);

		showActions = EditorGUILayout.Foldout(showActions, "Actions");
		if (showActions){
			int size = actionsProp.arraySize;
			if (size > 0){
				for(int i = 0; i < size; i++){
					if (cutscene.actions[i] == null){
						actionsProp.DeleteArrayElementAtIndex(i);
						continue;
					}
					DrawActionInspector(cutscene.actions[i], i);
					// Draw the new button
					if(GUILayout.Button("Add Action Here"))
					{
						AddAction(i + 1);
					}
				}
			}
			else{
				if(GUILayout.Button("Add Action"))
					{
						AddAction(0);
					}
			}
		}
		EditorGUILayout.PropertyField(unlockAtEnd);

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
			if (GUILayout.Button("^", GUILayout.Width (20))) {
				actionsProp.MoveArrayElement (index, index - 1);
			}
			if (GUILayout.Button("v", GUILayout.Width (20))) {
				actionsProp.MoveArrayElement (index, index + 1);
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

	void AddAction(int index){
		GameObject actionObject = new GameObject();
		actionObject.transform.parent = cutscene.transform;
		actionObject.transform.localPosition = Vector3.zero;
		SceneAction newAction = actionObject.AddComponent<SceneAction>();
				
		actionsProp.InsertArrayElementAtIndex(index);
		actionsProp.GetArrayElementAtIndex(index).objectReferenceValue = newAction;
	}

	void DrawInfo_Dialogue(SceneAction action){
		
	}

	void DrawInfo(SceneAction action){
		if (action.actionType == SceneAction.Type.DialogueLine){
			action.actionObject = EditorGUILayout.ObjectField("Speaker", action.actionObject, typeof(Speaker), true);
			action.actionString2 = EditorGUILayout.TextField("Speaker name (Override)", action.actionString2);
			action.actionString = EditorGUILayout.TextArea(action.actionString, GUILayout.ExpandHeight(true), GUILayout.MaxHeight(60));
		}
		else if (action.actionType == SceneAction.Type.Wait){
			action.actionFloat = EditorGUILayout.FloatField("Wait Time", action.actionFloat);
		}
		else if (action.actionType == SceneAction.Type.SetCameraTarget){
			action.actionVector3 = EditorGUILayout.Vector3Field("Camera Target", action.actionVector3);
			action.actionBool = EditorGUILayout.Toggle("track X", action.actionBool);
			action.actionBool2 = EditorGUILayout.Toggle("track Y", action.actionBool2);
			action.actionBool3 = EditorGUILayout.Toggle("snap", action.actionBool3);
		}
		else if (action.actionType == SceneAction.Type.UnlockCamera){
			action.actionBool = EditorGUILayout.Toggle("track X", action.actionBool);
			action.actionBool2 = EditorGUILayout.Toggle("track Y", action.actionBool2);
		}
		else if (action.actionType == SceneAction.Type.EnableObject){
			action.actionObject = EditorGUILayout.ObjectField("Object", action.actionObject, typeof(GameObject), true); 
			action.actionBool = EditorGUILayout.ToggleLeft("Enabled", action.actionBool);
		}
		else if (action.actionType == SceneAction.Type.InvokeMethod){
			action.actionObject = EditorGUILayout.ObjectField("Object", action.actionObject, typeof(MonoBehaviour), true); 
			action.actionString = EditorGUILayout.TextField("Method Name", action.actionString);
			action.actionBool = EditorGUILayout.ToggleLeft("Disable Dialogue", action.actionBool);
		}
		else if (action.actionType == SceneAction.Type.FlipObject){
			action.actionObject = EditorGUILayout.ObjectField("Object", action.actionObject, typeof(GameObject), true); 
		}
		else if (action.actionType == SceneAction.Type.TriggerAnimation){
			action.actionObject = EditorGUILayout.ObjectField("Animator", action.actionObject, typeof(Animator), true); 
			action.actionString = EditorGUILayout.TextField("Trigger Name", action.actionString);
		}
		else if (action.actionType == SceneAction.Type.PlaySound){
			action.actionObject = EditorGUILayout.ObjectField("Audio Clip", action.actionObject, typeof(AudioClip), true); 
		}
		else if (action.actionType == SceneAction.Type.Warp){
			action.actionVector3 = EditorGUILayout.Vector3Field("Target", action.actionVector3);
		}
	}

}
