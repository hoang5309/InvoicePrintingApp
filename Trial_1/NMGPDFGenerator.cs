using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Threading;

namespace Trial_1
{
    public class NMGPDFGenerator
    {

        //From button to top
        //left button (0, 0)
        //right top (611, 791)
        public bool debug = false;

        private const string errorCode = "Error Code: 006";

        private readonly string pageFontPath;
        private readonly string imbFontPath;
        private readonly string logoPath;
        private readonly string cardTypeImgePath;
        
        #region Magic strings

        private readonly string[] messages = {"\t\u2022 Northern Medical Group is excited to announce the opening of our new multi-specialty office "
                                            + "located at 159 Barnegat Road Poughkeepsie NY! Call our office today to schedule an appointment.",

                                            "\t\u2022 Feel free to contact us regarding any billing questions at billing@northernmed.com or "
                                            + "(845) 592-4915 to speak to one of our customer representatives.",
                                            };
        private const int maxPageLine = 30; //Total 34 line, including messages.

        private const string hqAddr = "Northern Medical Group PLLC\n159 Barnegat Road\nPoughkeepsie, NY 12601\n";
        private const string returnName = "Northern Medical Group PLLC";
        private const string returnAddr = "159 Barnegat Road | Lower Level \nPoughkeepsie, NY 12601-5402";
        private const string footerAddr = "\nNorthern Medical Group PLLC       159 Barnegat Road       Poughkeepsie, NY 12561";

        private const string mailInstr = "Please complete payment information above\nand send to the address below:";
        private const string infoChangeInstr = "[  ] Check if your billing information has changed, and please provide update(s) above.";
        private const string detachInstr = "Please detach and return top portion with payment.";

        private const string patient = "Patient: ";
        private const string fbiCall = "For billing inquiries, call: ";
        private const string accountNo = "Account No.";
        private const string statementDate = "Statement Date";
        private const string paymentDue = "Payment Due";
        private const string mailPay = "Mail Pay";
        private const string eypa = "Enter Your Payment Amount";
        private const string dollarSign = "$";
        private const string byCheck = "By Check";
        private const string ckeckNo = "Check No.";
        private const string payableTo = "Payable to: ";
        private const string byCard = "By Card";
        private const string cardNo = "Card No.";
        private const string expDate = "Exp. Date";
        private const string securCode = "Security Code";
        private const string sign = "Signature";
        private const string message = "Messages";
        private const string statDetail = "Statement Detail";
        private const string claimNo = "Claim No.";
        private const string visitDate = "Visit Date";
        private const string dos = "Description of Service";
        private const string charges = "Charges";
        private const string payments = "Payments";
        private const string balance = "Balance";
        private const string aging = "Aging";
        private const string aging_current = "Current";
        private const string aging_31 = "31 - 60";
        private const string aging_61 = "61 - 90";
        private const string aging_91 = "91 - 120";
        private const string aging_120 = "120+";

        private const string nextLine = "\n";
        private const string emptySpace = " ";

        #endregion

        #region Dynamic strings

        //Data
        private string dyn_CRSTCode;// = "0123456789";
        private string dyn_IMBCode;// = "TFFTADAATTATFAFDTDATFFTFDADDTTAFFATTADTDDTTDADADDTAAFFFFAAAATTFTT";
        private string dyn_MailAddr;// = "FirstName LastName\n31 South Ohioville Road\nNew Paltz, NY 12561-4012";

        private string dyn_PatientName;// = patient + "FirstName LastName" + "\n";
        private string dyn_RanderNumber;// = fbiCall + "(845) 592-4915" + "\n";
        private string dyn_AccountNo;// = "123456";
        private string dyn_StatementDate;// = "01/01/2017";
        private string dyn_PaymentDue;// = dollarSign + "999,999.99";
        private string dyn_PayableTo;// = payableTo + "Northern Medical Group";
        private string dyn_MainRightHeader;// = statementDate + " " + "01/01/2017" + "       " + accountNo + " " + "123456";

        private string dyn_Current;// = dollarSign + "123456.78";
        private string dyn_31;// = dollarSign + "123456.78";
        private string dyn_61;// = dollarSign + "123456.78";
        private string dyn_91;// = dollarSign + "123456.78";
        private string dyn_120;// = dollarSign + "123456.78";
        private string dyn_FooterLeft;// = fbiCall + "(845) 592-4915" + "              " + patient + "FirstName LastName";

        #endregion
        public Form1 x; 

