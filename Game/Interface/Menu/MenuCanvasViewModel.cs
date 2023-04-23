﻿using PhlegmaticOne.SharpTennis.Game.Common.Commands;

namespace PhlegmaticOne.SharpTennis.Game.Game.Interface.Menu
{
    public class MenuCanvasViewModel
    {
        public MenuCanvasViewModel(ICommand playButtonCommand, ICommand exitButtonCommand)
        {
            PlayButtonCommand = playButtonCommand;
            ExitButtonCommand = exitButtonCommand;
        }

        public ICommand PlayButtonCommand { get; }
        public ICommand ExitButtonCommand { get; }
    }
}
