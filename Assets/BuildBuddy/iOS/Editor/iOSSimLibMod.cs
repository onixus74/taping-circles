/*using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

/public static class iOSSimLibMod 
{
	private const string REGISTER_SYMBOL_FUNCTION_NAME = "mono_dl_register_symbol";
	private const string SIMULATOR_END_DEFINE = "#endif // !(TARGET_IPHONE_SIMULATOR)";
	private const string COMMENT_EDIT = " // Scopely Mod";
	private const string UNITY_SIGNATURE = "UnityNS";
	
	[PostProcessBuild]
	public static void OnPostProcessBuild( BuildTarget target, string path )
	{
		if (target != BuildTarget.iPhone) {
			//Debug.Log("Target is not iPhone Simulator. iOSSimLibMod will not run");
			return;
		}
		
		if(PlayerSettings.iOS.sdkVersion != iOSSdkVersion.SimulatorSDK)
		{
			//Debug.Log("Target is not iPhone Simulator. iOSSimLibMod will not run");
			return;
		}
		
		string registerMonoModulesPath = Path.Combine(path, "Libraries/RegisterMonoModules.cpp");        
		string[] lines = File.ReadAllLines(registerMonoModulesPath);
		List<string> newLineList = new List<string>(); 
		
		// Find Register Symbol Signature
		int currentLineIdx = 0;
		string currentLine = "";
		string registerSymbolSignature = "";
		for(int l = 0; l < lines.Length; ++l)
		{
			currentLineIdx = l;
			currentLine = lines[l];
			newLineList.Add(currentLine);
			
			if(currentLine.Contains(REGISTER_SYMBOL_FUNCTION_NAME))
			{
				registerSymbolSignature = currentLine;
				break;
			}
		}
		
		if(string.IsNullOrEmpty(registerSymbolSignature))
		{
			Debug.LogError("Could not find Register Symbol Signature");
			return;
		}
		
		// Find Position to add Signature
		for(int l = currentLineIdx+1; l < lines.Length; ++l)
		{
			currentLineIdx = l;
			currentLine = lines[l];
			newLineList.Add(currentLine);
			
			if(currentLine.Contains(SIMULATOR_END_DEFINE))
			{
				newLineList.Add(registerSymbolSignature + COMMENT_EDIT);
				break;
			}
		}
		
		if(newLineList.Count <= currentLineIdx)
		{
			Debug.LogError("Could not find position to add Register Symbol Signature");
			return;
		}
		
		// Find Functions to Add
		List<string> functionLines = new List<string>();
		for(int l = currentLineIdx+1; l < lines.Length; ++l)
		{
			currentLineIdx = l;
			currentLine = lines[l];
			newLineList.Add(currentLine);
			
			if(!currentLine.Contains(UNITY_SIGNATURE) && currentLine.Contains(REGISTER_SYMBOL_FUNCTION_NAME))
			{
				functionLines.Add(currentLine + COMMENT_EDIT);
			}
			
			if(currentLine.Contains(SIMULATOR_END_DEFINE))
			{
				break;
			}
		}
		
		if(functionLines.Count == 0)
		{
			//Debug.Log("No functions to add");
			return;
		}
		
		newLineList.AddRange(functionLines);
		
		// Add the rest of the file
		for(int l = currentLineIdx+1; l < lines.Length; ++l)
		{
			currentLineIdx = l;
			currentLine = lines[l];
			newLineList.Add(currentLine);
		}
		
		string newContent = "";
		foreach(string line in newLineList)
		{
			newContent += line + "\n";
		}
		
		File.WriteAllText(registerMonoModulesPath, newContent);
		
		Debug.Log("iOSSimLibMod added functions to RegisterMonoModules.cpp");
	}
}*/
