using PhlegmaticOne.SharpTennis.Game.Common.Commands;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface
{
    public class MenuCanvasViewModel
    {
        public ICommand PlayButtonCommand { get; set; }
        public ICommand ExitButtonCommand { get; set; }
    }
}
