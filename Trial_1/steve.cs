using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public NMGPDFGenerator createPDF;
        public NMGPatient newPatient;
        public IEnumerable<NMGPatient> patientData;
        public List<NMGPatientStatement> patientStatementList;
        public List<NMGPatient> patientList;

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            List<string> NMedList;
            //Open dialog and choose a raw file.
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.Title = "Select a text file";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fname in dialog.FileNames)// loop through each selected file 
                {
                    string[] lines = System.IO.File.ReadAllLines(fname);
                    if (lines[0] == "ecwPtStatement,1.0")//Current Format for Northern Medical Group.
                    {
                        NMedList = System.IO.File.ReadAllLines(fname).ToList();
                        DateTime today = DateTime.Today;
                        /*string date = today.ToString("dd-MM-yyyy");
                        string[] dateSplit = date.Split('-');
                        string location = dateSplit[2] + @"\" + dateSplit[1] + @"\" + date;
                        string newFolder = @"C:\Invoice\Clients\Northern Medical Group\" + location;
                        string PDFFolder = @"C:\Invoice\PDF Files";

                        Directory.CreateDirectory(newFolder);
                        string comText = newFolder + @"\NMED01_" + dateSplit[2] + "_" + dateSplit[1] + "_" + date + ".txt";
                        string PDFFile = newFolder + @"\NMED01_" + dateSplit[2] + "_" + dateSplit[1] + "_" + date + ".pdf";
                        string PDFFile2 = PDFFolder + @"\NMED01_" + dateSplit[2] + "_" + dateSplit[1] + "_" + date + ".pdf";*/
                        string resources = @"C:\Invoice\PDF Tools";
                        string fileName = @"C:\Invoice\PDF Files\";
                        createPDF = new NMGPDFGenerator(resources);
                        patientList = new List<NMGPatient>(); // Stores different Patient
                        patientStatementList = new List<NMGPatientStatement>(); //Stores each line of Patient Statement

                        for (int i = 1; i < lines.Length; i++)
                        {// read in each line
                            if (lines[i] == "ecwPtStatement") // indicator for client format  
                            {
                                var j = i + 1;
                                if (j < lines.Length) //Spliting Patient info and adding it to newPatient
                                {
                                    string[] infoPatient = lines[j].Split(',');
                                    List<string> infoPatientList = new List<string>(infoPatient);
                                    int curr = 0;
                                    while (curr < infoPatientList.Count)
                                    {
                                        int qCount = checkForOneQuote(infoPatientList[curr]);
                                        if (qCount < 2)
                                        {
                                            int nextWord = curr + 1;
                                            while (qCount < 2)
                                            {
                                                qCount = checkForOneQuote(infoPatientList[nextWord]);
                                                if (qCount >= 2) { break; }
                                                infoPatientList[curr] += infoPatientList[nextWord];
                                                infoPatientList.RemoveAt(nextWord);
                                            }
                                        }
                                        curr++;
                                    }
                                    newPatient = new NMGPatient(infoPatient[4]);
                                    string guid1 = System.Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                                    char charToTrim1 = '"';
                                    var data1 = new List<string[]>()
                                    {
                                            new string[]{infoPatientList[4], guid1, infoPatientList[0], infoPatientList[1], infoPatientList[2], infoPatientList[3], infoPatientList[5],
                                            infoPatientList[6], infoPatientList[7], infoPatientList[8], infoPatientList[9], infoPatientList[10], infoPatientList[11], infoPatientList[12],
                                            infoPatientList[13], infoPatientList[14], infoPatientList[15], infoPatientList[16], infoPatientList[17], infoPatientList[18], infoPatientList[19],
                                            infoPatientList[20], infoPatientList[21], infoPatientList[22], infoPatientList[23], infoPatientList[24], infoPatientList[25], infoPatientList[26],
                                            infoPatientList[27], infoPatientList[28]}
                                    };
                                    newPatient.PatientFirstName = infoPatientList[0].Trim(charToTrim1);
                                    newPatient.PatientMiddleName = infoPatientList[1].Trim(charToTrim1);
                                    newPatient.PatientLastName = infoPatientList[2].Trim(charToTrim1);
                                    newPatient.PaymentDue = infoPatientList[5].Trim(charToTrim1);
                                    int AcNo;
                                    if (Int32.TryParse(infoPatientList[4].Trim(charToTrim1), out AcNo))
                                    {
                                        newPatient.AccountNo = AcNo;
                                    }
                                    DateTime billDate;
                                    if (DateTime.TryParse(infoPatientList[3].Trim(charToTrim1), out billDate))
                                    {
                                        newPatient.BillDate = billDate;
                                    }
                                    newPatient.MailFirstName = infoPatientList[6].Trim(charToTrim1);
                                    newPatient.MailMiddleName = infoPatientList[7].Trim(charToTrim1);
                                    newPatient.MailLastName = infoPatientList[8].Trim(charToTrim1);
                                    newPatient.MailAddressLine1 = infoPatientList[9].Trim(charToTrim1);
                                    newPatient.MailAddressLine2 = infoPatientList[10].Trim(charToTrim1);
                                    newPatient.MailCity = infoPatientList[11].Trim(charToTrim1);
                                    newPatient.MailState = infoPatientList[12].Trim(charToTrim1);
                                    newPatient.MailZip = infoPatientList[13].Trim(charToTrim1);
                                    newPatient.RenderedName = infoPatientList[14].Trim(charToTrim1);
                                    newPatient.RenderedAddressLine1 = infoPatientList[15].Trim(charToTrim1);
                                    newPatient.RenderedAddressLine2 = infoPatientList[16].Trim(charToTrim1);
                                    newPatient.RenderedCity = infoPatientList[17].Trim(charToTrim1);
                                    newPatient.RenderedState = infoPatientList[18].Trim(charToTrim1);
                                    newPatient.RenderedZip = infoPatientList[19].Trim(charToTrim1);
                                    newPatient.PayableTo = infoPatientList[20].Trim(charToTrim1);
                                    newPatient.Unknowing1 = infoPatientList[21].Trim(charToTrim1);
                                    newPatient.Unknowing2 = infoPatientList[22].Trim(charToTrim1);
                                    newPatient.AgingCurrent = infoPatientList[23].Trim(charToTrim1);
                                    newPatient.Aging31_60 = infoPatientList[24].Trim(charToTrim1);
                                    newPatient.Aging61_90 = infoPatientList[25].Trim(charToTrim1);
                                    newPatient.Aging91_120 = infoPatientList[26].Trim(charToTrim1);
                                    newPatient.Aging120 = infoPatientList[27].Trim(charToTrim1);
                                    newPatient.InquireyPhone = infoPatientList[28].Trim(charToTrim1);
                                    patientList.Add(newPatient);
                                }
                                var h = j + 1;
                                if (lines.Length > h)
                                {
                                    while (lines[h] != "ecwPtStatement") //Iterates through each line for statement until it reaches next patient
                                    {
                                        if (h < lines.Length)
                                        {
                                            string[] infoPatientStatement = lines[h].Split(',');
                                            List<string> infoPatientStatementList = new List<string>(infoPatientStatement);
                                            int curr1 = 0;
                                            while (curr1 < infoPatientStatementList.Count)
                                            {
                                                int qCount1 = checkForOneQuote(infoPatientStatementList[curr1]);
                                                if (qCount1 < 2)
                                                {
                                                    int nextWord1 = curr1 + 1;
                                                    while (qCount1 < 2)
                                                    {
                                                        qCount1 = checkForOneQuote(infoPatientStatementList[nextWord1]);
                                                        if (qCount1 >= 2) { break; }
                                                        infoPatientStatementList[curr1] += infoPatientStatementList[nextWord1];
                                                        infoPatientStatementList.RemoveAt(nextWord1);
                                                    }
                                                }
                                                curr1++;
                                            }
                                            string guid2 = System.Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                                            char charToTrim2 = '"';
                                            var data2 = new List<string[]>()
                                            {
                                                new string[]{infoPatientStatementList[0], infoPatientStatementList[1], infoPatientStatementList[2],
                                                infoPatientStatementList[3], infoPatientStatementList[4], infoPatientStatementList[5], infoPatientStatementList[6],
                                                infoPatientStatementList[7]}
                                            };
                                            NMGPatientStatement newPatientStatement = new NMGPatientStatement();
                                            newPatientStatement.AccountNo = infoPatientStatementList[0].Trim(charToTrim2);
                                            newPatientStatement.ClaimNo = infoPatientStatementList[1].Trim(charToTrim2);
                                            DateTime ViDate;
                                            DateTime AcDate;
                                            if (DateTime.TryParse(infoPatientStatementList[2].Trim(charToTrim2), out ViDate))
                                            {
                                                newPatientStatement.VisitDate = ViDate;
                                            }
                                            if (DateTime.TryParse(infoPatientStatementList[3].Trim(charToTrim2), out AcDate))
                                            {
                                                newPatientStatement.ActivityDate = AcDate;
                                            }
                                            newPatientStatement.SetDescription(infoPatientStatementList[4].Trim(charToTrim2));
                                            newPatientStatement.Charges = infoPatientStatementList[5].Trim(charToTrim2);
                                            newPatientStatement.Payments = infoPatientStatementList[6].Trim(charToTrim2);
                                            newPatientStatement.Balance = infoPatientStatementList[7].Trim(charToTrim2);
                                            patientStatementList.Add(newPatientStatement);
                                            h++;
                                        }
                                    }
                                    newPatient.SetStatement(patientStatementList);
                                }
                                int patientStatementListSize = patientStatementList.Count;
                                patientStatementList.RemoveRange(0, patientStatementListSize);
                            }
                        }
                        createPDF.GeneratorPDF(patientList, fileName);
                    }
                }
            }
        }
        private int checkForOneQuote(string checkString)
        {
            int count = 0;
            foreach (var q in checkString)
            {
                if (q == '"')
                {
                    count++;
                }
            }
            return count;
        }
    }
}

