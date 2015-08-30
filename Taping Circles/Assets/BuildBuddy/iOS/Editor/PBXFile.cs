using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System;
using System.IO;

namespace BuildBuddy {
	public enum BuildPhase {
		NONE,
		FRAMEWORKS,
		SOURCES,
		RESOURCES
	};

	[System.Serializable]
	public class PBXFile : ScriptableObject, IComparable {

		[SerializeField]
		private string _group;
		[SerializeField]
		private string _newGroup;
		[SerializeField]
		private string compilerFlags = "";
		[SerializeField]
		protected bool required = true;
		[SerializeField]
		private bool _removed;

		public BuildPhase buildPhase { get; private set; }
		public string group { get { return _group; } set { _group = value.Trim (); } }
		private bool _isCustomFramework;
		public bool isCustomFramework { 
			get { return _isCustomFramework && this as PbxSdkFile == null; } 
			private set { _isCustomFramework = value; } 
		}
		protected string fileRefID;
		public string absolutePath { get; protected set; }
		private string lastKnownFileType;
		private string buildRefID;

		protected new string name;
		protected string relativePath;
		protected string sourceTree = "";
		protected List<string> optionalSettings = new List<string>();
		

		private bool display;
		private bool fileNotFound = false;

		public bool edited;
		public bool removed { get { return _removed; } protected set { _removed = value; } }

