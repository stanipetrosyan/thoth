using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using UnityEditor;

namespace Editor.Exp {
    [Serializable]
    [Graph(AssetExtension)]
    public class DialogueGraph: Graph {

        public const string AssetExtension = "dialoguegraph";
        
        [MenuItem("Assets/Create/Dialogue Graph", false)]
        private static void CreateAssetFile() {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<DialogueGraph>();
            
        }
        
    }
}