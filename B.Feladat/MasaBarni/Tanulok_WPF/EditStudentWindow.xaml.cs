using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tanulok_WPF
{
    public partial class EditStudentWindow : Window, INotifyPropertyChanged
    {
        public Student EditedStudent { get; private set; }

        private string _studentName;
        public string StudentName
        {
            get => _studentName;
            set { _studentName = value; OnPropertyChanged(nameof(StudentName)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public EditStudentWindow(Student input)
        {
            InitializeComponent();

            EditedStudent = new Student
            {
                Name = input.Name,
                ClassName = input.ClassName,
                MathGrade = input.MathGrade,
                PhysicsGrade = input.PhysicsGrade
            };

            StudentName = EditedStudent.Name;
            DataContext = this;

            Loaded += (s, e) =>
            {
                SetComboByValue(ClassCombo, input.ClassName);
                SetComboByIndex(MathCombo, input.MathGrade - 1);
                SetComboByIndex(PhysicsCombo, input.PhysicsGrade - 1);
                UpdatePreview();
            };
        }

        private void SetComboByValue(ComboBox combo, string value)
        {
            foreach (var item in combo.Items)
            {
                if (item is ComboBoxItem cbi && cbi.Content?.ToString() == value)
                {
                    combo.SelectedItem = cbi;
                    return;
                }
            }
            if (combo.Items.Count > 0) combo.SelectedIndex = 0;
        }

        private void SetComboByIndex(ComboBox combo, int index)
        {
            if (index >= 0 && index < combo.Items.Count)
                combo.SelectedIndex = index;
        }

        private int GetComboGrade(ComboBox combo)
        {
            if (combo.SelectedItem is ComboBoxItem cbi &&
                int.TryParse(cbi.Content?.ToString(), out int val))
                return val;
            return combo.SelectedIndex + 1;
        }

        private void GradeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (MathCombo == null || PhysicsCombo == null || PreviewAvgBlock == null) return;
            int math = GetComboGrade(MathCombo);
            int physics = GetComboGrade(PhysicsCombo);
            PreviewAvgBlock.Text = ((math + physics) / 2.0).ToString("F2");
        }

        private void SaveStudent(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(StudentName))
            {
                MessageBox.Show("A tanuló nevét meg kell adni!", "Hiányzó adat",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            EditedStudent.Name = StudentName.Trim();
            EditedStudent.ClassName = (ClassCombo.SelectedItem as ComboBoxItem)?.Content?.ToString()
                                         ?? EditedStudent.ClassName;
            EditedStudent.MathGrade = GetComboGrade(MathCombo);
            EditedStudent.PhysicsGrade = GetComboGrade(PhysicsCombo);

            DialogResult = true;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
