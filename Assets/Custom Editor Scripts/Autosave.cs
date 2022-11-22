using UnityEditor;
using UnityEngine;
#if (UNITY_EDITOR) 
using UnityEditor.SceneManagement;
 
[InitializeOnLoad]
public class AutoSave{
    // Static constructor that gets called when unity fires up.
    static AutoSave(){
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) => {
            
            if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying){
                // Save the scene and the assets.
                Debug.Log("Autosaved!");

                // Scenes
                EditorSceneManager.SaveOpenScenes();

                // Assets
                AssetDatabase.SaveAssets();
            }
        };
    }
}
#endif