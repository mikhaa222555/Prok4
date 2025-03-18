using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Prok4
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Net.Http;
    using System.Text;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using static global::Prok4.TaskManagerApp.TaskManagerApp.ObjectToVisibilityConverter;
    using static Prok4.TaskManagerApp.TaskManagerApp.ObjectToVisibilityConverter;

    namespace Prok4
    {
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public partial class MainWindow : Window
        {
            public MainWindow()
            {

            }
            // --- Модели ---
            public class User
            {
                public int UserId { get; set; }
                public string Username { get; set; }
                public string Password { get; set; } // Не храним в открытом виде!
                public string Email { get; set; }
                public bool IsAdmin { get; set; }
            }

            public class Task
            {
                public int TaskId { get; set; }
                public string TaskName { get; set; }
                public string TaskDescription { get; set; }
                public DateTime DueDate { get; set; }
                public string Priority { get; set; }
                public string Status { get; set; }
                public string AssignedUser { get; set; }  // Будем отображать имя пользователя
            }

            public class Department
            {
                public int DepartmentId { get; set; }
                public string DepartmentName { get; set; }
                public string EncryptedData { get; set; }
            }

            // --- ViewModel (пример) ---
            public class MainViewModel : ViewModelBase
            {
                private readonly HttpClient _httpClient = new HttpClient();
                private const string BaseUrl = "http://your-server-address/api"; // Замените на адрес вашего сервера
                private ObservableCollection<Task> _tasks = new ObservableCollection<Task>();
                private User _currentUser;

                public ObservableCollection<Task> Tasks
                {
                    get => _tasks;
                    set { _tasks = value; OnPropertyChanged(); }
                }

                private void OnPropertyChanged()
                {
                    throw new NotImplementedException();
                }

                public User CurrentUser
                {
                    get => _currentUser;
                    set { _currentUser = value; OnPropertyChanged(); }
                }

                public ICommand LoginCommand { get; }
                public ICommand LoadTasksCommand { get; }

                public MainViewModel()
                {
                    LoginCommand = new RelayCommand(Login, CanLogin);
                    LoadTasksCommand = new RelayCommand(LoadTasks, CanLoadTasks);

                }

                private bool CanLogin(object parameter) => true;

                private async void Login(object parameter)
                {
                    if (parameter is object[] values && values.Length == 2 &&
                        values[0] is string username && values[1] is string password)
                    {
                        try
                        {
                            var requestData = new { username = username, password = password };
                            var json = System.Text.Json.JsonSerializer.Serialize(requestData); // Используем JsonSerializer

                            var content = new StringContent(json, Encoding.UTF8, "application/json");
                            var response = await _httpClient.PostAsync($"{BaseUrl}/login", content);

                            if (response.IsSuccessStatusCode)
                            {
                                var responseContent = await response.Content.ReadAsStringAsync();
                                CurrentUser = System.Text.Json.JsonSerializer.Deserialize<User>(responseContent);
                                MessageBox.Show($"Login successful! Welcome, {CurrentUser.Username}");
                                LoadTasks();
                            }
                            else
                            {
                                MessageBox.Show($"Login failed: {response.StatusCode}");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }

                private void LoadTasks()
                {
                    throw new NotImplementedException();
                }

                private bool CanLoadTasks(object parameter) => CurrentUser != null;

                private async void LoadTasks(object parameter)
                {
                    try
                    {
                        var response = await _httpClient.GetAsync($"{BaseUrl}/tasks");
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync();
                            Tasks = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<Task>>(responseContent); // Десериализация
                        }
                        else
                        {
                            MessageBox.Show($"Failed to load tasks: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
            }
        }
    }
    namespace TaskManagerApp
    {
        // --- Enum для статуса задачи ---
        public class TaskStatusEnum
        {
            public List<TaskStatus> Values
            {
                get
                {
                    return new List<TaskStatus> { TaskStatus.Faulted, TaskStatus.RanToCompletion, TaskStatus.Canceled, TaskStatus.Created, TaskStatus.Running };
                }
            }
        }



        namespace TaskManagerApp
        {
            public enum TaskPriority
            {
                Low,
                Medium,
                High,
                Critical
            }

            public static class TaskPriorityHelper
            {
                public static List<TaskPriority> GetTaskPriorities()
                {
                    return new List<TaskPriority> { TaskPriority.Low, TaskPriority.Medium, TaskPriority.High, TaskPriority.Critical };
                }
            }
        }






        namespace TaskManagerApp
        {
            // --- Конвертер для отображения/скрытия элементов ---
            public class ObjectToVisibilityConverter : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    return value != null ? Visibility.Visible : Visibility.Collapsed;
                }

                public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    return DependencyProperty.UnsetValue; // Или throw new NotSupportedException();
                }
                public class ObjectToVisibilityConverterw : IValueConverter
                {
                    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                    {
                        return value != null ? Visibility.Visible : Visibility.Collapsed;
                    }

                    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                    {
                        throw new NotImplementedException();
                    }
                }



                // --- Конвертер для передачи данных регистрации (для удобства передачи нескольких параметров) ---
                public class RegisterConverter : IMultiValueConverter
                {
                    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
                    {
                        if (values != null && values.Length == 3 &&
                            values[0] is string username &&
                            values[1] is string email &&
                            values[2] is string password)
                        {
                            return new object[] { username, email, password };
                        }
                        return null; // Return null if conversion fails
                    }

                    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
                    {
                        throw new NotSupportedException("ConvertBack is not supported for RegisterConverter");
                    }
                }

                // --- Базовый класс для RelayCommandBase (если у вас есть) ---
                public abstract class RelayCommandBase
                {
                    public event EventHandler? CanExecuteChanged;

                    protected virtual void OnCanExecuteChanged()
                    {
                        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                    }
                }

                // --- RelayCommand для реализации ICommand (упрощает привязку команд в MVVM) ---
                public class RelayCommand : RelayCommandBase, ICommand
                {
                    private readonly Action<object> _execute;
                    private readonly Predicate<object>? _canExecute;  // Use Predicate<object>?

                    public RelayCommand(Action<object> execute) : this(execute, null) { } // Constructor without canExecute

                    public RelayCommand(Action<object> execute, Predicate<object>? canExecute)
                    {
                        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                        _canExecute = canExecute;
                    }

                    bool ICommand.CanExecute(object parameter)
                    {
                        return _canExecute == null || _canExecute(parameter);
                    }

                    void ICommand.Execute(object parameter)
                    {
                        _execute(parameter);
                        // Optional:  Raise CanExecuteChanged after execution if relevant
                        OnCanExecuteChanged();
                    }
                }
            }
        }
    }
}
