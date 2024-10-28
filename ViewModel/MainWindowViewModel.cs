using GUI.Commands;
using GUI.ViewModel.Base;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace GUI.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        /*-------------------------States----------------------------*/
        #region Название_окна_state
        private string _Title = "Парсер";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Входная_строка_для_парсера_state
        private string _InputStringForParser = "";
        public string InputStringForParser
        {
            get => _InputStringForParser;
            set => Set(ref _InputStringForParser, value);
        }
        #endregion

        #region Результат_парсера_state
        private string _ResultParser = "";
        public string ResultParser
        {
            get => _ResultParser;
            set => Set(ref _ResultParser, value);
        }
        #endregion

        #region Прогресс_выполнения_state
        private int _Progres = 0;
        public int Progres
        {
            get => _Progres;
            set => Set(ref _Progres, value);
        }
        #endregion
        /*-----------------------------------------------------*/

        /*-------------------------Commads----------------------------*/
        #region С#_парсер_выполнение
        public ICommand CsParserCommand {  get; }
        private bool CanCsParserExecute(object p) => true;
        private void OnCsParserExecute(object p)
        {
            MessageBox.Show("C# comming soon");
        }
        #endregion

        #region ANTLR_парсер_выполнение
        public ICommand ANTLRParserCommand { get; }
        private bool CanANTLRParserExecute(object p) => true;
        private void OnANTLRParserExecute(object p)
        {
            MessageBox.Show("ANTLR comming soon");
        }
        #endregion

        #region закрытие_приложения
        public ICommand CloseAppCommand { get; }
        private bool CanCloseAppCommandExecute(object p)=> true;
        private void OnCloseAppCommandExecute(object p)
        {
            MessageBox.Show("Save Comming soon");
            Application.Current.Shutdown();
        }
        #endregion
        /*-----------------------------------------------------*/
        public MainWindowViewModel() 
        {
            CsParserCommand = new AppComands(OnCsParserExecute, CanCsParserExecute);
            ANTLRParserCommand = new AppComands(OnANTLRParserExecute, CanANTLRParserExecute);
            CloseAppCommand = new AppComands(OnCloseAppCommandExecute, CanCloseAppCommandExecute);
        }
    }
}
