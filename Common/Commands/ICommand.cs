namespace PhlegmaticOne.SharpTennis.Game.Common.Commands
{
    public interface ICommand
    {
        bool CanExecute(object parameter);
        void Execute(object parameter);
    }
}
