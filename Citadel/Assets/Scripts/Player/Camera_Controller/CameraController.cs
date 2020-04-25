﻿using NaughtyAttributes;
using UnityEngine;

namespace VHS
{
    public class CameraController : MonoBehaviour
    {
        #region Variables
        #region Data
        [Space, Header("Data")]
        [SerializeField] private CameraInputData camInputData = null;

        [Space, Header("Custom Classes")]
        [SerializeField] private CameraZoom cameraZoom = null;
        [SerializeField] private CameraSwaying cameraSway = null;       

        #endregion

        #region Settings
        [Space, Header("Look Settings")]
        [SerializeField] public Vector2 sensitivity = Vector2.zero;
        [SerializeField] private Vector2 smoothAmount = Vector2.zero;
        [SerializeField] [MinMaxSlider(-90f, 90f)] private Vector2 lookAngleMinMax = Vector2.zero;
        #endregion

        #region Private
        private float m_yaw;
        private float m_pitch;

        private float m_desiredYaw;
        private float m_desiredPitch;

        #region Components                    
        private Transform m_pitchTranform;
        private Camera m_cam;
        #endregion
        #endregion

        #endregion

        #region BuiltIn Methods  
        void Awake()
        {
            GetComponents();
            InitValues();
            InitComponents();
            ChangeCursorState();
        }

        public void UpdateSensitiviy()
        {
            sensitivity = new Vector2(GameVars.instance.saveManager.SENSITIVITY, GameVars.instance.saveManager.SENSITIVITY);
        }

        public void UpdateSmoothing()
        {
            if (GameVars.instance.saveManager.SMOOTHING == true)
            {
                smoothAmount = new Vector2(30, 30);
            }
            else
            {
                smoothAmount = new Vector2(9999, 9999);
            }
        }

        void LateUpdate()
        {
            CalculateRotation();
            SmoothRotation();
            ApplyRotation();
            HandleZoom();
        }
        #endregion

        #region Custom Methods
        void GetComponents()
        {
            m_pitchTranform = transform.GetChild(0).transform;
            m_cam = GetComponentInChildren<Camera>();
        }

        void InitValues()
        {
            m_yaw = transform.eulerAngles.y;
            m_desiredYaw = m_yaw;
        }

        void InitComponents()
        {
            cameraZoom.Init(m_cam, camInputData);
            cameraSway.Init(m_cam.transform);
        }

        void CalculateRotation()
        {
            m_desiredYaw += camInputData.InputVector.x * sensitivity.x * Time.deltaTime;
            m_desiredPitch -= camInputData.InputVector.y * sensitivity.y * Time.deltaTime;

            m_desiredPitch = Mathf.Clamp(m_desiredPitch, lookAngleMinMax.x, lookAngleMinMax.y);
        }

        void SmoothRotation()
        {
            m_yaw = Mathf.Lerp(m_yaw, m_desiredYaw, smoothAmount.x * Time.deltaTime);
            m_pitch = Mathf.Lerp(m_pitch, m_desiredPitch, smoothAmount.y * Time.deltaTime);
        }

        void ApplyRotation()
        {
            transform.eulerAngles = new Vector3(0f, m_yaw, 0f);
            m_pitchTranform.localEulerAngles = new Vector3(m_pitch, 0f, 0f);
        }

        public void RotatePlayer(Vector3 rotationToSet)
        {
            InteractionController.instance.teleportAnimator.SetTrigger("Activate");
            m_desiredYaw = rotationToSet.y;
            m_yaw = rotationToSet.y;

            m_desiredPitch = Mathf.Clamp(rotationToSet.x, lookAngleMinMax.x, lookAngleMinMax.y);
            m_pitch = m_desiredPitch;

            ApplyRotation();
            StartCoroutine(InteractionController.instance.fpsController.inputHandler.HaultMove(0.2f));
        }

        public void HandleSway(Vector3 _inputVector, float _rawXInput)
        {
            cameraSway.SwayPlayer(_inputVector, _rawXInput);
        }

        void HandleZoom()
        {
            if (GameVars.instance && !GameVars.instance.isPaused)
            {
                if (InteractionController.instance.hasPlayerDied == false)
                {
                    if (InteractionController.instance.switchingWeapons == false)
                    {
                        if (InteractionController.instance.currentWeapon)
                        {
                            WeaponController currentWeapon = InteractionController.instance.currentWeapon.GetComponent<WeaponController>();
                            if(currentWeapon.weaponParams.isRapidFire)
                            {
                                if (camInputData.ZoomHeld)
                                {
                                    currentWeapon.OnAttack();
                                }
                            }
                            else
                            {
                                if (camInputData.ZoomClicked)
                                {
                                    currentWeapon.OnAttack();
                                }
                            }
                        }

                        if (camInputData.ReloadClicked)
                        {
                            if (InteractionController.instance.currentWeapon)
                            {
                                InteractionController.instance.currentWeapon.GetComponent<WeaponController>().OnReload();
                            }
                        }
                    }
                }
            }
        }

        public void ChangeRunFOV(bool _returning)
        {
            cameraZoom.ChangeRunFOV(_returning, this);
        }

        void ChangeCursorState()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        #endregion
    }
}
