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
            Console_Write("Wpisz \"HELP\" aby wyświetlić dostępne komendy.");
            Console_Write("Wpisz \"EXAMPLE <komenda>\" aby wyświetlić przykładowe komendy.");
        }

        private void ConsoleSubmit_Click(object sender, RoutedEventArgs e)
        {
            string consoleText = ConsoleInput.Text;
            ConsoleInput.Text = "";
            Console_Write(consoleText);

            string[] data = consoleText.Split();
            switch (data[0])
            {
                case "HELP":
                    Console_Write("EXAMPLE <komenda> -- wyświetlenie przykładów użycia komend.");
                    Console_Write("MOV <cel> <źródło> -- MOV kopiuje dane z źródła do celu.");
                    break;
                case "EXAMPLE":
                    if (data.Length == 2)
                    {
                        ExampleCommands(data[1]);
                    }
                    else
                    {
                        Console_Write($"Niepoprawne użycie komendy.");
                    }
                    break;
                case "MOV":
                    if (data.Length == 3)
                    {
                        Console_Write($"Wykonano MOV {data[2]} -> {data[1]}");

                        if (data[2].Substring(data[2].Length - 1) == "h")
                        {
                            MOV(data[1], data[2].TrimEnd('h'));
                        }
                        else if (data[2].Substring(data[2].Length - 1) == "H")
                        {
                            MOV(data[1], data[2].TrimEnd('H'));
                        }
                        else
                        {
                            MOV(data[1], CalculateToHex(data[2]));
                        }
                    }
                    else
                    {
                        Console_Write($"Niepoprawna ilość danych.");
                    }
                    break;
                default:
                    Console_Write("Niepoprawna komenda.");
                    break;
            }
        }

        private string CalculateToHex(string v)
        {
            int t = int.Parse(v);
            return t.ToString("X");
        }

        private void MOV(string v1, string v2)
        {
            SetRegisterContent(v1, v2);
        }

        private void ExampleCommands(string v)
        {
            switch (v)
            {
                case "MOV":
                    Console_Write("MOV AX BX -- Skopiowanie rejestru BX do AX.");
                    Console_Write("MOV AX 10 -- Wklejenie wartości 10(decimal) do rejestru AX.");
                    Console_Write("MOV AX 15h -- Wklejenie wartości 0x0015(hexadecimal) do rejestru AX.");
                    Console_Write("MOV AL 15h -- Wklejenie wartości 0x15(hexadecimal) do rejestru AL.");
                    break;
                default:
                    Console_Write($"Brak komendy o nazwie {v}");
                    break;
            }
        }

        private void Console_Write(string o)
        {
            _Console.Text = $"{_Console.Text}{Environment.NewLine}{o}";
            _Console.ScrollToEnd();
        }

        private void SetRegisterContent(string type, string data)
        {
            switch (type)
            {
                case "AX":
                    data = CheckRegisterDataExquals4(data);
                    AXContent.Content = $"0x{data}";
                    char[] c = data.ToCharArray();
                    AHContent.Content = $"0x{c[0]}{c[1]}";
                    ALContent.Content = $"0x{c[2]}{c[3]}";
                    break;
                case "BX":
                    data = CheckRegisterDataExquals4(data);
                    BXContent.Content = $"0x{data}";
                    char[] c1 = data.ToCharArray();
                    BHContent.Content = $"0x{c1[0]}{c1[1]}";
                    BLContent.Content = $"0x{c1[2]}{c1[3]}";
                    break;
                case "CX":
                    data = CheckRegisterDataExquals4(data);
                    CXContent.Content = $"0x{data}";
                    char[] c2 = data.ToCharArray();
                    CHContent.Content = $"0x{c2[0]}{c2[1]}";
                    CLContent.Content = $"0x{c2[2]}{c2[3]}";
                    break;
                case "DX":
                    data = CheckRegisterDataExquals4(data);
                    DXContent.Content = $"0x{data}";
                    char[] c3 = data.ToCharArray();
                    DHContent.Content = $"0x{c3[0]}{c3[1]}";
                    DLContent.Content = $"0x{c3[2]}{c3[3]}";
                    break;
                case "AH":
                    SetMainRegister("AX", CheckRegisterDataExquals2(data), "H");
                    break;
                case "AL":
                    SetMainRegister("AX", CheckRegisterDataExquals2(data), "L");
                    break;
                case "BH":
                    SetMainRegister("BX", CheckRegisterDataExquals2(data), "H");
                    break;
                case "BL":
                    SetMainRegister("BX", CheckRegisterDataExquals2(data), "L");
                    break;
                case "CH":
                    SetMainRegister("CX", CheckRegisterDataExquals2(data), "H");
                    break;
                case "CL":
                    SetMainRegister("CX", CheckRegisterDataExquals2(data), "L");
                    break;
                case "DH":
                    SetMainRegister("DX", CheckRegisterDataExquals2(data), "H");
                    break;
                case "DL":
                    SetMainRegister("DX", CheckRegisterDataExquals2(data), "L");
                    break;
                default:
                    break;
            }
        }

        private void SetMainRegister(string type, string data, string pos)
        {
            switch (type)
            {
                case "AX":
                    char[] ax = AXContent.Content.ToString().ToCharArray();
                    if (pos == "L")
                    {
                        ALContent.Content = $"0x{data}";
                        data = $"0x{ax[2]}{ax[3]}{data}";
                        AXContent.Content = data;
                    }
                    else if (pos == "H")
                    {
                        AHContent.Content = $"0x{data}";
                        data = $"0x{data}{ax[4]}{ax[5]}";
                        AXContent.Content = data;
                    }
                    break;
                case "BX":
                    char[] bx = BXContent.Content.ToString().ToCharArray();
                    if (pos == "L")
                    {
                        BLContent.Content = $"0x{data}";
                        data = $"0x{bx[2]}{bx[3]}{data}";
                        BXContent.Content = data;
                    }
                    else if (pos == "H")
                    {
                        BHContent.Content = $"0x{data}";
                        data = $"0x{data}{bx[4]}{bx[5]}";
                        BXContent.Content = data;
                    }
                    break;
                case "CX":
                    char[] cx = CXContent.Content.ToString().ToCharArray();
                    if (pos == "L")
                    {
                        CLContent.Content = $"0x{data}";
                        data = $"0x{cx[2]}{cx[3]}{data}";
                        CXContent.Content = data;
                    }
                    else if (pos == "H")
                    {
                        CHContent.Content = $"0x{data}";
                        data = $"0x{data}{cx[4]}{cx[5]}";
                        CXContent.Content = data;
                    }
                    break;
                case "DX":
                    char[] dx = DXContent.Content.ToString().ToCharArray();
                    if (pos == "L")
                    {
                        DLContent.Content = $"0x{data}";
                        data = $"0x{dx[2]}{dx[3]}{data}";
                        DXContent.Content = data;
                    }
                    else if (pos == "H")
                    {
                        DHContent.Content = $"0x{data}";
                        data = $"0x{data}{dx[4]}{dx[5]}";
                        DXContent.Content = data;
                    }
                    break;
                default:
                    break;
            }
        }

        private string CheckRegisterDataExquals4(string data)
        {
            if (data.Length == 4)
            {
                return data;
            }
            else if (data.Length < 4)
            {
                for (int i = data.Length; i < 4; i++)
                {
                    data = "0" + data;
                }
                return data;
            }
            else if (data.Length > 4)
            {
                Console_Write("Przekroczono limit rejestru!");
                return "0000";
            }
            else
            {
                return "0000";
            }
        }
        private string CheckRegisterDataExquals2(string data)
        {
            if (data.Length == 2)
            {
                return data;
            }
            else if (data.Length < 2)
            {
                for (int i = data.Length; i < 2; i++)
                {
                    data = "0" + data;
                }
                return data;
            }
            else if (data.Length > 2)
            {
                Console_Write("Przekroczono limit rejestru!");
                return "00";
            }
            else
            {
                return "00";
            }
        }
    }
}
