using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace BuildBuddy {
	public class AndroidXmlEditor {
		private XmlDocument manifestXML = new XmlDocument ();
		private string manifestPath = Application.dataPath+"/Plugins/Android/AndroidManifest.xml";

		public XmlDocument document { get { return manifestXML; } }

		private readonly string defaultManifest = 
			"<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
			"<manifest xmlns:android=\"http://schemas.android.com/apk/res/android\" package=\"\" android:theme=\"@android:style/Theme.NoTitleBar\" android:versionName=\"1.0\" android:versionCode=\"1\" android:installLocation=\"preferExternal\">" +
				"<supports-screens android:smallScreens=\"true\" android:normalScreens=\"true\" android:largeScreens=\"true\" android:xlargeScreens=\"true\" android:anyDensity=\"true\" />" +
				"<application android:icon=\"@drawable/app_icon\" android:label=\"@string/app_name\" android:debuggable=\"false\">" +
					"<activity android:name=\"com.unity3d.player.UnityPlayerNativeActivity\" android:label=\"@string/app_name\" android:screenOrientation=\"landscape\" android:launchMode=\"singleTask\" android:configChanges=\"mcc|mnc|locale|touchscreen|keyboard|keyboardHidden|navigation|orientation|screenLayout|uiMode|screenSize|smallestScreenSize|fontScale\">" +
					"<intent-filter>" +
	        			"<action android:name=\"android.intent.action.MAIN\" />" +
						"<category android:name=\"android.intent.category.LAUNCHER\" />" +
					"</intent-filter>" +
	      			"<meta-data android:name=\"unityplayer.UnityActivity\" android:value=\"true\" />" +
					"<meta-data android:name=\"unityplayer.ForwardNativeEventsToDalvik\" android:value=\"false\" />" +
					"</activity>" +
 				" </application>" +
				"<uses-sdk android:minSdkVersion=\"9\" android:targetSdkVersion=\"19\" />" +
				"<uses-feature android:glEsVersion=\"0x00020000\" />" +
			"</manifest>";

		public AndroidXmlEditor() {
			try {
				manifestXML.Load (manifestPath);
			} catch (DirectoryNotFoundException) {
				Directory.CreateDirectory(Application.dataPath+"/Plugins/Android");
				manifestXML.LoadXml (defaultManifest);
				manifestXML.Save(manifestPath);
			} catch (FileNotFoundException) {
				manifestXML.LoadXml (defaultManifest);
				manifestXML.Save(manifestPath);
			}
		}
		public AndroidXmlEditor (string xml) {
			manifestXML.LoadXml (xml);
		}
		public List<XmlNode> GetManifestElements() {
			List<XmlNode> elementList = new List<XmlNode> ();
			foreach (XmlNode childElement in manifestXML.ChildNodes ) {
				elementList.AddRange (GetAllChildNodes(childElement));
			}
			return elementList;
		}
		private List<XmlNode> GetAllChildNodes(XmlNode element) {
			List<XmlNode> elementList = new List<XmlNode> ();
			elementList.Add (element);
			if (element.HasChildNodes) {
				foreach (XmlNode childElement in element.ChildNodes) {
					elementList.AddRange (GetAllChildNodes(childElement));
				}
			}
			return elementList;
		}

		public void ApplyChanges(AndroidWindowData data) {
			foreach (ManifestElement element in data.editedList) {
				element.ApplyChanges (manifestXML);
			}
			if (data.isTemplate) {
				AndroidTemplateManager.SaveExistingTemplate(data);
			}
			else 
				manifestXML.Save (manifestPath);
		}
		public override string ToString ()
		{
			return manifestXML.OuterXml;
		}
	}
}