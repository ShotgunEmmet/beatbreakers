using UnityEngine;
using System.Collections;

public class DownloadObb : MonoBehaviour {
	
	void Start()
	{
#if !UNITY_EDITOR && UNITY_ANDROID
			string expPath = GooglePlayDownloader.GetExpansionFilePath();
			if (expPath == null)
			{
					 Debug.LogWarning("External storage is not available!");
			}
			else
			{
				string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				string patchPath = GooglePlayDownloader.GetPatchOBBPath(expPath);
				
				 Debug.LogWarning( "Main = ..."  + ( mainPath == null ? " NOT AVAILABLE" :  mainPath.Substring(expPath.Length)));
				 Debug.LogWarning( "Patch = ..." + (patchPath == null ? " NOT AVAILABLE" : patchPath.Substring(expPath.Length)));
				if (mainPath == null || patchPath == null)
					GooglePlayDownloader.FetchOBB();
			}
#endif

	}
}
