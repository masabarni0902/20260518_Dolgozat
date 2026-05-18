using System.ComponentModel;
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;

namespace Tanulok_WPF
{
    public class Student: INotifyPropertyChanged
    {
        private string _name;
        private string _classname;
        private int _mathGrade;
        private int _physicsGrade;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string ClassName
        {
            get => _classname;
            set { _classname = value; OnPropertyChanged(nameof(ClassName)); }
        }

        public int MathGrade
        {
            get => _mathGrade;
            set { _mathGrade = value; OnPropertyChanged(nameof(MathGrade)); OnPropertyChanged(nameof(Average)); }
        }

        public int PhysicsGrade
        {
            get => _physicsGrade;
            set { _physicsGrade = value; OnPropertyChanged(nameof(PhysicsGrade)); OnPropertyChanged(nameof(Average)); }
        }

        public double Average => (MathGrade + PhysicsGrade) / 2.0;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public ObservableCollection<Student> Students { get; set; }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set { _selectedStudent = value; OnPropertyChanged(nameof(SelectedStudent)); }
        }
        public int SelectedStudentIndex { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        public MainWindow()
        {
            InitializeComponent();

            Students = new ObservableCollection<Student>
            {
                new Student { Name = "Kovács Bence",   ClassName = "10.A", MathGrade = 5, PhysicsGrade = 4 },
                new Student { Name = "Nagy Eszter",    ClassName = "10.A", MathGrade = 3, PhysicsGrade = 4 },
            };

            Students.CollectionChanged += (s, e) => UpdateStats();
            DataContext=this;
            UpdateStats();
        }

        private void UpdateStats()
        {
            StudentCountBlock.Text = $"{Students.Count} Tanuló";

            if(Students.Count == 0)
            {
                ClassAvgBlock.Text = "-";
                BestStudentBlock.Text = "-";
                WorstStudentBlock.Text = "-";
                return;
            }

            double classAvg = Students.Average(s => s.Average);
            ClassAvgBlock.Text = classAvg.ToString("F2");

            var best = Students.OrderByDescending(s => s.Average).First();
            BestStudentBlock.Text = $"{best.Name} ({best.Average:F2})";

            var worst = Students.OrderBy(s => s.Average).First();
            WorstStudentBlock.Text = $"{worst.Name} ({worst.Average:F2})";
        }

        private void EditCommand(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Student rowStudent)
                SelectedStudent = rowStudent;

            if (SelectedStudent == null)
            {
                MessageBox.Show("Kérjük, válasszon ki egy tanulót!", "Figyelmeztetés",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            EditStudentWindow editWindow = new EditStudentWindow(SelectedStudent);

            if (editWindow.ShowDialog() == true)
            {
                Student updated = editWindow.EditedStudent;
                int index = Students.IndexOf(SelectedStudent);
                if (index != -1)
                    Students[index] = updated;
            }
            else
            {
                Student updated = editWindow.EditedStudent;
                int index = Students.IndexOf(SelectedStudent);
                if (index != -1)
                    Students[index] = updated;
            }
            UpdateStats();
        }

        private void DeleteCommand(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Student rowStudent)
                SelectedStudent = rowStudent;
            if (SelectedStudent == null) return;
            var result = MessageBox.Show(
                $"Biztosan törölni szeretnéd \"{SelectedStudent.Name}\" adatait?",
                "Törlés megerősítése",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Students.Remove(SelectedStudent);
                UpdateStats();
            }
        }

        private void AddStudentCommand(object sender, RoutedEventArgs e)
        {
            NewStudentWindow newWindow = new NewStudentWindow(Students);
            newWindow.ShowDialog();
           UpdateStats();
        }
    }
}