using BarcodeLib.BarcodeReader;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BarcodeReader = BarcodeLib.BarcodeReader.BarcodeReader;
using BarcodeReaderInZXing = ZXing.BarcodeReader;

namespace WinformDragDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.DragEnter += textbox1_DragEnter;
            textBox1.DragDrop += textbox1_DragDrop;
        }

        private void textbox1_DragEnter(object sender, DragEventArgs e)
        {
            IDataObject dataObject = e.Data;
            if (dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void textbox1_DragDrop(object sender, DragEventArgs e)
        {
            IDataObject dataObject = e.Data;
            string barcodeString = string.Empty;
            if (dataObject == null)
            {
                return;
            }
            if (dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])dataObject.GetData(DataFormats.FileDrop);
                OptimizeSetting optimizeSetting = new OptimizeSetting();
                optimizeSetting.setMaxOneBarcodePerPage(true);
                foreach (var file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    string filePath = fileInfo.FullName;
                    barcodeString = GetBarCodes(filePath);
                    if (barcodeString.Length == 0)
                    {
                        textBox1.Clear();
                    }
                }
                textBox1.Text = barcodeString;
            }
        }

        private string GetBarCodes(string filePath, OptimizeSetting optimizeSetting)
        {
            string[] barCodes = BarcodeReader.read(filePath, BarcodeReader.CODE39);
            BarcodeReader.read(filePath, BarcodeReader.CODE39, optimizeSetting);
            return barCodes.Length == 0 ? string.Empty : barCodes[0];
        }

        private string GetBarCodes(string filePath)
        {
            BarcodeReaderInZXing reader = new BarcodeReaderInZXing();

            reader.Options.CharacterSet = "UTF-8";
            Bitmap bitmap = new Bitmap(filePath);
            ZXing.Result result = reader.Decode(bitmap);
            return result == null ? " " : result.Text;
        }
    }
}