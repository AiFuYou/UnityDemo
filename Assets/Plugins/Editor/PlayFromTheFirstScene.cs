using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[InitializeOnLoad]
public static class PlayFromTheFirstScene
 {      
     const string playFromFirstMenuStr = "File/Always Start From Scene 0 &p";
 
     static bool playFromFirstScene
     {
         get{return EditorPrefs.HasKey(playFromFirstMenuStr) && EditorPrefs.GetBool(playFromFirstMenuStr);}
         set{EditorPrefs.SetBool(playFromFirstMenuStr, value);}
     }
 
     [MenuItem(playFromFirstMenuStr, false, 156)]
     static void PlayFromFirstSceneCheckMenu() 
     {
         playFromFirstScene = !playFromFirstScene;
         Menu.SetChecked(playFromFirstMenuStr, playFromFirstScene);
 
         ShowNotifyOrLog(playFromFirstScene ? "Play from scene 0" : "Play from current scene");
     }
 
     // The menu won't be gray out, we use this validate method for update check state
     [MenuItem(playFromFirstMenuStr, true)]
     static bool PlayFromFirstSceneCheckMenuValidate()
     {
         Menu.SetChecked(playFromFirstMenuStr, playFromFirstScene);
         return true;
     }
 
     // This method is called before any Awake. It's the perfect callback for this feature
     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] 
     static void LoadFirstSceneAtGameBegins()
     {
         if(!playFromFirstScene)
             return;
 
         if(EditorBuildSettings.scenes.Length == 0)
         {
             Debug.LogWarning("The scene build list is empty. Can't play from first scene.");
             return;
         }
 
         foreach(GameObject go in Object.FindObjectsOfType<GameObject>())
             go.SetActive(false);
         
         SceneManager.LoadScene(0);
     }
 
     static void ShowNotifyOrLog(string msg)
     {
         if(Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
             EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
         else
             Debug.Log(msg); // When there's no scene view opened, we just print a log
     }
 }
