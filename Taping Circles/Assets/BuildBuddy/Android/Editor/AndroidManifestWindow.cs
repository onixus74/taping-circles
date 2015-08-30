using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Xml;

namespace BuildBuddy {
	public class AndroidManifestWindow : EditorWindow {

		[MenuItem("Window/BuildBuddy/AndroidManifest Editor")]
		private static void ShowWindow() {
			EditorWindow.GetWindow<AndroidManifestWindow>(false, "AndroidManifest Editor");
		}

		private AndroidWindowData data;
		private List<AndroidWindowData> templateElements;

		private string templateName;
		private Vector2 scrollPos;

		void OnEnable() {
			data = AndroidWindowData.CreateInstance (new AndroidXmlEditor ());
			templateElements = AndroidTemplateManager.GetTemplates ();

		}
		void OnDestroy() {

		}
		void OnUpdate() {
			this.Repaint();
		}

		void OnGUI() {
			if (data == null)
				OnEnable ();
			Undo.RecordObject (data, "Manifest Window");
			GUI.color = Color.white;
			scrollPos = EditorGUILayout.BeginScrollView (scrollPos);
			data.OnGUI ();
			EditorGUILayout.Space ();
			#region UpdateButton
			EditorGUILayout.BeginHorizontal ();
			{
				if (data.dirty) {
					GUI.color = Color.green;
				}
				else {
					GUI.color = Color.grey;
				}
				if (GUILayout.Button ("Apply Changes")) {
					ApplyChanges();
				}
				if (data.dirty) {
					GUI.color = Color.red;
				}
				else {
					GUI.color = Color.grey;
				}
				if (GUILayout.Button ("Clear Changes")) {
					ReloadDocument();
				}
				GUI.color = Color.white;
			}
			EditorGUILayout.EndHorizontal();
			#endregion
			EditorGUILayout.Space ();

			templateName = EditorGUILayout.TextField ("Template name: ", templateName);
			if (GUILayout.Button ("Save as Template")) {
				AndroidXmlEditor templateEditor = new AndroidXmlEditor(data.ToString());
				AndroidWindowData newTemplate = AndroidWindowData.CreateInstance(templateEditor);
				newTemplate.name = templateName;
				AndroidTemplateManager.SaveTemplate(newTemplate);
			}
			for (int i = 0; i < templateElements.Count; i++) {
				EditorGUILayout.BeginHorizontal();
				{
					templateElements[i].display = EditorGUILayout.Foldout (templateElements[i].display, templateElements[i].name+":");
					if (GUILayout.Button("Import", GUILayout.Width (50))) {
						data.Merge(templateElements[i]);
						data.dirty = true;
					}
					if (GUILayout.Button ("Delete", GUILayout.Width (50))) {
						AndroidTemplateManager.DeleteTemplate(templateElements[i--]);
						EditorGUILayout.EndHorizontal();
						continue;
					}
				}
				EditorGUILayout.EndHorizontal();
				if (templateElements[i].display) {
					BBGuiHelper.BeginIndent();
					{
						templateElements[i].OnGUI ();
						EditorGUILayout.BeginHorizontal ();
						{
							if (templateElements[i].dirty) {
								GUI.color = Color.green;
							}
							else {
								GUI.color = Color.grey;
							}
							if (GUILayout.Button ("Apply Changes")) {
								templateElements[i].ApplyChanges();
							}
							if (templateElements[i].dirty) {
								GUI.color = Color.red;
							}
							else {
								GUI.color = Color.grey;
							}
							if (GUILayout.Button ("Clear Changes")) {
								AndroidTemplateManager.ReloadTemplate(i);
							}
							GUI.color = Color.white;
						}
						EditorGUILayout.EndHorizontal();
					}
					BBGuiHelper.EndIndent();
				}
			}

			EditorGUILayout.EndScrollView ();
			//Repaint on Undo
			if (Event.current.type == EventType.ValidateCommand) {
				switch (Event.current.commandName) {
				case "UndoRedoPerformed":
					this.Repaint ();
					break;
				}
			}
		}
		void Update() {

		}

		private void ApplyChanges() {
			data.ApplyChanges ();
		}
		private void ReloadDocument() {
			OnEnable ();
			data.dirty = false;
		}
	}
}