using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Forms = System.Windows.Forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DownloadManager;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Dictionary<string, string> files;
        private List<Label> labelsNames;
        private List<Label> labelsSizes;
        private List<ProgressBar> progressBars;
        private List<TextBlock> textBlocks;
        private static Logger logger;

        public MainWindow()
        {
            InitializeComponent();
            files = new Dictionary<string, string>();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileWithLinksPath = GetFilePath();
                StringGetter stringGetter = new StringGetter();
                var strings = stringGetter.GetStringsFromFile(fileWithLinksPath);

                StartFileDownloader(strings, fileWithLinksPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }
        private static string GetFilePath()
        {
            Forms.OpenFileDialog fileDialog = new Forms.OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false
            };

            if (fileDialog.ShowDialog() != Forms.DialogResult.OK) throw new Exception("File was not selected");

            var filePath = fileDialog.FileName;
            return filePath;
        }

        private void StartFileDownloader(List<string> strings, string fileWithLinksPath)
        {
            IFileDownloader fileDownloader = new FileDownloader();
            fileDownloader.SetDegreeOfParallelism(1);
            fileDownloader.OnFileProgress += ManageProgress;
            fileDownloader.OnFailed += PrintError;
            fileDownloader.OnDownloaded += SetReady;

            CreateControls(strings);

            var pathToSave = fileWithLinksPath.Substring(0, fileWithLinksPath.LastIndexOf('\\'));
            logger = new Logger(pathToSave);

            int id = 0;
            foreach (var address in strings)
            {
                files.Add(id.ToString(), address);
                fileDownloader.AddFileToDownloadingQueue(id.ToString(), address, pathToSave + $@"\IMG_{id}.jpg");
                id++;
            }
        }

        private void CreateControls(List<string> strings)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
            labelsNames = new List<Label>(strings.Count);
            labelsSizes = new List<Label>(strings.Count);
            progressBars = new List<ProgressBar>(strings.Count);
            textBlocks = new List<TextBlock>(strings.Count);

            for (int i = 0; i < strings.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });

                Thickness margin;
                margin.Left = 0;
                margin.Top = 0;
                labelsNames.Add(new Label());
                labelsNames[i].Margin = margin;
                labelsNames[i].Content = strings[i].Substring(strings[i].LastIndexOf('/') + 1);
                labelsNames[i].HorizontalAlignment = 0;
                labelsNames[i].VerticalAlignment = 0;
                Grid.SetColumn(labelsNames[i], 0);
                Grid.SetRow(labelsNames[i], i + 1);
                grid.Children.Add(labelsNames[i]);

                Thickness marginSizes;
                marginSizes.Left = 0;
                marginSizes.Top = 0;
                labelsSizes.Add(new Label());
                labelsSizes[i].Margin = marginSizes;
                labelsSizes[i].Content = "";
                labelsSizes[i].HorizontalAlignment = HorizontalAlignment.Left;
                labelsSizes[i].VerticalAlignment = 0;
                Grid.SetColumn(labelsSizes[i], 1);
                Grid.SetRow(labelsSizes[i], i + 1);

                grid.Children.Add(labelsSizes[i]);

                Thickness marginProgressBars;
                marginProgressBars.Left = 0;
                marginProgressBars.Top = 0;

                progressBars.Add(new ProgressBar());
                progressBars[i].Margin = marginProgressBars;
                progressBars[i].HorizontalAlignment = 0;
                progressBars[i].VerticalAlignment = VerticalAlignment.Center;
                progressBars[i].Width = 100;
                progressBars[i].Height = 20;

                progressBars[i].Maximum = 100;
                progressBars[i].Minimum = 0;
                progressBars[i].SmallChange = 0.1;
                progressBars[i].LargeChange = 1;

                Grid.SetColumn(progressBars[i], 2);
                Grid.SetRow(progressBars[i], i + 1);
                grid.Children.Add(progressBars[i]);

                Thickness textBlocksMargin;
                textBlocksMargin.Left = 40;
                textBlocksMargin.Top = 5;

                textBlocks.Add(new TextBlock());
                textBlocks[i].Margin = textBlocksMargin;
                textBlocks[i].HorizontalAlignment = 0;
                textBlocks[i].VerticalAlignment = VerticalAlignment.Center;
                textBlocks[i].Width = 100;
                textBlocks[i].Height = 20;
                Grid.SetColumn(textBlocks[i], 2);
                Grid.SetRow(textBlocks[i], i + 1);

                grid.Children.Add(textBlocks[i]);
            }
        }

        private void ManageProgress(string Id, int fileSize, int downloaded)
        {
            var id = int.Parse(Id);
            int percent = fileSize != 0 ? (int)(downloaded * 1.0 / fileSize * 100) : 1;
            progressBars[id].Value = percent;
            textBlocks[id].Text = percent + "%";

            if (labelsSizes[id].Content == "") labelsSizes[int.Parse(Id)].Content = fileSize + " bytes";
        }

        private void PrintError(string downloaded, Exception ex)
        {
            var exceptionId = (ex.InnerException as FileDownloader.DownloadException)?.Id;
            logger.AddLog(ex.Message + ex.StackTrace);
        }


        // Не работает!
        private void SetReady(string Id)
        {
            var id = int.Parse(Id);

            progressBars[id].Value = 100;
            textBlocks[id].Text = 100 + "%";
        }
    }
}