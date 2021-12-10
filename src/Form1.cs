using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace _Max_ParserGUI
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFileDialogPathToSrumdb = new OpenFileDialog();
        string pathToMax;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        static float GetMBfromBytes(long bytes)
        {
            return (float)bytes / 1024 / 1024;
        }

        static float GetGBfromBytes(long bytes)
        {
            return (float)bytes / 1024 / 1024 / 1024;
        }

        private void button2_Click(object sender, EventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                string message = "It is the fully free software created by Krzysztof Gajewski.\r\n\r\nIcons made by Pixel perfect from www.flaticon.com";
                string title = "$Max - Parser";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogPathToSrumdb.InitialDirectory = @"C:\";
            openFileDialogPathToSrumdb.Filter = "All files (*.*)|*.*";

            if (openFileDialogPathToSrumdb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxPathToMaxFile.Text = openFileDialogPathToSrumdb.FileName;
                pathToMax = textBoxPathToMaxFile.Text;
            }
        }

        private void parseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(textBoxPathToMaxFile.Text) && !String.IsNullOrEmpty(textBoxPathToMaxFile.Text))
                {
                    if (File.Exists(textBoxPathToMaxFile.Text))
                    {

                        byte[] ByteContentOfMax = System.IO.File.ReadAllBytes(pathToMax);
                        string ContentOfMax = BitConverter.ToString(ByteContentOfMax);

                        if (ByteContentOfMax.Length != 32)
                        {
                            MessageBox.Show("The size of the provided file does not match the size of the $Max file, please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        else
                        {
                            byte[] ByteUsnJrnlSize = new byte[8];
                            for (int i = 0; i < 8; i++)
                            {
                                ByteUsnJrnlSize[i] = ByteContentOfMax[i];
                            }

                            Array.Reverse(ByteUsnJrnlSize);
                            string StringReverseUsnJrnlSize = BitConverter.ToString(ByteUsnJrnlSize);
                            long decValue = long.Parse(StringReverseUsnJrnlSize.Replace("-", ""), System.Globalization.NumberStyles.HexNumber);

                            textBoxContent.Text = ContentOfMax.Replace('-', ' ');
                            textBoxMaxSizeMB.Text = GetMBfromBytes(decValue) + " MB";
                            textBoxMaxSizeGB.Text = GetGBfromBytes(decValue) + " GB";
                        }
                    }
                    else
                    {
                        MessageBox.Show("File does not exist!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please provide the path to the $Max file!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(textBoxInput.Text) && !String.IsNullOrEmpty(textBoxInput.Text))
                {
                    string input = "";

                    if (textBoxInput.Text.Contains("-"))
                    {
                        input = textBoxInput.Text.Replace("-", "");
                    }

                    else if (textBoxInput.Text.Contains(" "))
                    {
                        input = textBoxInput.Text.Replace(" ", "");
                    }

                    else
                    {
                        input = textBoxInput.Text;
                    }

                    byte[] ByteInput = StringToByteArray(input);

                    Console.WriteLine(ByteInput.Length);
                    if (ByteInput.Length != 32)
                    {

                        MessageBox.Show("The size of the provided bytes does not match the size of the $Max file, please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    byte[] ByteUsnJrnlSize = new byte[8];
                    for (int i = 0; i < 8; i++)
                    {
                        ByteUsnJrnlSize[i] = ByteInput[i];
                    }

                    Array.Reverse(ByteUsnJrnlSize);
                    string StringReverseUsnJrnlSize = BitConverter.ToString(ByteUsnJrnlSize);
                    long decValue = long.Parse(StringReverseUsnJrnlSize.Replace("-", ""), System.Globalization.NumberStyles.HexNumber);

                    textBoxMaxSizeMB.Text = GetMBfromBytes(decValue) + " MB";
                    textBoxMaxSizeGB.Text = GetGBfromBytes(decValue) + " GB";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
