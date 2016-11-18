/*
 * DemoCameraEditor.cs
 * 
 * Draggable Camera that orbits around a pivot point or scrolls between to points. 
 * 
 * @Author: Chris Thodesen
 * @Contact: chris@yetikatt.com
 * @Version: 1.1
 */
/// IMPORTS //
using UnityEngine;

namespace com.yetikatt.Demo
{
    public class DemoSceneCamera : MonoBehaviour
    {
        // CONSTANTS //
		/// <summary>
		/// The integer value representing Carousel mode
		/// </summary>
        const int MODE_CAROUSEL = 0;

		/// <summary>
		/// The integer value representing Side Scrolling mode.
		/// </summary>
        const int MODE_SIDE_SCROLLER = 1;

		/// <summary>
		/// The value denoting that side scrolling should be 
		/// done on the X axis.
		/// </summary>
        const int X_AXIS = 0;

		/// <summary>
		/// The value denoting that side scrolling should be 
		/// done on the Y axis.
		/// </summary>
        const int Y_AXIS = 1;

		/// <summary>
		/// The value denoting that side scrolling should be 
		/// done on the Z axis.
		/// </summary>
        const int Z_AXIS = 2;

		/// <summary>
		/// The amount of friction to apply to the cameras 
		/// velocity if it is above it's maximum speed
		/// </summary>
        const float FRICTION = 0.95f;

        // DELEGATES //
		/// <summary>
		/// Is assigned the current function to use during the update loop.
		/// </summary>
        public delegate void State();
        public event State state;

		/// <summary>
		/// Is assigned the axis limiting function to use during side 
		/// scrolling mode.
		/// </summary>
        public delegate void LimitAxis();
        public event LimitAxis limitAxis;

        // VARIABLES //
		/// <summary>
		/// The origin of the pivot point to orbit around in Carousel mode
		/// </summary>
        public Vector3 PivotOrigin = new Vector3();

		/// <summary>
		/// The maximum position to scroll to in Side Scrolling Mode
		/// </summary>
        public Vector3 MaxPosition = new Vector3();

		/// <summary>
		/// The minimum position to scroll to in Side Scrolling Mode
		/// </summary>
        public Vector3 MinPosition = new Vector3();

		/// <summary>
		/// String Array holding the names of the Camera axis options that can be selected
		/// </summary>
        public string[] CameraAxisOptions = new string[] { "X Axis", "Y Axis", "Z Axis" };

		/// <summary>
		/// The axis to scroll on during Side Scrolling Mode.
		/// </summary>
        public int ScrollAxis = 0;

		/// <summary>
		/// String Array holding the names of the camera mode options that can be selected
		/// </summary>
        public string[] CameraModeOptions = new string[] { "Carousel Mode", "Sidescroller Mode" };

		/// <summary>
		/// The current camera mode (0 is Carousel mode, 1 is Side Scrolling mode)
		/// </summary>
        public int CameraMode = 0;

		/// <summary>
		/// The intial transform.position of the camera.
		/// </summary>
        private Vector3 _startPosition;

		/// <summary>
		/// Boolean value representing if the camera is currently being dragged by the user.
		/// </summary>
        private bool _isDragging = false;

		/// <summary>
		/// The x position of the mouse during the last update loop.
		/// </summary>
        private float _lastMouseX = 0f;

		/// <summary>
		/// The Maximum Speed at which the camera can travel
		/// </summary>
        public float MaxSpeed = 1f;

		/// <summary>
		/// The current speed of the camera.
		/// </summary>
		private float _speed;

		/// <summary>
		/// The current angle of the Carousel Camera
		/// </summary>
        private float _angle = 0f;

		/// <summary>
		/// The speed at which the camera was travelling when it was released by the user
		/// </summary>
        private float _releaseSpeed = 0f;

		/// <summary>
		/// The distance the camera is from the Pivot Origin.
		/// </summary>
        private float _dist;

		/// <summary>
		/// Boolean value representing if the camera should automatically scroll without user input
		/// </summary>
        public bool DoAutoMove = true;

		/// <summary>
		/// The axis on which the camera should move during Side Scrolling mode
		/// </summary>
        private Vector3 _direction;

		/// <summary>
		/// The current transform.position of the camera
		/// </summary>
        private Vector3 _position;

		/// <summary>
		/// Unity's start function, used to setup the Camera
		/// </summary>
        void Start()
        {
            _startPosition = transform.position;
            _dist = Vector3.Distance(_startPosition, PivotOrigin);
            _speed = Random.Range(0, 2) < 1 ? MaxSpeed : -MaxSpeed;

            switch (CameraMode)
            {
            case MODE_CAROUSEL:
			    state = UpdateCarousel;
                break;
            case MODE_SIDE_SCROLLER:
				SetupSideScroller(ScrollAxis);
	            state = UpdateSideScroller;
                    break;
            }
        }

		/// <summary>
		/// Sets up SideScrolling mode based on the provided axis
		/// </summary>
		/// <param name="axis">the axis to setup the camera for</param>
		void SetupSideScroller(int axis)
		{
			_position = transform.position;
			switch(axis)
			{
			case X_AXIS:
				SetupSideScrollerParameters(ref MinPosition.x,ref MaxPosition.x,Vector3.right);
				limitAxis = LimitXPosition;
				break;
			case Y_AXIS:
				SetupSideScrollerParameters(ref MinPosition.y,ref MaxPosition.y,Vector3.up);
				limitAxis = LimitYPosition;
				break;
			case Z_AXIS:
				SetupSideScrollerParameters(ref MinPosition.z,ref MaxPosition.z,Vector3.forward);
				limitAxis = LimitZPosition;
				break;
			}
		}

