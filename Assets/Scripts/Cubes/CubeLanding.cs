using BallDrop.Base;
using UnityEngine;

namespace BallDrop
{
    public class CubeLanding : BaseCube
    {
        public override void ActivateAndSetPosition(Transform parent, float zpos)
        {
            base.ActivateAndSetPosition(parent, zpos);
            SetColor(ColorData.Instance.GetPrimaryColor());
            //SetTexture(GameData.Instance.GetPrimaryTexture());
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}