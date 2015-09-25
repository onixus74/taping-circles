using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace BuildBuddy {
	[System.Serializable]
	public sealed class ManifestProvider : ManifestElement{
		
		[SerializeField] private List<ManifestMetaData> metaDataList = new List<ManifestMetaData>();
		[SerializeField] private List<ManifestGrantUriPermission> grantUriPermissionList = new List<ManifestGrantUriPermission>();
		[SerializeField] private List<ManifestPathPermission> pathPermissionList = new List<ManifestPathPermission> ();
		[SerializeField] private bool displayMetaData;
		[SerializeField] private bool displayGrantUriPermissions;
		[SerializeField] private bool displayPathPermissions;
		[SerializeField] private bool displayAttributes;
		
		[SerializeField] private new string name;
		[SerializeField] private List<string> authorities = new List<string>();
		[SerializeField] private bool enabled = true;
		[SerializeField] private bool exported = true;
		[SerializeField] private bool grantUriPermissions;
		[SerializeField] private string icon = "";
		[SerializeField] private int initOrder;
		[SerializeField] private string label = "";
		[SerializeField] private bool multiProcess;
		[SerializeField] private string permission = "";
		[SerializeField] private string process = "";
		[SerializeField] private string readPermission = "";
		[SerializeField] private bool syncable;
		[SerializeField] private string writePermission = "";

		private bool display;
		
		//Constructed by editor window
		public static ManifestProvider CreateInstance() { 
			ManifestProvider provider = ScriptableObject.CreateInstance<ManifestProvider> ();
			provider.node = null;
			provider.elementEditStatus = EditStatus.EDITED;
			return provider;
		}
		//Constructed from existing entry in AndroidManifest
		public static ManifestProvider CreateInstance(XmlNode node) {
			ManifestProvider provider = ScriptableObject.CreateInstance<ManifestProvider> ();
			provider.node = (XmlElement)node;
			provider.elementEditStatus = EditStatus.NONE;
			provider.Initialize();
			return provider;
		}
		public override void OnGUI() {
			bool changed = false;
			display = EditorGUILayout.Foldout (display, "Provider: " + (!display ? name : ""));
			EditorGUI.BeginChangeCheck ();
			if (display) {
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
							for (int i = 0; i < authorities.Count ;i++) {
								EditorGUILayout.BeginHorizontal();
								{
									authorities[i] = EditorGUILayout.TextField (authorities[i]);
									if (GUILayout.Button ("Remove")) {
										authorities.RemoveAt (i--);
									}
								}
								EditorGUILayout.EndHorizontal();
							}
							enabled = EditorGUILayout.Toggle ("Enabled: ", enabled);
							exported = EditorGUILayout.Toggle ("Exported: ", exported);
							grantUriPermissions = EditorGUILayout.Toggle ("Grant URI Permissions: ", grantUriPermissions);
							icon = EditorGUILayout.TextField ("Icon: ", icon);
							initOrder = EditorGUILayout.IntField ("Init Order: ", initOrder);
							label = EditorGUILayout.TextField ("Label: ", label);
							multiProcess = EditorGUILayout.Toggle ("Multiprocess: ", multiProcess);
							permission = EditorGUILayout.TextField("Permission: ", permission);
							process = EditorGUILayout.TextField ("Process: ", process);
							readPermission = EditorGUILayout.TextField ("Read Permission: ", readPermission);
							syncable = EditorGUILayout.Toggle ("Syncable: ", syncable);
							writePermission = EditorGUILayout.TextField ("Write Permission: ", writePermission);
						}
						BBGuiHelper.EndIndent();
						changed |= EditorGUI.EndChangeCheck();
					}
					#endregion
					#region metadata
					displayMetaData = EditorGUILayout.Foldout (displayMetaData, "Meta-Data: ("+metaDataList.Count+")");
					if (displayMetaData) {
						for (int i = 0; i < metaDataList.Count; i++) {
							Undo.RecordObject(metaDataList[i], "Meta-Data");
							if (metaDataList[i].ElementEditStatus != EditStatus.REMOVED) {
								metaDataList[i].OnGUI ();
							}
							if (metaDataList[i].ElementEditStatus != EditStatus.NONE)
								changed = true;
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
					#region granturipermission
					displayGrantUriPermissions = EditorGUILayout.Foldout (displayGrantUriPermissions, "Grant-Uri-Permissions: ("+grantUriPermissionList.Count+")");
					if (displayGrantUriPermissions) {
						for (int i = 0; i < grantUriPermissionList.Count; i++) {
							Undo.RecordObject(grantUriPermissionList[i], "Grant-Uri-Permissions");
							if (grantUriPermissionList[i].ElementEditStatus != EditStatus.REMOVED) {
								grantUriPermissionList[i].OnGUI ();
							}
							if (grantUriPermissionList[i].ElementEditStatus != EditStatus.NONE) {
								changed = true;
							}
						}
						BBGuiHelper.EndIndent();
						GUILayout.BeginHorizontal();
						{
							EditorGUILayout.Space();
							if (GUILayout.Button ("New Grant-Uri-Permission", BBGuiHelper.ButtonWidth())) {
								grantUriPermissionList.Add (ManifestGrantUriPermission.CreateInstance(node));
								changed = true;
							}
						}
						GUILayout.EndHorizontal();
						BBGuiHelper.BeginIndent();
					}
					#endregion
					#region pathpermission
					displayPathPermissions = EditorGUILayout.Foldout (displayPathPermissions, "Path-Permissions: ("+pathPermissionList.Count+")");
					if (displayPathPermissions) {
						for (int i = 0; i < pathPermissionList.Count; i++) {
							Undo.RecordObject(pathPermissionList[i], "Path-Permissions");
							if (pathPermissionList[i].ElementEditStatus != EditStatus.REMOVED) {
								pathPermissionList[i].OnGUI ();
							}
							if (pathPermissionList[i].ElementEditStatus != EditStatus.NONE) {
								changed = true;
							}
						}
						BBGuiHelper.EndIndent();
						GUILayout.BeginHorizontal();
						{
							EditorGUILayout.Space();
							if (GUILayout.Button ("New Path-Permission", BBGuiHelper.ButtonWidth())) {
								pathPermissionList.Add (ManifestPathPermission.CreateInstance(node));
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
					if (GUILayout.Button ("Remove Provider", BBGuiHelper.ButtonWidth())) {
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
				} else if (element.Name.Equals ("grant-uri-permission")) {
					grantUriPermissionList.Add (ManifestGrantUriPermission.CreateInstance(node, element));
				} else if (element.Name.Equals ("path-permission")) {
					pathPermissionList.Add (ManifestPathPermission.CreateInstance (node, element));
				}
			}
			if (node.HasAttribute ("android:name")) {
				name = node.Attributes["android:name"].Value;
			}

			InitializeAuthorities ();
			enabled = InitializeBoolAttribute ("android:enabled", true);
			exported = InitializeBoolAttribute ("android:exported", true);
			grantUriPermissions = InitializeBoolAttribute ("android:grantUriPermissions", false);
			if (node.HasAttribute ("android:icon")) {
				icon = node.Attributes["android:icon"].Value;
			}
			if (node.HasAttribute ("android:initOrder")) {
				initOrder = System.Convert.ToInt32 (node.Attributes["android:icon"].Value);
			}
			if (node.HasAttribute ("android:label")) {
				label = node.Attributes["android:label"].Value;
			}
			multiProcess = InitializeBoolAttribute ("android:multiprocess", false);
			if (node.HasAttribute ("android:permission")) {
				permission = node.Attributes["android:permission"].Value;
			}
			if (node.HasAttribute ("android:process")) {
				process = node.Attributes["android:process"].Value;
			}
			if (node.HasAttribute ("android:readPermission")) {
				readPermission = node.Attributes["android:readPermission"].Value;
			}
			syncable = InitializeBoolAttribute ("android:syncable", false);
			if (node.HasAttribute ("android:writePermission")) {
				writePermission = node.Attributes["android:writePermission"].Value;
			}

		}
		#region override
		protected override void CreateNode(XmlDocument document) {
			node = document.CreateElement ("provider");
			XmlNode applicationElement = document.GetElementsByTagName ("application") [0];
			applicationElement.AppendChild (node);
		}
		protected override void UpdateAttributes(XmlDocument document) {
			for (int i = 0; i < metaDataList.Count; i++) {
				ManifestMetaData metaData = metaDataList[i];
				if (metaData.ElementEditStatus == EditStatus.REMOVED) {
					metaDataList.RemoveAt (i);
					i--;
				}
				metaData.ApplyChanges(document);
			}
			for (int i = 0; i < grantUriPermissionList.Count; i++) {
				ManifestGrantUriPermission uriPermission = grantUriPermissionList[i];
				if (uriPermission.ElementEditStatus == EditStatus.REMOVED) {
					grantUriPermissionList.RemoveAt (i);
					i--;
				}
				uriPermission.ApplyChanges(document);
			}
			for (int i = 0; i < pathPermissionList.Count; i++) {
				ManifestPathPermission pathPermission = pathPermissionList[i];
				if (pathPermission.ElementEditStatus == EditStatus.REMOVED) {
					pathPermissionList.RemoveAt (i);
					i--;
				}
				pathPermission.ApplyChanges(document);
			}
			CreateAndroidAttribute (document, "name", name);
			CreateAndroidAttribute (document, "authorities", AuthoritiesToString());
			UpdateOptionalAttribute (document, "enabled", !enabled, "false");
			UpdateOptionalAttribute (document, "exported", !exported, "false");
			UpdateOptionalAttribute (document, "grantUriPermissions", grantUriPermissions, "true");
			UpdateOptionalAttribute (document, "icon", !icon.Equals (""), icon);
			UpdateOptionalAttribute (document, "initOrder", initOrder != 0, ""+initOrder);
			UpdateOptionalAttribute (document, "label", !label.Equals (""), label);
			UpdateOptionalAttribute (document, "multiprocess", multiProcess, "true");
			UpdateOptionalAttribute (document, "permission", !permission.Equals (""), permission);
			UpdateOptionalAttribute (document, "process", !process.Equals (""), process);
			UpdateOptionalAttribute (document, "readPermission", !readPermission.Equals (""), readPermission);
			UpdateOptionalAttribute (document, "syncable", syncable, "true");
			UpdateOptionalAttribute (document, "writePermission", !writePermission.Equals(""), writePermission);
		}
		#endregion

		private void InitializeAuthorities() {
			if (node.HasAttribute ("android:authorities")) {
				authorities = node.Attributes["android:authorities"].Value.Split (';').ToList ();
			}
		}
		private string AuthoritiesToString() {
			if (authorities.Count == 0)
				return "";
			string authoritiesString = "";
			foreach (string authority in authorities) {
				authoritiesString += (";"+authority);
			}
			return authoritiesString.Substring(1);
		}
	}
}