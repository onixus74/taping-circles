using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System;

namespace BuildBuddy {
	[Serializable]
	public sealed class ManifestApplication : ManifestElement{

		[SerializeField]private new string name = "";
		[SerializeField] private bool allowTaskReparenting;
		[SerializeField] private bool allowBackup;
		[SerializeField] private string backupAgent = "";
		[SerializeField] private bool debuggable;
		[SerializeField] private string description = "";
		[SerializeField] private bool enabled = true;
		[SerializeField] private bool hasCode = true;
		[SerializeField] private bool hardwareAccelerated = true;
		[SerializeField] private string icon = "";
		[SerializeField] private bool killAfterRestore = true;
		[SerializeField] private bool largeHeap;
		[SerializeField] private string label = "";
		[SerializeField] private string logo = "";
		[SerializeField] private string manageSpaceActivity = "";
		[SerializeField] private string permission = "";
		[SerializeField] private bool persistent;
		[SerializeField] private string process = "";
		[SerializeField] private bool restoreAnyVersion;
		[SerializeField] private string requiredAccountType = "";
		[SerializeField] private string restrictedAccountType = "";
		[SerializeField] private bool supportsRtl;
		[SerializeField] private string taskAffinity = "";
		[SerializeField] private bool testOnly;
		[SerializeField] private string theme = "";
		[SerializeField] private string uiOptions = "";
		[SerializeField] private bool vmSafeMode;

		private bool display;

		//Constructed from existing entry in AndroidManifest or constructed as child of an Activity
		public static ManifestApplication CreateInstance(XmlNode node) {
			ManifestApplication application = ScriptableObject.CreateInstance<ManifestApplication> ();
			application.node = (XmlElement)node;
			application.elementEditStatus = EditStatus.NONE;
			if (node != null) {
				application.Initialize ();
			}
			else 
				Debug.LogError ("Manifest doesn't have application tags");
			return application;
		}
		public override void OnGUI() {
			Undo.RecordObject (this, "Application");
			display = EditorGUILayout.Foldout (display, "Application:");
			if (display) {
				EditorGUI.BeginChangeCheck ();
				BBGuiHelper.BeginIndent();
				{
					name = EditorGUILayout.TextField ("Name: ", name);
					allowTaskReparenting = EditorGUILayout.Toggle ("Allow Task Reparenting: ", allowTaskReparenting);
					allowBackup = EditorGUILayout.Toggle("Allow Backup: ", allowBackup);
					backupAgent = EditorGUILayout.TextField ("Backup Agent:", backupAgent);
					debuggable = EditorGUILayout.Toggle("Debuggable", debuggable);
					EditorGUILayout.LabelField ("Description:");
					description = EditorGUILayout.TextArea (description);
					enabled = EditorGUILayout.Toggle ("Enabled:", enabled);
					hasCode = EditorGUILayout.Toggle ("Has Code:", hasCode);
					hardwareAccelerated = EditorGUILayout.Toggle ("Hardware Accelerated", hardwareAccelerated);
					icon = EditorGUILayout.TextField("Icon: ", icon);
					killAfterRestore = EditorGUILayout.Toggle ("Kill After Restore:", killAfterRestore);
					largeHeap = EditorGUILayout.Toggle("Large Heap:", largeHeap);
					label = EditorGUILayout.TextField ("Label:", label);
					logo = EditorGUILayout.TextField("Logo:", logo);
					manageSpaceActivity = EditorGUILayout.TextField ("Manage Space Activity:", manageSpaceActivity);
					permission = EditorGUILayout.TextField ("Permission:", permission);
					persistent = EditorGUILayout.Toggle ("Persistent:", persistent);
					process = EditorGUILayout.TextField ("Process:", process);
					restoreAnyVersion = EditorGUILayout.Toggle ("Restore Any Version:", restoreAnyVersion);
					requiredAccountType = EditorGUILayout.TextField ("Required Account Type:", requiredAccountType);
					restrictedAccountType = EditorGUILayout.TextField ("Restricted Account Type:", restrictedAccountType);
					supportsRtl = EditorGUILayout.Toggle ("Supports Rtl:", supportsRtl);
					taskAffinity = EditorGUILayout.TextField ("Task Affinity", taskAffinity);
					testOnly = EditorGUILayout.Toggle ("Test Only", testOnly);
					theme = EditorGUILayout.TextField ("Theme", theme);
					uiOptions = EditorGUILayout.TextField ("UI Options:", uiOptions);
					vmSafeMode = EditorGUILayout.Toggle("VM Safe Mode:", vmSafeMode);
					if (EditorGUI.EndChangeCheck ()) {
						elementEditStatus = EditStatus.EDITED;
					}
				}
				BBGuiHelper.EndIndent();
			}
		}
		
