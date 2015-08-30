using UnityEngine;
using System.Xml;
using System.Collections.Generic;

namespace BuildBuddy {
	public class PListEditor {

		private string filePath;
		private const string doctype = "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">";
		private XmlDocument document;

		public PListEditor (string path) {
			filePath = path + "/Info.plist";
			document = new XmlDocument ();
			document.Load (filePath);
		}
		public void AddPListEntries(List<PlistEntry> entries) {
			foreach (PlistEntry entry in entries) {
				entry.SerializeToPList(document, document.GetElementsByTagName("dict")[0]);
			}
			document.Save (filePath);
			//Bad hack to get around XmlDocument modifying doctype;
			string[] lines = System.IO.File.ReadAllLines (filePath);
			lines [1] = doctype;
			System.IO.File.WriteAllLines (filePath, lines);
		}
	}
}