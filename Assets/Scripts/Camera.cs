using UnityEngine;
using System.Collections;

namespace DesertSurvival
{
    public class Camera : MonoBehaviour
    {
        #region Properties

        [SerializeField]
        private LayerMask layerMask; //layermask of ground or other objects that affect height

        #region Movement Properties
        [SerializeField]
        private float keyboardMovementSpeed = 20f;
        [SerializeField]
        private float mouseMovementSpeed = 20f;
        [SerializeField]
        private float mouseMovementBorderThickness = 10f;
        #endregion

        #region Panning Properties
        [SerializeField]
        private float mousePanSpeed = 150f;
        [SerializeField]
        private float keyboardPanSpeed = 50f;
        [SerializeField]
        private float minVerticalPan = 25;
        [SerializeField]
        private float maxVerticalPan = 90;
        #endregion

        #region Zooming & Height Properties
        [SerializeField]
        private float mouseZoomSensivity = 250f;
        [SerializeField]
        private float minHeight = 50f;
        [SerializeField]
        private float maxHeight = 200f;
        [SerializeField]
        private float currentHeight = 100f;
        [SerializeField]
        private float dynamicHeightTolerance = 0.75f;
        #endregion

        #endregion

        #region Unity Methods
        void Start()
        {
            transform.position = new Vector3(0, currentHeight, 0);
        }

        void Update()
        {
            // Movements
            KeyboardMovement();
            MouseMovement();

            // Mouse panning
            MousePanning();

            // Keyboard pannings
            KeyboardPanning();

            MouseZoom();
            SetDynamicHeight();
        }
        #endregion

        #region Custom Methods

        #region Keyboard Movement 
        void KeyboardMovement()
        {
            // Get keyboard input values
            Vector3 keyboardMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // Move object if input value is different then zero
            if (keyboardMovementInput != Vector3.zero)
            {
                keyboardMovementInput *= keyboardMovementSpeed; // Multiply with movement speed
                keyboardMovementInput *= transform.position.y / 10; // Multiply with 10th of current height
                keyboardMovementInput *= UnityEngine.Time.deltaTime; // Multiply with deltaTime
                // Multiply movement input with Y axis rotation.
                keyboardMovementInput = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * keyboardMovementInput;
                // Convert world space to local space 
                keyboardMovementInput = transform.InverseTransformDirection(keyboardMovementInput);
                // Apply movement
                transform.Translate(keyboardMovementInput, Space.Self);
            }
        }
        #endregion

        #region Mouse Movement 
        void MouseMovement()
        {
            Vector3 movementVector = new Vector3(0, 0, 0);

            // Check mouse positions 
            if (Input.mousePosition.x >= Screen.width - mouseMovementBorderThickness)
                movementVector.x = 1;
            if (Input.mousePosition.x <= mouseMovementBorderThickness)
                movementVector.x = -1;
            if (Input.mousePosition.y >= Screen.height - mouseMovementBorderThickness)
                movementVector.z = 1;
            if (Input.mousePosition.y <= mouseMovementBorderThickness)
                movementVector.z = -1;

            if (movementVector != Vector3.zero)
            {
                movementVector *= mouseMovementSpeed;
                movementVector *= transform.position.y / 10; // Multiply with 10th of current height
                movementVector *= UnityEngine.Time.deltaTime;
                movementVector = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * movementVector;
                movementVector = transform.InverseTransformDirection(movementVector);

                transform.Translate(movementVector, Space.Self);
            }
        }
        #endregion

        #region Keyboard Panning
        void KeyboardPanning()
        {
            #region Horizontal Keyboard Panning
            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(Vector3.up, 1 * UnityEngine.Time.deltaTime * keyboardPanSpeed, Space.World);
            if (Input.GetKey(KeyCode.E))
                transform.Rotate(Vector3.up, -1 * UnityEngine.Time.deltaTime * keyboardPanSpeed, Space.World);
            #endregion

            #region Vertical Keyboard Panning
            Vector3 currentRotation = transform.localRotation.eulerAngles;
            float rotation = 0;

            // Set rotation value if any input is pressed.
            if (Input.GetKey(KeyCode.X))
                rotation = 1 * UnityEngine.Time.deltaTime * keyboardPanSpeed;
            if (Input.GetKey(KeyCode.Z))
                rotation = -1 * UnityEngine.Time.deltaTime * keyboardPanSpeed;

            // Clamp between 0 to 90.
            if (currentRotation.x + rotation >= maxVerticalPan)
                rotation = maxVerticalPan - currentRotation.x;
            else if (currentRotation.x + rotation <= minVerticalPan)
                rotation = minVerticalPan - currentRotation.x;

            // Set new rotation
            transform.Rotate(transform.right, rotation, Space.World);
            #endregion
        }
        #endregion

        #region Mouse Panning
        void MousePanning()
        {
            #region Horizontal Mouse Panning
            // Horizontal Mouse Panning
            if (Input.GetKey(KeyCode.Mouse2))
            {
                transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * UnityEngine.Time.deltaTime * mousePanSpeed, Space.World);
            }
            #endregion

            #region Vertical Mouse Panning
            if (Input.GetKey(KeyCode.Mouse2))
            {
                float rotation = -Input.GetAxis("Mouse Y") * UnityEngine.Time.deltaTime * mousePanSpeed;
                Vector3 currentRotation = transform.localRotation.eulerAngles;

                if (currentRotation.x + rotation >= maxVerticalPan)
                    rotation = maxVerticalPan - currentRotation.x;
                else if (currentRotation.x + rotation <= minVerticalPan)
                    rotation = minVerticalPan - currentRotation.x;

                transform.Rotate(transform.right, rotation, Space.World);
            }
            #endregion
        }
        #endregion

        #region Get Distance To Ground
        private float DistanceToGround()
        {
            RaycastHit hit;
            float distanceToGround = -1f;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
                distanceToGround = (hit.point - transform.position).magnitude;

            return distanceToGround;
        }
        #endregion

        #region Mouse Zoom
        void MouseZoom()
        {
            float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
            if (mouseScroll != 0)
            {
                Vector3 zoomDirection = transform.forward * mouseScroll * UnityEngine.Time.deltaTime * mouseZoomSensivity;
                float newHeight = transform.position.y + zoomDirection.y;

                if (newHeight >= minHeight && newHeight <= maxHeight)
                {
                    currentHeight = newHeight;
                    transform.Translate(zoomDirection, Space.World);
                }
                else if (newHeight < minHeight)
                {
                    currentHeight = minHeight;
                    transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
                }
                else if (newHeight > maxHeight)
                {
                    currentHeight = maxHeight;
                    transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
                }


            }
        }
        #endregion

        #region Set Dynamic Height
        void SetDynamicHeight()
        {
            if (DistanceToGround() == -1)
                return;

            if (DistanceToGround() != currentHeight)
            {
                float distanceToGround = DistanceToGround();

                if(currentHeight > distanceToGround)
                {
                    float difference = currentHeight - distanceToGround;
                    if (difference / currentHeight >= dynamicHeightTolerance)
                        transform.position = new Vector3(transform.position.x, transform.position.y + difference, transform.position.z);
                }
                if (currentHeight < distanceToGround)
                {
                    float difference = distanceToGround - currentHeight;
                    if (difference / distanceToGround >= dynamicHeightTolerance)
                        transform.position = new Vector3(transform.position.x, transform.position.y - difference, transform.position.z);
                }
            }

        }
        #endregion

        #region Limit Movement
        void LimitMovement()
        {
        
        }
        #endregion
        #endregion
    }
}