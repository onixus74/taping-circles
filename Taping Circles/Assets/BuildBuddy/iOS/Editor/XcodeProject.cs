using UnityEngine;
using System.Collections.Generic;

namespace BuildBuddy {
	public class XcodeProject {
		private List<PBXFile> fileReferences;
		private List<PlistEntry> plistEntries;
		private List<string> headerPaths;
		private List<string> libraryPaths;

		private HashSet<string> frameworkSearchPaths = new HashSet<string> ();
		private string linkerFlags = "";
		private string projectPath;

		private const string DEFAULT_FRAMEWORK_SEARCH_PATH = "\"$(inherited)\"";

		public XcodeProject(string path) {
			projectPath = path;
			XcodeSerializer serializer = XcodeSerializer.CreateInstance();
			fileReferences = new List<PBXFile> (serializer.LoadPBXFiles());
			linkerFlags = serializer.LoadLinkerFlags ();
			plistEntries = serializer.LoadPListEntries ();
			headerPaths = serializer.LoadHeaderSearchPaths ();
			for (int i = 0; i < headerPaths.Count; i++) {
				headerPaths[i] = serializer.BuildAbsolutePath(headerPaths[i]);
			}
			libraryPaths = serializer.LoadLibrarySearchPaths ();
			for (int i = 0; i < libraryPaths.Count; i++) {
				libraryPaths[i] = serializer.BuildAbsolutePath(libraryPaths[i]);
			}
		}
		public void EditProject() {
			foreach (PBXFile fileReference in fileReferences) {
				fileReference.MakePathRelative(projectPath);
				if (fileReference.isCustomFramework) {
					frameworkSearchPaths.Add (System.IO.Path.GetDirectoryName(fileReference.absolutePath));
				}
			}
			PBXEditor editor = new PBXEditor (projectPath);
			PListEditor plistEditor = new PListEditor (projectPath);
			plistEditor.AddPListEntries (plistEntries);
			try {
				editor.AddFileReferences(fileReferences);
				editor.AddLinkerFlags (linkerFlags);
				editor.AddHeaderSearchPaths (headerPaths);
				editor.AddLibrarySearchPaths (libraryPaths);
				if (frameworkSearchPaths.Count > 0) {
					frameworkSearchPaths.Add (DEFAULT_FRAMEWORK_SEARCH_PATH);
					editor.AddFrameworkSearchPaths (frameworkSearchPaths);
				}
			} 
			catch (System.ArgumentOutOfRangeException) {
				Debug.LogError ("Another script has modified the Xcode project and BuildBuddy cannot run");
				return;
			}
			editor.Save ();
		}
	}
}