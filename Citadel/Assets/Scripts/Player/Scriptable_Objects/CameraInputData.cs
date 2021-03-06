﻿using UnityEngine;

namespace VHS
{

    [CreateAssetMenu(fileName = "CameraInputData", menuName = "FirstPersonController/Data/CameraInputData", order = 0)]
    public class CameraInputData : ScriptableObject
    {
        #region Data
        Vector2 m_inputVector;
        bool m_isZooming;
        bool m_zoomClicked;
        bool m_zoomHeld;
        bool m_zoomReleased;
        bool m_reloadClicked;
        bool m_reloadReleased;
        #endregion

        #region Properties
        public Vector2 InputVector => m_inputVector;
        public float InputVectorX
        {
            set => m_inputVector.x = value;
        }

        public float InputVectorY
        {
            set => m_inputVector.y = value;
        }

        public bool IsZooming
        {
            get => m_isZooming;
            set => m_isZooming = value;
        }

        public bool ZoomClicked
        {
            get => m_zoomClicked;
            set => m_zoomClicked = value;
        }

        public bool ZoomHeld
        {
            get => m_zoomHeld;
            set => m_zoomHeld = value;
        }

        public bool ZoomReleased
        {
            get => m_zoomReleased;
            set => m_zoomReleased = value;
        }

        public bool ReloadClicked
        {
            get => m_reloadClicked;
            set => m_reloadClicked = value;
        }

        public bool ReloadReleased
        {
            get => m_reloadReleased;
            set => m_reloadReleased = value;
        }
        #endregion

        #region Custom Methods
        public void ResetInput()
        {
            m_inputVector = Vector2.zero;
            m_isZooming = false;
            m_zoomClicked = false;
            m_zoomReleased = false;
        }
        #endregion
    }
}
