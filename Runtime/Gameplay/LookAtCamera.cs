// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Makes the attached object face the camera each frame.
    /// Useful for health bars, name plates, and other billboard UI in world space.
    /// Caches Camera.main in Awake to avoid per-frame lookups.
    /// </summary>
    public class LookAtCamera : MonoBehaviour
    {
        /// <summary>
        /// The billboard method to use.
        /// </summary>
        public enum BillboardMode
        {
            /// <summary>Object looks directly at the camera position.</summary>
            LookAt,
            /// <summary>Object faces away from the camera (inverted LookAt).</summary>
            LookAtInverted,
            /// <summary>Object's forward matches the camera's forward direction.</summary>
            CameraForward,
            /// <summary>Object's forward is the inverse of camera's forward.</summary>
            CameraForwardInverted
        }

        [SerializeField] BillboardMode mode = BillboardMode.CameraForwardInverted;

        Transform cachedCameraTransform;

        void Awake()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cachedCameraTransform = mainCamera.transform;
            }
        }

        void LateUpdate()
        {
            if (cachedCameraTransform == null) return;
            ApplyBillboard();
        }

        void OnEnable()
        {
            if (cachedCameraTransform == null) return;
            ApplyBillboard();
        }

        /// <summary>
        /// Sets the billboard mode at runtime.
        /// </summary>
        /// <param name="newMode">The new billboard mode.</param>
        public void SetMode(BillboardMode newMode)
        {
            mode = newMode;
        }

        void ApplyBillboard()
        {
            switch (mode)
            {
                case BillboardMode.LookAt:
                    transform.LookAt(cachedCameraTransform.position);
                    break;

                case BillboardMode.LookAtInverted:
                    Vector3 direction = (transform.position - cachedCameraTransform.position).normalized;
                    transform.LookAt(transform.position + direction);
                    break;

                case BillboardMode.CameraForward:
                    transform.forward = cachedCameraTransform.forward;
                    break;

                case BillboardMode.CameraForwardInverted:
                    transform.forward = -cachedCameraTransform.forward;
                    break;
            }
        }
    }
}
