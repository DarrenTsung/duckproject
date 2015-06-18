﻿#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using UnityEditor;
using UnityEngine;
using ParadoxNotion;
using UnityObject = UnityEngine.Object;

namespace ParadoxNotion.Design{

    /// <summary>
    /// Task specific Editor field
    /// </summary>

	partial class EditorUtils {

		static string lastSearch = string.Empty;
		static List<ScriptInfo> searchResults = new List<ScriptInfo>();
		

		public static void TaskField<T>(T task, ITaskSystem ownerSystem, Action<T> callback) where T : NodeCanvas.Framework.Task{
			TaskField(task, ownerSystem, typeof(T), (Task t)=> { callback((T)t); });
		}

		public static void TaskField(Task task, ITaskSystem ownerSystem, Type baseType, Action<Task> callback){
			if (task == null){
				TaskSelectionButton(ownerSystem, baseType, callback);
			} else {
				Task.ShowTaskInspectorGUI(task, callback);
			}
		}

        public static void TaskSelectionButton<T>(ITaskSystem ownerSystem, Action<T> callback) where T : NodeCanvas.Framework.Task
        {
			TaskSelectionButton(ownerSystem, typeof(T), (Task t)=> { callback((T)t); });
		}

		//Shows a button that when clicked, pops a context menu with a list of tasks deriving the base type specified. When something is selected the callback is called
		//On top of that it also shows a search field for Tasks
		public static void TaskSelectionButton(ITaskSystem ownerSystem, Type baseType, Action<Task> callback){

			Action<Type> TaskTypeSelected = (t)=> {
				var newTask = Task.Create(t, ownerSystem);
				Undo.RecordObject(ownerSystem.baseObject, "New Task");
				callback(newTask);
			};

			GUI.backgroundColor = lightBlue;
			if (GUILayout.Button("Add " + baseType.Name.SplitCamelCase() )){
				var menu = GetTypeSelectionMenu(baseType, TaskTypeSelected);
				if (Task.copiedTask != null && baseType.IsAssignableFrom( Task.copiedTask.GetType()) )
					menu.AddItem(new GUIContent(string.Format("Paste ({0})", Task.copiedTask.name) ), false, ()=> { callback( Task.copiedTask.Duplicate(ownerSystem) ); });
				menu.ShowAsContext();
				//CoolContextMenu.Show(menu, Event.current.mousePosition, "Add Task");
				Event.current.Use();
			}

			GUI.backgroundColor = Color.white;
			GUILayout.BeginHorizontal();
			var search = EditorGUILayout.TextField(lastSearch, (GUIStyle)"ToolbarSeachTextField");
			if (GUILayout.Button("", (GUIStyle)"ToolbarSeachCancelButton")){
				search = string.Empty;
				GUIUtility.keyboardControl = 0;
			}
			GUILayout.EndHorizontal();

			if (!string.IsNullOrEmpty(search)){

				if (search != lastSearch)
					searchResults = GetScriptInfosOfType(baseType);

				GUILayout.BeginVertical("TextField");
				foreach (var taskInfo in searchResults){
					if (taskInfo.name.ToLower().Replace(" ", "").Contains(search.ToLower())){
						if (GUILayout.Button(taskInfo.name)){
							search = string.Empty;
							GUIUtility.keyboardControl = 0;
							TaskTypeSelected(taskInfo.type);
						}
					}
				}
				GUILayout.EndVertical();
			}

			lastSearch = search;
		}

	}
}

#endif