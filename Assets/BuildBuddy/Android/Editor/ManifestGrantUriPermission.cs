using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System;

namespace BuildBuddy {
	[Serializable]
	public sealed class ManifestGrantUriPermission : ManifestElement{
		
		private XmlElement parent = null;
		
		[SerializeField] private string path = "";
		[SerializeField] private string pathPattern = "";
		[SerializeField] private string pathPrefix = "";
		
		//Constructed by editor window
		public static ManifestGrantUriPermission CreateInstance() { 
			ManifestGrantUriPermission grantUriPerm = ScriptableObject.CreateInstance<ManifestGrantUriPermission> ();
			grantUriPerm.node = null;
			grantUriPerm.elementEditStatus = EditStatus.EDITED;
			return grantUriPerm;
		}
		//Constructed from existing entry in AndroidManifest or constructed as child of an Activity
		public static ManifestGrantUriPermission CreateInstance(XmlNode parent, XmlNode node = null) {
			ManifestGrantUriPermission grantUriPerm = ScriptableObject.CreateInstance<ManifestGrantUriPermission> ();
			grantUriPerm.parent = (XmlElement)parent;
			grantUriPerm.node = (XmlElement)node;
			grantUriPerm.elementEditStatus = EditStatus.NONE;
			if (node != null) {
				grantUriPerm.Initialize ();
			}
			return grantUriPerm;
		}
		public override void OnGUI() {
			EditorGUI.BeginChangeCheck ();
			path = EditorGUILayout.TextField ("Path: ", path);
			BBGuiHelper.BeginIndent (); 
			{
				pathPattern = EditorGUILayout.TextField ("PathPattern: ", pathPattern);
				GUILayout.BeginHorizontal (); 
				{	
					pathPrefix = EditorGUILayout.TextField("PathPrefix: ", pathPrefix);
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
		
		private void Initialize() {
			if (node.HasAttribute ("android:path")) {
				path = node.Attributes["android:path"].Value;
			}
			if (node.HasAttribute ("android:pathPattern")) {
				pathPattern = node.Attributes["android:pathPattern"].Value;
			}
			if (node.HasAttribute ("android:pathPrefix")) {
				pathPrefix = node.Attributes["android:pathPrefix"].Value;
			}

		}
		
		#region override
		protected override void CreateNode(XmlDocument document) {
			node = document.CreateElement ("grant-uri-permission");
			parent.AppendChild (node);
		}
		protected override void UpdateAttributes(XmlDocument document) {
			CreateAndroidAttribute (document, "path", path);
			UpdateOptionalAttribute (document, "pathPattern", !pathPattern.Equals (""), pathPattern);
			UpdateOptionalAttribute (document, "pathPrefix", !pathPrefix.Equals (""), pathPrefix);
		}
		#endregion
	}
}