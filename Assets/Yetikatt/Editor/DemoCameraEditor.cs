/*
 * DemoCameraEditor.cs
 * 
 * Custom Editor for the DemoCamera class. 
 * 
 * @Author: Chris Thodesen
 * @Contact: chris@yetikatt.com
 * @Version: 1.1
 */
// IMPORTS //
using UnityEngine;
using UnityEditor;

namespace com.yetikatt.Demo
{
    [CustomEditor(typeof(DemoSceneCamera)),ExecuteInEditMode]
    public class DemoSceneCameraEditor : Editor
    {
		/// <summary>
		/// The target of this custom editor window.
		/// </summary>
        private DemoSceneCamera _demoSceneCamera;

		/// <summary>
		/// The Transform of the target of this custom editor window.
		/// </summary>
        private Transform _handleTransform;

		/// <summary>
		/// The rotation of the target of this custom editor window.
		/// </summary>
        private Quaternion _handleRotation;

		/// <summary>
		/// The maximum bounds for sidescrolling.
		/// </summary>
        private Transform _maxTransform;

		/// <summary>
		/// The minimum bounds for sidescrolling.
		/// </summary>
        private Transform _minTransform;

		/// <summary>
		/// Overrides the default inspector
		/// </summary>
        public override void OnInspectorGUI()
        {
            _demoSceneCamera = target as DemoSceneCamera;

            GUILayout.Label("Demo Scene Camera Settings");

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Camera Mode: ");
            _demoSceneCamera.CameraMode = EditorGUILayout.Popup(_demoSceneCamera.CameraMode, _demoSceneCamera.CameraModeOptions);
            EditorGUILayout.EndHorizontal();

            if (_demoSceneCamera.CameraMode == 0)
            {
                EditorGUI.BeginChangeCheck();
                bool autoMove = EditorGUILayout.Toggle("Auto Rotate: ", _demoSceneCamera.DoAutoMove);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_demoSceneCamera, "Toggle AutoRotate");
                    EditorUtility.SetDirty(_demoSceneCamera);
                    _demoSceneCamera.DoAutoMove = autoMove;
                }

                EditorGUI.BeginChangeCheck();
                float maxSpeed = EditorGUILayout.Slider("Rotation Speed: ", _demoSceneCamera.MaxSpeed, 0f, 5f);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_demoSceneCamera, "Rotation Speed");
                    _demoSceneCamera.MaxSpeed = maxSpeed;
                    EditorUtility.SetDirty(_demoSceneCamera);
                }

                EditorGUI.BeginChangeCheck();
                Vector3 point = EditorGUILayout.Vector3Field("Pivot Position: ", _demoSceneCamera.PivotOrigin);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_demoSceneCamera, "Move Point");
                    _demoSceneCamera.PivotOrigin = point;
                    EditorUtility.SetDirty(_demoSceneCamera);
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Scroll Axis: ");
                _demoSceneCamera.ScrollAxis = EditorGUILayout.Popup(_demoSceneCamera.ScrollAxis, _demoSceneCamera.CameraAxisOptions);
                EditorGUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();
                bool autoMove = EditorGUILayout.Toggle("Auto Scroll: ", _demoSceneCamera.DoAutoMove);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_demoSceneCamera, "Toggle AutoScroll");
                    EditorUtility.SetDirty(_demoSceneCamera);
                    _demoSceneCamera.DoAutoMove = autoMove;
                }

                EditorGUI.BeginChangeCheck();
                float maxSpeed = EditorGUILayout.Slider("Scroll Speed: ", _demoSceneCamera.MaxSpeed, 0f, 5f);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_demoSceneCamera, "Scrolling Speed");
                    _demoSceneCamera.MaxSpeed = maxSpeed;
                    EditorUtility.SetDirty(_demoSceneCamera);
                }

                EditorGUI.BeginChangeCheck();
                Vector3 minPoint = EditorGUILayout.Vector3Field("Min Position: ", _demoSceneCamera.MinPosition);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_demoSceneCamera, "Move Min Point");
                    _demoSceneCamera.MinPosition = minPoint;
                    EditorUtility.SetDirty(_demoSceneCamera);
                }

                EditorGUI.BeginChangeCheck();
                Vector3 maxPoint = EditorGUILayout.Vector3Field("Max Position: ", _demoSceneCamera.MaxPosition);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_demoSceneCamera, "Move Max Point");
                    _demoSceneCamera.MaxPosition = maxPoint;
                    EditorUtility.SetDirty(_demoSceneCamera);
                }
            }
        }

		/// <summary>
		/// Displays the handles for positioning the rotational pivot of the Camera 
		/// or the minimum and maximum sidescrolling bounds of the camera based on the active mode.
		/// </summary>
        private void OnSceneGUI()
        {
            _demoSceneCamera = target as DemoSceneCamera;
            _handleTransform = _demoSceneCamera.transform;
            _handleRotation = Tools.pivotRotation == PivotRotation.Local ? _handleTransform.rotation : Quaternion.identity;

            if (_demoSceneCamera.CameraMode == 0)
            {
                EditorGUI.BeginChangeCheck();

                Vector3 point = _demoSceneCamera.PivotOrigin;
                point = Handles.DoPositionHandle(point, _handleRotation);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_demoSceneCamera, "Move Pivot Point");
                    EditorUtility.SetDirty(_demoSceneCamera);
                    _demoSceneCamera.PivotOrigin = point;
                }
            }
            else
            {
                EditorGUI.BeginChangeCheck();

                Vector3 maxPoint = _demoSceneCamera.MaxPosition;
                maxPoint = Handles.DoPositionHandle(maxPoint, _handleRotation);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_demoSceneCamera, "Move Max Point");
                    EditorUtility.SetDirty(_demoSceneCamera);
                    _demoSceneCamera.MaxPosition = maxPoint;
                }

                EditorGUI.BeginChangeCheck();

                Vector3 minPoint = _demoSceneCamera.MinPosition;
                minPoint = Handles.DoPositionHandle(minPoint, _handleRotation);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_demoSceneCamera, "Move Min Point");
                    EditorUtility.SetDirty(_demoSceneCamera);
                    _demoSceneCamera.MinPosition = minPoint;
                }
            }
        }
    }
}
