using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace BuildBuddy {
	public static class XcodePostProcessBuild {

		[PostProcessBuild]
		public static void OnPostProcessBuild(BuildTarget target, string pathToProject) {
			if (target != BuildTarget.iOS) {
				return;
			} else {
				XcodeProject project = new XcodeProject(pathToProject);
				project.EditProject();
			}
		}
	}
}