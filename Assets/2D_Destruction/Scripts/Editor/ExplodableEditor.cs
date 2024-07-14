﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Explodable))]
public class ExplodableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Explodable myTarget = (Explodable)target;

        myTarget.allowRuntimeFragmentation = EditorGUILayout.Toggle("Allow Runtime Fragmentation", myTarget.allowRuntimeFragmentation);
        myTarget.shatterType = (Explodable.ShatterType)EditorGUILayout.EnumPopup("Shatter Type", myTarget.shatterType);
        myTarget.extraPoints = EditorGUILayout.IntField("Extra Points", myTarget.extraPoints);
        myTarget.subshatterSteps = EditorGUILayout.IntField("Subshatter Steps", myTarget.subshatterSteps);
        if (myTarget.subshatterSteps > 1)
        {
            EditorGUILayout.HelpBox("Use subshatter steps with caution! Too many will break performance!!! Don't recommend more than 1", MessageType.Warning);
        }

        myTarget.fragmentLayer = EditorGUILayout.TextField("Fragment Layer", myTarget.fragmentLayer);
        myTarget.sortingLayerName = EditorGUILayout.TextField("Sorting Layer", myTarget.sortingLayerName);
        myTarget.orderInLayer = EditorGUILayout.IntField("Order In Layer", myTarget.orderInLayer);
        
        myTarget.minForceMagnitude = EditorGUILayout.FloatField("Min Force Magnitude", myTarget.minForceMagnitude);
        myTarget.maxForceMagnitude = EditorGUILayout.FloatField("Max Force Magnitude", myTarget.maxForceMagnitude);
        myTarget.forceDirection = EditorGUILayout.Vector2Field("Force Direction", myTarget.forceDirection);

        if (myTarget.GetComponent<PolygonCollider2D>() == null && myTarget.GetComponent<BoxCollider2D>() == null)
        {
            EditorGUILayout.HelpBox("You must add a BoxCollider2D or PolygonCollider2D to explode this sprite", MessageType.Warning);
        }
        else
        {
            if (GUILayout.Button("Generate Fragments"))
            {
                myTarget.fragmentInEditor();
                EditorUtility.SetDirty(myTarget);
            }
            if (GUILayout.Button("Destroy Fragments"))
            {
                myTarget.deleteFragments();
                EditorUtility.SetDirty(myTarget);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
