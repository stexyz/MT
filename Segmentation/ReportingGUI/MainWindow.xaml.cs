using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using ReportingLib;

namespace ReportingGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ReportTypeComboBox.ItemsSource = Enum.GetValues(typeof (ReportingLib.ReportGenerator.ReportType));
            ReportTypeComboBox.SelectedIndex = 0;
            ReportTextBox.Visibility = Visibility.Hidden;
            MyChart.Visibility = Visibility.Hidden;
            MyChart3.Visibility = Visibility.Hidden;
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FromDate.SelectedDate.HasValue && ToDate.SelectedDate.HasValue)
            {
                ReportGenerator.ReportType reportType = (ReportGenerator.ReportType) ReportTypeComboBox.SelectedItem;
                DateTime from = FromDate.SelectedDate.Value;
                DateTime to = ToDate.SelectedDate.Value;

                if (reportType == ReportGenerator.ReportType.MonthlySummary)
                {
                    IEnumerable<SummaryReport> monthlyReports = ReportGenerator.GenerateMonthlySummaryReports(from, to);
                    ReportTextBox.Clear();
                    ((LineSeries)MyChart.Series[0]).ItemsSource = monthlyReports.Select(mr => new KeyValuePair<DateTime, decimal>(mr.From, mr.TotalPrice));
                    ((LineSeries)MyChart.Series[1]).ItemsSource = monthlyReports.Select(mr => new KeyValuePair<DateTime, decimal>(mr.From, mr.TotalMargin));
                    ((LineSeries)MyChart.Series[2]).ItemsSource = monthlyReports.Select(mr => new KeyValuePair<DateTime, decimal>(mr.From, mr.RelativeMargin));
                    //((LineSeries)MyChart.Series[3]).ItemsSource = monthlyReports.Select(mr => new KeyValuePair<DateTime, decimal>(mr.From, mr.RelativeMargin * 1.5M));


                    //((LineSeries)MyChart2.Series[0]).ItemsSource = monthlyReports.Select(mr => new KeyValuePair<DateTime, decimal>(mr.From, mr.ShippingCount));
                    //((LineSeries)MyChart2.Series[1]).ItemsSource = monthlyReports.Select(mr => new KeyValuePair<DateTime, decimal>(mr.From, mr.PaymentCount));
                    ((LineSeries)MyChart3.Series[0]).ItemsSource = monthlyReports.Select(mr => new KeyValuePair<DateTime, decimal>(mr.From, mr.TotalShippingPrice));
                    ((LineSeries)MyChart3.Series[1]).ItemsSource = monthlyReports.Select(mr => new KeyValuePair<DateTime, decimal>(mr.From, mr.TotalPaymentPrice));


                    ReportTextBox.Visibility = Visibility.Visible;
                    MyChart.Visibility = Visibility.Visible;
                    MyChart3.Visibility = Visibility.Visible;
                }
                else
                {
                    ReportTextBox.Clear();
                    string report = ReportGenerator.GenerateReport(from, to, reportType);
                    ReportTextBox.AppendText(report);
                    MyChart.Visibility = Visibility.Hidden;
                    MyChart3.Visibility = Visibility.Hidden;
                    ReportTextBox.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
