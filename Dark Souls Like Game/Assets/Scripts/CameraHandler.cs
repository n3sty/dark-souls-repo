using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform, cameraTransform, cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed, followSpeed, pivotSpeed, minimumPivot = -35, maximumPivot = 35;
        private float targetPosition, defaultPosition, lookAngle, pivotAngle;

        public float cameraSphereRadius = .2f, cameraCollisionOffset = .2f, minimumCollisionOffset = .2f;



        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public void FollowTarget(float delta)
        {
            Vector3 TargetPosition = Vector3.SmoothDamp
                (myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = TargetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseInputX, float mouseInputY)
        {
            lookAngle += (mouseInputX * lookSpeed) / delta;
            pivotAngle -= (mouseInputY * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }

        public void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast
                (cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(distance - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / .2f);
            cameraTransform.localPosition = cameraTransformPosition;
        } 
    }
}
