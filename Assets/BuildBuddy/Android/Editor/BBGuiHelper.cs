using UnityEngine;
using System.Collections;

namespace BuildBuddy {
	public static class BBGuiHelper {

		private static float buttonWidth = 200;

		public static void BeginIndent (int indent = 12) {
			GUILayout.BeginHorizontal (); //GUI.skin.box
			GUILayout.Space (indent);
			GUILayout.BeginVertical ();
		}
		public static void EndIndent () {
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();
		}
		public static GUILayoutOption ButtonWidth() {
			return GUILayout.Width (buttonWidth);
		}
	}
}