		#region Constructors
		protected PBXFile() {}
		public static PBXFile CreateInstance(string absolutePath, string group) {
			PBXFile reference = ScriptableObject.CreateInstance<PBXFile> () as PBXFile;
			reference.group = group;
			reference._newGroup = group;
			reference.fileRefID = PBXEditor.GenerateID ();
			reference.absolutePath = absolutePath;
			reference.MakePathRelative (Application.dataPath);
			reference.AssignBuildPhase(System.IO.Path.GetExtension (reference.absolutePath));
			reference.name = System.IO.Path.GetFileName(reference.absolutePath);
			reference.edited = true;
			return reference;
		}
		public static PBXFile CreateInstance(XmlElement element) {
			PBXFile reference = ScriptableObject.CreateInstance<PBXFile> () as PBXFile;
			reference.fileRefID = PBXEditor.GenerateID ();
			reference.relativePath = element.Attributes ["path"].Value;
			reference.name = System.IO.Path.GetFileName(reference.relativePath);
			reference.group = element.Attributes ["group"].Value;
			reference._newGroup = reference.group;
			if (element.HasAttribute ("compilerFlags")) {
				reference.compilerFlags = element.Attributes ["compilerFlags"].Value;
				reference.optionalSettings.Add ("COMPILER_FLAGS = \"" + reference.compilerFlags + "\"; ");
			} else if (element.HasAttribute ("required")) {
				reference.required = System.Convert.ToBoolean (element.Attributes["required"].Value);
				reference.optionalSettings.Add ("ATTRIBUTES = (Weak, ); ");
			}
			reference.AssignBuildPhase (System.IO.Path.GetExtension (reference.relativePath));
			reference.BuildAbsolutePath ();
			reference.sourceTree = "SOURCE_ROOT";
			try {
				File.GetAttributes (reference.absolutePath);
			}
			catch (FileNotFoundException) {
				Debug.LogWarning ("Could find file " + reference.name + " at " + reference.absolutePath);
				reference.fileNotFound = true;
			}
			catch (DirectoryNotFoundException) {
				Debug.LogWarning ("Could find file " + reference.name + " at " + reference.absolutePath);
				reference.fileNotFound = true;
			}
			return reference;
		}
		public virtual PBXFile Clone() {
			PBXFile newFile = CreateInstance (this.absolutePath, this.group);
			newFile.compilerFlags = compilerFlags;
			return newFile;
		}
		#endregion
		public bool ValidPath()  {
			if (Directory.Exists (absolutePath) || File.Exists(absolutePath))
				return true;
			if (absolutePath.StartsWith ("($SDKROOT)"))
				return true;
			if (!fileNotFound)
				Debug.LogError ("File could not be found at path " + absolutePath);
			fileNotFound = true;
			return false;
		}
		public virtual void OnGUI() {
			if (removed)
				return;
			if (fileNotFound) {
				GUI.color = Color.red;
			}
			EditorGUILayout.BeginHorizontal ();
			{
				display = EditorGUILayout.Foldout (display, name);
				if (GUILayout.Button ("Remove")) {
					removed = true;
				}
			}
			EditorGUILayout.EndHorizontal ();
			if (display) {
				EditorGUI.BeginChangeCheck();
				BBGuiHelper.BeginIndent();
				{
					EditorGUILayout.LabelField ("Path: " + absolutePath);
					_newGroup = EditorGUILayout.TextField("Group:", _newGroup);
					if (buildPhase == BuildPhase.SOURCES) {
						compilerFlags = EditorGUILayout.TextField ("Compiler Flags: ", compilerFlags);
					}
					if (buildPhase == BuildPhase.FRAMEWORKS) {
						required = EditorGUILayout.Toggle ("Required: ", required);
					}
				}
				BBGuiHelper.EndIndent();
				if (EditorGUI.EndChangeCheck()) {
					edited = true;
				}
			}
			GUI.color = Color.white;
		}
		public virtual void Serialize(XmlDocument document) {
			group = _newGroup;
			XmlElement element = document.CreateElement ("pbxfile");
			element.SetAttribute("path", relativePath);
			element.SetAttribute ("group", group);
			if (!compilerFlags.Equals(""))
				element.SetAttribute ("compilerFlags", compilerFlags);
			if (!required) {
				element.SetAttribute ("required", ""+required);
			}
			XmlNode parent = document.GetElementsByTagName (XcodeSerializer.PBXPROJ_TOKEN)[0];
			parent.AppendChild (element);
		}
		public bool HasPBXBuildFile() {
			return buildPhase != BuildPhase.NONE;
		}
		public void MakePathRelative(string projectPath) {
			if (sourceTree.Equals ("SDKROOT"))
				return;
			System.Uri fileUri = new System.Uri (absolutePath);
			System.Uri projectUri = new System.Uri (projectPath);
			relativePath = "../"+projectUri.MakeRelativeUri (fileUri).ToString ();
		}
		// absolutePath should equal Application.dataPath + relativePath
		// This correctly removes all instances of "../" from the middle of the absolute path
		private void BuildAbsolutePath () {
			string[] absolutePathArray = Application.dataPath.Split ('/');
			string[] relativePathArray = relativePath.Split ('/');
			int numToRemove = System.Array.FindAll(relativePathArray, s => s.Equals ("..")).Length;
			absolutePath = System.String.Join ("/", absolutePathArray, 0, absolutePathArray.Length - numToRemove)
				+ "/" + System.String.Join ("/", relativePathArray, numToRemove, relativePathArray.Length - numToRemove);
		}

		#region ToStrings
		public string PBXFileReferenceToString () {
			StringBuilder builder = new StringBuilder ();
			builder.Append ("\t\t" + fileRefID + " /* " + name + " */ = {");
			builder.Append("isa = PBXFileReference; ");
			builder.Append("lastKnownFileType = " + lastKnownFileType+"; ");
			builder.Append("name = \"" + name + "\"; ");
			builder.Append("path = \"" + relativePath + "\"; ");
			builder.Append("sourceTree = " + sourceTree+"; ");
			builder.Append("};");
			return builder.ToString();
		}
		public string PBXBuildFileToString() {
			StringBuilder builder = new StringBuilder ();
			builder.Append ("\t\t" + buildRefID + " /* " + name + " in ");
			if (buildPhase == BuildPhase.FRAMEWORKS) {
				builder.Append ("Frameworks */ = {");
			} else if (buildPhase == BuildPhase.SOURCES) {
				builder.Append ("Sources */ = {");
			} else {
				builder.Append ("Resources */ = {");
			}
			builder.Append ("isa = PBXBuildFile; ");
			builder.Append ("fileRef = "+fileRefID + " /* "+name+" */; ");
			if (optionalSettings.Count > 0) {
				builder.Append ("settings = {");
				foreach (string optionalAttribute in optionalSettings) {
						builder.Append (optionalAttribute);
				}
				builder.Append ("}; ");
			}
			builder.Append ("};");
			return builder.ToString();
		}
		public string BuildPhaseToString() {
			return "\t\t\t\t" + buildRefID + " /* " + name + " */,";
		}
		public string GroupToString() {
			return "\t\t\t\t" + fileRefID + " /* " + name + " */,";
		}
		#endregion

