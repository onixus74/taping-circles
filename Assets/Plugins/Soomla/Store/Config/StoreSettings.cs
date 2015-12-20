/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Soomla.Store
{

#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	/// <summary>
	/// This class holds the store's configurations.
	/// </summary>
	public class StoreSettings : ISoomlaSettings
	{

		private static string StoreModulePrefix = "Store";

#if UNITY_EDITOR

		static StoreSettings instance = new StoreSettings();

		static string currentModuleVersion = "1.9.3";

		static StoreSettings()
		{
			SoomlaEditorScript.addSettings(instance);

			List<string> additionalDependFiles = new List<string>(); //Add files that not tracked in file_list
			additionalDependFiles.Add("Assets/Plugins/Android/Soomla/libs/AndroidStoreAmazon.jar");
			additionalDependFiles.Add("Assets/Plugins/Android/Soomla/libs/in-app-purchasing-2.0.1.jar");
			additionalDependFiles.Add("Assets/Plugins/Android/Soomla/libs/AndroidStoreGooglePlay.jar");
			SoomlaEditorScript.addFileList("Store", "Assets/Soomla/store_file_list", additionalDependFiles.ToArray());
		}


		GUIContent noneBPLabel = new GUIContent("You have your own Billing Service");
		GUIContent playLabel = new GUIContent("Google Play");
		GUIContent playSsvLabel = new GUIContent("Fraud Protection [?]:", "Check if you want to turn on purchases verification with SOOMLA Fraud Protection Service.");
		GUIContent playClientIdLabel = new GUIContent("Client ID");
		GUIContent playClientSecretLabel = new GUIContent("Client Secret");
		GUIContent playRefreshTokenLabel = new GUIContent("Refresh Token");
		GUIContent playVerifyOnServerFailureLabel = new GUIContent("Verify On Server Failure [?]:", "Check if you want your purchases get validated if server failure happens.");


		GUIContent amazonLabel = new GUIContent("Amazon");
		GUIContent publicKeyLabel = new GUIContent("API Key [?]:", "The API key from Google Play dev console (just in case you're using Google Play as billing provider).");
		GUIContent testPurchasesLabel = new GUIContent("Test Purchases [?]:", "Check if you want to allow purchases of Google's test product ids.");
		GUIContent packageNameLabel = new GUIContent("Package Name [?]", "Your package as defined in Unity.");
		GUIContent wp8SimulatorModeLabel = new GUIContent("Run in Simulator (x86 build)");
		GUIContent wp8TestModeLabel = new GUIContent("Simulate Store. (Don't forget to adapt IAPMock.xml to fit your IAPs)");

		GUIContent iosSsvLabel = new GUIContent("Fraud Protection [?]:", "Check if you want to turn on purchases verification with SOOMLA Fraud Protection Service.");
    	GUIContent iosVerifyOnServerFailureLabel = new GUIContent("Verify On Server Failure [?]:", "Check if you want your purchases get validated if server failure happens.");

		GUIContent frameworkVersion = new GUIContent("Store Version [?]", "The SOOMLA Framework Store Module version. ");

		public void OnEnable() {
			// Generating AndroidManifest.xml
//			ManifestTools.GenerateManifest();
		}

		public void OnModuleGUI() {

		}

		public void OnInfoGUI() {
			SoomlaEditorScript.RemoveSoomlaModuleButton(frameworkVersion, currentModuleVersion, "Store");
			SoomlaEditorScript.LatestVersionField ("unity3d-store", currentModuleVersion, "New version available!", "http://library.soom.la/fetch/unity3d-store-only/latest?cf=unity");
			EditorGUILayout.Space();
		}

		public void OnSoomlaGUI() {

		}

		public void OnIOSGUI()
		{
			EditorGUILayout.HelpBox("Store Settings", MessageType.None);
			
			IosSSV = EditorGUILayout.Toggle(iosSsvLabel, IosSSV);
			if (IosSSV) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField(SoomlaEditorScript.EmptyContent, SoomlaEditorScript.SpaceWidth, SoomlaEditorScript.FieldHeight);
				IosVerifyOnServerFailure = EditorGUILayout.Toggle(iosVerifyOnServerFailureLabel, IosVerifyOnServerFailure);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Space();
		}

		public void OnAndroidGUI()
		{
			EditorGUILayout.HelpBox("Store Settings", MessageType.None);

			EditorGUILayout.BeginHorizontal();
			SoomlaEditorScript.SelectableLabelField(packageNameLabel, PlayerSettings.bundleIdentifier);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Billing Service Selection", MessageType.None);

			if (!GPlayBP && !AmazonBP && !NoneBP) {
					GPlayBP = true;
			}

			NoneBP = EditorGUILayout.ToggleLeft(noneBPLabel, NoneBP);

			bool update;
			bpUpdate.TryGetValue("none", out update);
			if (NoneBP && !update) {
				setCurrentBPUpdate("none");

				AmazonBP = false;
				GPlayBP = false;
				SoomlaManifestTools.GenerateManifest();
				handlePlayBPJars(true);
				handleAmazonBPJars(true);
				}


			GPlayBP = EditorGUILayout.ToggleLeft(playLabel, GPlayBP);

			if (GPlayBP) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField(publicKeyLabel, SoomlaEditorScript.FieldWidth, SoomlaEditorScript.FieldHeight);
				AndroidPublicKey = EditorGUILayout.TextField(AndroidPublicKey, SoomlaEditorScript.FieldHeight);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(SoomlaEditorScript.EmptyContent, SoomlaEditorScript.SpaceWidth, SoomlaEditorScript.FieldHeight);
				AndroidTestPurchases = EditorGUILayout.Toggle(testPurchasesLabel, AndroidTestPurchases);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(SoomlaEditorScript.EmptyContent, SoomlaEditorScript.SpaceWidth, SoomlaEditorScript.FieldHeight);
				PlaySsvValidation = EditorGUILayout.Toggle(playSsvLabel, PlaySsvValidation);
				EditorGUILayout.EndHorizontal();

				if (PlaySsvValidation) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					EditorGUILayout.LabelField(playClientIdLabel, SoomlaEditorScript.FieldWidth, SoomlaEditorScript.FieldHeight);
					PlayClientId = EditorGUILayout.TextField(PlayClientId, SoomlaEditorScript.FieldHeight);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					EditorGUILayout.LabelField(playClientSecretLabel, SoomlaEditorScript.FieldWidth, SoomlaEditorScript.FieldHeight);
					PlayClientSecret = EditorGUILayout.TextField(PlayClientSecret, SoomlaEditorScript.FieldHeight);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					EditorGUILayout.LabelField(playRefreshTokenLabel, SoomlaEditorScript.FieldWidth, SoomlaEditorScript.FieldHeight);
					PlayRefreshToken = EditorGUILayout.TextField(PlayRefreshToken, SoomlaEditorScript.FieldHeight);
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					EditorGUILayout.LabelField(SoomlaEditorScript.EmptyContent, SoomlaEditorScript.SpaceWidth, SoomlaEditorScript.FieldHeight);
					PlayVerifyOnServerFailure = EditorGUILayout.Toggle(playVerifyOnServerFailureLabel, PlayVerifyOnServerFailure);
					EditorGUILayout.EndHorizontal();
				}
			}

			bpUpdate.TryGetValue("play", out update);
			if (GPlayBP && !update) {
				setCurrentBPUpdate("play");

				AmazonBP = false;
				NoneBP = false;
				SoomlaManifestTools.GenerateManifest();
				handlePlayBPJars(false);
				handleAmazonBPJars(true);
			}


			AmazonBP = EditorGUILayout.ToggleLeft(amazonLabel, AmazonBP);
			bpUpdate.TryGetValue("amazon", out update);
			if (AmazonBP && !update) {
				setCurrentBPUpdate("amazon");

				GPlayBP = false;
				NoneBP = false;
				SoomlaManifestTools.GenerateManifest();
				handlePlayBPJars(true);
				handleAmazonBPJars(false);
			}
			EditorGUILayout.Space();
		}


        public void OnWP8GUI()
        {
			EditorGUILayout.HelpBox("Store Settings", MessageType.None);

			WP8SimulatorBuild = EditorGUILayout.ToggleLeft(wp8SimulatorModeLabel, WP8SimulatorBuild);
			EditorGUILayout.Space();
			WP8TestMode = EditorGUILayout.ToggleLeft(wp8TestModeLabel, WP8TestMode);
         
			EditorGUILayout.Space();
        }




		/** Billing Providers util functions **/

		private void setCurrentBPUpdate(string bpKey) {
			bpUpdate[bpKey] = true;
			var buffer = new List<string>(bpUpdate.Keys);
			foreach(string key in buffer) {
				if (key != bpKey) {
					bpUpdate[key] = false;
				}
			}
		}

		private Dictionary<string, bool> bpUpdate = new Dictionary<string, bool>();
		private static string bpRootPath = Application.dataPath + "/WebPlayerTemplates/SoomlaConfig/android/android-billing-services/";
        private static string wp8RootPath = Application.dataPath + "/WebPlayerTemplates/SoomlaConfig/wp8/";

		public static void handlePlayBPJars(bool remove) {
			try {
				if (remove) {
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/Soomla/libs/AndroidStoreGooglePlay.jar");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/Soomla/libs/AndroidStoreGooglePlay.jar.meta");
				} else {
					FileUtil.CopyFileOrDirectory(bpRootPath + "google-play/AndroidStoreGooglePlay.jar",
					                             Application.dataPath + "/Plugins/Android/Soomla/libs/AndroidStoreGooglePlay.jar");
				}
			}catch {}
		}

		public static void handleAmazonBPJars(bool remove) {
			try {
				if (remove) {
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/Soomla/libs/AndroidStoreAmazon.jar");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/Soomla/libs/AndroidStoreAmazon.jar.meta");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/Soomla/libs/in-app-purchasing-2.0.1.jar");
					FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/Android/Soomla/libs/in-app-purchasing-2.0.1.jar.meta");
				} else {
					FileUtil.CopyFileOrDirectory(bpRootPath + "amazon/AndroidStoreAmazon.jar",
					                             Application.dataPath + "/Plugins/Android/Soomla/libs/AndroidStoreAmazon.jar");
					FileUtil.CopyFileOrDirectory(bpRootPath + "amazon/in-app-purchasing-2.0.1.jar",
					                             Application.dataPath + "/Plugins/Android/Soomla/libs/in-app-purchasing-2.0.1.jar");
				}
			}catch {}
		}



