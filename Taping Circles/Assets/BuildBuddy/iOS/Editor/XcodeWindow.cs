using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BuildBuddy{
	[System.Serializable]
	public class XcodeWindow : EditorWindow {
		[MenuItem("Window/BuildBuddy/Xcode Project Editor")]
		private static void ShowWindow() {
			EditorWindow.GetWindow<XcodeWindow> (false, "Xcode Project Editor");
		}

		private XcodeSerializer serializer;
		private List<XcodeSerializer> templates;
		private string templateName;

		private Vector2 scrollPosition;

		void OnEnable() {
			serializer = XcodeSerializer.CreateInstance ();
			templates = XcodeTemplateManager.GetTemplates ();
		}
		void OnDestroy() {
			
		}
		void OnUpdate() {
			this.Repaint();
		}
		void OnGUI() {
			if (serializer == null)
				OnEnable();
			scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);
			{
				serializer.OnGUI();
				EditorGUILayout.Space ();
				templateName = EditorGUILayout.TextField ("Template name: ", templateName);
				if (GUILayout.Button ("Save as Template")) {
					XcodeSerializer templateSerializer = XcodeSerializer.CreateInstance(serializer.ToString(), true);
					templateSerializer.name = templateName;
					XcodeTemplateManager.SaveTemplate(templateSerializer);
				}
				EditorGUILayout.Space();
				for (int i = 0; i < templates.Count; i++) {
					EditorGUILayout.BeginHorizontal();
					{
						templates[i].display = EditorGUILayout.Foldout (templates[i].display, templates[i].name);
						if (GUILayout.Button ("Import", GUILayout.Width(50))) 
							serializer.Merge(templates[i]);
						if (GUILayout.Button ("Delete", GUILayout.Width(50))) {
							XcodeTemplateManager.DeleteTemplate(templates[i--]);
							continue;
						}
					}
					EditorGUILayout.EndHorizontal();
					BBGuiHelper.BeginIndent();
					{
						if (templates[i].display) {
							EditorGUILayout.BeginHorizontal();
							{
								EditorGUILayout.LabelField("Name: ", GUILayout.Width(75));
								templates[i].name = EditorGUILayout.TextField (templates[i].name);
							}
							EditorGUILayout.EndHorizontal();
							templates[i].OnGUI ();
						}
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
	}
}