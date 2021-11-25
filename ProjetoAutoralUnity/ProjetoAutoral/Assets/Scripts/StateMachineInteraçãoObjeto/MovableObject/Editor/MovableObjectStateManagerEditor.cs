using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MovableObjectStateManager))]
[CanEditMultipleObjects]
public class MovableObjectStateManagerEditor : Editor
{
    private bool foldOutAudio;
    public override void OnInspectorGUI()
    {
        MovableObjectStateManager movableObjectStateManager = (MovableObjectStateManager)target;
        SerializedObject serializedObject = new UnityEditor.SerializedObject(movableObjectStateManager);
        movableObjectStateManager.isMovable = EditorGUILayout.Toggle("Is Movable", movableObjectStateManager.isMovable);
        movableObjectStateManager.playAudio = EditorGUILayout.Toggle("Play Audio", movableObjectStateManager.playAudio);
        if (movableObjectStateManager.playAudio)
        {
            if (movableObjectStateManager.GetComponent<AudioSource>() == null)
            {
                movableObjectStateManager.audioSource = movableObjectStateManager.gameObject.AddComponent<AudioSource>();
                movableObjectStateManager.audioSource.playOnAwake = false;
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
        serializedObject.ApplyModifiedProperties();
    }
}
