/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

using UnityEngine;
using System.IO;
using System;
using Soomla;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Grow.Highway
{

	#if UNITY_EDITOR
	[InitializeOnLoad]
	#endif
	/// <summary>
	/// This class holds the store's configurations.
	/// </summary>
	public class HighwaySettings : ISoomlaSettings
	{

		private static string HighwaySettingsPrefix = "Highway";

		#if UNITY_EDITOR
		static HighwaySettings instance = new HighwaySettings();

		static string currentModuleVersion = "2.1.3";

		static HighwaySettings()
		{
			SoomlaEditorScript.addSettings(instance);

			SoomlaEditorScript.addFileList("Highway", "Assets/Soomla/highway_file_list", new string[] {});
		}

		GUIContent highwayGameKeyLabel = new GUIContent("Game Key [?]:", "The GROW Highway game key for your game");
		GUIContent highwayEnvKeyLabel = new GUIContent("Env Key [?]:", "The GROW Highway environment key for your game");
		GUIContent frameworkVersion = new GUIContent("Highway Version [?]", "The GROW Framework Highway Module version. ");

		public void OnEnable() {
			// No enabling, leave empty and let StoreManifestTools do the work
		}

		public void OnModuleGUI() {
		}

		public void OnInfoGUI() {
			SoomlaEditorScript.RemoveSoomlaModuleButton(frameworkVersion, currentModuleVersion, "Highway");
			SoomlaEditorScript.LatestVersionField("unity3d-highway", currentModuleVersion, "New version available!", "http://library.soom.la/fetch/unity3d-highway/latest?cf=unity");

			EditorGUILayout.Space();
		}

		public void OnSoomlaGUI() {
			///
			/// Create Highway Game key and Env key labels and text fields
			///
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(highwayGameKeyLabel,  GUILayout.Width(150), SoomlaEditorScript.FieldHeight);
			HighwayGameKey = EditorGUILayout.TextField(HighwayGameKey, SoomlaEditorScript.FieldHeight);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(highwayEnvKeyLabel,  GUILayout.Width(150), SoomlaEditorScript.FieldHeight);
			HighwayEnvKey = EditorGUILayout.TextField(HighwayEnvKey, SoomlaEditorScript.FieldHeight);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}

		public void OnIOSGUI() {

		}
		
		public void OnAndroidGUI() {

		}
		
		
		public void OnWP8GUI() {

		}

		#endif

		public static string HIGHWAY_GAME_KEY_DEFAULT_MESSAGE = "[YOUR GAME KEY]";

		public static string HighwayGameKey
		{
			get {
				string value = SoomlaEditorScript.GetConfigValue(HighwaySettingsPrefix, "HighwayGameKey");
				return value != null ? value : HIGHWAY_GAME_KEY_DEFAULT_MESSAGE;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(HighwaySettingsPrefix, "HighwayGameKey");
				if (v != value)
				{
					SoomlaEditorScript.SetConfigValue(HighwaySettingsPrefix, "HighwayGameKey", value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}

		public static string HIGHWAY_ENV_KEY_DEFAULT_MESSAGE = "[YOUR ENV KEY]";

		public static string HighwayEnvKey
		{
			get {
				string value =  SoomlaEditorScript.GetConfigValue(HighwaySettingsPrefix, "HighwayEnvKey");
				return value != null ? value : HIGHWAY_ENV_KEY_DEFAULT_MESSAGE;
			}
			set
			{
				string v = SoomlaEditorScript.GetConfigValue(HighwaySettingsPrefix, "HighwayEnvKey");
				if (v != value)
				{
					SoomlaEditorScript.SetConfigValue(HighwaySettingsPrefix, "HighwayEnvKey", value);
					SoomlaEditorScript.DirtyEditor ();
				}
			}
		}
	}
}
