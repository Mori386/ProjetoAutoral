using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(IOStateManager))]
[CanEditMultipleObjects]

public class EditorGUIPropertyField : Editor
{
    private bool foldOutTextBoxOnSuccessiveInteraction;
    private bool foldOutTextBoxOnFailedInteraction;
    public override void OnInspectorGUI()
    {
        IOStateManager iOStateManager = (IOStateManager)target;
        SerializedObject serializedObject = new UnityEditor.SerializedObject(iOStateManager);
        EditorGUILayout.LabelField("Successive Interaction Consequence");
        iOStateManager.onSuccessiveInteractionConsequence = EditorGUILayout.TextArea(iOStateManager.onSuccessiveInteractionConsequence);
        iOStateManager.needItemToInteract = EditorGUILayout.Toggle("Need Item To Interact", iOStateManager.needItemToInteract);
        if (iOStateManager.needItemToInteract)
        {
            iOStateManager.destroyItemOnSuccessiveInteraction = EditorGUILayout.Toggle("Destroy Item On Interaction", iOStateManager.destroyItemOnSuccessiveInteraction);
            SerializedProperty serializedPropertyItemNeeded = serializedObject.FindProperty("itemsNeeded");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("needToInteract"));
            EditorGUILayout.PropertyField(serializedPropertyItemNeeded);
        }
        GUIStyle myTextAreaStyle = new GUIStyle(EditorStyles.textArea) { wordWrap = true, fixedHeight = 150, fixedWidth = 400 };

        foldOutTextBoxOnSuccessiveInteraction = EditorGUILayout.Foldout(foldOutTextBoxOnSuccessiveInteraction, "Text On Successive Interaction", true);
        if (foldOutTextBoxOnSuccessiveInteraction)
        {
            iOStateManager.textBoxOnSuccessiveInteraction = EditorGUILayout.TextArea(iOStateManager.textBoxOnSuccessiveInteraction, myTextAreaStyle);
        }
        if (iOStateManager.needItemToInteract==true)
        {
            foldOutTextBoxOnFailedInteraction = EditorGUILayout.Foldout(foldOutTextBoxOnFailedInteraction, "Text On Failed Interaction",true);
            if(foldOutTextBoxOnFailedInteraction)
            {
                iOStateManager.textBoxOnFailedInteraction = EditorGUILayout.TextArea(iOStateManager.textBoxOnFailedInteraction, myTextAreaStyle);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}