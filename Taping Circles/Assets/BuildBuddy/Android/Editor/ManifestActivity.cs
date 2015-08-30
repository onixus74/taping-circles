using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using System;

namespace BuildBuddy {
	[Serializable]
	public sealed class ManifestActivity : ManifestElement{

		private static readonly string[] configChangesArray = {
						"mcc",
						"mnc",
						"locale",
						"touchscreen",
						"keyboard",
						"keyboardHidden",
						"navigation",
						"screenLayout",
						"fontScale",
						"uiMode",
						"orientation",
						"screenSize",
						"smallestScreenSize"
				};
		private static readonly string[] launchModeArray = {"standard", "singleTop", "singleTask", "singleInstance" };
		private static readonly string[] screenOrientationArray = {
						"unspecified",
						"behind",
						"landscape",
						"portrait",
						"reverseLandscape",
						"reversePortrait",
						"sensorLandscape",
						"sensorPortrait",
						"userLandsacpe",
						"userPortrait",
						"sensor",
						"fullSensor",
						"nosensor",
						"user",
						"fullUser",
						"locked"
				};
		private static readonly string[] uiOptionsArray = {"none", "splitActionBarWhenNarrow" };
		private static readonly string[] windowSoftInputModeArray = {
						"stateUnspecified",
						"stateUnchanged",
						"stateHidden",
						"stateAlwaysHidden",
						"stateVisible",
						"stateAlwaysVisible",
						"adjustUnspecified",
						"adjustResize",
						"adjustPan"
				};


		[SerializeField]private List<ManifestIntentFilter> intentFilterList = new List<ManifestIntentFilter> ();
		[SerializeField]private List<ManifestMetaData> metaDataList = new List<ManifestMetaData>();
		[SerializeField]private bool displayIntentFilter;
		[SerializeField]private bool displayMetaData;
		[SerializeField]private bool displayAttributes;

		[SerializeField]private bool allowTaskReparenting;
		[SerializeField]private bool alwaysRetainTaskState;
		[SerializeField]private bool clearTaskOnLaunch;
		[SerializeField]private int configChanges;
		[SerializeField]private bool enabled = true;
		[SerializeField]private bool excludeFromRecents;
		[SerializeField]private bool exported = true;
		[SerializeField]private bool finishOnTaskLaunch;
		[SerializeField]private bool hardwareAccelerated;
		[SerializeField]private string icon = "";
		[SerializeField]private string label = "";
		[SerializeField]private string launchMode = launchModeArray[0];
		[SerializeField]private bool multiprocess;
		[SerializeField]private bool noHistory;
		[SerializeField]private string permission = "";
		[SerializeField]private string process = "";
		[SerializeField]private string screenOrientation = screenOrientationArray[0];
		[SerializeField]private bool stateNotNeeded;
		[SerializeField]private string taskAffinity = "";
		[SerializeField]private string theme = "";
		[SerializeField]private string uiOptions = uiOptionsArray[0];
		[SerializeField]private int windowSoftInputMode = 0;

		[SerializeField]private bool display;

