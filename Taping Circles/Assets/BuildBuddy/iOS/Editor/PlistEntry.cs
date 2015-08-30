using UnityEditor;
using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System;

namespace BuildBuddy {
	[Serializable]
	public class PlistEntry : ScriptableObject {

		public enum EntryTypes {
			STRING,
			BOOL,
			NUMBER,
			DATA,
			DATE,
			ARRAY,
			DICTIONARY
		};

		public string key {get; private set;}
		public EntryTypes entryType { get; private set; }
		private int arrayIndex = -1;
		private bool isArrayEntry { get { return arrayIndex != -1; } }
		public bool removed { get; private set;}

		[SerializeField]
		private string valueString {get; set;}
		[SerializeField]
		private string valueData { get; set; }
		[SerializeField]
		private string valueDate {get; set;}
		[SerializeField]
		private bool valueBool { get; set; }
		[SerializeField]
		private float valueNumber { get; set; }
		private List<PlistEntry> valueEntries = new List<PlistEntry>();

		private bool display;
		public bool edited;

		private PlistEntry (){}
		public static PlistEntry CreateInstance(){
			PlistEntry entry =  ScriptableObject.CreateInstance<PlistEntry> () as PlistEntry;
			entry.edited = true;
			return entry;
		}
		public static PlistEntry CreateInstance(XmlElement element, bool isArrayEntry = false) {
			PlistEntry entry = ScriptableObject.CreateInstance<PlistEntry> () as PlistEntry;
			entry.key = element.Attributes ["key"].Value;
			entry.entryType = entry.StringToEntryType(element.Attributes ["type"].Value);
			if (entry.isArrayEntry) {
				entry.arrayIndex = 0;
			}
			switch (entry.entryType) {
			case EntryTypes.STRING:
				entry.valueString = element.Attributes ["value"].Value;
				break;
			case EntryTypes.BOOL:
				entry.valueBool = Convert.ToBoolean(element.Attributes ["value"].Value);
				break;
			case EntryTypes.NUMBER:
				entry.valueNumber = (float)Convert.ToDouble (element.Attributes ["value"].Value);
				break;
			case EntryTypes.DATA:
				entry.valueData = element.Attributes ["value"].Value;
				break;
			case EntryTypes.DATE:
				entry.valueDate = element.Attributes ["value"].Value;
				break;
			default:
				foreach (XmlElement child in element.ChildNodes) {
					entry.valueEntries.Add (PlistEntry.CreateInstance(child, entry.entryType == EntryTypes.ARRAY));
				}
				break;
			}
			return entry;
		}
		public PlistEntry Clone() {
			PlistEntry newEntry = PlistEntry.CreateInstance ();
			newEntry.key = key;
			newEntry.entryType = entryType;
			newEntry.arrayIndex = arrayIndex;
			newEntry.valueString = valueString;
			newEntry.valueData = valueData;
			newEntry.valueData = valueDate;
			newEntry.valueBool = valueBool;
			newEntry.valueNumber = valueNumber;
			newEntry.valueEntries = new List<PlistEntry> ();
			foreach (PlistEntry nestedEntry in valueEntries) {
				newEntry.valueEntries.Add (nestedEntry.Clone ());
			}
			return newEntry;
		}
		public void OnGUI() {
			if (removed)
				return;
			EditorGUILayout.BeginVertical (GUI.skin.box);
			EditorGUILayout.BeginHorizontal (); 
			{
				display = EditorGUILayout.Foldout (display, "Plist Entry: " + (isArrayEntry ? "Array Entry "+arrayIndex : key));
				if (GUILayout.Button ("Remove")) {
					removed = true;
				}
			}
			EditorGUILayout.EndHorizontal ();
			if (!display) {
				EditorGUILayout.EndVertical();
				return;
			}
			EditorGUI.BeginChangeCheck ();
			BBGuiHelper.BeginIndent ();
			{
				if (isArrayEntry) {
					EditorGUILayout.LabelField("Key: Array Entry " + arrayIndex);
				} else {
					key = EditorGUILayout.TextField ("Key:", key);
				}
				entryType = (EntryTypes)EditorGUILayout.EnumPopup("Type:", entryType);
				if (entryType == EntryTypes.DICTIONARY || entryType == EntryTypes.ARRAY) {
					if (EditorGUI.EndChangeCheck ()) {
						edited = true;
					}
					BBGuiHelper.BeginIndent();
					for (int i = 0; i < valueEntries.Count; i++) {
						if (valueEntries[i].removed) {
							valueEntries.RemoveAt (i--);
							continue;
						}
						BBGuiHelper.BeginIndent ();
						valueEntries[i].arrayIndex = (entryType == EntryTypes.ARRAY ? i : -1);
						Undo.RecordObject(valueEntries[i], "Plist Entry");
						valueEntries[i].OnGUI ();
						BBGuiHelper.EndIndent();
					}
					BBGuiHelper.EndIndent();
					EditorGUI.BeginChangeCheck();
				}
				switch (entryType) {
				case EntryTypes.STRING:
					valueString = EditorGUILayout.TextField("Value:", valueString);
					break;
				case EntryTypes.BOOL:
					valueBool = EditorGUILayout.Toggle ("Value:", valueBool);
					break;
				case EntryTypes.NUMBER:
					valueNumber = EditorGUILayout.FloatField ("Value:", valueNumber);
					break;
				case EntryTypes.DATA:
					valueData = EditorGUILayout.TextField("Value:", valueData);
					break;
				case EntryTypes.DATE:
					valueDate = EditorGUILayout.TextField("Value:", valueDate);
					break;
				default:
					BBGuiHelper.BeginIndent();
					if (GUILayout.Button ("New Plist Entry")) {
						valueEntries.Add (PlistEntry.CreateInstance());
					}
					BBGuiHelper.EndIndent();
					break;
				}
			}
			BBGuiHelper.EndIndent ();
			if (EditorGUI.EndChangeCheck ()) {
				edited = true;
			}
			EditorGUILayout.EndVertical ();
		}
		public void Serialize(XmlDocument document, XmlNode parent) {
			XmlElement element = document.CreateElement ("entry");
			element.SetAttribute ("key", key);
			element.SetAttribute ("type", entryType.ToString());
			switch (entryType) {
			case EntryTypes.STRING:
				element.SetAttribute("value", valueString);
				break;
			case EntryTypes.BOOL:
				element.SetAttribute("value", valueBool+"");
				break;
			case EntryTypes.NUMBER:
				element.SetAttribute("value", valueNumber+"");
				break;
			case EntryTypes.DATA:
				element.SetAttribute ("value", valueData);
				break;
			case EntryTypes.DATE:
				element.SetAttribute("value", valueDate);
				break;
			default:
				element.SetAttribute("value", valueEntries.Count+"");
				for (int i = 0; i < valueEntries.Count; i++) {
					if (valueEntries[i].removed) {
						valueEntries.RemoveAt (i--);
						continue;
					}
					valueEntries[i].Serialize(document, element);
				}
				break;
			}
			parent.AppendChild (element);
		}
		public void SerializeToPList(XmlDocument document, XmlNode parent) {
			XmlElement node;
			if (!isArrayEntry) {
				for (int i = 0; i < parent.ChildNodes.Count; i++) {
					if (parent.ChildNodes[i].Name.Equals ("key") && parent.ChildNodes[i].InnerText.Equals(key)) {
						parent.RemoveChild(parent.ChildNodes[i--]);
						parent.RemoveChild(parent.ChildNodes[i--]);
					}
				}
				node = document.CreateElement ("key");
				node.InnerText = key;
				parent.AppendChild(node);
			}
			XmlElement valueElement;
			switch (entryType) {
			case EntryTypes.STRING:
				valueElement = document.CreateElement ("string");
				parent.AppendChild (valueElement);
				valueElement.InnerText = valueString;
				break;
			case EntryTypes.BOOL:
				valueElement = document.CreateElement (valueBool?"true":"false");
				parent.AppendChild (valueElement);
				break;
			case EntryTypes.NUMBER:
				if  (Math.Floor (valueNumber) == valueNumber) {
					valueElement = document.CreateElement ("integer");
					valueElement.InnerText = (int)valueNumber+"";
				} else {
					valueElement = document.CreateElement ("real");
					valueElement.InnerText = valueNumber+"";
				}
				parent.AppendChild (valueElement);
				break;
			case EntryTypes.DATA:
				valueElement = document.CreateElement ("data");
				parent.AppendChild (valueElement);
				valueElement.InnerText = valueData;
				break;
			case EntryTypes.DATE:
				valueElement = document.CreateElement ("date");
				parent.AppendChild (valueElement);
				valueElement.InnerText = valueDate;
				break;
			case EntryTypes.ARRAY:
				valueElement = document.CreateElement ("array");
				parent.AppendChild (valueElement);
				foreach (PlistEntry entry in valueEntries) {
					entry.SerializeToPList(document, valueElement);
				}
				break;
			case EntryTypes.DICTIONARY:
				valueElement = document.CreateElement ("dict");
				parent.AppendChild (valueElement);
				foreach (PlistEntry entry in valueEntries) {
					entry.SerializeToPList(document, valueElement);
				}
				break;
			}
		}
		private EntryTypes StringToEntryType (string type) {
			if (type.Equals (EntryTypes.ARRAY.ToString())) {
				return EntryTypes.ARRAY;
			} else if (type.Equals (EntryTypes.BOOL.ToString())) {
				return EntryTypes.BOOL;
			} else if (type.Equals (EntryTypes.DATA.ToString())) {
				return EntryTypes.DATA;
			} else if (type.Equals(EntryTypes.DATA.ToString())) {
				return EntryTypes.DATE;
			} else if (type.Equals(EntryTypes.NUMBER.ToString())) {
				return EntryTypes.NUMBER;
			} else if (type.Equals (EntryTypes.DICTIONARY.ToString())) {
				return EntryTypes.DICTIONARY;
			}
			return EntryTypes.STRING;
		}
	}
}