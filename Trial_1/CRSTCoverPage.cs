using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace Trial_1
{
    public class CRSTCoverPage
    {
        //From button to top
        //left button (0, 0)
        //right top (611, 791)
        public bool debug = false;

        private readonly string crstLogoPath;

        #region Magic string
        private const string client = "Client:";
        private const string date = "Date:";
        private const string mailPieceCount = "Mail Piece Count:";
        private const string pageCount = "Page Count:";
        private const string ops = "Cornerstone Productions/OPS:";
        private const string page = " Page";
        private const string pageGroup = "Page Group";
        private const string count = "Count";
        private const string initial = "Initial";
        private const string filesLocation = "Files Location:";
        private const string empty = " ";
        private const string clientName = "Northern Medical Group";
        #endregion

        public CRSTCoverPage(string aResourcePath)
        {
            crstLogoPath = aResourcePath + "\\Resources\\Images\\CRSTLogo.png";
        }

        public void PrintCoverPage(IDictionary<int, int> aPageList, int aTotalPatients, int aTotalPages, string[] aFilePath, string aPath)
        {
            if (aPageList == null) throw new ArgumentNullException("Error");

            //aPath += "\\CRST_CoverPage.pdf";
            DateTime currentTime = DateTime.Now;

            Document doc = new Document(PageSize.LETTER, 0, 0, 0, 0);//611*791
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream(aPath, FileMode.Create));

            doc.Open();

            Rectangle rect;

            //For Logo information
            rect = new Rectangle(30, 715, 310, 780);
            addLogoColumn(wri, rect);

            //For Client and Date information
            rect = new Rectangle(30, 650, 310, 714);
            addClientColumn(wri, rect, currentTime);

            //For Job information
            rect = new Rectangle(450, 650, 540, 770);
            addJobInfoColumn(wri, rect, aTotalPatients, aTotalPages);

            //For OPS sign
            rect = new Rectangle(30, 610, 310, 625);
            addOPSSignColumn(wri, rect);

            //For OPS date
            rect = new Rectangle(450, 610, 540, 625);
            addOPSDateColumn(wri, rect);

            //For Page Detail
            rect = new Rectangle(30, 250, 595, 595);
            addDetailColumn(wri, rect, aPageList);

            //For File Location
            rect = new Rectangle(30, 50, 595, 240);
            addFileLocationColumn(wri, rect, aFilePath);

            doc.Close();
        }

        private void addFileLocationColumn(PdfWriter writer, Rectangle rect, string[] aFilePath)
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
             * Files Location:
             * X:/XXX/XXX/XXX/XXXX/XXX.txt
             * =============================================================================
             */
            //Files Location:
            paragraph = new Paragraph();
            paragraph.SetLeading(5, 1);
            paragraph.Add(new Chunk(filesLocation, getFont(10)));
            ct.AddElement(paragraph);

            //X:/XXX/XXX/XXX/XXXX/XXX.txt
            for (int i = 0; i < aFilePath.Length; i++)
            {
                paragraph = new Paragraph();
                paragraph.SetLeading(2, 1);
                paragraph.Add(new Chunk(aFilePath[i], getFont(8)));
                ct.AddElement(paragraph);
            }

            ct.Go();
        }

        private void addDetailColumn(PdfWriter writer, Rectangle rect, IDictionary<int, int> aPageInfo)
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

            PdfPTable paymentInfoTable = new PdfPTable(3);
            //paymentInfoTable.WidthPercentage = 97f;

            PdfPCell specialCell;
            Phrase text;

            /* =============================================================================
             *  Page       Count       Initials
             * 1 Page     9999999
             * =============================================================================
             */
            //Page Group
            text = new Phrase(pageGroup, getFont(12, true));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            //Count
            text = new Phrase(count, getFont(12, true));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            //Initials
            text = new Phrase(initial, getFont(12, true));
            specialCell = new PdfPCell(text);
            specialCell.HorizontalAlignment = 1;
            paymentInfoTable.AddCell(specialCell);

            int value;
            for (int i = 0; i < aPageInfo.Count; i++)
            {
                //X Page
                text = new Phrase(aPageInfo.ElementAt(i).Key + page, getFont(12));
                specialCell = new PdfPCell(text);
                paymentInfoTable.AddCell(specialCell);

                //9999999
                value = aPageInfo.ElementAt(i).Value / aPageInfo.ElementAt(i).Key;
                text = new Phrase(value.ToString(), getFont(12));
                specialCell = new PdfPCell(text);
                paymentInfoTable.AddCell(specialCell);

                //sing
                text = new Phrase(empty, getFont(12));
                specialCell = new PdfPCell(text);
                paymentInfoTable.AddCell(specialCell);
            }

            ct.AddElement(paymentInfoTable);

            ct.Go();
        }

        private void addOPSDateColumn(PdfWriter writer, Rectangle rect)
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
             * Date:
             * =============================================================================
             */
            //Mail Piece Count:
            paragraph = new Paragraph();
            paragraph.Add(new Chunk(date, getFont(12)));
            ct.AddElement(paragraph);

            ct.Go();
        }

        private void addOPSSignColumn(PdfWriter writer, Rectangle rect)
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
             * Cornerstone Productions/OPS:
             * =============================================================================
             */
            //Mail Piece Count:
            paragraph = new Paragraph();
            paragraph.Add(new Chunk(ops, getFont(12)));
            ct.AddElement(paragraph);

            ct.Go();
        }

        private void addJobInfoColumn(PdfWriter writer, Rectangle rect, int aTotalPatient, int aTotalPage)
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
             * Mail Piece Count:
             * 9999999
             * Page Count:
             * 9999999
             * =============================================================================
             */
            //Mail Piece Count:
            paragraph = new Paragraph();
            paragraph.Add(new Chunk(mailPieceCount, getFont(11)));
            ct.AddElement(paragraph);

            //9999999
            paragraph = new Paragraph();
            paragraph.Add(new Chunk(aTotalPatient.ToString(), getFont(17, true)));
            ct.AddElement(paragraph);

            //Empty
            paragraph = new Paragraph();
            paragraph.SetLeading(1, 1);
            paragraph.Add(new Chunk(empty, getFont(20, true)));
            ct.AddElement(paragraph);

            //Page Count:
            paragraph = new Paragraph();
            paragraph.Add(new Chunk(pageCount, getFont(11)));
            ct.AddElement(paragraph);

            //9999999
            paragraph = new Paragraph();
            paragraph.Add(new Chunk(aTotalPage.ToString(), getFont(17, true)));
            ct.AddElement(paragraph);

            ct.Go();
        }

        private void addClientColumn(PdfWriter writer, Rectangle rect, DateTime aCurrentTime)
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
             * Client:
             * XXXXXXXX XXXXXXX XXXXX
             * Date:
             * 01/01/2017
             * =============================================================================
             */
            //Client:
            paragraph = new Paragraph();
            paragraph.Add(new Chunk(client, getFont(12)));
            ct.AddElement(paragraph);

            //XXXXXXXX XXXXXXX XXXXX
            paragraph = new Paragraph();
            paragraph.Add(new Chunk(clientName, getFont(16, true)));
            ct.AddElement(paragraph);

            //Date:
            paragraph = new Paragraph();
            paragraph.Add(new Chunk(date, getFont(12)));
            ct.AddElement(paragraph);

            //XXXXXXXX XXXXXXX XXXXX
            paragraph = new Paragraph();
            paragraph.Add(new Chunk(aCurrentTime.ToString("MM/dd/yyyy"), getFont(16, true)));
            ct.AddElement(paragraph);

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

            /* =============================================================================
             * Logo section
             * =============================================================================
             */
            //Logo
            Image logo = Image.GetInstance(crstLogoPath);
            logo.ScalePercent(20f);
            ct.AddElement(logo);

            ct.Go();
        }

        private Font getFont(int aSize, bool isBold = false)
        {
            return isBold ? FontFactory.GetFont("Arial", aSize, Font.BOLD) : FontFactory.GetFont("Arial", aSize);
        }
    }
}
