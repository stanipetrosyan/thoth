using UnityEditor;

namespace Blindword.Thoth.Editor
{
    public class HelloPackageMenu
    {
        [MenuItem("Tools/Hello/Log Hello")]
        public static void LogHello()
        { 
            // questa è una modifica
            HelloPackage.LogHello();
        }
    }
}