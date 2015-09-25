using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System;

namespace BuildBuddy {
	[Serializable]
	public sealed class ManifestData : ManifestElement{
		
		private XmlElement parent = null;

		[SerializeField]private string scheme = "";
		[SerializeField]private string host = "";
		[SerializeField]private string port = "";
		[SerializeField]private string path = "";
		[SerializeField]private string pathPattern = "";
		[SerializeField]private string pathPrefix = "";
		[SerializeField]private string mimeType = "";

		[SerializeField]private bool display;
		
		//Constructed by editor window
		public static ManifestData CreateInstance() {
			ManifestData data = ScriptableObject.CreateInstance<ManifestData> ();
			data.node = null;
			data.elementEditStatus = EditStatus.NONE;
			return data;
		}
		//Constructed from existing entry in AndroidManifest or constructed as child of an Activity
		public static ManifestData CreateInstance(XmlNode parent, XmlNode node = null) {
			ManifestData data = ScriptableObject.CreateInstance<ManifestData> ();
			data.parent = (XmlElement)parent;
			data.node = (XmlElement)node;
			data.elementEditStatus = EditStatus.NONE;
			if (node != null) {
				data.Initialize ();
			}
			return data;
		}
		public override void OnGUI() {
			EditorGUI.BeginChangeCheck ();
			display = EditorGUILayout.Foldout (display, "Data: ");
			if (display) {
				BBGuiHelper.BeginIndent (); 
				{
					scheme = EditorGUILayout.TextField ("Scheme: ", scheme);
					host = EditorGUILayout.TextField ("Host: ", host);
					port = EditorGUILayout.TextField ("Port: ", port);
					path = EditorGUILayout.TextField ("Path: ", path);
					pathPattern = EditorGUILayout.TextField ("PathPattern: ", pathPattern);
					pathPrefix = EditorGUILayout.TextField("PathPrefix: ", pathPrefix);
					GUILayout.BeginHorizontal (); 
					{	
						mimeType = EditorGUILayout.TextField("Mime Type: ", mimeType);
						if (EditorGUI.EndChangeCheck ()) {
							elementEditStatus = EditStatus.EDITED;
						}
						if (GUILayout.Button ("Remove ")) {
							elementEditStatus = EditStatus.REMOVED;
						}
					}
					GUILayout.EndHorizontal ();
				}
				BBGuiHelper.EndIndent ();
			}
		}
		
		private void Initialize() {
			if (node.HasAttribute ("android:scheme")) {
				scheme = node.Attributes["android:scheme"].Value;
			}
			if (node.HasAttribute ("android:host")) {
				host = node.Attributes["android:host"].Value;
			}
			if (node.HasAttribute ("android:port")) {
				port = node.Attributes["android:port"].Value;
			}
			if (node.HasAttribute ("android:path")) {
				path = node.Attributes["android:path"].Value;
			}
			if (node.HasAttribute ("android:pathPattern")) {
				pathPattern = node.Attributes["android:pathPattern"].Value;
			}
			if (node.HasAttribute ("android:pathPrefix")) {
				pathPrefix = node.Attributes["android:pathPrefix"].Value;
			}
			if (node.HasAttribute ("android:mimeType")) {
				mimeType = node.Attributes["android:mimeType"].Value;
			}
			
		}
		
		#region override
		protected override void CreateNode(XmlDocument document) {
			node = document.CreateElement ("data");
			parent.AppendChild (node);
		}
		protected override void UpdateAttributes(XmlDocument document) {
			UpdateOptionalAttribute (document, "scheme", !scheme.Equals (""), scheme);
			UpdateOptionalAttribute (document, "host", !host.Equals (""), host);
			UpdateOptionalAttribute (document, "port", !port.Equals (""), port);
			UpdateOptionalAttribute (document, "path", !path.Equals (""), path);
			UpdateOptionalAttribute (document, "pathPattern", !pathPattern.Equals (""), pathPattern);
			UpdateOptionalAttribute (document, "pathPrefix", !pathPrefix.Equals (""), pathPrefix);
			UpdateOptionalAttribute (document, "mimeType", !mimeType.Equals (""), mimeType);
		}
		#endregion
	}
}