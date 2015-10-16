using UnityEngine;
using System.IO;
using System.Collections;

public class VideoReader : MonoBehaviour {
    public string videoPath;
	void Start () {
        videoPath = "C:/Users/Public/Videos/Sample Videos/bla.wmv";
        using(FileStream file = File.Open(videoPath,FileMode.Open))
        {
            file.Unlock(0,long.MaxValue);
            Debug.LogError(File.ReadAllText(videoPath));
        }
	}
	
}
