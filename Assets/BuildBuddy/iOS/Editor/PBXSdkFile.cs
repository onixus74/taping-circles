using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using System.IO;
using System.Linq;

namespace BuildBuddy {
	public class PbxSdkFile : PBXFile  {

		private const string sdkPath = "/Applications/Xcode.app/Contents/Developer/Platforms/";
		private const string iphoneSdkPath = "iPhoneOS.platform/Developer/SDKs/";
		private const string simulatorSdkPath = "iPhoneSimulator.platform/Developer/SDKs/";

		private const string FRAMEWORK_PATH = "System/Library/Frameworks/";
		private const string LIBRARY_PATH = "usr/lib/";

		public static string[] sdkFiles;

		private string[] currentSdkFiles;
		private string sdkFileFilter = "";
		private string currentSdkFile;

		private int frameworkIndex;
		private bool displayFramework;

		public static PbxSdkFile CreateInstance() {
			PbxSdkFile framework = ScriptableObject.CreateInstance<PbxSdkFile> () as PbxSdkFile;
			framework.group = "Frameworks";
			framework.edited = true;
			if (sdkFiles == null) {
				Debug.LogError ("iOS SDK not found");
				framework.removed = true;
				return framework;
			}
			framework.currentSdkFiles = sdkFiles;
			framework.currentSdkFile = sdkFiles [0];
			framework.name = framework.currentSdkFile;
			return framework;
		}
		public static new PbxSdkFile CreateInstance(XmlElement element) {
			PbxSdkFile framework = ScriptableObject.CreateInstance<PbxSdkFile> () as PbxSdkFile;
			if (sdkFiles == null) {
				try {
					LoadDeviceFrameworks();
				} catch (DirectoryNotFoundException) {
					Debug.LogWarning ("Coulnd't find iOS SDK");
				}
			}
			framework.fileRefID = PBXEditor.GenerateID ();
			framework.name = element.Attributes ["name"].Value;
			framework.frameworkIndex = System.Array.IndexOf (sdkFiles, framework.name);
			if (framework.frameworkIndex == -1) {
				Debug.LogWarning ("Could not find "+ framework.name + " in iOS SDK.");
				framework.removed = true;
			}
			framework.currentSdkFile = sdkFiles [framework.frameworkIndex];
			framework.currentSdkFiles = sdkFiles;
			framework.group = element.Attributes ["group"].Value;
			if (framework.name.Contains (".framework"))
				framework.relativePath = FRAMEWORK_PATH + framework.name;
			else 
				framework.relativePath = LIBRARY_PATH + framework.name;
			framework.sourceTree = "SDKROOT";
			framework.absolutePath = "($" + framework.sourceTree + ")" + framework.relativePath;
			if (element.HasAttribute("required")) {
				framework.required = System.Convert.ToBoolean (element.Attributes["required"].Value);
				if (!framework.required)
					framework.optionalSettings.Add ("ATTRIBUTES = (Weak, ); ");
			}
			framework.AssignBuildPhase(System.IO.Path.GetExtension(framework.relativePath));
			return framework;
		}
		public override PBXFile Clone() {
			PbxSdkFile file = PbxSdkFile.CreateInstance ();
			file.currentSdkFile = this.currentSdkFile;
			file.name = this.name;
			file.absolutePath = this.absolutePath;

			return file;
		}
		public static void LoadDeviceFrameworks() {
			string path = sdkPath + iphoneSdkPath;
			string[] sdkVersionArray = Directory.GetDirectories (path);
			string frameworkPath = sdkVersionArray.Last () + "/" + FRAMEWORK_PATH;
			string libsPath = sdkVersionArray.Last () + "/" + LIBRARY_PATH;
			string[] deviceFrameworks = Directory.GetDirectories (frameworkPath).Select (p => Path.GetFileName(p)).ToArray();
			string[] deviceLibs = Directory.GetFiles (libsPath).Where (p => Path.GetExtension(p).Equals(".dylib")).Select (p => Path.GetFileName (p)).ToArray ();
			sdkFiles = new string[deviceFrameworks.Length + deviceLibs.Length];
			deviceFrameworks.CopyTo (sdkFiles, 0);
			deviceLibs.CopyTo (sdkFiles, deviceFrameworks.Length);
			System.Array.Sort (sdkFiles);
		}
		public override void OnGUI () {
			if (removed)
				return;
			EditorGUILayout.BeginHorizontal ();
			{
				displayFramework = EditorGUILayout.Foldout (displayFramework, currentSdkFile);
				if (GUILayout.Button ("Remove")) {
					removed = true;
				}
			}
			EditorGUILayout.EndHorizontal ();
			if (displayFramework) {
				EditorGUI.BeginChangeCheck();
				BBGuiHelper.BeginIndent ();
				{
					EditorGUILayout.BeginHorizontal();
					{
						sdkFileFilter = EditorGUILayout.TextField (sdkFileFilter);
						currentSdkFiles = sdkFiles.Where (p => p.IndexOf (sdkFileFilter,  System.StringComparison.CurrentCultureIgnoreCase) >= 0).ToArray();
						frameworkIndex = EditorGUILayout.Popup (Mathf.Max (System.Array.IndexOf (currentSdkFiles, currentSdkFile), 0), currentSdkFiles, GUILayout.Width (200));
						try {
							currentSdkFile = currentSdkFiles[frameworkIndex];
						} catch (System.IndexOutOfRangeException) {
							currentSdkFile = sdkFiles[0];
						}
						if (frameworkIndex >= currentSdkFiles.Length)
							frameworkIndex = 0;
					}
					EditorGUILayout.EndHorizontal();
					required = EditorGUILayout.Toggle ("Required", required);
				}
				BBGuiHelper.EndIndent ();
				if (EditorGUI.EndChangeCheck()) {
					edited = true;
					name = currentSdkFile;
				}
			}
		}
		public override void Serialize(XmlDocument document) {
			if (frameworkIndex > currentSdkFiles.Length) {
				Debug.LogError ("Could not find an Sdk file containing token " + currentSdkFiles);
				return;
			}
			XmlElement element = document.CreateElement ("pbxfile");
			element.SetAttribute ("name", currentSdkFiles[frameworkIndex]);
			element.SetAttribute("path", XcodeSerializer.FRAMEWORK_PATH_TOKEN);
			element.SetAttribute ("group", group);
			element.SetAttribute ("required", ""+required);
			XmlNode parent = document.GetElementsByTagName ("pbxproj")[0];
			parent.AppendChild (element);
		}
	}
}