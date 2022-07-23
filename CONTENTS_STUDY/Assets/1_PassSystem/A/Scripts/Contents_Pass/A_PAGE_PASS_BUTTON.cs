using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(A_PAGE_PASS))] 
public class A_PAGE_PASS_BUTTON : Editor 
{ 
    public override void OnInspectorGUI() 
    { 
        base.OnInspectorGUI();
        A_PAGE_PASS generator = (A_PAGE_PASS)target; 
        if (GUILayout.Button("SetScroll")) 
        { 
            generator.SetScrollView();
        }
    } 
}
