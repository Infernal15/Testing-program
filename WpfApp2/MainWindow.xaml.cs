using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, Dictionary<string, Dictionary<string, bool>>> test_lib;
        int current_position = 0;
        string main_key = "";
        Grid[] grids;
        List<string> questions;
        int max_mark = 0;
        int current_mark = 0;
        public MainWindow()
        {
            InitializeComponent();
            this.Width = 298;
            this.Height = 386;
            prev.Visibility = Visibility.Hidden;
            next.Visibility = Visibility.Hidden;
            end.Visibility = Visibility.Hidden;
            quest_label.Visibility = Visibility.Hidden;
            questions = new List<string>();
            test_lib = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>();
            test_lib["HTML"] = new Dictionary<string, Dictionary<string, bool>>();
            test_lib["HTML"]["Тег br використовується для:"] = new Dictionary<string, bool>();
            test_lib["HTML"]["Тег br використовується для:"]["Жирногого шрифту"] = false;
            test_lib["HTML"]["Тег br використовується для:"]["Обгорнення жирного шрифту"] = false;
            test_lib["HTML"]["Тег br використовується для:"]["Розриву сторінки"] = true;
            test_lib["HTML"]["Тег br використовується для:"]["Створення горизонтальної лінії"] = false;
            test_lib["HTML"]["Тег hr використовується для:"] = new Dictionary<string, bool>();
            test_lib["HTML"]["Тег hr використовується для:"]["Жирногого шрифту"] = false;
            test_lib["HTML"]["Тег hr використовується для:"]["Обгорнення жирного шрифту"] = false;
            test_lib["HTML"]["Тег hr використовується для:"]["Розриву сторінки"] = true;
            test_lib["HTML"]["Тег hr використовується для:"]["Створення горизонтальної лінії"] = true;
            foreach (string tmp in test_lib.Keys)
                test_list.Items.Add(tmp);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (test_list.SelectedIndex != -1)
            {
                this.Width = 773;
                this.Height = 416;
                label.Visibility = Visibility.Hidden;
                test_list.Visibility = Visibility.Hidden;
                start.Visibility = Visibility.Hidden;
                exit.Visibility = Visibility.Hidden;

                main_key = test_list.SelectedItem.ToString();
                int size = test_lib[main_key].Count;
                quest_label.Visibility = Visibility.Visible;
                prev.Visibility = Visibility.Visible;
                prev.IsEnabled = false;
                if (size > 1)
                    next.Visibility = Visibility.Visible;
                else
                    end.Visibility = Visibility.Visible;
                grids = new Grid[size];
                int i = 0;
                foreach (string tmp in test_lib[main_key].Keys)
                {
                    grids[i] = new Grid();
                    grids[i].Name = $"Grid{i}";
                    main_grid.Children.Add(grids[i]);
                    grids[i].Margin = new Thickness(165, 99, 0, 0);
                    grids[i].ShowGridLines = true;

                    questions.Add(tmp);
                    int ttp = 0;
                    List<string> quest = new List<string>();
                    foreach (string tps in test_lib[main_key][tmp].Keys)
                    {
                        quest.Add(tps);
                        if (test_lib[main_key][tmp][tps] == true)
                        {
                            ttp++;
                            max_mark++;
                        }
                    }

                    for (int j = 0; j < test_lib[main_key][tmp].Count; j++)
                    {
                        RowDefinition row = new RowDefinition();
                        row.Height = new GridLength(25);
                        grids[i].RowDefinitions.Add(row);
                    }

                    List<int> uses = new List<int>();
                    Random rnd = new Random();

                    if (ttp == 1)
                    {
                        int j = 0;
                        while (true)
                        {
                            if (uses.Count == test_lib[main_key][tmp].Count)
                                break;
                            int rar = rnd.Next(0, test_lib[main_key][tmp].Count);
                            if (!uses.Contains(rar))
                            {
                                RadioButton radio = new RadioButton();
                                radio.Content = quest[rar];
                                grids[i].Children.Add(radio);
                                Grid.SetRow(radio, j);
                                uses.Add(rar);
                                j++;
                            }
                        }
                    }
                    else if (ttp > 1)
                    {
                        int j = 0;
                        while (true)
                        {
                            if (uses.Count == test_lib[main_key][tmp].Count)
                                break;
                            int rar = rnd.Next(0, test_lib[main_key][tmp].Count);
                            if (!uses.Contains(rar))
                            {
                                CheckBox box = new CheckBox();
                                box.Content = quest[rar];
                                grids[i].Children.Add(box);
                                Grid.SetRow(box, j);
                                uses.Add(rar);
                                j++;
                            }
                        }
                    }
                    if (i == 0)
                        grids[i].Visibility = Visibility.Visible;
                    else
                        grids[i].Visibility = Visibility.Hidden;

                    i++;
                }
            }
            quest_label.Content = questions[0];
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void prev_Click(object sender, RoutedEventArgs e)
        {
            grids[current_position].Visibility = Visibility.Hidden;
            current_position--;
            grids[current_position].Visibility = Visibility.Visible;
            quest_label.Content = questions[current_position];
            if (current_position == 0)
                prev.IsEnabled = false;
            if (current_position < test_lib[main_key].Count)
            {
                next.Visibility = Visibility.Visible;
                end.Visibility = Visibility.Hidden;
            }
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            grids[current_position].Visibility = Visibility.Hidden;
            current_position++;
            grids[current_position].Visibility = Visibility.Visible;
            quest_label.Content = questions[current_position];
            if (current_position > 0)
                prev.IsEnabled = true;
            if (current_position == test_lib[main_key].Count - 1)
            {
                next.Visibility = Visibility.Hidden;
                end.Visibility = Visibility.Visible;
            }
        }

        private void end_Click(object sender, RoutedEventArgs e)
        {
            int current_mark = 0;
            for (int i = 0; i < test_lib[main_key].Count; i++)
            {
                RadioButton[] radios = grids[i].Children.OfType<RadioButton>().ToArray();
                if (radios.Length != 0)
                {
                    for (int j = 0; j < radios.Length; j++)
                        if (test_lib[main_key][questions[i]][radios[j].Content.ToString()] == true && radios[j].IsChecked == true)
                            current_mark++;
                }
                else
                {
                    CheckBox[] box = grids[i].Children.OfType<CheckBox>().ToArray();
                    for (int j = 0; j < box.Length; j++)
                        if (test_lib[main_key][questions[i]][box[j].Content.ToString()] == true && box[j].IsChecked == true)
                            current_mark++;
                }
            }
            MessageBox.Show($"Your score {current_mark}/{max_mark}");
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            MessageBox.Show("!!!!");
        }
    }
}
