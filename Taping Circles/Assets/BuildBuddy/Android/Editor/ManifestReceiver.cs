using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using System;

namespace BuildBuddy {
	[Serializable]
	public sealed class ManifestReceiver : ManifestElement{

		[SerializeField] private List<ManifestIntentFilter> intentFilterList = new List<ManifestIntentFilter> ();
		[SerializeField] private List<ManifestMetaData> metaDataList = new List<ManifestMetaData>();
		[SerializeField] private bool displayIntentFilter;
		[SerializeField] private bool displayMetaData;
		[SerializeField] private bool displayAttributes;
		
		[SerializeField] private new string name;
		[SerializeField] private bool enabled = true;
		[SerializeField] private bool exported = true;
		[SerializeField] private string icon = "";
		[SerializeField] private string label = "";
		[SerializeField] private string permission = "";
		[SerializeField] private string process = "";
		
		[SerializeField] private bool display;
		
		//Constructed by editor window
		public static ManifestReceiver CreateInstance() { 
			ManifestReceiver manifestReceiver = ScriptableObject.CreateInstance<ManifestReceiver> ();
			manifestReceiver.node = null;
			manifestReceiver.elementEditStatus = EditStatus.EDITED;
			return manifestReceiver;
		}
		//Constructed from existing entry in AndroidManifest
		public static ManifestReceiver CreateInstance(XmlNode node) {
			ManifestReceiver manifestReceiver = ScriptableObject.CreateInstance<ManifestReceiver> ();
			manifestReceiver.node = (XmlElement)node;
			manifestReceiver.elementEditStatus = EditStatus.NONE;
			manifestReceiver.Initialize();
			return manifestReceiver;
		}
		public override void OnGUI() {
			bool changed = false;
			display = EditorGUILayout.Foldout (display, "Receiver: " + (!display ? name : ""));
			if (display) {
				EditorGUI.BeginChangeCheck();
				name = EditorGUILayout.TextField ("Name: ", name);
				changed |= EditorGUI.EndChangeCheck();
				BBGuiHelper.BeginIndent();
				{
					#region attributes
					displayAttributes = EditorGUILayout.Foldout (displayAttributes, "Attributes: ");
					if (displayAttributes) {
						EditorGUI.BeginChangeCheck();
						BBGuiHelper.BeginIndent();
						{
							enabled = EditorGUILayout.Toggle ("Enabled: ", enabled);
							exported = EditorGUILayout.Toggle ("Exported: ", exported);
							icon = EditorGUILayout.TextField ("Icon: ", icon);
							label = EditorGUILayout.TextField ("Label: ", label);
							permission = EditorGUILayout.TextField("Permission: ", permission);
							process = EditorGUILayout.TextField ("Process: ", process);
						}
						BBGuiHelper.EndIndent();
						changed |= EditorGUI.EndChangeCheck();
					}
					#endregion
					#region intentfilter
					displayIntentFilter = EditorGUILayout.Foldout (displayIntentFilter, "Intent-Filters: ("+intentFilterList.Count+")");
					if (displayIntentFilter) {
						for (int i = 0; i < intentFilterList.Count; i++) {
							Undo.RecordObject(intentFilterList[i], "Intent-Filter");
							if (intentFilterList[i].ElementEditStatus != EditStatus.REMOVED) {
								intentFilterList[i].OnGUI ();
							}
							if (intentFilterList[i].ElementEditStatus != EditStatus.NONE)
								changed = true;
						}
						BBGuiHelper.EndIndent();
						GUILayout.BeginHorizontal();
						{
							EditorGUILayout.Space();
							if (GUILayout.Button ("New Intent-Filter", BBGuiHelper.ButtonWidth())) {
								intentFilterList.Add (ManifestIntentFilter.CreateInstance(node));
								changed = true;
							}
						}
						GUILayout.EndHorizontal();
						BBGuiHelper.BeginIndent();
					}
					#endregion
					#region metadata
					displayMetaData = EditorGUILayout.Foldout (displayMetaData, "Meta-Data: ("+intentFilterList.Count+")");
					if (displayMetaData) {
						for (int i = 0; i < metaDataList.Count; i++) {
							Undo.RecordObject(metaDataList[i], "Meta-Data");
							if (metaDataList[i].ElementEditStatus != EditStatus.REMOVED) {
								metaDataList[i].OnGUI ();
							}
							if (metaDataList[i].ElementEditStatus != EditStatus.NONE) {
								changed = true;
							}                                                         
						}
						BBGuiHelper.EndIndent();
						GUILayout.BeginHorizontal();
						{
							EditorGUILayout.Space();
							if (GUILayout.Button ("New Meta-Data", BBGuiHelper.ButtonWidth())) {
								metaDataList.Add (ManifestMetaData.CreateInstance(node));
								changed = true;
							}
						}
						GUILayout.EndHorizontal();
						BBGuiHelper.BeginIndent();
					}
					#endregion
				}
				BBGuiHelper.EndIndent();
				GUILayout.BeginHorizontal ();
				{
					EditorGUILayout.Space();
					if (changed) {
						elementEditStatus = EditStatus.EDITED;
					}
					if (GUILayout.Button ("Remove Receiver", BBGuiHelper.ButtonWidth())) {
						elementEditStatus = EditStatus.REMOVED;
					}
				}
				GUILayout.EndHorizontal ();
			}
		}
		
