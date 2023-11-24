using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Hw._04._11._23
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Analyze.Click += Analyze_Click;
            Save.Click += Save_Click;
        }
        private async void Analyze_Click(object sender, RoutedEventArgs e)
        {
            string inputText = InputText.Text;
            List<string> selectedItems = Choice.SelectedItems.Cast<ListBoxItem>().Select(item => item.Content.ToString()).ToList();

            Report.Text = "Зачекайте, виконується аналіз...";
            await Task.Delay(1000);

            await Task.Run(async () =>
            {
             string report = "Звіт:\n";

                if (selectedItems.Contains("Кількість речень"))
                {
                    int sentenceCount = await Task.Run(() =>
                    {
                        string[] sentences = inputText.Split('.', '!', '?');
                        return sentences.Length - 1;
                    });

                    report += $"Кількість речень: {sentenceCount}\n";
                }
                if (selectedItems.Contains("Кількість символів"))
                {
                    int characterCount = await Task.Run(() =>
                    {
                        return inputText.Length;
                    });

                    report += $"Кількість символів: {characterCount}\n";
                }
                if (selectedItems.Contains("Кількість слів"))
                {
                    int wordCount = await Task.Run(() =>
                    {
                        string[] words = inputText.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        return words.Length;
                    });

                    report += $"Кількість слів: {wordCount}\n";
                }
                if (selectedItems.Contains("Кількість питальних речень"))
                {
                    int questionSentencesCount = await Task.Run(() =>
                    {
                        string[] sentences = inputText.Split('?');
                        return sentences.Length - 1;
                    });

                    report += $"Кількість питальних речень: {questionSentencesCount}\n";
                }
                if (selectedItems.Contains("Кількість окличних речень"))
                {
                    int exclamationSentencesCount = await Task.Run(() =>
                    {
                        string[] sentences = inputText.Split('!');
                        return sentences.Length - 1;
                    });

                    report += $"Кількість окличних речень: {exclamationSentencesCount}\n";
                }

                Dispatcher.Invoke(() =>
            {
                Report.Text = report;
            });
        });
        }

    private void Save_Click(object sender, RoutedEventArgs e)
            {
            string reportText = Report.Text;
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
               string filePath = saveFileDialog.FileName;
                try
                {
                  System.IO.File.WriteAllText(filePath, reportText);
                  MessageBox.Show("Звіт успішно збережено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при збереженні звіту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
 