		private void Initialize() {
			if (node.HasAttribute ("android:name")) {
				name = node.Attributes["android:name"].Value;
			}
			if (node.HasAttribute ("android:backupAgent")) {
				backupAgent = node.Attributes["android:backupAgent"].Value;
			}
			if (node.HasAttribute ("android:description")) {
				description = node.Attributes["android:description"].Value;
			}
			if (node.HasAttribute ("android:icon")) {
				icon = node.Attributes["android:icon"].Value;
			}
			if (node.HasAttribute ("android:label")) {
				label = node.Attributes["android:label"].Value;
			}
			if (node.HasAttribute ("android:logo")) {
				logo = node.Attributes["android:logo"].Value;
			}
			if (node.HasAttribute ("android:permission")) {
				permission = node.Attributes["android:permission"].Value;
			}
			if (node.HasAttribute ("android:process")) {
				process = node.Attributes["android:process"].Value;
			}
			if (node.HasAttribute ("android:requiredAccountType")) {
				requiredAccountType = node.Attributes["android:requiredAccountType"].Value;
			}
			if (node.HasAttribute ("android:restrictedAccountType")) {
				restrictedAccountType = node.Attributes["android:restrictedAccountType"].Value;
			}
			if (node.HasAttribute ("android:taskAffinity")) {
				taskAffinity = node.Attributes["android:taskAffinity"].Value;
			}
			if (node.HasAttribute ("android:theme")) {
				theme = node.Attributes["android:theme"].Value;
			}
			if (node.HasAttribute ("android:uiOptions")) {
				uiOptions = node.Attributes["android:uiOptions"].Value;
			}
			allowTaskReparenting = InitializeBoolAttribute ("android:allowTaskReparenting", false);
			allowBackup = InitializeBoolAttribute ("android:allowBackup", false);
			debuggable = InitializeBoolAttribute ("android:debuggable", false);
			enabled = InitializeBoolAttribute ("android:enabled", true);
			hasCode = InitializeBoolAttribute ("android:hasCode", true);
			hardwareAccelerated = InitializeBoolAttribute ("android:hardwareAccelerated", true);
			killAfterRestore = InitializeBoolAttribute ("android:killAfterRestore", true);
			largeHeap = InitializeBoolAttribute ("android:largeHeap", false);
			persistent = InitializeBoolAttribute ("android:persistent", false);
			restoreAnyVersion = InitializeBoolAttribute ("android:restoreAnyVersion", false);
			supportsRtl = InitializeBoolAttribute ("android:supportsRtl", false);
			testOnly = InitializeBoolAttribute ("android:testOnly", false);
			vmSafeMode = InitializeBoolAttribute ("android:vmSafeMode", false);
		}
		
		#region override
		protected override void CreateNode(XmlDocument document) {
			node = (XmlElement)document.GetElementsByTagName ("application") [0];
		}
		protected override void UpdateAttributes(XmlDocument document) {
			UpdateOptionalAttribute (document, "name", !name.Equals (""), name);
			UpdateOptionalAttribute (document, "backupAgent", !backupAgent.Equals (""), backupAgent);
			UpdateOptionalAttribute (document, "description", !description.Equals (""), description);
			UpdateOptionalAttribute (document, "icon", !icon.Equals (""), icon);
			UpdateOptionalAttribute (document, "label", !label.Equals (""), label);
			UpdateOptionalAttribute (document, "logo", !logo.Equals (""), logo);
			UpdateOptionalAttribute (document, "permission", !permission.Equals (""), permission);
			UpdateOptionalAttribute (document, "process", !process.Equals (""), process);
			UpdateOptionalAttribute (document, "requiredAccountType", !requiredAccountType.Equals (""), requiredAccountType);
			UpdateOptionalAttribute (document, "restrictedAccountType", !restrictedAccountType.Equals (""), restrictedAccountType);
			UpdateOptionalAttribute (document, "taskAffinity", !taskAffinity.Equals (""), taskAffinity);
			UpdateOptionalAttribute (document, "theme", !theme.Equals (""), theme);
			UpdateOptionalAttribute (document, "uiOptions", !uiOptions.Equals (""), uiOptions);
			UpdateOptionalAttribute (document, "allowTaskReparenting", allowTaskReparenting, "true");
			UpdateOptionalAttribute (document, "allowBackup", allowBackup, "true");
			UpdateOptionalAttribute (document, "debuggable", debuggable, "true");
			UpdateOptionalAttribute (document, "enabled", !enabled, "false");
			UpdateOptionalAttribute (document, "hasCode", !hasCode, "false");
			UpdateOptionalAttribute (document, "hardwareAccelerated", !hardwareAccelerated, "false");
			UpdateOptionalAttribute (document, "killAfterRestore", !killAfterRestore, "false");
			UpdateOptionalAttribute (document, "largeHeap", largeHeap, "true");
			UpdateOptionalAttribute (document, "persistent", persistent, "true");
			UpdateOptionalAttribute (document, "restoreAnyVersion", restoreAnyVersion, "true");
			UpdateOptionalAttribute (document, "supportsRtl", supportsRtl, "true");
			UpdateOptionalAttribute (document, "testOnly", testOnly, "true");
			UpdateOptionalAttribute (document, "vmSafeMode", vmSafeMode, "true");
		}
		#endregion
	}
}