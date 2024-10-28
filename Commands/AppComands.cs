using GUI.Commands.Base;

namespace GUI.Commands
{
    internal class AppComands : BaseCommand
    {
        private readonly Action<object> _Execute;
        private readonly Func<object, bool>? _CanExecute;

        public AppComands(Action<object> Execute, Func<object, bool>? CanExecute = null) 
        {
            _CanExecute = CanExecute;
            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute)); 
        }

        public override bool CanExecute(object? parameter) => _CanExecute?.Invoke(parameter) ?? true;

        public override void Execute(object? parameter) => _Execute(parameter);
    }
}
