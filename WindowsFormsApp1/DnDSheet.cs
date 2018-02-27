using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace DnDSheet
{
    public partial class DnDSheet : Form
    {
        private Character newChar = new Character();

        public DnDSheet()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "PDF files|*.pdf", ValidateNames = true, Multiselect = false })
            {
                if(ofd.ShowDialog() == DialogResult.OK)
                    try
                    {
                        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(ofd.FileName);
                        StringBuilder sb = new StringBuilder();
                        for(int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            sb.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                        }
                        //richTextBox1.Text = sb.ToString();
                        //AcroFields fields = reader.AcroFields;
                        //string val = fields.GetField("Character_Name");
                        //richTextBox1.Text = val;
                        //reader.Close();

                        StringBuilder build = new StringBuilder();
                        AcroFields fields = reader.AcroFields;
                        foreach (var item in reader.AcroFields.Fields)
                        {
                            string val1 = fields.GetField(item.Key.ToString());
                            build.Append(val1 + Environment.NewLine);
                        }
                        reader.Close();


                        //string pdfTemplate = @"C:\Users\Tau\Desktop\Pathfinder_Charactersheet_Joe.pdf";
                        // title the form
                        //this.Text += " - " + pdfTemplate;
                        // create a new PDF reader based on the PDF template document

                        //PdfReader pdfReader = new PdfReader(pdfTemplate);
                        // create and populate a string builder with each of the

                        // field names available in the subject PDF
                        //foreach (KeyValuePair<string, iTextSharp.text.pdf.AcroFields.Item> de in pdfReader.AcroFields.Fields)

                        //{
                        //    sb.Append(de.Key.ToString() + Environment.NewLine);
                        //}
                        // Write the string builder's content to the form's textbox which includes form data names

                        //richTextBox2.Text = sb.ToString();
                        //richTextBox2.SelectionStart = 0;

                        label2.Text = fields.GetField("Character_Player");
                        characterName.Text = fields.GetField("Character_Name");
                        characterRace.Text = fields.GetField("Character_Race");
                        characterSize.Text = fields.GetField("Character_Size");
                        //Character newChar = new Character();
                        newChar.charName = fields.GetField("Character_Name");
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }

        private void btnSaveChar_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you want to save this character?", "Save Character", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BinarySerialization.WriteToBinaryFile<Character>(@"C:\Users\Tau\Desktop\CompSci\Visual Studio Projects\character.bin", newChar);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Character newChar = BinarySerialization.ReadFromBinaryFile<Character>(@"C:\Users\Tau\Desktop\CompSci\Visual Studio Projects\character.bin");
            characterName.Text = newChar.charName;
        }
    }
}



/// <summary>
/// Functions for performing common binary Serialization operations.
/// <para>All properties and variables will be serialized.</para>
/// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
/// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
/// </summary>
public static class BinarySerialization
{
    /// <summary>
    /// Writes the given object instance to a binary file.
    /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
    /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
    /// </summary>
    /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
    /// <param name="filePath">The file path to write the object instance to.</param>
    /// <param name="objectToWrite">The object instance to write to the XML file.</param>
    /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
    public static void WriteToBinaryFile<Character>(string filePath, Character objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }

    /// <summary>
    /// Reads an object instance from a binary file.
    /// </summary>
    /// <typeparam name="T">The type of object to read from the XML.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the binary file.</returns>
    public static Character ReadFromBinaryFile<Character>(string filePath)
    {
        using (Stream stream = File.Open(filePath, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (Character)binaryFormatter.Deserialize(stream);
        }
    }
}
