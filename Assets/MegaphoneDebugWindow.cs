using UnityEditor;
using UnityEngine;

public class MegaphoneDebugWindow : EditorWindow
{
    private bool _hasAnalytics;

    private bool _shouldShowAnalyticsWarning;

    [MenuItem("Tools/Megaphone/Megaphone Debug")]
    public static void ShowWindow()
    {
        MegaphoneDebugWindow window = GetWindow<MegaphoneDebugWindow>("Custom Window");
        window.title = "Megaphone Debug Window";
    }

    private void OnGUI()
    {
        
        ToggleAnalytics();
    }

    private void ToggleAnalytics()
    {
        if(_shouldShowAnalyticsWarning)
        {
            EditorGUILayout.HelpBox("This is a warning message!", MessageType.Warning);
        }

        string variableString = _hasAnalytics ? "off" : "on";
        if (GUILayout.Button($"Turn {variableString} analytics"))
        {
            _hasAnalytics = !_hasAnalytics;
            _shouldShowAnalyticsWarning = true;
        }
    }
}
