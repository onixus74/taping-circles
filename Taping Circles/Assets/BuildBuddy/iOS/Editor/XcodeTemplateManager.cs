using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;

namespace BuildBuddy {
	public static class XcodeTemplateManager {
		
		private const string keyPrefix = "BBIOS";
		private static List<XcodeSerializer> elements;
		
		public static void SaveTemplate(XcodeSerializer data) {
			elements.Add (data);
			EditorPrefs.SetString (keyPrefix + (elements.Count - 1), data.ToString ());
		}
		public static void SaveExistingTemplate(XcodeSerializer template) {
			int index = elements.IndexOf (template);
			if (index == -1)
				return;
			EditorPrefs.SetString (keyPrefix + index, template.ToString ());
		}
		public static List<XcodeSerializer> GetTemplates() {
			elements = new List<XcodeSerializer> ();
			int i = 0;
			while (EditorPrefs.HasKey (keyPrefix + i)) {
				string savedPref = EditorPrefs.GetString (keyPrefix + i);
				string name = savedPref.Substring(0, savedPref.IndexOf('<'));;
				string xml = savedPref.Substring (savedPref.IndexOf('<'));
				elements.Add (XcodeSerializer.CreateInstance(xml, true));
				elements[i].name = name;
				elements[i].isTemplate = true;
				i++;
			}
			return elements;
		}
		public static XcodeSerializer ReloadTemplate(int i) {
			string savedPref = EditorPrefs.GetString (keyPrefix + i);
			string name = savedPref.Substring(0, savedPref.IndexOf('<'));;
			string xml = savedPref.Substring (savedPref.IndexOf('<'));
			elements[i] = XcodeSerializer.CreateInstance(xml, true);
			elements[i].name = name;
			elements[i].isTemplate = true;
			return elements [i];
		}
		public static void DeleteTemplate(XcodeSerializer element) {
			int index = elements.IndexOf (element);
			elements.RemoveAt (index);
			for (int i = index; i < elements.Count; i++) {
				EditorPrefs.SetString (keyPrefix + i, elements[i].ToString());
			}
			EditorPrefs.DeleteKey (keyPrefix + elements.Count);
		}
		private static void DeleteTemplate(int index) {
			while (EditorPrefs.HasKey (keyPrefix + ++index)) {
				EditorPrefs.SetString(keyPrefix + (index - 1), EditorPrefs.GetString(keyPrefix + index));
			}
			EditorPrefs.DeleteKey (keyPrefix + index);
		}
	}
}