using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;

namespace AAa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CifradoManager cifradoManager;

        public MainWindow()
        {
            InitializeComponent();
            cifradoManager = new CifradoManager();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            int blockSize = GetSelectedBlockSize();
            cifradoManager.GenerarTablaCifrado(blockSize);
            string textoCifrado = cifradoManager.CifrarTexto(InputTextBox.Text, blockSize);
            ResultTextBox.Text = textoCifrado;
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            int blockSize = GetSelectedBlockSize();
            string textoDescifrado = cifradoManager.DescifrarTexto(InputTextBox.Text, blockSize);
            ResultTextBox.Text = textoDescifrado;
        }

        private int GetSelectedBlockSize()
        {
            string selectedValue = (BlockSizeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            int blockSize = int.Parse(selectedValue.Split(' ')[0]);
            return blockSize;
        }

        private void SaveEncryptedButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                // Guardar el texto cifrado
                File.WriteAllText(saveFileDialog.FileName, ResultTextBox.Text);

                // Guardar la tabla de cifrado
                SaveFileDialog saveTableDialog = new SaveFileDialog();
                saveTableDialog.Filter = "Text Files (*.txt)|*.txt";
                saveTableDialog.Title = "Guardar Tabla de Cifrado";
                if (saveTableDialog.ShowDialog() == true)
                {
                    cifradoManager.GuardarTablaCifrado(saveTableDialog.FileName);
                }
            }
        }


        private void LoadEncryptedButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                // Cargar el texto cifrado
                string contenidoCifrado = File.ReadAllText(openFileDialog.FileName);
                InputTextBox.Text = contenidoCifrado;

                // Cargar la tabla de cifrado
                OpenFileDialog openTableDialog = new OpenFileDialog();
                openTableDialog.Filter = "Text Files (*.txt)|*.txt";
                openTableDialog.Title = "Cargar Tabla de Cifrado";
                if (openTableDialog.ShowDialog() == true)
                {
                    cifradoManager.CargarTablaCifrado(openTableDialog.FileName);
                }
            }
        }

    }
}