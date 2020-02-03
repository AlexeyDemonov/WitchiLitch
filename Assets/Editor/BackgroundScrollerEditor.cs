﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[CustomEditor(typeof(BackgroundScroller))]
public class BackgroundScrollerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if/*user presses*/(GUILayout.Button("Request Scroll Speed"))
        {
            var instance = (BackgroundScroller)target;
            var method = typeof(BackgroundScroller).GetMethod("RequestScrollSpeed", BindingFlags.Instance | BindingFlags.NonPublic);
            method.Invoke(instance, null);
        }
    }
}
