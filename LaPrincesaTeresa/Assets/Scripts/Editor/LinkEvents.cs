using Level;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LinkEvents : EditorWindow
    {
        [MenuItem("LaPrincesita/EventLinker")]
        private static void ShowWindow()
        {
            var window = GetWindow<LinkEvents>();
            window.titleContent = new GUIContent("Event linker");
            window.Show();
        }

        [SerializeField, RequireInterface(typeof(IHinter))]
        public Object hinterObject;
        private void OnGUI()
        {
            
        }
    }
}