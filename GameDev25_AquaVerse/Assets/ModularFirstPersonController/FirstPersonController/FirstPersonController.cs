using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;

    public bool canMove = true;

    #region Camera Movement Variables

    public Camera playerCamera;

    public float fov = 60f;
    public bool invertCamera = false;
    public bool cameraCanMove = true;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    #endregion

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;

    private bool isWalking = false;

    #region Jump

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;

    private bool isGrounded = false;

    #endregion

    #region Crouch

    public bool enableCrouch = true;
    public bool holdToCrouch = true;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    private bool isCrouched = false;
    private Vector3 originalScale;

    #endregion
    #endregion

    #region Head Bob

    public bool enableHeadBob = true;
    public Transform joint;
    public float bobSpeed = 10f;
    public Vector3 bobAmount = new Vector3(.15f, .05f, 0f);

    private Vector3 jointOriginalPos;
    private float timer = 0;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
        jointOriginalPos = joint.localPosition;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera.fieldOfView = fov;
    }

    private void Update()
    {
        if (!canMove) return;

        if (cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

            pitch += (invertCamera ? 1 : -1) * mouseSensitivity * Input.GetAxis("Mouse Y");
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }

        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        if (enableCrouch)
        {
            if (Input.GetKeyDown(crouchKey) && !holdToCrouch)
            {
                Crouch();
            }
            if (Input.GetKeyDown(crouchKey) && holdToCrouch)
            {
                isCrouched = false;
                Crouch();
            }
            else if (Input.GetKeyUp(crouchKey) && holdToCrouch)
            {
                isCrouched = true;
                Crouch();
            }
        }

        CheckGround();

        if (enableHeadBob)
        {
            HeadBob();
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        if (playerCanMove)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            isWalking = (targetVelocity.x != 0 || targetVelocity.z != 0) && isGrounded;

            targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = targetVelocity - velocity;
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;
        }

        if (isCrouched && !holdToCrouch)
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        if (isCrouched)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            walkSpeed /= speedReduction;
            isCrouched = false;
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            walkSpeed *= speedReduction;
            isCrouched = true;
        }
    }

    private void HeadBob()
    {
        if (isWalking)
        {
            timer += Time.deltaTime * bobSpeed;
            joint.localPosition = new Vector3(
                jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x,
                jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y,
                jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z
            );
        }
        else
        {
            timer = 0;
            joint.localPosition = Vector3.Lerp(joint.localPosition, jointOriginalPos, Time.deltaTime * bobSpeed);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FirstPersonController)), InitializeOnLoadAttribute]
public class FirstPersonControllerEditor : Editor
{
    FirstPersonController fpc;
    SerializedObject SerFPC;

    private void OnEnable()
    {
        fpc = (FirstPersonController)target;
        SerFPC = new SerializedObject(fpc);
    }

    public override void OnInspectorGUI()
    {
        SerFPC.Update();

        GUILayout.Label("First Person Controller", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        fpc.playerCamera = (Camera)EditorGUILayout.ObjectField("Camera", fpc.playerCamera, typeof(Camera), true);
        fpc.fov = EditorGUILayout.Slider("FOV", fpc.fov, 10f, 179f);
        fpc.cameraCanMove = EditorGUILayout.Toggle("Enable Camera Movement", fpc.cameraCanMove);
        fpc.invertCamera = EditorGUILayout.Toggle("Invert Y-Axis", fpc.invertCamera);
        fpc.mouseSensitivity = EditorGUILayout.Slider("Mouse Sensitivity", fpc.mouseSensitivity, 0.1f, 10f);
        fpc.maxLookAngle = EditorGUILayout.Slider("Max Look Angle", fpc.maxLookAngle, 10, 90);

        EditorGUILayout.Space();
        fpc.playerCanMove = EditorGUILayout.Toggle("Enable Movement", fpc.playerCanMove);
        fpc.walkSpeed = EditorGUILayout.Slider("Walk Speed", fpc.walkSpeed, 0.1f, 20f);

        EditorGUILayout.Space();
        fpc.enableJump = EditorGUILayout.Toggle("Enable Jump", fpc.enableJump);
        fpc.jumpKey = (KeyCode)EditorGUILayout.EnumPopup("Jump Key", fpc.jumpKey);
        fpc.jumpPower = EditorGUILayout.Slider("Jump Power", fpc.jumpPower, 0.1f, 20f);

        EditorGUILayout.Space();
        fpc.enableCrouch = EditorGUILayout.Toggle("Enable Crouch", fpc.enableCrouch);
        fpc.holdToCrouch = EditorGUILayout.Toggle("Hold to Crouch", fpc.holdToCrouch);
        fpc.crouchKey = (KeyCode)EditorGUILayout.EnumPopup("Crouch Key", fpc.crouchKey);
        fpc.crouchHeight = EditorGUILayout.Slider("Crouch Height", fpc.crouchHeight, 0.1f, 2f);
        fpc.speedReduction = EditorGUILayout.Slider("Speed Reduction", fpc.speedReduction, 0.1f, 1f);

        EditorGUILayout.Space();
        fpc.enableHeadBob = EditorGUILayout.Toggle("Enable Head Bob", fpc.enableHeadBob);
        fpc.joint = (Transform)EditorGUILayout.ObjectField("Camera Joint", fpc.joint, typeof(Transform), true);
        fpc.bobSpeed = EditorGUILayout.Slider("Bob Speed", fpc.bobSpeed, 1f, 20f);
        fpc.bobAmount = EditorGUILayout.Vector3Field("Bob Amount", fpc.bobAmount);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(fpc);
            Undo.RecordObject(fpc, "FPC Change");
            SerFPC.ApplyModifiedProperties();
        }
    }
}
#endif
