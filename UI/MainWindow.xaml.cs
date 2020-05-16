﻿using System;
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

namespace UI
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConsoleSubmit_Click(object sender, RoutedEventArgs e)
        {
            string consoleText = ConsoleInput.Text;
            Console_Write(consoleText);
            
        }

        private void Console_Write(string o)
        {
            _Console.Text = $"{_Console.Text}{Environment.NewLine}{o}";
            _Console.ScrollToEnd();
        }
    }
}
