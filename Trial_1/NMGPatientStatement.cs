using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace Trial_1
{
    public class NMGPatientStatement
    {
        public string AccountNo { get; set; }
        public string ClaimNo { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Description { get; private set; }
        public string Charges { get; set; }
        public string Payments { get; set; }
        public string Balance { get; set; }
        
        public int DescriptionLine { get; private set; }

        private int getLine(string aString, Font aFont, float aSize, float aWidth)
        {
            if (aFont == null || aSize <= 0) throw new ArgumentNullException("Font and size cannot be null or less/equal then 0.");

            if (String.IsNullOrEmpty(aString))
            {
                return 0;
            }

            BaseFont bf = aFont.GetCalculatedBaseFont(true);
            aString = aString.TrimStart(' ');
            aString = aString.TrimEnd(' ');
            string[] input = aString.Split(' ');
            int line = 1;

            string temp = "";
            for (int i = 0; i < input.Length; i++)
            {
                string currentWord = input[i];
                float currentWordSize = bf.GetWidthPoint(currentWord, 9);

                if (currentWordSize > aSize)
                {
                    for (int j = 0; j < currentWord.Length; j++)
                    {
                        temp += currentWord[j];
                        if (bf.GetWidthPoint(temp, 9) <= aWidth)
                        {
                            if (j == currentWord.Length - 1)
                            {
                                temp += " ";
                            }
                            continue;
                        }
                        else
                        {
                            line++;
                            if (j == currentWord.Length - 1)
                            {
                                temp = currentWord[j].ToString() + " ";
                            }
                            else
                            {
                                temp = currentWord[j].ToString();
                            }
                        }
                    }
                }
                else
                {
                    temp += input[i];
                    if (bf.GetWidthPoint(temp, 9) <= aWidth)
                    {
                        if (i == input.Length - 1)
                        {
                            temp += " ";
                        }
                        continue;
                    }
                    else
                    {
                        line++;
                        if (i == input.Length - 1)
                        {
                            temp = input[i];
                        }
                        else
                        {
                            temp = input[i] + " ";
                        }
                    }
                }
            }

            return line;
        }

        public void SetDescription(string aDesription)
        {
            DescriptionLine = getLine(aDesription, FontFactory.GetFont("Arial", 9), 9f, 255.555542f); //Get and set the description line.
            Description = aDesription; //Set description.
        }

        public override string ToString()
        {
            string str = "";

            str += "AccountNo-" + AccountNo + ",";
            str += "ClaimNo-" + ClaimNo + ",";
            str += "VisitDate-" + VisitDate + ",";
            str += "ActivityDate-" + ActivityDate + ",";
            str += "Description-" + Description + ",";
            str += "Charges-" + Charges + ",";
            str += "Payments-" + Payments + ",";
            str += "Balance-" + Balance + ",";

            return str;
        }
    }
}
