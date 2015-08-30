using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System;

namespace BuildBuddy {
	[Serializable]
	public sealed class ManifestPathPermission : ManifestElement{
		
		private XmlElement parent = null;
		
		[SerializeField] private string path = "";
		[SerializeField] private string pathPattern = "";
		[SerializeField] private string pathPrefix = "";
		[SerializeField] private string permission = "";
		[SerializeField] private string readPermission = "";
		[SerializeField] private string writePermission = "";

		//Constructed by editor window
		public static ManifestPathPermission CreateInstance() {
			ManifestPathPermission pathPermission = ScriptableObject.CreateInstance<ManifestPathPermission> ();
			pathPermission.node = null;
			pathPermission.elementEditStatus = EditStatus.EDITED;
			return pathPermission;
		}
		//Constructed from existing entry in AndroidManifest or constructed as child of an Activity
		public static ManifestPathPermission CreateInstance(XmlNode parent, XmlNode node = null) {
			ManifestPathPermission pathPermission = ScriptableObject.CreateInstance<ManifestPathPermission> ();
			pathPermission.parent = (XmlElement)parent;
			pathPermission.node = (XmlElement)node;
			pathPermission.elementEditStatus = EditStatus.NONE;
			if (node != null) {
				pathPermission.Initialize ();
			}
			return pathPermission;
		}
		public override void OnGUI() {
			EditorGUI.BeginChangeCheck ();
			path = EditorGUILayout.TextField ("Path: ", path);
			BBGuiHelper.BeginIndent (); 
			{
				pathPattern = EditorGUILayout.TextField ("PathPattern: ", pathPattern);
				pathPrefix = EditorGUILayout.TextField("PathPrefix: ", pathPrefix);
				permission = EditorGUILayout.TextField ("Permission: ", permission);
				readPermission = EditorGUILayout.TextField ("Read Permission: ", readPermission);
				writePermission = EditorGUILayout.TextField ("Write Permission: ", writePermission);
				GUILayout.BeginHorizontal (); 
				{	
					if (EditorGUI.EndChangeCheck ()) {
						elementEditStatus = EditStatus.EDITED;
					}
					if (GUILayout.Button ("Remove Path-Permission ")) {
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
			if (node.HasAttribute ("android:permission")) {
				permission = node.Attributes["android:permission"].Value;
			}
			if (node.HasAttribute ("android:readPermission")) {
				readPermission = node.Attributes["android:readPermission"].Value;
			}
			if (node.HasAttribute ("android:writePermission")) {
				writePermission = node.Attributes["android:writePermission"].Value;
			}
			
		}
		
		#region override
		protected override void CreateNode(XmlDocument document) {
			node = document.CreateElement ("path-permisssion");
			parent.AppendChild (node);
		}
		protected override void UpdateAttributes(XmlDocument document) {
			CreateAndroidAttribute (document, "path", path);
			UpdateOptionalAttribute (document, "pathPattern", !pathPattern.Equals (""), pathPattern);
			UpdateOptionalAttribute (document, "pathPrefix", !pathPrefix.Equals (""), pathPrefix);
			UpdateOptionalAttribute (document, "permission", !permission.Equals (""), permission);
			UpdateOptionalAttribute (document, "readPermission", !readPermission.Equals (""), readPermission);
			UpdateOptionalAttribute (document, "writePermission", !writePermission.Equals (""), writePermission);
		}
		#endregion
	}
}