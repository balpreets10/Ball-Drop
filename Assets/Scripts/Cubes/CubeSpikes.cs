using BallDrop.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class CubeSpikes : BaseCube
    {
        [Header("Spike Cube Properties")]
        [SerializeField]
        public MeshRenderer[] SpikeRenderer;

        private List<Vector3> InitialSpikePositions = new List<Vector3>();

        public override void ActivateAndSetPosition(Transform parent, float zpos)
        {
            base.ActivateAndSetPosition(parent, zpos);
            SetColor(ColorData.Instance.GetPrimaryColor());
            SetSpikeColor(ColorData.Instance.GetSecondaryColor());
            SetSpikesPosition();
            MoveSpikes();
        }

        private void SetSpikesPosition()
        {
            foreach (MeshRenderer meshRenderer in SpikeRenderer)
            {
                InitialSpikePositions.Add(meshRenderer.transform.localPosition);
            }
        }

        public override void Deactivate()
        {
            int counter = 0;
            foreach (MeshRenderer meshRenderer in SpikeRenderer)
            {
                LeanTween.cancel(meshRenderer.gameObject);
                meshRenderer.transform.localPosition = InitialSpikePositions[counter++];
            }
            base.Deactivate();
        }

        private void MoveSpikes()
        {
            foreach (MeshRenderer spike in SpikeRenderer)
            {
                LeanTween.moveLocalY(spike.gameObject, -0.0302f, .5f).setLoopPingPong();
            }
        }

        private void SetSpikeTextures(Texture texture)
        {
            for (int i = 0; i < SpikeRenderer.Length; i++)
            {
                SpikeRenderer[i].sharedMaterial.SetTexture("_MainTex", texture);
            }
        }

        private void SetSpikeColor(Color color)
        {
            for (int i = 0; i < SpikeRenderer.Length; i++)
            {
                SpikeRenderer[i].material.SetColor("_CubeColor", color);
            }
        }

        public void GetRenderer()
        {
            m_Renderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            SpikeRenderer = new MeshRenderer[4];
            for (int i = 0; i < 4; i++)
                SpikeRenderer[i] = transform.GetChild(0).GetChild(i + 1).GetComponent<MeshRenderer>();
        }
    }
}