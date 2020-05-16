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
                    Console_Write("XCHG <reg> <reg> -- Przemienna zamiana wartości między rejestrami");
                    Console_Write("NOT <reg> -- W postaci binarnej zamiana 1 <-> 0");
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
                    if (data[1] == "AX" || data[1] == "BX" || data[1] == "CX" || data[1] == "DX" || data[1] == "AL" || data[1] == "AH" || data[1] == "BL" || data[1] == "BH" || data[1] == "CL" || data[1] == "CH" || data[1] == "DL" || data[1] == "DH")
                    {
                        if (data.Length == 3)
                        {
                            Console_Write($"Wykonuję MOV {data[2]} -> {data[1]} ...");
                            if (data[2] == "AX" || data[2] == "BX" || data[2] == "CX" || data[2] == "DX")
                            {
                                MOV(data[1], GetDataFromMainReg(data[2]));
                            }
                            else if (data[2] == "AL" || data[2] == "AH" || data[2] == "BL" || data[2] == "BH" || data[2] == "CL" || data[2] == "CH" || data[2] == "DL" || data[2] == "DH")
                            {
                                MOV(data[1], GetDataFromReg(data[2]));
                            }
                            else if (data[2].Substring(data[2].Length - 1) == "h")
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
                    }
                    else
                    {
                        Console_Write("Niepoprawne użycie komendy.");
                    }
                    break;
                case "XCHG":
                    if (data.Length == 3)
                    {
                        Console_Write($"Wykonuję XCHG {data[1]} - {data[2]} ...");
                        if (data[1] == "AX" || data[1] == "BX" || data[1] == "CX" || data[1] == "DX")
                        {
                            if (data[2] == "AX" || data[2] == "BX" || data[2] == "CX" || data[2] == "DX")
                            {
                                XCHG(data[1], data[2], 0);
                            }
                            else
                            {
                                Console_Write("Niepoprawne użycie komendy.");
                            }
                        }
                        else if (data[1] == "AL" || data[1] == "AH" || data[1] == "BL" || data[1] == "BH" || data[1] == "CL" || data[1] == "CH" || data[1] == "DL" || data[1] == "DH")
                        {
                            if (data[2] == "AL" || data[2] == "AH" || data[2] == "BL" || data[2] == "BH" || data[2] == "CL" || data[2] == "CH" || data[2] == "DL" || data[2] == "DH")
                            {
                                XCHG(data[1], data[2], 1);
                            }
                            else
                            {
                                Console_Write("Niepoprawne użycie komendy.");
                            }
                        }
                        else
                        {
                            Console_Write("Niepoprawne użycie komendy.");
                        }
                    }
                    else
                    {
                        Console_Write($"Niepoprawna ilość danych.");
                    }
                    break;
                case "NOT":
                    if (data.Length == 2)
                    {
                        Console_Write($"Wykonuję NOT dla {data[1]} ...");
                        if (data[1] == "AX" || data[1] == "BX" || data[1] == "CX" || data[1] == "DX")
                        {
                            NOT(data[1], 0);
                        }
                        else if (data[1] == "AL" || data[1] == "AH" || data[1] == "BL" || data[1] == "BH" || data[1] == "CL" || data[1] == "CH" || data[1] == "DL" || data[1] == "DH")
                        {
                            NOT(data[1], 1);
                        }
                        else
                        {
                            Console_Write("Niepoprawne użycie komendy.");
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

        private void NOT(string v1, int v2)
        {
            if (v2 == 0)
            {
                string v1d = BinToHex(Invert01(HexToBinary(GetDataFromMainReg(v1), 0)), 0);
                SetRegisterContent(v1, v1d);
            }
            else if (v2 == 1)
            {
                string v1d = BinToHex(Invert01(HexToBinary(GetDataFromReg(v1), 1)), 1);
                SetRegisterContent(v1, v1d);
            }
            else
            {
                Console_Write("Error");
            }
        }

        private string BinToHex(string v, int t)
        {
            if (t == 0)
            {
                return Convert.ToInt32(v, 2).ToString("X");
            }
            else
            {
                return Convert.ToInt32(v, 2).ToString("X");
            }
        }

        private string Invert01(string v)
        {
            char[] c = v.ToCharArray();
            string result = "";
            for (int i = 0; i < c.Length; i++)
            {
                if(c[i] == '0')
                {
                    result = result + "1";
                } 
                else if(c[i] == '1')
                {
                    result = result + "0";
                }
                else
                {
                    Console_Write("Error");
                }
            }
            return result;
        }

        private string HexToBinary(string v, int t)
        {
            string result;
            if (t == 1) {
                result = Convert.ToString(Convert.ToInt32(v, 16), 2).PadLeft(8, '0');
            }
            else
            {
                result = Convert.ToString(Convert.ToInt32(v, 16), 2).PadLeft(16, '0');
            }
            return result;
        }

        private void XCHG(string v1, string v2, int t)
        {
            if (t == 0)
            {
                string v1d = GetDataFromMainReg(v1);
                string v2d = GetDataFromMainReg(v2);
                SetRegisterContent(v1, v2d);
                SetRegisterContent(v2, v1d);
            }
            else if (t == 1)
            {
                string v1d = GetDataFromReg(v1);
                string v2d = GetDataFromReg(v2);
                SetRegisterContent(v1, v2d);
                SetRegisterContent(v2, v1d);
            }
            else
            {
                Console_Write("Error");
            }
        }

        private string GetDataFromReg(string v)
        {
            switch (v)
            {
                case "AL":
                    char[] al = ALContent.Content.ToString().ToCharArray();
                    return $"{al[2]}{al[3]}";
                    break;
                case "AH":
                    char[] ah = AHContent.Content.ToString().ToCharArray();
                    return $"{ah[2]}{ah[3]}";
                    break;
                case "BL":
                    char[] bl = BLContent.Content.ToString().ToCharArray();
                    return $"{bl[2]}{bl[3]}";
                    break;
                case "BH":
                    char[] bh = BHContent.Content.ToString().ToCharArray();
                    return $"{bh[2]}{bh[3]}";
                    break;
                case "CL":
                    char[] cl = CLContent.Content.ToString().ToCharArray();
                    return $"{cl[2]}{cl[3]}";
                    break;
                case "CH":
                    char[] ch = CHContent.Content.ToString().ToCharArray();
                    return $"{ch[2]}{ch[3]}";
                    break;
                case "DL":
                    char[] dl = DLContent.Content.ToString().ToCharArray();
                    return $"{dl[2]}{dl[3]}";
                    break;
                case "DH":
                    char[] dh = DHContent.Content.ToString().ToCharArray();
                    return $"{dh[2]}{dh[3]}";
                    break;

                default:
                    return "0000";
                    break;
            }
        }

        private string GetDataFromMainReg(string v)
        {
            switch (v)
            {
                case "AX":
                    char[] ax = AXContent.Content.ToString().ToCharArray();
                    return $"{ax[2]}{ax[3]}{ax[4]}{ax[5]}";
                    break;
                case "BX":
                    char[] bx = BXContent.Content.ToString().ToCharArray();
                    return $"{bx[2]}{bx[3]}{bx[4]}{bx[5]}";
                    break;
                case "CX":
                    char[] cx = CXContent.Content.ToString().ToCharArray();
                    return $"{cx[2]}{cx[3]}{cx[4]}{cx[5]}";
                    break;
                case "DX":
                    char[] dx = DXContent.Content.ToString().ToCharArray();
                    return $"{dx[2]}{dx[3]}{dx[4]}{dx[5]}";
                    break;
                default:
                    return "0000";
                    break;
            }
        }

        private string CalculateToHex(string v)
        {
            try
            {
                int t = int.Parse(v);
                return t.ToString("X");
            } catch (Exception e)
            {
                Console_Write(e.Message + " Zresetowano rejestr.");
                return "0000";
            }
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
                case "XCHG":
                    Console_Write("XCHG AX BX -- Przemienna zamiana wartości między rejestrami AX BX");
                    Console_Write("XCHG AL BH -- Przemienna zamiana wartości między rejestrami AL BH");
                    break;
                case "NOT":
                    Console_Write("NOT AX -- Jeśli rejestr posiadał 0xFFFF zmieni na 0x0000");
                    Console_Write("NOT AL -- Jeśli rejestr posiadał 0x0B zmieni na 0xF4");
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
                Console_Write("Przekroczono limit dla tego rejestru!");
                return "00";
            }
            else
            {
                return "00";
            }
        }


    }
}