        public NMGPDFGenerator(string aResourcePath, Form1 form)
        {
            pageFontPath = aResourcePath + "\\Resources\\Fonts\\3OF9_NEW.TTF";
            imbFontPath = aResourcePath + "\\Resources\\Fonts\\USPSIMBStandard.ttf";
            logoPath = aResourcePath + "\\Resources\\Images\\NMGLogo.png";
            cardTypeImgePath = aResourcePath + "\\Resources\\Images\\card-select.png";
            x = form;
        }
        
        public void GeneratorPDF(IEnumerable<NMGPatient> aPatientList, string aOutPutPath)
        {
            int count = 0;
            if (aPatientList == null) throw new ArgumentNullException(errorCode);
            DateTime currentTime = DateTime.Now;
            string fileName = "\\CRSTJob" + currentTime.ToString("yyyyMMdd_HHmmss") + ".pdf";
            //aOutPutPath += fileName;

            Document doc = new Document(PageSize.LETTER, 0, 0, 0, 0);//611*791
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(aOutPutPath, FileMode.Create));

            doc.Open();

            long timeInNumber = currentTime.ToFileTime();
            int pdfCounter = 0;
            foreach (var p in aPatientList)
            {
                count++;
                x.UpdateMember(count.ToString());
                setCurrentPatientInfo(p);
                for (int i = 0; i < p.StatementPageSapreted.Count(); i++)
                {
                    doc.NewPage();

                    pdfCounter++;
                    Rectangle rect;

                    //For left bar code
                    rect = new Rectangle(0, 590, 55, 750);
                    addLeftScanColumn(wri, rect, p.StatementPageSapreted.Count());

                    //For Logo information
                    rect = new Rectangle(55, 665, 300, 791);
                    addLogoColumn(wri, rect);

                    //For mailling information
                    rect = new Rectangle(65, 570, 310, 650);
                    addMailingColumn(wri, rect);

                    //For right side form
                    rect = new Rectangle(300, 665, 600, 791);
                    addFormColumn(wri, rect);

                    //For right side return address
                    rect = new Rectangle(390, 570, 600, 650);
                    addReturnAddress(wri, rect);

                    //For right bar CRST code
                    rect = new Rectangle(600, 570, 611, 780);
                    addRightCRSTCode(wri, rect, timeInNumber, pdfCounter, i+1, p.StatementPageSapreted.Count());

                    //For spreate line
                    rect = new Rectangle(0, 540, 611, 555);
                    addSpreateLine(wri, rect);

                    //For the main body
                    rect = new Rectangle(11, 75, 595, 540);
                    addDetail(wri, rect, p.StatementPageSapreted.ElementAt(i), i == p.StatementPageSapreted.Count() - 1 ? true : false);

                    //For button aging form
                    rect = new Rectangle(11, 20, 400, 75);
                    addAgingColumn(wri, rect);

                    //For payment due
                    rect = new Rectangle(505, 20, 595, 75);
                    addPaymentDueColumn(wri, rect);

                    //for button page information
                    rect = new Rectangle(11, 0, 570, 20);
                    addFooter(wri, rect, i+1, p.StatementPageSapreted.Count());
                }
            }
            x.UpdateMember("Done");
            doc.Close();
        }

