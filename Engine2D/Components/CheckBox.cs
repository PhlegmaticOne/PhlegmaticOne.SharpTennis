using System;
using System.Collections.Generic;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Components
{
    public class CheckBox : Selectable
    {
        private ImageComponent _backImage;
        private ImageComponent _checkedImage;

        public event Action<bool> Checked;

        public IReadOnlyList<GameObject> ImageObjects => new[] { _checkedImage.GameObject };

        public void Setup(string backImage, string checkedImage)
        {
            var go = new GameObject();
            var back = ImageComponent.Create(backImage, Vector2.Zero, Anchor.Center);
            var checkedImg = ImageComponent.Create(checkedImage, Vector2.Zero, Anchor.Center);
            var button = new ButtonComponent(back);
            
            _backImage = back;
            _checkedImage = checkedImg;
            var checkedGo = new GameObject();
            checkedGo.AddComponent(checkedImg, false);

            RectTransform = _backImage.RectTransform;
            button.RectTransform = back.RectTransform;

            go.AddComponent(back, false);
            go.AddComponent(button, false);
            go.AddComponent(this, false);
            button.OnClick.Add(OnCheck);
        }

        public void SetOffset(Vector2 offset)
        {
            RectTransform.Offset = offset;
            _checkedImage.RectTransform.Offset = offset;
        }


        private void OnCheck()
        {
            Check(!IsSelected);
            Checked?.Invoke(IsSelected);
        }

        public void Check(bool isCheck)
        {
            if (isCheck)
            {
                _checkedImage.Enabled = true;
                Select();
            }
            else
            {
                _checkedImage.Enabled = false;
                Deselect();
            }
        }

        public static CheckBox Create(string backImage, string checkedImage)
        {
            var box = new CheckBox();
            box.Setup(backImage, checkedImage);
            return box;
        }
    }
}
