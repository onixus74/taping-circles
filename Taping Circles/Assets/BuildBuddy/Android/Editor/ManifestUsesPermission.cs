using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System;

namespace BuildBuddy {
	[Serializable]
	public sealed class ManifestUsesPermission : ManifestElement{

		private static readonly string[] permissionArray = {"NONE", "ACCESS_CHECKIN_PROPERTIES", "ACCESS_COARSE_LOCATION", "ACCESS_FINE_LOCATION", "ACCESS_LOCATION_EXTRA_COMMANDS", "ACCESS_MOCK_LOCATION", "ACCESS_NETWORK_STATE", "ACCESS_SURFACE_FLINGER", "ACCESS_WIFI_STATE", "ACCOUNT_MANAGER", "ADD_VOICEMAIL", "AUTHENTICATE_ACCOUNTS", "BATTERY_STATS", "BIND_ACCESSIBILITY_SERVICE", "BIND_APPWIDGET", "BIND_DEVICE_ADMIN", "BIND_INPUT_METHOD", "BIND_NFC_SERVICE", "BIND_NOTIFICATION_LISTENER_SERVICE", "BIND_PRINT_SERVICE", "BIND_REMOTEVIEWS", "BIND_TEXT_SERVICE", "BIND_VPN_SERVICE", "BIND_WALLPAPER", "BLUETOOTH", "BLUETOOTH_ADMIN", "BLUETOOTH_PRIVILEGED", "BRICK", "BROADCAST_PACKAGE_REMOVED", "BROADCAST_SMS", "BROADCAST_STICKY", "BROADCAST_WAP_PUSH", "CALL_PHONE", "CALL_PRIVILEGED", "CAMERA", "CAPTURE_AUDIO_OUTPUT", "CAPTURE_SECURE_VIDEO_OUTPUT", "CAPTURE_VIDEO_OUTPUT", "CHANGE_COMPONENT_ENABLED_STATE", "CHANGE_CONFIGURATION", "CHANGE_NETWORK_STATE", "CHANGE_WIFI_MULTICAST_STATE", "CHANGE_WIFI_STATE", "CLEAR_APP_CACHE", "CLEAR_APP_USER_DATA", "CONTROL_LOCATION_UPDATES", "DELETE_CACHE_FILES", "DELETE_PACKAGES", "DEVICE_POWER", "DIAGNOSTIC", "DISABLE_KEYGUARD", "DUMP", "EXPAND_STATUS_BAR", "FACTORY_TEST", "FLASHLIGHT", "FORCE_BACK", "GET_ACCOUNTS", "GET_PACKAGE_SIZE", "GET_TASKS", "GET_TOP_ACTIVITY_INFO", "GLOBAL_SEARCH", "HARDWARE_TEST", "INJECT_EVENTS", "INSTALL_LOCATION_PROVIDER", "INSTALL_PACKAGES", "INSTALL_SHORTCUT", "INTERNAL_SYSTEM_WINDOW", "INTERNET", "KILL_BACKGROUND_PROCESSES", "LOCATION_HARDWARE", "MANAGE_ACCOUNTS", "MANAGE_APP_TOKENS", "MANAGE_DOCUMENTS", "MASTER_CLEAR", "MEDIA_CONTENT_CONTROL", "MODIFY_AUDIO_SETTINGS", "MODIFY_PHONE_STATE", "MOUNT_FORMAT_FILESYSTEMS", "MOUNT_UNMOUNT_FILESYSTEMS", "NFC", "PERSISTENT_ACTIVITY", "PROCESS_OUTGOING_CALLS", "READ_CALENDAR", "READ_CALL_LOG", "READ_CONTACTS", "READ_EXTERNAL_STORAGE", "READ_FRAME_BUFFER", "READ_HISTORY_BOOKMARKS", "READ_INPUT_STATE", "READ_LOGS", "READ_PHONE_STATE", "READ_PROFILE", "READ_SMS", "READ_SOCIAL_STREAM", "READ_SYNC_SETTINGS", "READ_SYNC_STATS", "READ_USER_DICTIONARY", "REBOOT", "RECEIVE_BOOT_COMPLETED", "RECEIVE_MMS", "RECEIVE_SMS", "RECEIVE_WAP_PUSH", "RECORD_AUDIO", "REORDER_TASKS", "RESTART_PACKAGES", "SEND_RESPOND_VIA_MESSAGE", "SEND_SMS", "SET_ACTIVITY_WATCHER", "SET_ALARM", "SET_ALWAYS_FINISH", "SET_ANIMATION_SCALE", "SET_DEBUG_APP", "SET_ORIENTATION", "SET_POINTER_SPEED", "SET_PREFERRED_APPLICATIONS", "SET_PROCESS_LIMIT", "SET_TIME", "SET_TIME_ZONE", "SET_WALLPAPER", "SET_WALLPAPER_HINTS", "SIGNAL_PERSISTENT_PROCESSES", "STATUS_BAR", "SUBSCRIBED_FEEDS_READ", "SUBSCRIBED_FEEDS_WRITE", "SYSTEM_ALERT_WINDOW", "TRANSMIT_IR", "UNINSTALL_SHORTCUT", "UPDATE_DEVICE_STATS", "USE_CREDENTIALS", "USE_SIP", "VIBRATE", "WAKE_LOCK", "WRITE_APN_SETTINGS", "WRITE_CALENDAR", "WRITE_CALL_LOG", "WRITE_CONTACTS", "WRITE_EXTERNAL_STORAGE", "WRITE_GSERVICES", "WRITE_HISTORY_BOOKMARKS", "WRITE_PROFILE", "WRITE_SECURE_SETTINGS", "WRITE_SETTINGS", "WRITE_SMS", "WRITE_SOCIAL_STREAM", "WRITE_SYNC_SETTINGS", "WRITE_USER_DICTIONARY"};
		private static readonly int maxAndroidSdk = 19;
		private static readonly int minAndroidSdk = 9;

