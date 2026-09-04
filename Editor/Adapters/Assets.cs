using UnityEditor;
using UnityEngine;

namespace Editor.Adapters {
    public static class Assets {
        
        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject {
            var asset = LoadAsset<T>(path, assetName);

            if (!asset) {
                asset = ScriptableObject.CreateInstance<T>();
            }

            var fullPath = $"{path}/{assetName}.asset";
            AssetDatabase.CreateAsset(asset, fullPath);

            return asset;
        }

        public static T UpsertAsset<T>(string path, string assetName) where T : ScriptableObject {
            var asset = LoadAsset<T>(path, assetName);
            var fullPath = $"{path}/{assetName}.asset";
            
            if (asset) {
                AssetDatabase.DeleteAsset(fullPath);
            }
            
            var assetUpdated = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(assetUpdated, fullPath);
            Debug.Log(assetUpdated);

            return assetUpdated;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject {
            return AssetDatabase.LoadAssetAtPath<T>($"{path}/{assetName}.asset");
        }
        
        public static void SaveAsset(Object asset) {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void CreateFolder(string path, string folderName) {
            if (AssetDatabase.IsValidFolder(path + "/" + folderName)) {
                return;
            }

            AssetDatabase.CreateFolder(path, folderName);
        }
    }
}