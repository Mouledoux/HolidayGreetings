/*
 * GridSnappingWindow.cs
 * 
 * EditorWindow extension which allows you to snap GameObjects 
 * position/scale/rotation.
 * 
 * @Author: Chris Thodesen
 * @Contact: chris@yetikatt.com
 * @Version: 1.2
 */

// IMPORTS //
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace com.yetikatt.Utils
{

	public class GridSnappingWindow : EditorWindow 
	{
		// INSTANCE //
		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static GridSnappingWindow Instance
		{
			get;
			private set;
		}

        // CONSTANTS //
		/// <summary>
		/// Integer value representing the left mouse button
		/// </summary>
        private const int LEFT_MOUSE_BUTTON = 0;

		/// <summary>
		/// The minimum width for float fields in this EditorWindow
		/// </summary>
        private const float FLOAT_MIN_WIDTH = 18f;

		/// <summary>
		/// The width for labels in this EditorWindow
		/// </summary>
        private const float LABEL_WIDTH = 16f;

		/// <summary>
		/// The minimum height for buttons in this EditorWindow
		/// </summary>
        private const float BUTTON_MIN_HEIGHT = 32f;

		/// <summary>
		/// The default lowest value GameObjects transform.position's should be restricted to.
		/// </summary>
        private const float DEFAULT_MINIMUM_RESTRICTION = -10f;

		/// <summary>
		/// The default maximum value GameObjects transform.position's should be restricted to.
		/// </summary>
        private const float DEFAULT_MAXIMUM_RESTRICTION = 10f;

        // VARIABLES //
		/// <summary>
		/// The List of each GameObject's change in transform.position from the previous update loop.
		/// </summary>
        private List<Vector3> positionChangeList = new List<Vector3>();

		/// <summary>
		/// The List of each GameObject's change in transform.localScale from the previous update loop.
		/// </summary>
        private List<Vector3> scaleChangeList = new List<Vector3>();

		/// <summary>
		/// The List of each GameObject's change in transform.eulerAngles from the previous update loop.
		/// </summary>
        private List<Vector3> rotationChangeList = new List<Vector3>();

		/// <summary>
		/// The List of each GameObject's transform.position during the previous update loop.
		/// </summary>
        private List<Vector3> previousPositionList = new List<Vector3>();

		/// <summary>
		/// The List of each GameObject's transform.localScale during the previous update loop.
		/// </summary>
        private List<Vector3> previousScaleList = new List<Vector3>();

		/// <summary>
		/// The List of each GameObject's transform.eulerAngels during the previous update loop.
		/// </summary>
        private List<Vector3> previousRotationList = new List<Vector3>();

		/// <summary>
		/// The List of each selected GameObject's initial transform.position, before
		/// the transforming began.
		/// </summary>
        private List<Vector3> startPositionList = new List<Vector3>();

		/// <summary>
		/// The List of each selected GameObject's initial transform.eulerAngles,
		/// before the transforming began.
		/// </summary>
        private List<Vector3> startScaleList = new List<Vector3>();

		/// <summary>
		/// The List of each selected GameObject's initial transform.localScale,
		/// before the transforming began.
		/// </summary>
        private List<Vector3> startRotationList = new List<Vector3>();

		/// <summary>
		/// The array of GUILayoutOptions for float fields in this EditorWindow.
		/// </summary>
        private GUILayoutOption[] _floatOptions;

		/// <summary>
		/// The array of GUILayoutOptions for label fields in this EditorWindow.
		/// </summary>
        private GUILayoutOption[] _labelOptions;

		/// <summary>
		/// The array of GUILayoutOptions for general buttons in this EditorWindow.
		/// </summary>
        private GUILayoutOption[] _buttonOptions;

		/// <summary>
		/// The array of GUILayoutOptions for axis buttons (X/Y/Z) in this EditorWindow.
		/// </summary>
        private GUILayoutOption[] _axisOptions;

		/// <summary>
		/// The GUIStyle for Labels
		/// </summary>
        private GUIStyle _labelStyle = new GUIStyle();

		/// <summary>
		/// The GUIStyle for the Title.
		/// </summary>
        private GUIStyle _titleStyle = new GUIStyle();

		/// <summary>
		/// Boolean value for if position snapping should be enabled
		/// </summary>
        public bool doSnapPosition = true;

		/// <summary>
		/// Boolean value for if position snapping on the X axis should be enabled
		/// if position snapping itself is enabled.
		/// </summary>
        public bool doSnapPosX = true;

		/// <summary>
		/// Boolean value for if position snapping on the Y axis should be enabled
		/// if position snapping itself is enabled.
		/// </summary>
        public bool doSnapPosY = true;

		/// <summary>
		/// Boolean value for if position snapping on the Z axis should be enabled
		/// if position snapping itself is enabled.		
		/// </summary>
        public bool doSnapPosZ = true;

		/// <summary>
		/// The default values to snap to for position snapping
		/// </summary>
        private Vector3 _snapPosition = new Vector3(1f, 0.5f, 1f);

		/// <summary>
		/// Boolean value for if scale snapping should be enabled	
		/// </summary>
        public bool doSnapScale = false;

		/// <summary>
		/// Boolean value for if scale snapping on the X axis should be enabled
		/// if scale snapping itself is enabled.		
		/// </summary>
        public bool doSnapScaleX = true;

		/// <summary>
		/// Boolean value for if scale snapping on the Y axis should be enabled
		/// if scale snapping itself is enabled.		
		/// </summary>
        public bool doSnapScaleY = true;

		/// <summary>
		/// Boolean value for if scale snapping on the Z axis should be enabled
		/// if scale snapping itself is enabled.		
		/// </summary>
        public bool doSnapScaleZ = true;

		/// <summary>
		/// The default values to snap to for scale snapping
		/// </summary>
        private Vector3 _snapScale = new Vector3(1f, 1f, 1f);

		/// <summary>
		/// Boolean value for if rotation snapping should be enabled	
		/// </summary>
        public bool doSnapRotation = false;

		/// <summary>
		/// Boolean value for if rotation snapping on the X axis should be enabled
		/// if rotation snapping itself is enabled.		
		/// </summary>
        public bool doSnapRotX = true;

		/// <summary>
		/// Boolean value for if rotation snapping on the Y axis should be enabled
		/// if rotation snapping itself is enabled.		
		/// </summary>
        public bool doSnapRotY = true;

		/// <summary>
		/// Boolean value for if rotation snapping on the Z axis should be enabled
		/// if rotation snapping itself is enabled.		
		/// </summary>
        public bool doSnapRotZ = true;

		/// <summary>
		/// The default values to snap to for rotation snapping
		/// </summary>
        private Vector3 _snapRotation = new Vector3(90f, 90f, 90f);

		/// <summary>
		/// Boolean value for if position restriction should be enabled	
		/// </summary>
        public bool doRestrictPosition = false;

		/// <summary>
		/// Boolean value for if position restriction on the X axis should be enabled
		/// if position restriction itself is enabled.		
		/// </summary>
        public bool doRestrictY = false;

		/// <summary>
		/// Boolean value for if position restriction on the Y axis should be enabled
		/// if position restriction itself is enabled.		
		/// </summary>
		public bool doRestrictX = false;

		/// <summary>
		/// Boolean value for if position restriction on the Z axis should be enabled
		/// if position restriction itself is enabled.		
		/// </summary>
		public bool doRestrictZ = false;

		/// <summary>
		/// The minimum bounds to restrict positions to for position restriction.
		/// </summary>
        private Vector3 _minimumPosition = new Vector3(DEFAULT_MINIMUM_RESTRICTION,
            DEFAULT_MINIMUM_RESTRICTION, DEFAULT_MINIMUM_RESTRICTION);

		/// <summary>
		/// The maximum bounds to restrict positions to for position restriction
		/// </summary>
        private Vector3 _maximumPosition = new Vector3(DEFAULT_MAXIMUM_RESTRICTION,
            DEFAULT_MAXIMUM_RESTRICTION, DEFAULT_MAXIMUM_RESTRICTION);

		/// <summary>
		/// Boolean value for if all snapping should be permitted or disabled.
		/// </summary>
        public bool SnappingToggled = true;

		/// <summary>
		/// Boolean value to check if the active selection has changed during the last 
		/// SceneGUI Update.
		/// </summary>
        private bool _editing = false;

		/// <summary>
		/// The Color for enabled buttons.
		/// </summary>
        private static Color _enabledColor;

		/// <summary>
		/// The Color for disabled buttons
		/// </summary>
        private static Color _disabledColor;


		/// <summary>
		/// Checks if the Window Instance currently exists
		/// </summary>
		/// <returns><c>true</c> if it does <c>false</c> otherwise.</returns>
        private static bool WindowActive()
		{
			return Instance != null;
		}

		/// <summary>
		/// Shows the Grid Snapping window, or closes it, if it is already open
		/// </summary>
		[MenuItem("Edit/Auto Snapping Tools/Grid Snapping %_l")]
		public static void ShowWindow()
		{
			if (WindowActive())
			{
				EditorWindow window = GetWindow(typeof(GridSnappingWindow), false, "Snapping Tools");
				window.Close();
			}
			else
			{
				EditorWindow window = GetWindow(typeof(GridSnappingWindow), false, "Snapping Tools");
				window.maxSize = new Vector2(256, 400);
                window.minSize = new Vector2(128, 400);
			}
		}

		/// <summary>
		/// Toggles the SnappingToggled Boolean, enabling/disabling all snapping
		/// </summary>
        [MenuItem("Edit/Auto Snapping Tools/Toggle Snapping %#_l")]
        public static void TogglePositionSnapping()
        {
            if (WindowActive())
            {
                Instance.SnappingToggled = !Instance.SnappingToggled;
                EditorWindow window = GetWindow(typeof(GridSnappingWindow), false, "Snapping Tools");
                window.Repaint();
            }
        }

		/// <summary>
		/// Carries out basic setup, registering functions to delegates and setting up GUILayoutOptions
		/// and styles.
		/// </summary>
        void OnEnable()
		{
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			SceneView.onSceneGUIDelegate += OnSceneGUI;
            Undo.undoRedoPerformed += Repaint;

            _floatOptions = new GUILayoutOption[] { GUILayout.MinWidth(FLOAT_MIN_WIDTH) };
            _labelOptions = new GUILayoutOption[] { GUILayout.Width(LABEL_WIDTH) };
            _buttonOptions = new GUILayoutOption[] { GUILayout.MinHeight(BUTTON_MIN_HEIGHT) };
            _axisOptions = new GUILayoutOption[] {GUILayout.MinWidth(24f)};

            

            _labelStyle.fontSize = 8;
            _labelStyle.alignment = TextAnchor.MiddleLeft;

            _titleStyle.fontSize = 18;
            _titleStyle.fontStyle = FontStyle.Bold;
            _titleStyle.wordWrap = true;
            _titleStyle.alignment = TextAnchor.MiddleCenter;

            _enabledColor = new Color(0.874f, 0.941f, 0.847f);
            _disabledColor = new Color(0.941f, 0.847f, 0.847f);

            Instance = this;
        }

		/// <summary>
		/// Removes functions from any assigned delegates befores closing.
		/// </summary>
		void OnDestroy()
		{
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
            Undo.undoRedoPerformed -= Repaint;
		}

		/// <summary>
		/// Shows the GridSnapping Editor Window.
		/// </summary>
		void OnGUI()
		{
            EditorGUILayout.Space();

            GUILayout.Label("SNAPPING TOOLS", _titleStyle);

            EditorGUILayout.Space();

            // Snapping Toggle Button //
            GUI.color = SnappingToggled ? _enabledColor : _disabledColor;
            EditorGUI.BeginChangeCheck();
            GUI.Button(EditorGUILayout.GetControlRect(_buttonOptions), SnappingToggled ? "Enabled" : "Disabled");
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Toggle Snapping");
                SnappingToggled = !SnappingToggled;
                EditorUtility.SetDirty(this);
            }

            EditorGUILayout.Space();

            // Snapping Disabled Check //
            EditorGUI.BeginDisabledGroup(!SnappingToggled);

            // POSITION SNAPPING //
            GUI.color = doSnapPosition ? _enabledColor : _disabledColor;
            EditorGUI.BeginChangeCheck();
            GUI.Button(EditorGUILayout.GetControlRect(_buttonOptions), "Snap Position");
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Toggle Position Snapping");
                doSnapPosition = !doSnapPosition;
                EditorUtility.SetDirty(this);
            }

            GUI.color = Color.white;

            if (doSnapPosition)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

                EditorGUILayout.BeginVertical();

                GUI.color = doSnapPosX ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "X");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle X Position Snapping");
                    doSnapPosX = !doSnapPosX;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                if (doSnapPosX)
                {
                    _snapPosition.x = EditorGUILayout.FloatField(_snapPosition.x, _axisOptions);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();

                GUI.color = doSnapPosY ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "Y");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle Y Position Snapping");
                    doSnapPosY = !doSnapPosY;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                if (doSnapPosY)
                {
                    _snapPosition.y = EditorGUILayout.FloatField(_snapPosition.y,_axisOptions);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();

                GUI.color = doSnapPosZ ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "Z");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle Z Position Snapping");
                    doSnapPosZ = !doSnapPosZ;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                if (doSnapPosZ)
                {
                    _snapPosition.z = EditorGUILayout.FloatField(_snapPosition.z, _axisOptions);
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            // SCALE SNAPPING //
            GUI.color = doSnapScale ? _enabledColor : _disabledColor;
            EditorGUI.BeginChangeCheck();
            GUI.Button(EditorGUILayout.GetControlRect(_buttonOptions), "Snap Scale");
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Toggle Scale Snapping");
                doSnapScale = !doSnapScale;
                EditorUtility.SetDirty(this);
            }

            GUI.color = Color.white;

            if (doSnapScale)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

                EditorGUILayout.BeginVertical();

                GUI.color = doSnapScaleX ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "X");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle X Scale Snapping");
                    doSnapScaleX = !doSnapScaleX;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                if (doSnapScaleX)
                {
                    _snapScale.x = EditorGUILayout.FloatField(_snapScale.x, _axisOptions);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();

                GUI.color = doSnapScaleY ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "Y");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle Y Scale Snapping");
                    doSnapScaleY = !doSnapScaleY;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                if (doSnapScaleY)
                {
                    _snapScale.y = EditorGUILayout.FloatField(_snapScale.y, _axisOptions);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();

                GUI.color = doSnapScaleZ ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "Z");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle Z Scale Snapping");
                    doSnapScaleZ = !doSnapScaleZ;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                if (doSnapScaleZ)
                {
                    _snapScale.z = EditorGUILayout.FloatField(_snapScale.z, _axisOptions);
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            // ROTATION SNAPPING //
            GUI.color = doSnapRotation ? _enabledColor : _disabledColor;
            EditorGUI.BeginChangeCheck();
            GUI.Button(EditorGUILayout.GetControlRect(_buttonOptions), "Snap Rotation");
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Toggle Rotation Snapping");
                doSnapRotation = !doSnapRotation;
                EditorUtility.SetDirty(this);
            }

            GUI.color = Color.white;

            if (doSnapRotation)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));

                EditorGUILayout.BeginVertical();

                GUI.color = doSnapRotX ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "X");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle X Rotation Snapping");
                    doSnapRotX = !doSnapRotX;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                if (doSnapRotX)
                {
                    _snapRotation.x = EditorGUILayout.FloatField(_snapRotation.x, _axisOptions);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();

                GUI.color = doSnapRotY ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "Y");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle Y Rotation Snapping");
                    doSnapRotY = !doSnapRotY;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                if (doSnapRotY)
                {
                    _snapRotation.y = EditorGUILayout.FloatField(_snapRotation.y, _axisOptions);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();

                GUI.color = doSnapRotZ ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "Z");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle Z Rotation Snapping");
                    doSnapRotZ = !doSnapRotZ;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                if (doSnapRotZ)
                {
                    _snapRotation.z = EditorGUILayout.FloatField(_snapRotation.z, _axisOptions);
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Space();

            // POSITION RESTRICTION //
            GUI.color = doRestrictPosition ? _enabledColor : _disabledColor;
            EditorGUI.BeginChangeCheck();
            GUI.Button(EditorGUILayout.GetControlRect(_buttonOptions), "Restrict Position");
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Toggle Position Restriction");
                doRestrictPosition = !doRestrictPosition;
                EditorUtility.SetDirty(this);
            }

            GUI.color = Color.white;

            if (doRestrictPosition)
            {
                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(false));
                EditorGUILayout.BeginHorizontal();

                GUI.color = doRestrictX ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "X");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle X Position Restriction");
                    doRestrictX = !doRestrictX;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                EditorGUI.BeginDisabledGroup(!doRestrictX);

                    EditorGUILayout.LabelField("Min", _labelStyle, _labelOptions);
                    _minimumPosition.x = EditorGUILayout.FloatField(_minimumPosition.x, _floatOptions);
                    EditorGUILayout.LabelField("Max", _labelStyle, _labelOptions);
                    _maximumPosition.x = EditorGUILayout.FloatField(_maximumPosition.x, _floatOptions);
                    _minimumPosition.x = _minimumPosition.x < _maximumPosition.x ? _minimumPosition.x : _maximumPosition.x;
                    _maximumPosition.x = _maximumPosition.x > _minimumPosition.x ? _maximumPosition.x : _minimumPosition.x;

                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUI.color = doRestrictY ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "Y");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle Y Position Restriction");
                    doRestrictY = !doRestrictY;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                EditorGUI.BeginDisabledGroup(!doRestrictY);

                    EditorGUILayout.LabelField("Min", _labelStyle, _labelOptions);
                    _minimumPosition.y = EditorGUILayout.FloatField(_minimumPosition.y, _floatOptions);
                    EditorGUILayout.LabelField("Max", _labelStyle, _labelOptions);
                    _maximumPosition.y = EditorGUILayout.FloatField(_maximumPosition.y, _floatOptions);
                    _minimumPosition.y = _minimumPosition.y < _maximumPosition.y ? _minimumPosition.y : _maximumPosition.y;
                    _maximumPosition.y = _maximumPosition.y > _minimumPosition.y ? _maximumPosition.y : _minimumPosition.y;

                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUI.color = doRestrictZ ? _enabledColor : _disabledColor;
                EditorGUI.BeginChangeCheck();
                GUI.Button(EditorGUILayout.GetControlRect(_axisOptions), "Z");
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Toggle Z Position Restriction");
                    doRestrictZ = !doRestrictZ;
                    EditorUtility.SetDirty(this);
                }

                GUI.color = Color.white;

                EditorGUI.BeginDisabledGroup(!doRestrictZ);

                    EditorGUILayout.LabelField("Min", _labelStyle, _labelOptions);
                    _minimumPosition.z = EditorGUILayout.FloatField(_minimumPosition.z, _floatOptions);
                    EditorGUILayout.LabelField("Max", _labelStyle, _labelOptions);
                    _maximumPosition.z = EditorGUILayout.FloatField(_maximumPosition.z, _floatOptions);
                    _minimumPosition.z = _minimumPosition.z < _maximumPosition.z ? _minimumPosition.z : _maximumPosition.z;
                    _maximumPosition.z = _maximumPosition.z > _minimumPosition.z ? _maximumPosition.z : _minimumPosition.z;

                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
        }

		/// <summary>
		/// Checks to see if the active selection has been modified.
		/// </summary>
		/// <param name="sceneView">The SceneView.</param>
		void OnSceneGUI(SceneView sceneView)
		{
            if(EditorApplication.isPlaying)
            {
                return;
            }

            if (!SnappingToggled)
            {
                return;
            }
            
            if (Selection.activeTransform != null)
            {
                if (Selection.activeTransform.hasChanged)
                    _editing = true;
                else
                    _editing = false;
            }
            else
            {
                _editing = false;
            }

            if (Event.current != null) {

                Event e = Event.current;
                
                if ( e.isMouse )
                {
                    if(e.button == LEFT_MOUSE_BUTTON)
                    {
                        if (e.type == EventType.mouseDown || e.type == EventType.MouseDown)
                        {
                            SetupArrays();
                        }
                    }
                }
			}
		}

		/// <summary>
		/// General update loop. Snaps selected transforms by which ever options are currently chosen
		/// </summary>
        void Update()
        {
            if (!SnappingToggled || EditorApplication.isPlaying || Selection.transforms.Length <= 0)
            {
                return;
            }

            if (_editing)
            {
                if (doSnapPosition)
                {
                    SnapPosition();
                }

                if (doSnapScale)
                {
                    SnapScale();
                }

                if (doSnapRotation)
                {
					// Rotation snapping requires PivotMode set to Pivot and PivotRotation set to Local to work properly 
					Tools.pivotMode = PivotMode.Pivot;
					Tools.pivotRotation = PivotRotation.Local;

                    SnapRotation();
                }

                if (doRestrictPosition)
                {
                    RestrictPosition();
                }
            }
        }

		/// <summary>
		/// Snaps the currently selected transforms to the specified position increments using their 
		/// transform.position properties.
		/// </summary>
        private void SnapPosition()
        {
            if (!WindowActive())
            {
                return;
            }

            if (positionChangeList.Count != Selection.transforms.Length ||
                            startPositionList.Count != Selection.transforms.Length ||
                            previousPositionList.Count != Selection.transforms.Length)
            {
                SetupArrays();
            }

            Vector3[] posArray = new Vector3[Selection.transforms.Length];
            for (int i = 0; i < Selection.transforms.Length; i++)
            {
                positionChangeList[i] += Selection.transforms[i].position - previousPositionList[i];
                previousPositionList[i] = Selection.transforms[i].position;

                Vector3 pos = startPositionList[i] + positionChangeList[i];
                if (doSnapPosX)
                {
                    pos.x = Round(pos.x, _snapPosition.x);
                }
                if (doSnapPosY)
                {
                    pos.y = Round(pos.y, _snapPosition.y);
                }
                if (doSnapPosZ)
                {
                    pos.z = Round(pos.z, _snapPosition.z);
                }
                posArray[i] = pos;
            }

            Undo.RecordObjects(Selection.transforms, "Snap Position");
            for (int i = 0; i < Selection.transforms.Length; i++)
            {
                Selection.transforms[i].position = posArray[i];
            }
            EditorUtility.SetDirty(Instance);
        }
        
		/// <summary>
		/// Snaps the currently selected transforms to the specified scale increments using their
		/// transform.localScale properties.
		/// </summary>
        private void SnapScale()
        {
            if (!WindowActive())
            {
                return;
            }

            if (scaleChangeList.Count != Selection.transforms.Length ||
                startScaleList.Count != Selection.transforms.Length ||
                previousScaleList.Count != Selection.transforms.Length)
            {
                SetupArrays();
            }

            Vector3[] scaleArray = new Vector3[Selection.transforms.Length];
            for (int i = 0; i < Selection.transforms.Length; i++)
            {
                scaleChangeList[i] += Selection.transforms[i].localScale - previousScaleList[i];
                previousScaleList[i] = Selection.transforms[i].localScale;

                Vector3 scale = startScaleList[i] + scaleChangeList[i];
                if (doSnapScaleX)
                {
                    scale.x = Round(scale.x, _snapScale.x);
                }
                if (doSnapScaleY)
                {
                    scale.y = Round(scale.y, _snapScale.y);
                }
                if (doSnapScaleZ)
                {
                    scale.z = Round(scale.z, _snapScale.z);
                }
                scaleArray[i] = scale;
            }

            Undo.RecordObjects(Selection.transforms, "Snap Scale");
            for (int i = 0; i < Selection.transforms.Length; i++)
            {
                Selection.transforms[i].localScale = scaleArray[i];
            }
            EditorUtility.SetDirty(Instance);
        }

		/// <summary>
		/// Snaps the currently selected transforms to their specified rotational increments
		/// using their transform.eulerAngles properties.
		/// [Note] Currently requires the Tool Handles in Unity to be set to Pivot and Local
		/// And will force these modes if rotation snapping is enabled.
		/// </summary>
        private void SnapRotation()
        {
            if (!WindowActive())
            {
                return;
            }

            if (rotationChangeList.Count != Selection.transforms.Length ||
               startRotationList.Count != Selection.transforms.Length ||
               previousRotationList.Count != Selection.transforms.Length)
            {
                SetupArrays();
            }

            Vector3[] rotationArray = new Vector3[Selection.transforms.Length];
            for (int i = 0; i < Selection.transforms.Length; i++)
            {
                rotationChangeList[i] += Selection.transforms[i].eulerAngles - previousRotationList[i];
                previousRotationList[i] = Selection.transforms[i].eulerAngles;

                Vector3 rot = startRotationList[i] + rotationChangeList[i];
                if (doSnapRotX)
                {
                    rot.x = Round(rot.x, _snapRotation.x);
                }
                if (doSnapRotY)
                {
                    rot.y = Round(rot.y, _snapRotation.y);
                }
                if (doSnapRotZ)
                {
                    rot.z = Round(rot.z, _snapRotation.z);
                }
                rotationArray[i] = rot;
            }

            Undo.RecordObjects(Selection.transforms, "Snap Rotation");
            for (int i = 0; i < Selection.transforms.Length; i++)
            {
                Selection.transforms[i].eulerAngles = rotationArray[i];
            }
            EditorUtility.SetDirty(Instance);
        }

		/// <summary>
		/// Restricts the position of the selected transforms to within the minimum and maximum
		/// bounds specified.
		/// </summary>
        private void RestrictPosition()
        {
            foreach (Transform t in Selection.transforms)
            {
                Vector3 pos = t.transform.position;

                if (doRestrictX)
                {
                    pos.x = Mathf.Clamp(pos.x, _minimumPosition.x, _maximumPosition.x);
                }

                if (doRestrictY)
                {
                    pos.y = Mathf.Clamp(pos.y, _minimumPosition.y, _maximumPosition.y);
                }

                if (doRestrictZ)
                {
                    pos.z = Mathf.Clamp(pos.z, _minimumPosition.z, _maximumPosition.z);
                }

                t.transform.position = pos;
            }
        }

		/// <summary>
		/// Clears the Start/Change/Previous Lists and stores the initial values for the
		/// start and previous Position/Scale/Rotation Lists
		/// </summary>
        private void SetupArrays()
        {
            startPositionList.Clear();
            startScaleList.Clear();
            startRotationList.Clear();

            positionChangeList.Clear();
            scaleChangeList.Clear();
            rotationChangeList.Clear();

            previousPositionList.Clear();
            previousScaleList.Clear();
            previousRotationList.Clear();

            for (int i = 0; i < Selection.transforms.Length; i++)
            {
                startPositionList.Add(Selection.transforms[i].position);
                previousPositionList.Add(Selection.transforms[i].position);

                startScaleList.Add(Selection.transforms[i].localScale);
                previousScaleList.Add(Selection.transforms[i].localScale);

                startRotationList.Add(Selection.transforms[i].eulerAngles);
                previousRotationList.Add(Selection.transforms[i].eulerAngles);

                positionChangeList.Add(Vector3.zero);
                scaleChangeList.Add(Vector3.zero);
                rotationChangeList.Add(Vector3.zero); 
            }
        }

		/// <summary>
		/// Round the input value to the nearest roundValue
		/// </summary>
		/// <param name="input">The value to round</param>
		/// <param name="roundValue">The increment to round to.</param>
        private float Round(float input, float roundValue)
        {
            if (roundValue == 0)
                return input;

            float d = input / roundValue;
            int r = Mathf.RoundToInt(d);
            float m = r * roundValue;
            return m;
        }
    }
}