        private void setCurrentPatientInfo(NMGPatient aPatient)
        {
            string mailingName;
            if (String.IsNullOrEmpty(aPatient.MailMiddleName))
            {
                mailingName = aPatient.MailFirstName + " " + aPatient.PatientLastName;
            }
            else
            {
                mailingName = aPatient.MailFirstName + " " + aPatient.MailMiddleName + " " + aPatient.PatientLastName;
            }

            string mailingAddress;
            if (String.IsNullOrEmpty(aPatient.MailAddressLine2))
            {
                mailingAddress = aPatient.MailAddressLine1 + "\n" + aPatient.MailCity + ", " + aPatient.MailState + " " + aPatient.MailZip;
            }
            else
            {
                mailingAddress = aPatient.MailAddressLine1 + "\n" + aPatient.MailAddressLine2 + "\n" + aPatient.MailCity + ", " + aPatient.MailState + " " + aPatient.MailZip;
            }

            dyn_CRSTCode = aPatient.TrayNumber + "-" + aPatient.SortPosition;
            dyn_IMBCode = aPatient.IMBarcode;
            dyn_MailAddr = mailingName + "\n" + mailingAddress;

            string patientName;
            if (String.IsNullOrEmpty(aPatient.PatientMiddleName))
            {
                patientName = aPatient.PatientFirstName + " " + aPatient.PatientLastName;
            }
            else
            {
                patientName = aPatient.PatientFirstName + " " + aPatient.PatientMiddleName + " " + aPatient.PatientLastName;
            }

            dyn_PatientName = patient + patientName + "\n";
            dyn_RanderNumber = fbiCall + aPatient.InquireyPhone + "\n";
            dyn_AccountNo = aPatient.AccountNo.ToString();
            dyn_StatementDate = aPatient.BillDate.ToString("MM/dd/yyyy");
            dyn_PaymentDue = dollarSign + aPatient.PaymentDue;
            dyn_PayableTo = payableTo + aPatient.PayableTo;
            dyn_MainRightHeader = statementDate + " " + aPatient.BillDate.ToString("MM/dd/yyyy") + "       " + accountNo + " " + aPatient.AccountNo;

            dyn_Current = dollarSign + aPatient.AgingCurrent;
            dyn_31 = dollarSign + aPatient.Aging31_60;
            dyn_61 = dollarSign + aPatient.Aging61_90;
            dyn_91 = dollarSign + aPatient.Aging91_120;
            dyn_120 = dollarSign + aPatient.Aging120;
            dyn_FooterLeft = fbiCall + aPatient.InquireyPhone + "              " + patient + patientName;
    }

        private List<List<NMGPatientStatement>> pageStatment(IEnumerable<NMGPatientStatement> aStatments, int aChunckNo)
        {
            //Max line can not be less then 0, and 1 description cannot take more then max line.
            if (aStatments == null || aChunckNo <= 0 || aStatments.Any(s => s.DescriptionLine > aChunckNo)) throw new ArgumentNullException(errorCode);

            List<List<NMGPatientStatement>> result = new List<List<NMGPatientStatement>>();

            List<NMGPatientStatement> chunck = new List<NMGPatientStatement>();
            int lineCounter = 0;
            for (int i = 0; i < aStatments.Count(); i++)
            {
                NMGPatientStatement currentStatement = aStatments.ElementAt(i);
                lineCounter += currentStatement.DescriptionLine;

                if (lineCounter <= aChunckNo && i < (aStatments.Count() - 1))
                {                    
                    chunck.Add(currentStatement);
                    continue;
                }
                else
                {
                    if (i == aStatments.Count() - 1)
                    {
                        chunck.Add(currentStatement);
                    }
                    result.Add(chunck);
                    chunck = new List<NMGPatientStatement>();                    
                    chunck.Add(currentStatement);                    
                    lineCounter = currentStatement.DescriptionLine;
                }
            }
            return result;
        }

        private void addLeftScanColumn(PdfWriter writer, Rectangle rect, int aTotalPage)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100f;
            Paragraph text = new Paragraph(aTotalPage.ToString(), getFont(fontName.ton_25));
            PdfPCell cell = new PdfPCell(text);
            //cell.HorizontalAlignment = 2;
            //cell.VerticalAlignment = 2;
            cell.Rotation = 90;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            ct.AddElement(table);

