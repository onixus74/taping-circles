using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System;

namespace BuildBuddy {
	[Serializable]
	public sealed class ManifestUsesFeature : ManifestElement{

		private static readonly string[] featureArray = {"NONE", "hardware.audio.low_latency", "hardware.bluetooth", "hardware.bluetooth_le", "hardware.camera", "hardware.camera.autofocus", "hardware.camera.flash", "hardware.camera.front", "hardware.camera.any", "hardware.consumerir", "hardware.location", "hardware.location.network", "hardware.location.gps", "hardware.microphone", "hardware.nfc", "hardware.nfc.hce", "hardware.sensor.accelerometer", "hardware.sensor.light", "hardware.sensor.proximity", "hardware.sensor.stepcounter", "hardware.sensor.stepdetector", "hardware.screen.landscape", "hardware.screen.portrait", "hardware.telephony", "hardware.telephony.cdma", "hardware.telephony.gsm", "hardware.type.television", "hardware.faketouch", "hardware.faketouch.multitouch.distinct", "hardware.faketouch.multitouch.jazzhand", "hardware.touchscreen", "hardware.touchscreen.multitouch", "hardware.touchscreen.multitouch.distinct", "hardware.touchscreen.multitouch.jazzhand", "hardware.usb.host", "hardware.usb.accessory", "hardware.wifi", "hardware.wifi.direct", "software.app_widgets", "software.device_admin", "software.home_screen", "software.input_methods", "software.live_wallpaper", "software.sip", "software.sip.voip"};

		[SerializeField] private string feature;
		[SerializeField] private bool required = true;

		//Constructed by editor window
		public static ManifestUsesFeature CreateInstance() {
			ManifestUsesFeature usesFeature = ScriptableObject.CreateInstance<ManifestUsesFeature> ();
			usesFeature.node = null;
			usesFeature.elementEditStatus = EditStatus.EDITED;
			usesFeature.feature = "NONE";
			return usesFeature;
		}
		//Constructed from existing entry in AndroidManifest
		public static ManifestUsesFeature CreateInstance(XmlNode node) {
			ManifestUsesFeature usesFeature = ScriptableObject.CreateInstance<ManifestUsesFeature> ();
			usesFeature.node = (XmlElement)node;
			usesFeature.elementEditStatus = EditStatus.NONE;
			usesFeature.Initialize();
			return usesFeature;
		}
		public override void OnGUI() {
			EditorGUI.BeginChangeCheck ();
			feature = featureArray[EditorGUILayout.Popup ("Feature: ", Array.IndexOf (featureArray, feature), featureArray)];
			BBGuiHelper.BeginIndent();
			{
				GUILayout.BeginHorizontal (); {
					required = EditorGUILayout.Toggle ("Required: ", required);
					if (EditorGUI.EndChangeCheck ()) {
						elementEditStatus = EditStatus.EDITED;
					}
					if (GUILayout.Button ("Remove")) {
						elementEditStatus = EditStatus.REMOVED;
					}
				}
				GUILayout.EndHorizontal ();
			}
			BBGuiHelper.EndIndent ();
		}
		
		private void Initialize() {
			if (node.HasAttribute ("android:name")) {
				string featurePrefix = "android.";
				foreach (String standardFeature in featureArray) {
					if (node.Attributes["android:name"].Value.Equals (featurePrefix + standardFeature)) {
						feature = standardFeature;
					}
				}
			}
			if (node.HasAttribute ("android:required")) {
				required = !node.Attributes["android:required"].Value.Equals ("false"); 
			}
		}
		private string FeatureToString() {
			return "android." + feature;
		}
		#region override
		protected override void CreateNode(XmlDocument document) {
			node = document.CreateElement ("uses-feature");
			XmlNode manifestElement = document.GetElementsByTagName ("manifest") [0];
			manifestElement.AppendChild (node);
		}
		protected override void UpdateAttributes(XmlDocument document) {
			CreateAndroidAttribute (document, "name", FeatureToString ());
			if (!required) {
				CreateAndroidAttribute (document, "required", "false");
			} 
			else if (node.HasAttribute ("android:required")) {
				node.RemoveAttribute ("android:required");
			}
		}
		#endregion
	}
}