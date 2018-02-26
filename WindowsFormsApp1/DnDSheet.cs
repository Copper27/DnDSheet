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

namespace DnDSheet
{
    public partial class DnDSheet : Form
    {
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
                        //var reader1 = new PdfReader(@"C:\Users\Tau\Desktop\Pathfinder_Charactersheet_Joe.pdf");
                        AcroFields fields = reader.AcroFields;
                        foreach (var item in reader.AcroFields.Fields)
                        {
                            string val1 = fields.GetField(item.Key.ToString());
                            build.Append(val1 + Environment.NewLine);
                        }
                        richTextBox1.Text = build.ToString();
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
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