		//Constructed by editor window
		public static ManifestActivity CreateInstance() { 
			ManifestActivity activity = ScriptableObject.CreateInstance<ManifestActivity> ();
			activity.node = null;
			activity.elementEditStatus = EditStatus.EDITED;
			return activity;
		}
		//Constructed from existing entry in AndroidManifest
		public static ManifestActivity CreateInstance(XmlNode node) {
			ManifestActivity activity = ScriptableObject.CreateInstance<ManifestActivity> ();
			activity.node = (XmlElement)node;
			activity.elementEditStatus = EditStatus.NONE;
			activity.Initialize();
			return activity;
		}
		public override void OnGUI() {
			bool changed = false;
			display = EditorGUILayout.Foldout (display, "Activity: " + (!display ? name : ""));
			if (display) {
				EditorGUI.BeginChangeCheck ();
				name = EditorGUILayout.TextField ("Name: ", name);
				changed |= EditorGUI.EndChangeCheck();
				#region attributes
				displayAttributes = EditorGUILayout.Foldout (displayAttributes, "Attributes: ");
				if (displayAttributes) {
					EditorGUI.BeginChangeCheck();
					BBGuiHelper.BeginIndent ();
					{
						allowTaskReparenting = EditorGUILayout.Toggle ("Allow Task Reparenting: ", allowTaskReparenting);
						alwaysRetainTaskState = EditorGUILayout.Toggle ("Always Retain Task State: ", alwaysRetainTaskState);
						clearTaskOnLaunch = EditorGUILayout.Toggle ("Clear Task On Launch: ", clearTaskOnLaunch);
						configChanges = EditorGUILayout.MaskField ("Config Changes: ", configChanges, configChangesArray);
						enabled = EditorGUILayout.Toggle ("Enabled: ", enabled);
						excludeFromRecents = EditorGUILayout.Toggle ("Exclude From Recents: ", excludeFromRecents);
						exported = EditorGUILayout.Toggle ("Exported: ", exported);
						hardwareAccelerated = EditorGUILayout.Toggle ("Hardware Accelerated: ", hardwareAccelerated);
						icon = EditorGUILayout.TextField ("Icon: ", icon);
						label = EditorGUILayout.TextField ("Label: ", label);
						launchMode = launchModeArray[EditorGUILayout.Popup ("Launch Mode: ", Array.IndexOf (launchModeArray, launchMode), launchModeArray)];
						multiprocess = EditorGUILayout.Toggle ("Multiprocess: ", multiprocess);
						noHistory = EditorGUILayout.Toggle ("No History: ", noHistory);
						permission = EditorGUILayout.TextField("Permission: ", permission);
						process = EditorGUILayout.TextField ("Process: ", process);
						screenOrientation = screenOrientationArray[EditorGUILayout.Popup("Screen Orientation: ", Array.IndexOf(screenOrientationArray, screenOrientation), screenOrientationArray)];
						stateNotNeeded = EditorGUILayout.Toggle ("State Not Needed", stateNotNeeded);
						taskAffinity = EditorGUILayout.TextField ("Task Affinity: ", taskAffinity);
						theme = EditorGUILayout.TextField ("Theme: ", theme);
						uiOptions = uiOptionsArray[EditorGUILayout.Popup("UI Options: ", Array.IndexOf(uiOptionsArray, uiOptions), uiOptionsArray)];
						windowSoftInputMode = EditorGUILayout.MaskField("Window Soft Input Mode: ", windowSoftInputMode, windowSoftInputModeArray);
					}
					BBGuiHelper.EndIndent();
					changed |= EditorGUI.EndChangeCheck();
				}
				#endregion
				#region intentfilter
				displayIntentFilter = EditorGUILayout.Foldout (displayIntentFilter, "Intent-Filters: ("+intentFilterList.Count+")");
				if (displayIntentFilter) {
					BBGuiHelper.BeginIndent ();
					{
						for (int i = 0; i < intentFilterList.Count; i++) {
							Undo.RecordObject (intentFilterList[i], "Intent-Filter");
							if (intentFilterList[i].ElementEditStatus != EditStatus.REMOVED) {
								intentFilterList[i].OnGUI ();
							}
							if (intentFilterList[i].ElementEditStatus != EditStatus.NONE) {
								changed = true;
							}
						}
					}
					BBGuiHelper.EndIndent ();
					GUILayout.BeginHorizontal();
					{
						EditorGUILayout.Space();
						if (GUILayout.Button ("New Intent-Filter", BBGuiHelper.ButtonWidth())) {
							intentFilterList.Add (ManifestIntentFilter.CreateInstance(node));
							changed = true;
						}
					}
					GUILayout.EndHorizontal();
				}
				#endregion
				#region metadata
				displayMetaData = EditorGUILayout.Foldout (displayMetaData, "Meta-Data: ("+metaDataList.Count+")");
				if (displayMetaData) {
					BBGuiHelper.BeginIndent ();
					{
						for (int i = 0; i < metaDataList.Count; i++) {
							Undo.RecordObject (metaDataList[i], "Meta-Data");
							if (metaDataList[i].ElementEditStatus != EditStatus.REMOVED) {
								metaDataList[i].OnGUI ();
							}
							if (metaDataList[i].ElementEditStatus != EditStatus.NONE) {
								changed = true;
							}
						}
					}
					BBGuiHelper.EndIndent ();
					GUILayout.BeginHorizontal();
					{
						EditorGUILayout.Space();
						if (GUILayout.Button ("New Meta-Data", BBGuiHelper.ButtonWidth())) {
							metaDataList.Add (ManifestMetaData.CreateInstance(node));
							changed = true;
						}
					}
					GUILayout.EndHorizontal();
				}
				#endregion
				GUILayout.BeginHorizontal ();
				{
					EditorGUILayout.Space();
					if (changed) {
						elementEditStatus = EditStatus.EDITED;
					}
					if (GUILayout.Button ("Remove Activity", BBGuiHelper.ButtonWidth())) {
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
			allowTaskReparenting = InitializeBoolAttribute ("android:allowTaskReparenting", false);
			alwaysRetainTaskState = InitializeBoolAttribute ("android:alwaysRetainTaskState", false);
			clearTaskOnLaunch= InitializeBoolAttribute ("android:clearTaskOnLaunch", false);
			InitializeConfigChanges ();
			enabled = InitializeBoolAttribute ("android:enabled", true);
			excludeFromRecents = InitializeBoolAttribute ("android:excludeFromRecents", false);
			exported = InitializeBoolAttribute ("android:exported", true);
			finishOnTaskLaunch = InitializeBoolAttribute ("android:finishOnTaskLaunch", false);
			hardwareAccelerated = InitializeBoolAttribute ("android:hardwareAccelerated", false);
			if (node.HasAttribute ("android:icon")) {
				icon = node.Attributes["android:icon"].Value;
			}
			if (node.HasAttribute ("android:label")) {
				label = node.Attributes["android:label"].Value;
			}
			if (node.HasAttribute ("android:launchMode")) {
				launchMode = node.Attributes["android:launchMode"].Value;
			}
			multiprocess = InitializeBoolAttribute ("android:multiprocess", false);
			noHistory = InitializeBoolAttribute ("android:noHistory", false);
			if (node.HasAttribute ("android:permission")) {
				permission = node.Attributes["android:permission"].Value;
			}
			if (node.HasAttribute ("android:process")) {
				process = node.Attributes["android:process"].Value;
			}
			if (node.HasAttribute ("android:screenOrientation")) {
				screenOrientation = node.Attributes["android:screenOrientation"].Value;
			}
			stateNotNeeded = InitializeBoolAttribute ("android:stateNotNeeded", false);
			if (node.HasAttribute ("android:taskAffinity")) {
				taskAffinity = node.Attributes["android:taskAffinity"].Value;
			}
			if (node.HasAttribute ("android:theme")) {
				theme = node.Attributes["android:theme"].Value;
			}
			if (node.HasAttribute ("android:uiOptions")) {
				uiOptions = node.Attributes["android:uiOptions"].Value;
			}
			InitializeWindowSoftInputMode ();
		}
		#region override
		protected override void CreateNode(XmlDocument document) {
			node = document.CreateElement ("activity");
			XmlNode applicationElement = document.GetElementsByTagName ("application") [0];
			applicationElement.AppendChild (node);
		}
		protected override void UpdateAttributes(XmlDocument document) {
			if (node.OwnerDocument != document) {
				CopyNode(document);
			}
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
			UpdateOptionalAttribute (document, "allowTaskReparenting", allowTaskReparenting, "true");
			UpdateOptionalAttribute (document, "alwaysRetainTaskState", alwaysRetainTaskState, "true");
			UpdateOptionalAttribute (document, "clearTaskOnLaunch", clearTaskOnLaunch, "true");
			UpdateOptionalAttribute (document, "configChanges", configChanges != 0, ConfigChangesToString ());
			UpdateOptionalAttribute (document, "enabled", !enabled, "false");
			UpdateOptionalAttribute (document, "excludeFromRecents", excludeFromRecents, "true");
			UpdateOptionalAttribute (document, "exported", !exported, "false");
			UpdateOptionalAttribute (document, "finishOnTaskLaunch", finishOnTaskLaunch, "true");
			UpdateOptionalAttribute (document, "hardwareAccelerated", hardwareAccelerated, "true");
			UpdateOptionalAttribute (document, "icon", !icon.Equals (""), icon);
			UpdateOptionalAttribute (document, "label", !label.Equals (""), label);
			UpdateOptionalAttribute (document, "launchMode", launchMode != launchModeArray[0], launchMode);
			UpdateOptionalAttribute (document, "multiprocess", multiprocess, "true");
			UpdateOptionalAttribute (document, "noHistory", noHistory, "true");
			UpdateOptionalAttribute (document, "permission", !permission.Equals (""), permission);
			UpdateOptionalAttribute (document, "process", !process.Equals (""), process);
			UpdateOptionalAttribute (document, "screenOrientation", screenOrientation != screenOrientationArray [0], screenOrientation);
			UpdateOptionalAttribute (document, "stateNotNeeded", stateNotNeeded, "true");
			UpdateOptionalAttribute (document, "taskAffinity", !taskAffinity.Equals (""), taskAffinity);
			UpdateOptionalAttribute (document, "theme", !theme.Equals (""), theme);
			UpdateOptionalAttribute (document, "uiOptions", !uiOptions.Equals (uiOptionsArray[0]), uiOptions);
			UpdateOptionalAttribute (document, "windowSoftInputMode", windowSoftInputMode != 0, WindowSoftInputModeToString());
		}
		#endregion

		private void InitializeConfigChanges() {
			configChanges = 0;
			if (!node.HasAttribute ("android:configChanges")) {
				return;
			}
			string[] changes = node.Attributes ["android:configChanges"].Value.Split ('|');
			foreach (string change in changes) {
				configChanges += ( 1 << Array.IndexOf (configChangesArray, change) );
			}
		}
		private string ConfigChangesToString() {
			string configChangesString = "";
			for (int i = 0; i < configChangesArray.Length; i++) {
				if ((configChanges & (1 << i)) != 0) {
					configChangesString += ("|"+configChangesArray[i]);
				}
			}
			if (configChangesString.Length > 0)
				return configChangesString.Substring(1);
			return configChangesString;
		}
		private void InitializeWindowSoftInputMode() {
			windowSoftInputMode = 0;
			if (!node.HasAttribute ("android:windowSoftInputMode")) {
				return;
			}
			string[] modes = node.Attributes ["android:windowSoftInputMode"].Value.Split ('|');
			foreach (string mode in modes) {
				windowSoftInputMode += ( 1 << Array.IndexOf (windowSoftInputModeArray, mode) );
			}
		}
		private string WindowSoftInputModeToString() {
			string modeString = "";
			for (int i = 0; i < windowSoftInputModeArray.Length; i++) {
				if ((windowSoftInputMode & (1 << i)) != 0) {
					modeString += ("|"+windowSoftInputModeArray[i]);
				}
			}
			if (modeString.Length > 0)
				return modeString.Substring(1);
			return modeString;
		}
	}
}