		/// <summary>
		/// Setups the direction of the SideScrolling Camera and checks that the 
		/// Minimum and maximum values are the correct way round
		/// </summary>
		/// <param name="min">The Minimum bounds of the camera</param>
		/// <param name="max">The Maximum bounds of the camera</param>
		/// <param name="direction">The Direction the camera should scroll in</param>
		void SetupSideScrollerParameters(ref float min,ref float max,Vector3 direction)
		{
			if(min > max)
			{
                float temp = max;
                max = min;
                min = temp;
			}
			_direction = direction;
		}

		/// <summary>
		/// Unity's built in Update loop, called the currently assigned state
		/// delegate, if one exists.
		/// </summary>
        void Update()
        {
            if (state != null)
                state();
        }

		/// <summary>
		/// Updates the camera in SideScrolling mode
		/// </summary>
        private void UpdateSideScroller()
        {
            PollInput();
            UpdatePosition();
        }

		/// <summary>
		/// Updates the position of the Camera, and limits its maximum velocity 
		/// and keeps it within it's bounds if it is a SideScrolling camera.
		/// </summary>
        private void UpdatePosition()
        {
            if (!_isDragging && DoAutoMove)
            {
                _position += (_speed * _direction) * Time.deltaTime;
            }
            else if (_isDragging)
            {
                float currentMouseX = (_lastMouseX - Input.mousePosition.x) / 64f;
                _position = _position + (currentMouseX * _direction);
                _lastMouseX = Input.mousePosition.x;
                _releaseSpeed = currentMouseX / Time.deltaTime;
            }

			if(limitAxis!=null)
				limitAxis();

			LimitMaximumVelocity();

            transform.position = _position;
        }

		/// <summary>
		/// Updates the camera in Carousel mode
		/// </summary>
        void UpdateCarousel()
        {
            PollInput();
            UpdateRotation();
            FacePivot();
        }

		/// <summary>
		/// Checks for user input.
		/// </summary>
        private void PollInput()
        {
            if (Input.GetMouseButton(0))
            {
                if (!_isDragging)
                {
                    _isDragging = true;
                    _lastMouseX = Input.mousePosition.x;
                }
            }
            else
            {
                if (_isDragging)
                {
                    _isDragging = false;
                    _speed = _releaseSpeed;
                }
            }

			if (CameraMode == MODE_CAROUSEL) {
					if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
						_dist -= 20f * Time.deltaTime;
						if(_dist <= 0)
						_dist = 0;
					}
					if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
						_dist += 20f * Time.deltaTime;
					}
			}

            transform.position = _position;
        }

		/// <summary>
		/// Limits the camera's position on the X axis.
		/// </summary>
        private void LimitXPosition()
        {
            if(_position.x > MaxPosition.x )
            {
                _position.x = MaxPosition.x;
                if (_speed > 0)
                    _speed = -_speed;
            }
            else if(_position.x < MinPosition.x)
            {
                _position.x = MinPosition.x;
                if (_speed < 0)
                    _speed = -_speed;
            }
        }

		/// <summary>
		/// Limits the camera's position on the Y axis
		/// </summary>
        private void LimitYPosition()
        {
            if (_position.y > MaxPosition.y)
            {
                _position.y = MaxPosition.y;
                if (_speed > 0)
                    _speed = -_speed;
            }
            else if (_position.y < MinPosition.y)
            {
                _position.y = MinPosition.y;
                if (_speed < 0)
                    _speed = -_speed;
            }
        }

		/// <summary>
		/// Limits the camera's position on the Z axis.
		/// </summary>
        private void LimitZPosition()
        {
            if (_position.z > MaxPosition.z)
            {
                _position.z = MaxPosition.z;
                if (_speed > 0)
                    _speed = -_speed;
            }
            else if (_position.z < MinPosition.z)
            {
                _position.z = MinPosition.z;
                if (_speed < 0)
                    _speed = -_speed;
            }
        }

		/// <summary>
		/// Updates the position of the camera and orbit's it around the PivotOrigin point.
		/// </summary>
        private void UpdateRotation()
        {
            if(!_isDragging && DoAutoMove)
            {
                _angle += _speed * Time.deltaTime;
            }
            else if(_isDragging)
            {
				float currentMouseX = (_lastMouseX - Input.mousePosition.x) / 360f;
                _angle = _angle + currentMouseX;
                _lastMouseX = Input.mousePosition.x;
                _releaseSpeed = currentMouseX / Time.deltaTime;
            }

            LimitMaximumVelocity();

            Vector3 rotationPosition = _startPosition;

			rotationPosition.x = PivotOrigin.x + Mathf.Cos(_angle) * _dist;
			rotationPosition.z = PivotOrigin.z + Mathf.Sin(_angle) * _dist;

			transform.position = rotationPosition;
        }

		/// <summary>
		/// Limits the maximum speed at which the camera can travel and decellerates it if
		/// it is faster than permitted.
		/// </summary>
        private void LimitMaximumVelocity()
        {
            if (Mathf.Abs(_speed) > MaxSpeed)
            {
                _speed *= FRICTION;
                if(Mathf.Abs(_speed) < MaxSpeed)
                {
                    _speed = _speed < 0 ? -MaxSpeed : MaxSpeed;
                }
            }
        }

		/// <summary>
		/// Turns the camera to face the PivotOrigin
		/// </summary>
        private void FacePivot()
        {
            transform.LookAt(PivotOrigin);
        }

		/// <summary>
		/// Unity's OnDestroy function, removes any assignments to the state or limitAxis delegates.
		/// </summary>
        void OnDestroy()
        {
            state = null;
            limitAxis = null;
        }
    }
}