		protected void AssignBuildPhase(string fileExtension) {
			switch (fileExtension) {
			case ".a":
				buildPhase = BuildPhase.FRAMEWORKS;
				lastKnownFileType = "archive.ar";
				break;
			case ".app":
				buildPhase = BuildPhase.NONE;
				lastKnownFileType = "wrapper.application";
				break;
			case ".s":
				buildPhase = BuildPhase.SOURCES;
				lastKnownFileType = "sourcecode.asm";
				break;
			case ".c":
				buildPhase = BuildPhase.SOURCES;
				lastKnownFileType = "sourcecode.c.c";
				break;
			case ".cpp":
				buildPhase = BuildPhase.SOURCES;
				lastKnownFileType = "sourcecode.cpp.cpp";
				break;
			case ".framework":
				buildPhase = BuildPhase.FRAMEWORKS;
				lastKnownFileType = "wrapper.framework";
				isCustomFramework = true;
				break;
			case ".h":
				buildPhase = BuildPhase.NONE;
				lastKnownFileType = "sourcecode.c.h";
				break;
			case ".pch":
				buildPhase = BuildPhase.NONE;
				lastKnownFileType = "sourcecode.c.h";
				break;
			case ".icns":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "image.icns";
				break;
			case ".m":
				buildPhase = BuildPhase.SOURCES;
				lastKnownFileType = "sourcecode.c.objc";
				break;
			case ".mm":
				buildPhase = BuildPhase.SOURCES;
				lastKnownFileType = "sourcecode.c.objcpp";
				break;
			case ".nib":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "wrapper.nib";
				break;
			case ".plist":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "text.plist.xml";
				break;
			case ".png":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "image.png";
				break;
			case ".rtf":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "text.rtf";
				break;
			case ".tiff":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "image.tiff";
				break;
			case ".txt":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "text";
				break;
			case ".cs":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "text";
				break;
			case ".xcodeproj":
				buildPhase = BuildPhase.NONE;
				lastKnownFileType = "wrapper.pb-project";
				break;
			case ".xib":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "file.xib";
				break;
			case ".xml":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "text.xml";
				break;
			case ".strings":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "text.plist.strings";
				break;
			case ".bundle":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "wrapper.plug-in";
				break;
			case ".dylib":
				buildPhase = BuildPhase.FRAMEWORKS;
				lastKnownFileType = "compiled.macho-o.dylib";
				break;
			case ".xcassets":
				buildPhase = BuildPhase.RESOURCES;
				lastKnownFileType = "folder.assetcatalog";
				break;
			default:
				if (fileExtension.Equals ("")) {
					Debug.LogWarning ("Nested folders must be imported individually");
				} else
					if (!fileExtension.Equals (".DS_Store"))
						Debug.LogWarning ("Build Buddy does not support "+fileExtension+" files");
				removed = true;
				buildPhase = BuildPhase.NONE;
				lastKnownFileType = "";
				break;
			}
			if (buildPhase != BuildPhase.NONE) {
				buildRefID = PBXEditor.GenerateID();
			}
		}
		public int CompareTo(object obj) {
			if (obj as PBXFile == null)
				return 1;
			PBXFile file = obj as PBXFile;
			if (this.group == null)
				return 1;
			if (this.group.CompareTo (file.group) == 0)
				return this.name.CompareTo (file.name);
			return this.group.CompareTo (file.group);
		}
	}
}