#endif







		/** Store Specific Variables **/


		public static string AND_PUB_KEY_DEFAULT = "YOUR GOOGLE PLAY PUBLIC KEY";

		public static string PLAY_CLIENT_ID_DEFAULT = "YOUR CLIENT ID";
		public static string PLAY_CLIENT_SECRET_DEFAULT = "YOUR CLIENT SECRET";
		public static string PLAY_REFRESH_TOKEN_DEFAULT = "YOUR REFRESH TOKEN";


		public static string AndroidPublicKey
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "AndroidPublicKey");
				return value != null ? value : AND_PUB_KEY_DEFAULT;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "AndroidPublicKey");
				if (v != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "AndroidPublicKey", value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static string PlayClientId
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "PlayClientId");
				return value != null ? value : PLAY_CLIENT_ID_DEFAULT;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "PlayClientId");
				if (v != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "PlayClientId", value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static string PlayClientSecret
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "PlayClientSecret");
				return value != null ? value : PLAY_CLIENT_SECRET_DEFAULT;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "PlayClientSecret");
				if (v != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "PlayClientSecret", value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static string PlayRefreshToken
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "PlayRefreshToken");
				return value != null ? value : PLAY_REFRESH_TOKEN_DEFAULT;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "PlayRefreshToken");
				if (v != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "PlayRefreshToken", value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool PlayVerifyOnServerFailure
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "PlayVerifyOnServerFailure");
				return value != null ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "PlayVerifyOnServerFailure");
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "PlayVerifyOnServerFailure", value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool AndroidTestPurchases
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "AndroidTestPurchases");
				return value != null ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "AndroidTestPurchases");
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "AndroidTestPurchases", value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool PlaySsvValidation
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "PlaySsvValidation");
				return value != null ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "PlaySsvValidation");
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "PlaySsvValidation", value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool IosSSV
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "IosSSV");
				return value != null ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "IosSSV");
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "IosSSV", value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool IosVerifyOnServerFailure
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "IosVerifyOnServerFailure");
				return value != null ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "IosVerifyOnServerFailure");
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "IosVerifyOnServerFailure", value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

    	public static bool NoneBP
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "NoneBP");
				return value != null ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "NoneBP");
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "NoneBP", value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool GPlayBP
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "GPlayBP");
				return value != null ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "GPlayBP");
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "GPlayBP", value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static bool AmazonBP
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "AmazonBP");
				return value != null ? Convert.ToBoolean(value) : false;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "AmazonBP");
				if (Convert.ToBoolean(v) != value)
				{
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "AmazonBP", value.ToString());
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

        public static bool WP8SimulatorBuild
        {
            get
            {
				string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "WP8SimulatorBuild");
                return value != null ? Convert.ToBoolean(value) : false;
            }
            set
            {
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "WP8SimulatorBuild");
                if (Convert.ToBoolean(v) != value)
                {
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "WP8SimulatorBuild", value.ToString());
                    SoomlaEditorScript.DirtyEditor();
