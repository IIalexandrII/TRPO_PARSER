using GUI.Commands;
using GUI.ViewModel.Base;
using System.Windows;
using System.Windows.Input;
using MyParser;
using Microsoft.Win32;
using System.IO;

namespace GUI.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    { 
        private MyParserCS_v2 _parserCS = new MyParserCS_v2();
        private string _currentFileName;

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

        #region загрузка_грамматики
        private static string _StartStatusGrammerText = "Грамматика --> ";
        private string _GrammerText = _StartStatusGrammerText+"(файл грамматики не выбран)";
        public string GrammerText
        {
            get => _GrammerText;
            set => Set(ref _GrammerText, value);
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
        private bool CanCsParserExecute(object p) => _parserCS.IsSetGrammer();
        private void OnCsParserExecute(object p)
        {
            Progres = 0;
            ResultParser = "";
            string[] inputLines = _InputStringForParser.Replace("\r","").Split('\n');
            List<string> resultLineCheck;
            int step = 100/inputLines.Length;
            foreach (string line in inputLines)
            {
                resultLineCheck = _parserCS.Check(line);
                ResultParser += "Input --> " + line + "\n";

                foreach (string result in resultLineCheck)
                {
                    ResultParser += result + "\n"; 
                }
                ResultParser += "\n";
                Progres += step;
            }
           
        }
        #endregion

        #region ANTLR_парсер_выполнение
        public ICommand ANTLRParserCommand { get; }
        private bool CanANTLRParserExecute(object p) => true;
        private void OnANTLRParserExecute(object p)
        {
            Progres = 0;
            ResultParser = "";
            string[] inputLines = _InputStringForParser.Replace("\r", "").Split('\n');
            List<string> resultLineCheck;
            int step = 100 / inputLines.Length;
            foreach (string line in inputLines)
            {
                resultLineCheck = _parserCS.CheckANTLR(line);
                ResultParser += "Input --> " + line + "\n";

                foreach (string result in resultLineCheck)
                {
                    ResultParser += result + "\n";
                }
                ResultParser += "\n";
                Progres += step;
            }
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

        #region Открыть__Сохранить__Сохранить_как
        public ICommand OpenFileCommand { get; }
        private bool CanOpenFileExecute(object p)=> true;
        private void onOpenFilExecutee(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите файл",
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _currentFileName = openFileDialog.FileName;
                    Title = $"Парсер ({_currentFileName})";

                    InputStringForParser = File.ReadAllText(_currentFileName);
      
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        
        public ICommand SaveFileCommand { get; }
        private bool CanSaveFileExecute(object p)=> true;
        private void onSaveFileExecute(object p)
        {
            if (string.IsNullOrEmpty(_currentFileName)) { onSaveAsFileExecute(p); return; }
            try
            {
                File.WriteAllText(_currentFileName, InputStringForParser);
                MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);         
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SaveAsFileCommand { get; }
        private bool CanSaveAsFileExecute(object p) => true;
        private void onSaveAsFileExecute(object p)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Сохранить файл",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, InputStringForParser);
                    _currentFileName = saveFileDialog.FileName; 
                    MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    Title = $"Парсер ({_currentFileName})";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region Загрузка_грамматики
        public ICommand LoadGrammerCommand { get; }
        private bool CanLoadGrammerCommandExecute(object p) => true;
        private void OnLoadGrammerCommandExecute(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите файл грамматики",
                Filter = "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _parserCS.SetGrammer(openFileDialog.FileName);
                    GrammerText = _StartStatusGrammerText + Path.GetFileName(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке грамматики: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region О_программе
        public ICommand ShowAboutProgrammCommand { get; }
        private bool CanShowAboutProgrammCommand(object p) => true;
        private void OnShowAboutProgrammCommandExecute(object p)
        {
            MessageBox.Show(
                "Программа реализует парсер собственной грамматики на основе оператора printf c++\n" +
                "Загрузка грамматикипроисходит из пункта меню \"настройки\" и представляет собой JSON файл\n" +
                "Также реализованы функции \"Сохранить\" \"Открыть\" и \"Сохранить как\", находящиеся в пункте меню \"Файл\"\n\n" +
                "Разработчик: Шунков Александр Александрович\n" +
                "    Группа: АСМ2-24\n" +
                "Почта: sasha.shunkov@mail.ru\n" +
                "Версия: 0.0.1",
                "О программе", MessageBoxButton.OK
                );
        }

        #endregion
        /*-----------------------------------------------------*/
        public MainWindowViewModel() 
        {
            CsParserCommand = new AppComands(OnCsParserExecute, CanCsParserExecute);

            ANTLRParserCommand = new AppComands(OnANTLRParserExecute, CanANTLRParserExecute);
            CloseAppCommand = new AppComands(OnCloseAppCommandExecute, CanCloseAppCommandExecute);

            OpenFileCommand = new AppComands(onOpenFilExecutee, CanOpenFileExecute);
            SaveFileCommand = new AppComands(onSaveFileExecute, CanSaveFileExecute);
            SaveAsFileCommand = new AppComands(onSaveAsFileExecute, CanSaveAsFileExecute);

            LoadGrammerCommand = new AppComands(OnLoadGrammerCommandExecute, CanLoadGrammerCommandExecute);

            ShowAboutProgrammCommand = new AppComands(OnShowAboutProgrammCommandExecute, CanShowAboutProgrammCommand);
        }
    }
}