            ct.Go();
        }

        private void addLogoColumn(PdfWriter writer, Rectangle rect)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            Paragraph paragraph;

            /* =============================================================================
             * Logo section
             * =============================================================================
             */
            //Logo
            Image logo = Image.GetInstance(logoPath);
            logo.ScalePercent(10f);
            ct.AddElement(logo);

            Phrase text = new Phrase(emptySpace, getFont(fontName.arial_6));
            text.SetLeading(0, 0);
            ct.AddElement(text);

            //Title
            paragraph = new Paragraph();
            paragraph.SetLeading(10, 0);
            paragraph.Add(new Chunk(hqAddr, getFont(fontName.arial_9_B)));
            ct.AddElement(paragraph);

            //Empty
            paragraph = new Paragraph();
            paragraph.SetLeading(8, 0);
            paragraph.Add(new Chunk(emptySpace, getFont(fontName.arial_6)));
            ct.AddElement(paragraph);

            /* =============================================================================
             * Patient section
             * =============================================================================
             */
            //Patient name
            paragraph = new Paragraph();
            paragraph.SetLeading(1, 1);
            paragraph.Add(new Chunk(dyn_PatientName, getFont(fontName.arial_9_B)));
            ct.AddElement(paragraph);

            //Detail
            paragraph = new Paragraph();
            paragraph.SetLeading(0, 2);
            paragraph.Add(new Chunk(dyn_RanderNumber, getFont(fontName.arial_9_B)));
            ct.AddElement(paragraph);

            //Empty
            paragraph = new Paragraph();
            paragraph.SetLeading(0, 2);
            paragraph.Add(new Chunk(emptySpace));
            ct.AddElement(paragraph);

            ct.Go();
        }

        private void addMailingColumn(PdfWriter writer, Rectangle rect)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            Paragraph paragraph;

            /* =============================================================================
             * Mailing section
             * =============================================================================
             */
            //CRST mail info
            paragraph = new Paragraph();
            paragraph.SetLeading(0, 2);
            paragraph.Add(new Chunk(dyn_CRSTCode, getFont(fontName.arial_6)));
            ct.AddElement(paragraph);

            //IMB code
            paragraph = new Paragraph();
            paragraph.SetLeading(12, 0);
            paragraph.Add(new Chunk(dyn_IMBCode, getFont(fontName.imb_18)));
            ct.AddElement(paragraph);

            //Empty
            paragraph = new Paragraph();
            paragraph.SetLeading(8, 0);
            paragraph.Add(new Chunk(emptySpace, getFont(fontName.arial_6)));
            ct.AddElement(paragraph);

            //Address
            paragraph = new Paragraph();
            paragraph.SetLeading(10, 0);
            paragraph.Add(new Chunk(dyn_MailAddr, getFont(fontName.arial_9)));
            ct.AddElement(paragraph);

            ct.Go();
        }

        private void addFormColumn(PdfWriter writer, Rectangle rect)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            PdfPTable paymentInfoTable = new PdfPTable(4);
            paymentInfoTable.WidthPercentage = 97f;
            BaseColor gray = new BaseColor(217, 217, 217);
            BaseColor black = BaseColor.BLACK;
            BaseColor white = BaseColor.WHITE;

            PdfPCell specialCell;
            Font specialFont;
            Phrase text;

            /* =============================================================================
             * Account No.       Statement Date       Payment Due
             *   123456            01/01/2017         $999,999.99
             * =============================================================================
             */
            //Account No.
            text = new Phrase(accountNo, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            //Statement Date
            text = new Phrase(statementDate, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            //Payment Due
            specialFont = getFont(fontName.arial_8_B);
            specialFont.Color = white;
            text = new Phrase(paymentDue, specialFont);
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = black;
            specialCell.Colspan = 2;
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            //123456 (Account No.)
            text = new Phrase(dyn_AccountNo, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            //01/01/2017 (Statement Date)
            text = new Phrase(dyn_StatementDate, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            //$999,999.99 (Payment Due)
            text = new Phrase(dyn_PaymentDue, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.Colspan = 2;
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            /* =============================================================================
             *           Mail Pay
             * Enter Your Payment Amount $
             * =============================================================================
             */
            //Mail Pay
            text = new Phrase(mailPay, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.Colspan = 4;
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            //Enter Your Payment Amount
            text = new Phrase(eypa, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.Colspan = 2;
            specialCell.HorizontalAlignment = 2;
            paymentInfoTable.AddCell(specialCell);

            //$
            text = new Phrase(dollarSign, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.Colspan = 2;
            paymentInfoTable.AddCell(specialCell);

            /* =============================================================================
             * By Check       Check No.
             * Payable to: XXXXXXXXXXXX
             * =============================================================================
             */
            //By Check
            text = new Phrase(byCheck, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            //Check No.
            text = new Phrase(ckeckNo, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.Colspan = 3;
            paymentInfoTable.AddCell(specialCell);

            //Payable to: XXXXXXXXXXXX
            text = new Phrase(dyn_PayableTo, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.Colspan = 4;
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            /* =============================================================================
             * By Card       Select Card:
             * Card No.
             * Exp. Date              Security Code:
             * Signature:
             * =============================================================================
             */
            //By Card
            text = new Phrase(byCard, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            //Select Card:
            specialCell = new PdfPCell();
            Image cardTypeImg = Image.GetInstance(cardTypeImgePath);
            cardTypeImg.ScalePercent(3f);
            specialCell.AddElement(cardTypeImg);
            specialCell.Colspan = 3;
            paymentInfoTable.AddCell(specialCell);

            //Card No.
            text = new Phrase(cardNo, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.Colspan = 4;
            paymentInfoTable.AddCell(specialCell);

            //Exp. Date
            text = new Phrase(expDate, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.Colspan = 2;
            paymentInfoTable.AddCell(specialCell);

            //Security Code
            text = new Phrase(securCode, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.Colspan = 2;
            paymentInfoTable.AddCell(specialCell);

            //Signature
            text = new Phrase(sign, getFont(fontName.arial_8));
            specialCell = new PdfPCell(text);
            specialCell.Colspan = 4;
            paymentInfoTable.AddCell(specialCell);

            ct.AddElement(paymentInfoTable);

            ct.Go();
        }

        private void addReturnAddress(PdfWriter writer, Rectangle rect)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            PdfPTable paymentInfoTable = new PdfPTable(4);
            paymentInfoTable.WidthPercentage = 97f;
            BaseColor gray = new BaseColor(217, 217, 217);
            BaseColor black = BaseColor.BLACK;
            BaseColor white = BaseColor.WHITE;

            Phrase text;

            /* =============================================================================
             * Please complete payment information above
             * and send to the address below:
             * 
             * Northern Medical Group PLLC Lower Level 
             * 159 Barnegat Road
             * Poughkeepsie, NY 12601-5402
             * =============================================================================
             */

            //Instruction
            text = new Phrase(mailInstr, getFont(fontName.arial_9_B));
            text.SetLeading(10, 0);
            ct.AddElement(text);

            //Empty
            text = new Phrase(emptySpace, getFont(fontName.arial_9));
            text.SetLeading(10, 0);
            ct.AddElement(text);

            //Return address
            text = new Phrase(returnName, getFont(fontName.arial_9_B));
            text.SetLeading(10, 0);
            ct.AddElement(text);

            text = new Phrase(returnAddr, getFont(fontName.arial_9));
            text.SetLeading(10, 0);
            ct.AddElement(text);

            ct.Go();
        }

        private void addRightCRSTCode(PdfWriter writer, Rectangle rect, long aCurrentTime, int aCurrentPDF, int aCurrentPage, int aTotalPage)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100f;
            Paragraph text = new Paragraph(aCurrentTime + "-:-" + aCurrentPDF + "-:-" + aCurrentPage + "/" + aTotalPage, getFont(fontName.arial_6));
            PdfPCell cell = new PdfPCell(text);
            cell.Rotation = 90;
            cell.BorderWidth = 0;
            table.AddCell(cell);

            ct.AddElement(table);

            ct.Go();
        }

        private void addSpreateLine(PdfWriter writer, Rectangle rect)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            PdfPTable tempTable = new PdfPTable(2);
            tempTable.WidthPercentage = 100f;

            PdfPCell tempCell;
            Phrase text;

            text = new Phrase(infoChangeInstr, getFont(fontName.arial_7));
            tempCell = new PdfPCell(text);
            tempCell.Border = 2;
            tempTable.AddCell(tempCell);

            text = new Phrase(detachInstr, getFont(fontName.arial_7));
            tempCell = new PdfPCell(text);
            tempCell.HorizontalAlignment = 2;
            tempCell.Border = 2;
            tempTable.AddCell(tempCell);

            ct.AddElement(tempTable);

            ct.Go();
        }

        private void addDetail(PdfWriter writer, Rectangle rect, List<NMGPatientStatement> aStatements, bool isLastPage)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            PdfPTable detailTable = new PdfPTable(9);
            detailTable.WidthPercentage = 100f;
            BaseColor gray = new BaseColor(217, 217, 217);
            BaseColor black = BaseColor.BLACK;

            PdfPCell specialCell;
            Phrase text;
            Paragraph paragraph;

            /* =============================================================================
             * Message section
             * =============================================================================
             */
            //Message
            text = new Phrase(message, getFont(fontName.arial_10_B));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.Colspan = 9;
            detailTable.AddCell(specialCell);

            //(Message) Detail
            specialCell = new PdfPCell();
            specialCell.UseAscender = true;
            specialCell.UseDescender = true;
            specialCell.SetLeading(12, 0);
            specialCell.Colspan = 9;
            specialCell.Border = 0;
            for (int i = 0; i < messages.Length; i++)
            {
                paragraph = new Paragraph();
                paragraph.SetLeading(11, 0);
                paragraph.Add(new Chunk(messages[i], getFont(fontName.arial_9)));
                specialCell.AddElement(paragraph);
            }
            detailTable.AddCell(specialCell);

            /* =============================================================================
             * Statement detail
             * =============================================================================
             */
            //Title
            text = new Phrase(statDetail, getFont(fontName.arial_10_B));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.Colspan = 4;
            specialCell.BorderWidthRight = 0;
            detailTable.AddCell(specialCell);

            text = new Phrase(dyn_MainRightHeader, getFont(fontName.arial_10_B));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.Colspan = 5;
            specialCell.BorderWidthLeft = 0;
            specialCell.HorizontalAlignment = 2;
            detailTable.AddCell(specialCell);

            //Header
            #region Header
            text = new Phrase(claimNo, getFont(fontName.arial_9));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            specialCell.BorderWidthTop = 0;
            specialCell.BorderWidthRight = 0;
            detailTable.AddCell(specialCell);

            text = new Phrase(visitDate, getFont(fontName.arial_9));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            specialCell.BorderWidthTop = 0;
            specialCell.BorderWidthRight = 0;
            detailTable.AddCell(specialCell);

            text = new Phrase(dos, getFont(fontName.arial_9));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            specialCell.Colspan = 4;
            specialCell.BorderWidthTop = 0;
            specialCell.BorderWidthRight = 0;
            detailTable.AddCell(specialCell);

            text = new Phrase(charges, getFont(fontName.arial_9));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            specialCell.BorderWidthTop = 0;
            specialCell.BorderWidthRight = 0;
            detailTable.AddCell(specialCell);

            text = new Phrase(payments, getFont(fontName.arial_9));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            specialCell.BorderWidthTop = 0;
            specialCell.BorderWidthRight = 0;
            detailTable.AddCell(specialCell);

            text = new Phrase(balance, getFont(fontName.arial_9));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            specialCell.BorderWidthTop = 0;
            detailTable.AddCell(specialCell);
            #endregion

            //Body
            #region Body
            DateTime dupVisitDate = DateTime.Now;
            int pageLine;
            if (isLastPage)
            {              
                int statementLines = aStatements.Sum(s => s.DescriptionLine);
                pageLine = (maxPageLine - statementLines) + aStatements.Count;
            }
            else
            {
                pageLine = aStatements.Count;
            }

            for (int i = 0; i < pageLine; i++)
            {
                string dyn_CliamNo = emptySpace;
                string dyn_VisiDate = emptySpace;
                string dyn_DOS = emptySpace;
                string dyn_Charges = emptySpace;
                string dyn_Payments = emptySpace;
                string dyn_Balance = emptySpace;

                if (!isLastPage || i < aStatements.Count)
                {
                    dyn_CliamNo = aStatements[i].ClaimNo.ToString();//"123456";
                    if (dupVisitDate.ToString("MM/dd/yyyy") != aStatements[i].VisitDate.ToString("MM/dd/yyyy"))
                    {
                        dupVisitDate = aStatements[i].VisitDate;
                        dyn_VisiDate = aStatements[i].VisitDate.ToString("MM/dd/yyyy");//"01/01/2017";
                    }                   
                    dyn_DOS = aStatements[i].Description;//"Claim: 123456, Provider: FirstName LastName, MD";
                    if (!String.IsNullOrWhiteSpace(aStatements[i].Charges))
                    {
                        dyn_Charges = dollarSign + aStatements[i].Charges;//"999,999.99";
                    }
                    if (!String.IsNullOrWhiteSpace(aStatements[i].Payments))
                    {
                        dyn_Payments = dollarSign + aStatements[i].Payments;//"999,999.99";
                    }
                    if (!String.IsNullOrWhiteSpace(aStatements[i].Balance))
                    {
                        dyn_Balance = dollarSign + aStatements[i].Balance;//"999,999.99";
                    }
                }               

                text = new Phrase(dyn_CliamNo, getFont(fontName.arial_9));
                specialCell = new PdfPCell(text);
                specialCell.UseAscender = true;
                specialCell.UseDescender = true;
                specialCell.SetLeading(11, 0);
                specialCell.BorderWidthTop = 0;
                if (i < pageLine - 1) specialCell.BorderWidthBottom = 0;
                specialCell.BorderWidthRight = 0;
                detailTable.AddCell(specialCell);
              
                text = new Phrase(dyn_VisiDate, getFont(fontName.arial_9));
                specialCell = new PdfPCell(text);
                specialCell.UseAscender = true;
                specialCell.UseDescender = true;
                specialCell.SetLeading(11, 0);
                specialCell.BorderWidthTop = 0;
                if (i < pageLine - 1) specialCell.BorderWidthBottom = 0;
                specialCell.BorderWidthRight = 0;
                detailTable.AddCell(specialCell);

                text = new Phrase(dyn_DOS, getFont(fontName.arial_9));
                specialCell = new PdfPCell(text);
                specialCell.UseAscender = true;
                specialCell.UseDescender = true;
                specialCell.SetLeading(11, 0);
                specialCell.Colspan = 4;
                specialCell.BorderWidthTop = 0;
                if (i < pageLine - 1) specialCell.BorderWidthBottom = 0;
                specialCell.BorderWidthRight = 0;
                detailTable.AddCell(specialCell);

                text = new Phrase(dyn_Charges, getFont(fontName.arial_9));
                specialCell = new PdfPCell(text);
                specialCell.UseAscender = true;
                specialCell.UseDescender = true;
                specialCell.SetLeading(11, 0);
                specialCell.BorderWidthTop = 0;
                if (i < pageLine - 1) specialCell.BorderWidthBottom = 0;
                specialCell.BorderWidthRight = 0;
                detailTable.AddCell(specialCell);

                text = new Phrase(dyn_Payments, getFont(fontName.arial_9));
                specialCell = new PdfPCell(text);
                specialCell.UseAscender = true;
                specialCell.UseDescender = true;
                specialCell.SetLeading(11, 0);
                specialCell.BorderWidthTop = 0;
                if (i < pageLine - 1) specialCell.BorderWidthBottom = 0;
                specialCell.BorderWidthRight = 0;
                detailTable.AddCell(specialCell);

                text = new Phrase(dyn_Balance, getFont(fontName.arial_9));
                specialCell = new PdfPCell(text);
                specialCell.UseAscender = true;
                specialCell.UseDescender = true;
                specialCell.SetLeading(11, 0);
                specialCell.BorderWidthTop = 0;
                if (i < pageLine - 1) specialCell.BorderWidthBottom = 0;
                detailTable.AddCell(specialCell);             
            }
            #endregion

            ct.AddElement(detailTable);
            ct.Go();
        }

        private void addAgingColumn(PdfWriter writer, Rectangle rect)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            PdfPTable agingTable = new PdfPTable(6);
            agingTable.WidthPercentage = 100f;
            BaseColor gray = new BaseColor(217, 217, 217);
            BaseColor black = BaseColor.BLACK;

            PdfPCell specialCell;
            Phrase text;
            Paragraph paragraph;

            /* =============================================================================
             *         Current         31 - 60         61 - 90         91 - 120         120+
             * Aging
             *        $123456.78      $123456.78     $123456.78       $123456.78     $123456.78
             * =============================================================================
             */
            //Aging
            text = new Phrase(aging, getFont(fontName.arial_10_B));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.Rowspan = 2;
            specialCell.HorizontalAlignment = 1;
            specialCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            agingTable.AddCell(specialCell);

            //Current
            text = new Phrase(aging_current, getFont(fontName.arial_10_B));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.HorizontalAlignment = 1;
            agingTable.AddCell(specialCell);

            //31-60
            text = new Phrase(aging_31, getFont(fontName.arial_10_B));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.HorizontalAlignment = 1;
            agingTable.AddCell(specialCell);

            //61-90
            text = new Phrase(aging_61, getFont(fontName.arial_10_B));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.HorizontalAlignment = 1;
            agingTable.AddCell(specialCell);

            //91-120
            text = new Phrase(aging_91, getFont(fontName.arial_10_B));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.HorizontalAlignment = 1;
            agingTable.AddCell(specialCell);

            //120+
            text = new Phrase(aging_120, getFont(fontName.arial_10_B));
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = gray;
            specialCell.HorizontalAlignment = 1;
            agingTable.AddCell(specialCell);

            //$--Current
            text = new Phrase(dyn_Current, getFont(fontName.arial_10));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            agingTable.AddCell(specialCell);

            //$--31-60
            text = new Phrase(dyn_31, getFont(fontName.arial_10));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            agingTable.AddCell(specialCell);

            //$--61-90
            text = new Phrase(dyn_61, getFont(fontName.arial_10));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            agingTable.AddCell(specialCell);

            //$--91-120
            text = new Phrase(dyn_91, getFont(fontName.arial_10));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            agingTable.AddCell(specialCell);

            //$--120+
            text = new Phrase(dyn_120, getFont(fontName.arial_10));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            agingTable.AddCell(specialCell);

            ct.AddElement(agingTable);

            /* =============================================================================
             * Northern Medical Group PLLC     159 Barnegat Road    Poughkeepsie,NY 12561
             * =============================================================================
             */
            paragraph = new Paragraph();
            paragraph.SetLeading(10, 0);
            paragraph.Add(new Chunk(footerAddr, getFont(fontName.arial_7)));
            ct.AddElement(paragraph);

            ct.Go();
        }

        private void addPaymentDueColumn(PdfWriter writer, Rectangle rect)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            PdfPTable paymentDueTable = new PdfPTable(1);
            paymentDueTable.WidthPercentage = 100f;
            BaseColor gray = new BaseColor(217, 217, 217);
            BaseColor black = BaseColor.BLACK;
            BaseColor white = BaseColor.WHITE;

            PdfPCell specialCell;
            Phrase text;
            Font specialFont;

            /* =============================================================================
             *   Payment
             *     Due
             * $999,999.99
             * =============================================================================
             */
            //Payment Due
            specialFont = getFont(fontName.arial_14_B);
            specialFont.Color = white;
            text = new Phrase(paymentDue, specialFont);
            specialCell = new PdfPCell(text);
            specialCell.BackgroundColor = black;
            specialCell.HorizontalAlignment = 1;
            paymentDueTable.AddCell(specialCell);

            //$999,999.99
            text = new Phrase(dyn_PaymentDue, getFont(fontName.arial_14_B));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            paymentDueTable.AddCell(specialCell);

            ct.AddElement(paymentDueTable);
            ct.Go();
        }

        private void addFooter(PdfWriter writer, Rectangle rect, int aCurrentPage, int aTotalPage)
        {
            if (debug)
            {
                rect.Border = Rectangle.BOX;
                rect.BorderWidth = 0.5f;
                rect.BorderColor = BaseColor.RED;
            }

            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(rect);
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(rect);
            ct.UseAscender = true;

            PdfPTable footerTable = new PdfPTable(2);
            footerTable.WidthPercentage = 100f;
            BaseColor gray = new BaseColor(217, 217, 217);
            BaseColor black = BaseColor.BLACK;

            PdfPCell specialCell;
            Phrase text;

            /* =============================================================================
             * For billing inquireies, call (845) 592-4915   Patient:Xxxxxxx Yyyyyyy   Page 1 of 2
             * =============================================================================
             */
            //Left section
            text = new Phrase(dyn_FooterLeft, getFont(fontName.arial_7));
            specialCell = new PdfPCell(text);
            specialCell.Border = 0;
            footerTable.AddCell(specialCell);

            //Right section
            string dyn_Page = "Page " + aCurrentPage + " of " + aTotalPage;
            text = new Phrase(dyn_Page, getFont(fontName.arial_9_B));
            specialCell = new PdfPCell(text);
            specialCell.Border = 0;
            specialCell.HorizontalAlignment = 2;
            footerTable.AddCell(specialCell);

            ct.AddElement(footerTable);
            ct.Go();
        }

        private Font getFont(fontName aFont)
        {
            switch (aFont)
            {
                case fontName.arial_6:
                    return FontFactory.GetFont("Arial", 6);
                case fontName.arial_7:
                    return FontFactory.GetFont("Arial", 7);
                case fontName.arial_8:
                    return FontFactory.GetFont("Arial", 8);
                case fontName.arial_8_B:
                    return FontFactory.GetFont("Arial", 8, Font.BOLD);
                case fontName.arial_9:
                    return FontFactory.GetFont("Arial", 9);
                case fontName.arial_9_B:
                    return FontFactory.GetFont("Arial", 9, Font.BOLD);
                case fontName.arial_10:
                    return FontFactory.GetFont("Arial", 10);
                case fontName.arial_10_B:
                    return FontFactory.GetFont("Arial", 10, Font.BOLD);
                case fontName.arial_14_B:
                    return FontFactory.GetFont("Arial", 14, Font.BOLD);
                case fontName.imb_18:
                    BaseFont imb = BaseFont.CreateFont(imbFontPath, BaseFont.CP1250, BaseFont.EMBEDDED);
                    return new Font(imb, 18, Font.NORMAL);
                case fontName.ton_25:
                    BaseFont ton = BaseFont.CreateFont(pageFontPath, BaseFont.CP1250, BaseFont.EMBEDDED);
                    return new Font(ton, 25, Font.NORMAL);
                default:
                    return null;
            }
        }

        private enum fontName
        {
            arial_6,
            arial_7,
            arial_8,
            arial_8_B,
            arial_9,
            arial_9_B,
            arial_10,
            arial_10_B,
            arial_14_B,
            imb_18,
            ton_25
        }
    }
}