#if UNITY_EDITOR
                    if (value == true)
                    {
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/sqlite3.dll");
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/Sqlite.dll");
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/Sqlite.winmd");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "x86/sqlite3.soomladll",Application.dataPath + "/Plugins/WP8/sqlite3.dll");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "x86/Sqlite.soomladll",Application.dataPath + "/Plugins/WP8/Sqlite.dll");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "x86/Sqlite.soomlawinmd",Application.dataPath + "/Plugins/WP8/Sqlite.winmd");
                    }
                    else
                    {
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/sqlite3.dll");
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/Sqlite.dll");
                        FileUtil.DeleteFileOrDirectory(Application.dataPath + "/Plugins/WP8/Sqlite.winmd");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "ARM/sqlite3.soomladll",Application.dataPath + "/Plugins/WP8/sqlite3.dll");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "ARM/Sqlite.soomlawinmd",Application.dataPath + "/Plugins/WP8/Sqlite.winmd");
                        FileUtil.CopyFileOrDirectory(wp8RootPath + "ARM/Sqlite.soomladll",Application.dataPath + "/Plugins/WP8/Sqlite.dll");
                    }
#endif
                }
            }
        }

        public static bool WP8TestMode
        {
            get
            {
                string value = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "WP8TestMode");
                return value != null ? Convert.ToBoolean(value) : false;
            }
            set
            {
				string v = SoomlaEditorScript.GetConfigValue(StoreModulePrefix, "WP8TestMode");
                if (Convert.ToBoolean(v) != value)
                {
					SoomlaEditorScript.SetConfigValue(StoreModulePrefix, "WP8TestMode", value.ToString());
                    SoomlaEditorScript.DirtyEditor();
                }
            }
        }
	}
}
