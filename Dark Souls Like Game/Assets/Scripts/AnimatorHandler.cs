using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator anim;
        int horizontal, vertical;
        public bool canRotate;

        public void Initialize()
        {
            anim = GetComponent<Animator>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < .55f)
                v = .5f;
            else if (verticalMovement > .55f)
                v = 1f;
            else if (verticalMovement < 0f && verticalMovement > -.55f)
                v = -.5f;
            else if (verticalMovement < -.55f)
                v = -1f;
            else
                v = 0;
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < .55f)
                h = .5f;
            else if (horizontalMovement > .55f)
                h = 1f;
            else if (horizontalMovement < 0f && horizontalMovement > -.55f)
                h = -.5f;
            else if (horizontalMovement < -.55f)
                h = -1f;
            else
                h = 0;
            #endregion

            anim.SetFloat(vertical, v, .1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, .1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            canRotate = true;
        }

        public void StopRotation()
        {
            canRotate = false;
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, .2f);
        }
    }
}
