using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Tanulok_WPF
{
    public partial class NewStudentWindow : Window, INotifyPropertyChanged
    {
        private string _studentName;
        public string StudentName
        {
            get => _studentName;
            set { _studentName = value; OnPropertyChanged(nameof(StudentName)); }
        }

        private string _studentClassName = "10.A";
        public string StudentClassName
        {
            get => _studentClassName;
            set { _studentClassName = value; OnPropertyChanged(nameof(StudentClassName)); }
        }

        private int _studentMathGrade = 3;
        public int StudentMathGrade
        {
            get => _studentMathGrade;
            set { _studentMathGrade = value; OnPropertyChanged(nameof(StudentMathGrade)); UpdatePreview(); }
        }

        private int _studentPhysicsGrade = 3;
        public int StudentPhysicsGrade
        {
            get => _studentPhysicsGrade;
            set { _studentPhysicsGrade = value; OnPropertyChanged(nameof(StudentPhysicsGrade)); UpdatePreview(); }
        }

        public ObservableCollection<Student> Students { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public NewStudentWindow(ObservableCollection<Student> list)
        {
            InitializeComponent();
            Students = list;
            DataContext = this;
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (PreviewAvgBlock != null)
                PreviewAvgBlock.Text = ((StudentMathGrade + StudentPhysicsGrade) / 2.0).ToString("F2");
        }

        private int ParseComboItem(object selectedItem)
        {
            if (selectedItem is ComboBoxItem cbi && int.TryParse(cbi.Content?.ToString(), out int val))
                return val;
            if (selectedItem is int i)
                return i;
            if (selectedItem is string s && int.TryParse(s, out int sv))
                return sv;
            return 3; 
        }

        private string ParseComboString(object selectedItem)
        {
            if (selectedItem is ComboBoxItem cbi)
                return cbi.Content?.ToString() ?? "10.A";
            return selectedItem?.ToString() ?? "10.A";
        }

        private void AddStudent(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StudentName))
            {
                MessageBox.Show("A tanuló nevét meg kell adni!", "Hiányzó adat",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                NameBox.Focus();
                return;
            }

            var mathCombo = FindName("") as ComboBox; 
            int math = StudentMathGrade;
            int physics = StudentPhysicsGrade;


            Student newStudent = new Student
            {
                Name = StudentName.Trim(),
                ClassName = StudentClassName,
                MathGrade = math,
                PhysicsGrade = physics
            };

            Students.Add(newStudent);
            this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e) => this.Close();
    }
}
