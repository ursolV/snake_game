using UnityEditor;
using System.IO;
using UnityEngine;

public class AssetBundleCreator
{
    [MenuItem("Snake Game/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        string platformSubdirectory;
        BuildTarget buildTarget;

#if UNITY_STANDALONE_OSX
        platformSubdirectory = RuntimePlatform.OSXPlayer.ToString();
        buildTarget = BuildTarget.StandaloneOSX;
#elif UNITY_STANDALONE_WIN
        platformSubdirectory = RuntimePlatform.WindowsPlayer.ToString();
        buildTarget = BuildTarget.StandaloneWindows;
#else
        throw new System.Exception("AssetBundle build is not configured for this platform.");
#endif

        var bundleDirectory = Path.Combine(Application.streamingAssetsPath, platformSubdirectory);

        if (!Directory.Exists(bundleDirectory))
            Directory.CreateDirectory(bundleDirectory);

        BuildPipeline.BuildAssetBundles(
            bundleDirectory,
            BuildAssetBundleOptions.None,
            buildTarget
        );
    }
}