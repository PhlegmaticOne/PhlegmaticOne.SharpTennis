using System;
using PhlegmaticOne.SharpTennis.Game.Common.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components.Base;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Transform;
using SharpDX;
using SharpDX.Direct2D1;

namespace PhlegmaticOne.SharpTennis.Game.Engine2D.Components
{
    public class SelectableComponent<T> : Selectable
    {
        private ImageComponent _imageComponent;
        private TextComponent _textComponent;
        private ButtonComponent _buttonComponent;

        public T Value { get; private set; }

        public event Action<SelectableComponent<T>> Selected;

        public void Setup(ImageComponent imageComponent, TextComponent textComponent,
            ButtonComponent buttonComponent, T value)
        {
            Value = value;
            textComponent.RectTransform = imageComponent.RectTransform;
            _imageComponent = imageComponent;
            _textComponent = textComponent;
            _buttonComponent = buttonComponent;
            RectTransform = imageComponent.RectTransform;
            _buttonComponent.OnClick.Add(Select);
            _textComponent.BrushChanged += TextComponentOnBrushChanged;
            Brush = textComponent.Brush;
        }

        public void Setup(string imagePath, int fontSize, T value)
        {
            var go = new GameObject();
            var image = ImageComponent.Create(imagePath, Vector2.Zero, Anchor.Center);
            var textComponent = TextComponent.Create(Colors.White, value.ToString(), TextFormatData.DefaultForSize(fontSize));
            var button = new ButtonComponent(image, textComponent);
            Setup(image, textComponent, button, value);
            go.AddComponent(image, false);
            go.AddComponent(button, false);
            go.AddComponent(this, false);
            go.AddComponent(textComponent, false);
        }

        private void TextComponentOnBrushChanged(Brush obj)
        {
            Brush = obj;
        }

        public override void Dispose()
        {
            _textComponent.BrushChanged -= TextComponentOnBrushChanged;
        }

        public static SelectableComponent<T> Create(string imagePath, int fontSize, T value)
        {
            var result = new SelectableComponent<T>();
            result.Setup(imagePath, fontSize, value);
            return result;
        }

        public void UpdateValue(T value)
        {
            Value = value;
            _textComponent.Text = value.ToString();
        }

        protected override void OnSelect()
        {
            Selected?.Invoke(this);
        }
    }
}
