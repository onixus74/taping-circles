using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using System;

namespace BuildBuddy {
	[Serializable]
	public sealed class ManifestUsesLibrary : ManifestElement{

		[SerializeField] private new string name;
		[SerializeField] private bool required;


		//Constructed by editor window
		public static ManifestUsesLibrary CreateInstance() { 
			ManifestUsesLibrary usesLibrary = ScriptableObject.CreateInstance<ManifestUsesLibrary>();
			usesLibrary.node = null;
			usesLibrary.elementEditStatus = EditStatus.EDITED;
			return usesLibrary;
		}
		//Constructed from existing entry in AndroidManifest
		public static ManifestUsesLibrary CreateInstance(XmlNode node) {
			ManifestUsesLibrary usesLibrary = ScriptableObject.CreateInstance<ManifestUsesLibrary>();
			usesLibrary.node = (XmlElement)node;
			usesLibrary.elementEditStatus = EditStatus.NONE;
			usesLibrary.Initialize();
			return usesLibrary;
		}
		public override void OnGUI() {
			EditorGUI.BeginChangeCheck ();
			name = EditorGUILayout.TextField ("Name: ", name);
			BBGuiHelper.BeginIndent();
			{
				EditorGUILayout.BeginHorizontal ();
				{
					required = EditorGUILayout.Toggle ("Required: ", required);
					if (EditorGUI.EndChangeCheck ()) {
						elementEditStatus = EditStatus.EDITED;
					}
					if (GUILayout.Button ("Remove Uses-Library")) {
						elementEditStatus = EditStatus.REMOVED;
					}
				}
				EditorGUILayout.EndHorizontal ();
			}
			BBGuiHelper.EndIndent ();
		}
		
		private void Initialize() {
			if (node.HasAttribute ("android:name")) {
				name = node.Attributes["android:name"].Value;
			}
			required = InitializeBoolAttribute ("android:required", true);
		}
		#region override
		protected override void CreateNode(XmlDocument document) {
			node = document.CreateElement ("uses-library");
			XmlNode applicationElement = document.GetElementsByTagName ("application") [0];
			applicationElement.AppendChild (node);
		}
		protected override void UpdateAttributes(XmlDocument document) {
			CreateAndroidAttribute (document, "name", name);
			UpdateOptionalAttribute (document, "required", !required, "false");
		}
		#endregion
	}
}