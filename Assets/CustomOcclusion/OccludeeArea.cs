using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CustomOcclusion
{
    public class OccludeeArea : MonoBehaviour
    {
        [SerializeField] private Renderer[] interiorRenderers;
        [SerializeField] private Cell[] cells;

        private bool isVisible = true;
        private void Awake()
        {
            OcclusionManager.OnUpdate += UpdateOcclusion;
        }

        private void OnDisable()
        {
                OcclusionManager.OnUpdate -= UpdateOcclusion;
        }
        private void UpdateOcclusion()
        {
            bool isInteriorVisible = false;
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].IsVisible)
                {
                    isInteriorVisible = true;
                    break;
                }
            }

            if (isInteriorVisible)
                ShowRenderers();
            else if (isVisible && !isInteriorVisible)
                HideRenderers();

            isVisible = isInteriorVisible;
        }
        private void ShowRenderers()
        {
            for (int i = 0; i < interiorRenderers.Length; i++)
            {              
                interiorRenderers[i].enabled = true;
            }
        }

        private void HideRenderers()
        {
            for (int i = 0; i < interiorRenderers.Length; i++)
            {
                interiorRenderers[i].enabled = false;
            }
        }
    }
}
