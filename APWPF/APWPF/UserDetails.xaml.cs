using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace ADWPF
{
    /// <summary>
    /// Interaction logic for UserDetails.xaml
    /// </summary>
    public partial class UserDetails : Window
    {
        List<string> userdata;
        public UserDetails(List<string> userdata)
        {
            InitializeComponent();
            this.userdata = userdata;
        }

        public void ReadUserData()
        {

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            ShowPrintPreview(contentForPrinting);
        }

        private void ShowPrintPreview(FrameworkElement wpfElement)
        {
            if (File.Exists("print_preview.xps") == true)
                File.Delete("print_preview.xps");

            //--- Create xps document ---
            XpsDocument doc = new XpsDocument("print_preview.xps", FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            SerializerWriterCollator outputDocument = writer.CreateVisualsCollator();
            outputDocument.BeginBatchWrite();
            outputDocument.Write(wpfElement);
            outputDocument.EndBatchWrite();

            FixedDocumentSequence preview = doc.GetFixedDocumentSequence();
            doc.Close();

            Window window = new Window();
            window.Content = new DocumentViewer { Document = preview };
            window.ShowDialog();

            writer = null;
            outputDocument = null;
            doc = null;
        }
    }
}
