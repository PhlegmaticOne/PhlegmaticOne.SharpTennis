using System.Threading.Tasks;
using PhlegmaticOne.SharpTennis.Game.Common.Infrastructure;
using PhlegmaticOne.SharpTennis.Game.Common.Tween;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Components;
using PhlegmaticOne.SharpTennis.Game.Engine2D.Popups;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Loading
{
    public class LoadingPopup : PopupBase
    {
        private ImageComponent _loadingIcon;
        private bool _isClosed;
        public void Setup(ImageComponent loadingIcon) => _loadingIcon = loadingIcon;

        protected override void OnShow()
        {
            _isClosed = false;
            _loadingIcon.RectTransform.IsPivot = true;
            RotateAsync(0.5f);
        }

        protected override void OnClose()
        {
            _isClosed = true;
            _loadingIcon.RectTransform.DoKill();
        }

        private void RotateAsync(float oneSpinTime)
        {
            Task.Run(async () =>
            {
                while (_isClosed == false)
                {
                    await Task.Delay((int)(Time.DeltaT * 1000));
                    var delta = 360 / (oneSpinTime / Time.DeltaT);
                    _loadingIcon.RectTransform.Rotate(new Vector3(0, 0, delta));
                }
            });
        }
    }
}