		[SerializeField]private int maxSDKVersion = maxAndroidSdk;
		[SerializeField] private bool display;

		[SerializeField]private bool standardPermission = true;

		//Constructed by editor window
		public static ManifestUsesPermission CreateInstance() { 
			ManifestUsesPermission usesPermission = ScriptableObject.CreateInstance<ManifestUsesPermission> ();
			usesPermission.node = null;
			usesPermission.elementEditStatus = EditStatus.EDITED;
			usesPermission.name = permissionArray[0];
			return usesPermission;
		}
		//Constructed from existing entry in AndroidManifest
		public static ManifestUsesPermission CreateInstance(XmlNode node) {
			ManifestUsesPermission usesPermission = ScriptableObject.CreateInstance<ManifestUsesPermission> ();
			usesPermission.node = (XmlElement)node;
			usesPermission.elementEditStatus = EditStatus.NONE;
			usesPermission.Initialize();
			return usesPermission;
		}
		public override void OnGUI() {
			display = EditorGUILayout.Foldout (display, "Uses-Permission: " + name);
			if (!display)
				return;	
			EditorGUI.BeginChangeCheck ();
			GUILayout.BeginHorizontal (); 
			{
				GUILayout.Label ("Permission: ");
				if (standardPermission) {
					if (Array.IndexOf (permissionArray, name) == -1) {
						name = permissionArray[0];
					} 
					name = permissionArray [EditorGUILayout.Popup (Array.IndexOf (permissionArray, name), permissionArray)];
				}
				else {
					name = EditorGUILayout.TextField(name);
				}
			}
			GUILayout.EndHorizontal ();
			standardPermission = EditorGUILayout.Toggle ("Standard Permission: ", standardPermission);
			BBGuiHelper.BeginIndent();
			{
				GUILayout.BeginHorizontal (); 
				{
					maxSDKVersion = EditorGUILayout.IntSlider ("Max SDK Version: ", maxSDKVersion, minAndroidSdk, maxAndroidSdk);
					if (EditorGUI.EndChangeCheck ()) {
						elementEditStatus = EditStatus.EDITED;
					}
					if (GUILayout.Button ("Remove")) {
						elementEditStatus = EditStatus.REMOVED;
					}
				}
				GUILayout.EndHorizontal ();
			}
			BBGuiHelper.EndIndent();
		}

		private void Initialize() {
			if (node.HasAttribute ("android:name")) {
				string permissionPrefix = "android.permission.";
				foreach (String standardPermission in permissionArray) {
					if (node.Attributes["android:name"].Value.Equals (permissionPrefix + standardPermission)) {
						name = standardPermission;
					}
				}
				if (name.Equals("")) {
					name =  node.Attributes["android:name"].Value;
				}
			}
			if (node.HasAttribute ("android:maxSdkVersion")) {
				maxSDKVersion = Convert.ToInt32(node.Attributes["android:maxSdkVersion"].Value); 
			}
		}
		private string PermissionToString() {
			return "android.permission." + name;
		}
		#region override
		protected override void CreateNode(XmlDocument document) {
			node = document.CreateElement ("uses-permission");
			XmlNode manifestElement = document.GetElementsByTagName ("manifest") [0];
			manifestElement.AppendChild (node);
		}
		protected override void UpdateAttributes(XmlDocument document) {
			CreateAndroidAttribute (document, "name", PermissionToString ());
			if (maxSDKVersion != maxAndroidSdk) {
				CreateAndroidAttribute (document, "maxSdkVersion", "" + maxSDKVersion);
			} 
			else if (maxSDKVersion == maxAndroidSdk && node.HasAttribute ("android:maxSdkVersion")) {
				node.RemoveAttribute ("android:maxSdkVersion");
			}
		}
		#endregion
	}
}
