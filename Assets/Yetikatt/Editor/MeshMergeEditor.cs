/*
 * MeshMergeEditor.cs
 * 
 * Custom Editor for the MeshMerge class. 
 * 
 * @Author: Chris Thodesen
 * @Contact: chris@yetikatt.com
 * @Version: 1.2
 */
using UnityEngine;
using UnityEditor;

namespace com.yetikatt.Utils
{
    [CustomEditor(typeof(MeshMerge))]
    public class MeshMergeEditor : Editor
    {
		// VARIABLES //
		/// <summary>
		/// The GUIStyle for disabled elements
		/// </summary>
        GUIStyle disabledStyle = new GUIStyle();

		/// <summary>
		/// The GUIStyle for the title
		/// </summary>
        GUIStyle titleStyle = new GUIStyle();


		/// <summary>
		/// Sets up the GUIStyles for the MeshMerge Inspector
		/// </summary>
        void OnEnable()
        {
            // Style for disabled options.
            disabledStyle.alignment = TextAnchor.MiddleLeft;
            disabledStyle.fontStyle = FontStyle.Normal;
            disabledStyle.normal.textColor = Color.grey;

            // Style for Title label
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.normal.textColor = Color.black;   
        }

		/// <summary>
		/// Overrides the MeshMerge default inspector
		/// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Merge Options", titleStyle);
            EditorGUILayout.Space();

            MeshMerge meshMerger = target as MeshMerge;

            if (!meshMerger.Merged)
            {

                meshMerger.SaveAsNew = EditorGUILayout.Toggle("Save as new:", meshMerger.SaveAsNew);

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Parent Transform mode: ");
                meshMerger.CenterParentMode = EditorGUILayout.Popup(meshMerger.CenterParentMode, meshMerger.centeringOptions);
                EditorGUILayout.EndHorizontal();

                if( meshMerger.CenterParentMode == 2 )
                {
                    meshMerger.GridCenteringX = EditorGUILayout.FloatField("X Value:", meshMerger.GridCenteringX);
                    meshMerger.GridCenteringY = EditorGUILayout.FloatField("Y Value:", meshMerger.GridCenteringY);
                    meshMerger.GridCenteringZ = EditorGUILayout.FloatField("Z Value:", meshMerger.GridCenteringZ);
                }


                if (GUILayout.Button("Combine Meshes"))
                {
                    Undo.RecordObject(meshMerger, "Combine Meshes");
                    meshMerger.CombineMeshes(meshMerger.SaveAsNew, !meshMerger.SaveAsNew, meshMerger.CenterParentMode);
                    EditorUtility.SetDirty(meshMerger);
                }
            }
            else
            {
                GUILayout.Label("Save as new ", disabledStyle);
                GUILayout.Label("Center parent ", disabledStyle);

                if (GUILayout.Button("Separate Mesh"))
                {
                    Undo.RecordObject(meshMerger, "Seperate Mesh");
                    meshMerger.SeperateMesh();
                    EditorUtility.SetDirty(meshMerger);
                }

                EditorGUILayout.Space();
                GUILayout.Label("Save Options", titleStyle);
                EditorGUILayout.Space();

                meshMerger.UseCustomPath = EditorGUILayout.BeginToggleGroup("Use Custom Path: ", meshMerger.UseCustomPath);
                    EditorGUILayout.BeginHorizontal();
                        GUILayout.Label("Custom Path: ");
                        meshMerger.CustomPrefabPath = EditorGUILayout.TextField(meshMerger.CustomPrefabPath);
                    EditorGUILayout.EndHorizontal();
                if(meshMerger.UseCustomPath)
                {
                    EditorGUILayout.HelpBox("The Custom Path field specifies where the prefab will be saved. If this isn't used it will be saved where you save the mesh. "+
                        "The format is the relative path from the assets folder (i.e.  Assets/Prefabs/  will save your prefab in a folder called 'Prefabs')", MessageType.Info);
                }
                EditorGUILayout.EndToggleGroup();

                if (GUILayout.Button("Save Mesh"))
                {
                    Undo.RecordObject(meshMerger, "Save Mesh");
                    meshMerger.SaveMesh();
                    EditorUtility.SetDirty(meshMerger);
                }

                if (GUILayout.Button("Save Mesh as new"))
                {
                    Undo.RecordObject(meshMerger, "Save Mesh as new");
                    meshMerger.SaveMeshAsNew();
                    EditorUtility.SetDirty(meshMerger);
                }
                
                if(GUILayout.Button("Save Mesh and Prefab"))
                {
                    Undo.RecordObject(meshMerger, "Save Mesh and Prefab");
                    meshMerger.SaveAsset();
                    EditorUtility.SetDirty(meshMerger);
                }

				if( !meshMerger.ColliderAdded )
				{
					if(GUILayout.Button("Add Collider"))
					{
						Undo.RecordObject(meshMerger, "Setup Collider");
						meshMerger.SetupCollider();
						EditorUtility.SetDirty(meshMerger);
					}
				} else {
					if(GUILayout.Button ("Remove Collider"))
					{
						Undo.RecordObject(meshMerger, "Remove Collider");
						meshMerger.ClearCollider();
						EditorUtility.SetDirty(meshMerger);

						// The custom inspector has removed a component on this game object. Stop excution of current draw iteration so as not to try to access Destroyed Component
						EditorGUIUtility.ExitGUI();
					}
				}
            }
        }
    }
}
