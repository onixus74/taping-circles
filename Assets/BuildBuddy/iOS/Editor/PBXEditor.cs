using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BuildBuddy {
	public class PBXEditor {

		private const string VERSION_HEADER_TOKEN = "// !$*UTF8*$!";
		private const string PBXFILEREFERENCE_TOKEN = "/* Begin PBXFileReference section */";
		private const string PBXBUILDFILE_TOKEN = "/* Begin PBXBuildFile section */";
		private const string PBXFRAMEWORKSBUILDPHASE_TOKEN = "/* Begin PBXFrameworksBuildPhase section */";
		private const string PBXSOURCESBUILDPHASE_TOKEN = "/* Begin PBXSourcesBuildPhase section */";
		private const string PBXRESOURCESBUILDPHASE_TOKEN = "/* Begin PBXResourcesBuildPhase section */";
		private const string PBXGROUPBEGIN_TOKEN = "/* Begin PBXGroup section */";
		private const string PBXGROUPEND_TOKEN = "/* End PBXGroup section */";
		private const string PBXLINKERFLAG_TOKEN = "\t\t\t\tOTHER_LDFLAGS = (";
		private const string DEFAULT_GROUP_TOKEN = "CustomTemplate";

		private const string XCBUILDCONFIG_TOKEN = "\t\t\tisa = XCBuildConfiguration;";
		private const string FRAMEWORK_PATH_OPEN_TOKEN = "\t\t\t\tFRAMEWORK_SEARCH_PATHS = (";
		private const string LIBRARY_PATH_OPEN_TOKEN = "\t\t\t\tLIBRARY_SEARCH_PATHS = (";
		private const string HEADER_PATH_OPEN_TOKEN = "\t\t\t\tHEADER_SEARCH_PATHS = (";
		private const string FRAMEWORK_PATH_CLOSE_TOKEN = "\t\t\t\t);";

		private static int numIds = 0;
		private string filePath;

		List<string> projectList = new List<string> ();

		public PBXEditor(string path) {
			filePath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
			projectList.AddRange (File.ReadAllLines (filePath));
			if (!projectList [0].Equals (VERSION_HEADER_TOKEN)) {
				Debug.LogError ("PBX Project in invalid format");
				return;
			}
		}
		public void AddFileReferences (IEnumerable<PBXFile> referenceList) {
			AddBuildFiles (referenceList.Where (r => r.HasPBXBuildFile ()));
			int groupBeginIndex = projectList.IndexOf (PBXGROUPBEGIN_TOKEN) + 1;
			int groupEndIndex = projectList.IndexOf (PBXGROUPEND_TOKEN);
			List<string> groupList = projectList.GetRange (groupBeginIndex, groupEndIndex - groupBeginIndex);
			foreach (PBXFile reference in referenceList) {
				if (reference.ValidPath())
					AddGroupReference(reference.group, reference.GroupToString(), groupList);
			}
			projectList.RemoveRange (groupBeginIndex, groupEndIndex - groupBeginIndex);
			projectList.InsertRange (groupBeginIndex, groupList);
			int index = projectList.IndexOf (PBXFILEREFERENCE_TOKEN) + 1;
			foreach (PBXFile reference in referenceList) {
				if (reference.ValidPath())
					projectList.Insert (index, reference.PBXFileReferenceToString());
			}
		}
		public void AddLinkerFlags(string flags) {
			if (flags.Equals ("") || flags == null)
				return;
			string[] flagsArray = flags.Split (' ');
			for (int i = 0; i < flagsArray.Length; i++) { 
					flagsArray[i] = "\t\t\t\t\t"+flagsArray[i]+",";
			}
			int buildFileIndex = projectList.IndexOf (PBXLINKERFLAG_TOKEN);
			projectList.RemoveRange (buildFileIndex + 1, 3);
			projectList.InsertRange (buildFileIndex + 1, flagsArray);
			buildFileIndex = projectList.LastIndexOf (PBXLINKERFLAG_TOKEN);
			projectList.RemoveRange (buildFileIndex + 1, 3);
			projectList.InsertRange (buildFileIndex + 1, flagsArray);
		}
		public void AddFrameworkSearchPaths(IEnumerable<string> searchPaths) {
			int debugSearchPathIndex = projectList.IndexOf (XCBUILDCONFIG_TOKEN) + 5;
			AddFrameworkSearchPaths(searchPaths, debugSearchPathIndex);
			int releaseSearchPathIndex = projectList.IndexOf (XCBUILDCONFIG_TOKEN, debugSearchPathIndex) + 5;
			AddFrameworkSearchPaths (searchPaths, releaseSearchPathIndex);
		}
		private void AddFrameworkSearchPaths(IEnumerable<string> searchPaths, int index) {
			List<string> searchPathStrings = new List<string> ();
			searchPathStrings.Add (FRAMEWORK_PATH_OPEN_TOKEN);
			foreach (string path in searchPaths) {
				searchPathStrings.Add ("\t\t\t\t\t"+path+",");
			}
			searchPathStrings.Add (FRAMEWORK_PATH_CLOSE_TOKEN);
			projectList.InsertRange (index, searchPathStrings);
		}
		public void AddHeaderSearchPaths(List<string> searchPaths) {
			List<string> formattedSearchPaths = new List<string> (searchPaths);
			for (int i = 0; i < formattedSearchPaths.Count; i++) {
				if (Directory.Exists (searchPaths[i]))
					formattedSearchPaths[i] = "\t\t\t\t\t\""+formattedSearchPaths[i]+"\",";
				else {
					Debug.LogError ("HEADER search path "+ searchPaths[i] + " does not exist");
					formattedSearchPaths.RemoveAt (i--);
				}
			}
			int debugSearchPathIndex = projectList.IndexOf (HEADER_PATH_OPEN_TOKEN);
			projectList.InsertRange (debugSearchPathIndex + 1, formattedSearchPaths);
			int releaseSearchPathIndex = projectList.IndexOf (HEADER_PATH_OPEN_TOKEN, debugSearchPathIndex + searchPaths.Count());
			projectList.InsertRange (releaseSearchPathIndex + 1, formattedSearchPaths);
		}
		public void AddLibrarySearchPaths(List<string> searchPaths) {
			List<string> formattedSearchPaths = new List<string> (searchPaths);
			for (int i = 0; i < formattedSearchPaths.Count; i++) {
				if (Directory.Exists (searchPaths[i]))
					formattedSearchPaths[i] = "\t\t\t\t\t\""+formattedSearchPaths[i]+"\",";
				else {
					Debug.LogError ("Framework search path "+ searchPaths[i] + " does not exist");
					formattedSearchPaths.RemoveAt (i--);
				}
			}
			int debugSearchPathIndex = projectList.IndexOf (LIBRARY_PATH_OPEN_TOKEN);
			projectList.InsertRange (debugSearchPathIndex + 1, formattedSearchPaths);
			int releaseSearchPathIndex = projectList.IndexOf (LIBRARY_PATH_OPEN_TOKEN, debugSearchPathIndex + searchPaths.Count());
			projectList.InsertRange (releaseSearchPathIndex + 1, formattedSearchPaths);
		}
		private void AddBuildFiles (IEnumerable<PBXFile> buildFiles) {
			//Assumes that PBXBuildFile section is before all PBXBuildPhase sectinos in pbxproj
			int buildFileIndex = projectList.IndexOf (PBXBUILDFILE_TOKEN) + 1;
			foreach (PBXFile file in buildFiles) {
				projectList.Insert(buildFileIndex, file.PBXBuildFileToString());
				switch (file.buildPhase) {
				case BuildPhase.FRAMEWORKS:
					projectList.Insert (projectList.IndexOf(PBXFRAMEWORKSBUILDPHASE_TOKEN) + 5, file.BuildPhaseToString());
					break;
				case BuildPhase.SOURCES:
					projectList.Insert (projectList.IndexOf(PBXSOURCESBUILDPHASE_TOKEN) + 5, file.BuildPhaseToString ());
					break;
				case BuildPhase.RESOURCES:
					projectList.Insert (projectList.IndexOf (PBXRESOURCESBUILDPHASE_TOKEN) + 5, file.BuildPhaseToString());
					break;
				}
			}
		}
		private void AddGroupReference(string groupName, string referenceID, List<string> groupList) {
			if (groupName.Equals("")) {
				groupName = DEFAULT_GROUP_TOKEN;
			}
			int index = groupList.IndexOf ("\t\t\tpath = " + groupName+";");
			if (index == -1) {
				index = groupList.IndexOf ("\t\t\tname = " + groupName+";");
			}
			if (index == -1) {
				index = CreateGroup(groupName, groupList);
			}
			index -= 1;
			groupList.Insert (index, referenceID);
		}
		private int CreateGroup(string groupName, List<string> groupList) {
			string refID = GenerateID ();
			int templateIndex = groupList.IndexOf ("\t\t\tname = CustomTemplate;") - 2;
			groupList.Insert (templateIndex, "\t\t\t\t" + refID + " /* " + groupName + " */,"); 
			groupList.Add ("\t\t" + refID + " /* " + groupName + " */ = {");
			groupList.Add ("\t\t\tisa = PBXGroup;");
			groupList.Add ("\t\t\tchildren = (");
			int index = groupList.Count + 1;
			groupList.Add ("\t\t\t);");
			groupList.Add ("\t\t\tname = " + groupName + ";");
			groupList.Add ("\t\t\tsourceTree = SOURCE_ROOT;");
			groupList.Add ("\t\t};");
			return index;
		}
	    public void Save() {
			File.WriteAllLines (filePath, projectList.ToArray ());
		}

		/* Generates a 24 digit unique hexadecimal id */
		public static string GenerateID() {
			string id = System.DateTime.Now.Ticks + "" + (numIds++);
			while (id.Length < 24) {
				id += ""+0;
			}
			return id;
		}
	}
}