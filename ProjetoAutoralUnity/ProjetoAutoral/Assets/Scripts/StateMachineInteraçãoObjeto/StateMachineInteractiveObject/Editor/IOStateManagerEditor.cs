﻿using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(IOStateManager))]
[CanEditMultipleObjects]

public class EditorGUIPropertyField : Editor
{
    private bool foldOutTextBoxOnDisabledSuccessiveInteract;
    private bool foldOutTextBoxOnSuccessiveInteraction;
    private bool foldOutTextBoxOnFailedInteraction;
    private bool foldOutAudio;
    public override void OnInspectorGUI()
    {
        IOStateManager iOStateManager = (IOStateManager)target;
        SerializedObject serializedObject = new UnityEditor.SerializedObject(iOStateManager);
        iOStateManager.canSuccessiveInteract = EditorGUILayout.Toggle("Can Successive Interact", iOStateManager.canSuccessiveInteract);
        if (iOStateManager.canSuccessiveInteract)
        {
            EditorGUILayout.LabelField("Successive Interaction Consequence");
            iOStateManager.onSuccessiveInteractionConsequence = EditorGUILayout.TextArea(iOStateManager.onSuccessiveInteractionConsequence);
            iOStateManager.singleTimeUse = EditorGUILayout.Toggle("Single Time Use", iOStateManager.singleTimeUse);
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
            if (iOStateManager.needItemToInteract == true)
            {
                foldOutTextBoxOnFailedInteraction = EditorGUILayout.Foldout(foldOutTextBoxOnFailedInteraction, "Text On Failed Interaction", true);
                if (foldOutTextBoxOnFailedInteraction)
                {
                    iOStateManager.textBoxOnFailedInteraction = EditorGUILayout.TextArea(iOStateManager.textBoxOnFailedInteraction, myTextAreaStyle);
                }
            }
            iOStateManager.playAudio = EditorGUILayout.Toggle("Play Audio", iOStateManager.playAudio);
            if (iOStateManager.playAudio)
            {
                if (iOStateManager.GetComponent<AudioSource>() == null)
                {
                    iOStateManager.audioSource = iOStateManager.gameObject.AddComponent<AudioSource>();
                    iOStateManager.audioSource.playOnAwake = false;
                }
                foldOutAudio = EditorGUILayout.Foldout(foldOutAudio, "Audio Configs", true);
                if (foldOutAudio)
                {
                    SerializedProperty serializedPropertyAudioSource = serializedObject.FindProperty("audioSource");
                    EditorGUILayout.PropertyField(serializedPropertyAudioSource);
                    SerializedProperty serializedPropertyAudioClip = serializedObject.FindProperty("audioClip");
                    EditorGUILayout.PropertyField(serializedPropertyAudioClip);
                }
            }
        }
        else
        {
            foldOutTextBoxOnDisabledSuccessiveInteract = EditorGUILayout.Foldout(foldOutTextBoxOnDisabledSuccessiveInteract, "Text On Disabled Successive Interaction", true);
            if (foldOutTextBoxOnDisabledSuccessiveInteract)
            {
                GUIStyle myTextAreaStyle = new GUIStyle(EditorStyles.textArea) { wordWrap = true, fixedHeight = 150, fixedWidth = 400 };
                iOStateManager.textOnDisabledSuccessiveInteract = EditorGUILayout.TextArea(iOStateManager.textOnDisabledSuccessiveInteract, myTextAreaStyle);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}