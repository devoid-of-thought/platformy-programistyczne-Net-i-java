using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Linq;
using Problem_Plecakowy;

namespace GUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var solveButton = this.FindControl<Button>("SolveButton");
        if (solveButton != null)
        {
            solveButton.Click += SolveButton_Click;
        }
    }

    private void SolveButton_Click(object? sender, RoutedEventArgs e)
    {
        var itemsCountTextBox = this.FindControl<TextBox>("ItemsCountText");
        var capacityTextBox = this.FindControl<TextBox>("CapacityText");
        var seedTextBox = this.FindControl<TextBox>("SeedText");
        var resultTextBox = this.FindControl<TextBox>("ResultSummaryTextBox");
        var itemsToPackTextBox = this.FindControl<TextBox>("ItemsToPackText");

        if (itemsCountTextBox != null && capacityTextBox != null && seedTextBox != null && resultTextBox != null && itemsToPackTextBox != null)
        {
            itemsCountTextBox.Background = Brushes.White;
            capacityTextBox.Background = Brushes.White;
            seedTextBox.Background = Brushes.White;

            bool isValid = true;

            if (!int.TryParse(itemsCountTextBox.Text, out int n) || n <= 0)
            {
                itemsCountTextBox.Background = Brushes.LightPink;
                isValid = false;
            }

            if (!int.TryParse(capacityTextBox.Text, out int capacity) || capacity <= 0)
            {
                capacityTextBox.Background = Brushes.LightPink;
                isValid = false;
            }

            if (!int.TryParse(seedTextBox.Text, out int seed))
            {
                seedTextBox.Background = Brushes.LightPink;
                isValid = false;
            }

            if (!isValid)
            {
                resultTextBox.Text = "Wprowadź poprawne wartości liczbowe w podświetlonych polach.";
                itemsToPackTextBox.Text = string.Empty;
                return;
            }

            try
            {       
                Problem problem = new Problem(n, seed);
                Result result = problem.Solve(capacity);

                resultTextBox.Text = $"Wynik:\n{result.ToString()}";

                itemsToPackTextBox.Text = $"Przedmioty do spakowania:\n{string.Join(Environment.NewLine, problem.ItemList.Select(i => $"No: {i.id}, Val: {i.value}, Weight: {i.weight}"))}";
            }
            catch (Exception ex)
            {
                resultTextBox.Text = $"Wystąpił błąd:\n{ex.Message}";
            }
        }
    }
}