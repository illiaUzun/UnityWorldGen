using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGEN))]
public class MapGenEditor : Editor {
    public override void OnInspectorGUI() {
        MapGEN mapGen = (MapGEN)target;

        if (DrawDefaultInspector() && mapGen.autoUpdate) {
            mapGen.Generate();
        }
        if (GUILayout.Button("Generate new map")) {
            mapGen.Generate();
        }
    }
}