		private void Initialize() {
			foreach (XmlNode element in node.ChildNodes) {
				if (element.Name.Equals ("meta-data")) {
					metaDataList.Add (ManifestMetaData.CreateInstance(node, element));
				}
				if (element.Name.Equals ("intent-filter")) {
					intentFilterList.Add (ManifestIntentFilter.CreateInstance(node, element));
				}
			}
			
			if (node.HasAttribute ("android:name")) {
				name = node.Attributes["android:name"].Value;
			}
		
			enabled = InitializeBoolAttribute ("android:enabled", true);
			exported = InitializeBoolAttribute ("android:exported", true);
			if (node.HasAttribute ("android:icon")) {
				icon = node.Attributes["android:icon"].Value;
			}
			if (node.HasAttribute ("android:label")) {
				label = node.Attributes["android:label"].Value;
			}
			if (node.HasAttribute ("android:permission")) {
				permission = node.Attributes["android:permission"].Value;
			}
			if (node.HasAttribute ("android:process")) {
				process = node.Attributes["android:process"].Value;
			}
		}
		#region override
		protected override void CreateNode(XmlDocument document) {
			node = document.CreateElement ("receiver");
			XmlNode applicationElement = document.GetElementsByTagName ("application") [0];
			applicationElement.AppendChild (node);
		}
		protected override void UpdateAttributes(XmlDocument document) {
			for (int i = 0; i < intentFilterList.Count; i++) {
				ManifestIntentFilter intentFIlter = intentFilterList[i];
				if (intentFIlter.ElementEditStatus == EditStatus.REMOVED) {
					intentFilterList.RemoveAt (i);
					i--;
				}
				intentFIlter.ApplyChanges(document);
			}
			for (int i = 0; i < metaDataList.Count; i++) {
				ManifestMetaData metaData = metaDataList[i];
				if (metaData.ElementEditStatus == EditStatus.REMOVED) {
					metaDataList.RemoveAt (i);
					i--;
				}
				metaData.ApplyChanges(document);
			}
			CreateAndroidAttribute (document, "name", name);
			UpdateOptionalAttribute (document, "enabled", !enabled, "false");
			UpdateOptionalAttribute (document, "exported", !exported, "false");
			UpdateOptionalAttribute (document, "icon", !icon.Equals (""), icon);
			UpdateOptionalAttribute (document, "label", !label.Equals (""), label);
			UpdateOptionalAttribute (document, "permission", !permission.Equals (""), permission);
			UpdateOptionalAttribute (document, "process", !process.Equals (""), process);
		}
		#endregion

	}
}