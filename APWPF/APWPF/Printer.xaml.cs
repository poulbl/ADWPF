using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Printing;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Windows.Media;
using System;

namespace ADWPF
{
    /// <summary>
    /// Interaction logic for Printer.xaml
    /// </summary>
    public partial class Printer : Window
    {
        private FixedDocumentSequence _document;
        private string fileName = "print_previw.xps";
        public Printer(string senderr)
        {
            InitializeComponent();

            XpsDocument hello;
            if (File.Exists(fileName))
            {
                File.Delete(fileName); //TODO hvis man canceller print og printer en ny så er filen i brug.
            }
            hello = new XpsDocument(fileName, System.IO.FileAccess.ReadWrite);
            XpsDocumentWriter xpsdw = XpsDocument.CreateXpsDocumentWriter(hello);

            Paragraph flowParagraph = new Paragraph();
            flowParagraph.Inlines.Add(senderr);
            FlowDocument flowDoc = new FlowDocument(flowParagraph);
            IDocumentPaginatorSource idpSource = flowDoc;
            DocumentPaginator docPaginator = idpSource.DocumentPaginator;
            xpsdw.Write(docPaginator);
            _document = hello.GetFixedDocumentSequence();
            hello.Close();
            PreviewD.Document = _document;
        }

        private void PrintSimpleTextButton_Click(object sender, RoutedEventArgs e)
        {
            Visual v = sender as Visual;
            FrameworkElement fe = v as FrameworkElement;
            if (fe == null)
                return;

            // Create a PrintDialog  
            PrintDialog printDlg = new PrintDialog();

            // TEST 
            if (printDlg.ShowDialog() == true)
            {
                //store original scale
                Transform originalScale = fe.LayoutTransform;
                //get selected printer capabilities
                PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);

                //get scale of the print wrt to screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / fe.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
                               fe.ActualHeight);

                //Transform the Visual to scale
                fe.LayoutTransform = new ScaleTransform(scale, scale);

                //get the size of the printer page
                Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                //update the layout of the visual to the printer page size.
                fe.Measure(sz);
                fe.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

                //now print the visual to printer to fit on the one page.
                printDlg.PrintVisual(v, "My Print");

                //apply the original transform.
                fe.LayoutTransform = originalScale;
            }
            // TEST

            printDlg.PrintQueue = LocalPrintServer.GetDefaultPrintQueue();
            printDlg.PrintTicket = printDlg.PrintQueue.DefaultPrintTicket;

            // Create a FlowDocument dynamically.  
            //printDlg.PrintTicket.PageOrientation = PageOrientation.Portrait;
            //printDlg.PrintTicket.PageScalingFactor = 90;
            //printDlg.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);
            //printDlg.PrintTicket.PageBorderless = PageBorderless.Borderless;

            // Call PrintDocument method to send document to printer  
            if (printDlg.ShowDialog() == true)
            {
                _document.PrintTicket = printDlg.PrintTicket;
                XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printDlg.PrintQueue);
                writer.WriteAsync(_document, printDlg.PrintTicket);
            }
        }
    }
}
