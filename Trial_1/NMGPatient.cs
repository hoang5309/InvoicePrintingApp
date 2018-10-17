using System;
using System.Collections.Generic;
using System.Linq;

namespace Trial_1
{
    public class NMGPatient : IEquatable<NMGPatient>//, IComparable<NMGPatient>
    {
        private const int maxPageLine = 30;
        
        public string PatientFirstName { get; set; }
        public string PatientMiddleName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime BillDate { get; set; }
        public int AccountNo { get; set; }
        public string PaymentDue { get; set; }
        public string MailFirstName { get; set; }
        public string MailMiddleName { get; set; }
        public string MailLastName { get; set; }
        public string MailAddressLine1 { get; set; }
        public string MailAddressLine2 { get; set; }
        public string MailCity { get; set; }
        public string MailState { get; set; }
        public string MailZip { get; set; }
        public string RenderedName { get; set; }
        public string RenderedAddressLine1 { get; set; }
        public string RenderedAddressLine2 { get; set; }
        public string RenderedCity { get; set; }
        public string RenderedState { get; set; }
        public string RenderedZip { get; set; }
        public string PayableTo { get; set; }
        public string Unknowing1 { get; set; }
        public string Unknowing2 { get; set; }
        public string AgingCurrent { get; set; }
        public string Aging31_60 { get; set; }
        public string Aging61_90 { get; set; }
        public string Aging91_120 { get; set; }
        public string Aging120 { get; set; }
        public string InquireyPhone { get; set; }

        public string IMBarcode { get; set; }
        public int SortPosition { get; set; }
        public int TrayNumber { get; set; }
        public int PageNumber => StatementPageSapreted.Count();

        public string ID { get; set; }
        public PatientMailingStatus AddressStatus { get; private set; }
        //public IEnumerable<NMGPatientStatement> Statement { get; private set; }
        public IEnumerable<List<NMGPatientStatement>> StatementPageSapreted { get; private set; }

        
        //Need to return error before input when a statement line is large then 31
        private void sepratePage(IEnumerable<NMGPatientStatement> aStatement)
        {
            List<List<NMGPatientStatement>> tempResult = new List<List<NMGPatientStatement>>();

            List<NMGPatientStatement> chunck = new List<NMGPatientStatement>();
            tempResult.Add(chunck);
            int pageLine = 0;
            for (int i = 0; i < aStatement.Count(); i++)
            {
                NMGPatientStatement currentStatement = aStatement.ElementAt(i);
                pageLine += currentStatement.DescriptionLine;

                if (pageLine > maxPageLine)
                {
                    chunck = new List<NMGPatientStatement>();
                    chunck.Add(currentStatement);
                    tempResult.Add(chunck);
                    pageLine = currentStatement.DescriptionLine;
                }
                else
                {
                    chunck.Add(currentStatement);
                }                
            }

            StatementPageSapreted = tempResult;
        }

        public void SetStatement(IEnumerable<NMGPatientStatement> aStatement)
        {
            sepratePage(aStatement);
        }

        public void UpdataPatient(NMGPatient aPatient)
        {
            if (aPatient.ID != ID || AccountNo != aPatient.AccountNo) throw new ArgumentException("Error");

            MailFirstName = aPatient.MailFirstName;
            MailMiddleName = aPatient.MailMiddleName;
            MailLastName = aPatient.MailLastName;
            MailAddressLine1 = aPatient.MailAddressLine1;
            MailAddressLine2 = aPatient.MailAddressLine2;
            MailCity = aPatient.MailCity;
            MailState = aPatient.MailState;
            MailZip = aPatient.MailZip;

            IMBarcode = aPatient.IMBarcode;
            SortPosition = aPatient.SortPosition;
            TrayNumber = aPatient.TrayNumber;
            AddressStatus = PatientMailingStatus.Verified;
        }

        public NMGPatient(string aID)
        {
            ID = aID;
            AddressStatus = PatientMailingStatus.Imported;
        }

        public bool Equals(NMGPatient aPatient)
        {
            if (aPatient == null) return false;
            return (this.ID.Equals(aPatient.ID));
        }

        public override bool Equals(object aObj)
        {
            if (aObj == null) return false;
            NMGPatient patient = aObj as NMGPatient;
            return patient == null ? false : Equals(patient);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            string str = "";

            str += "PatientFirstName-" + PatientFirstName + ",";
            str += "PatientMiddleName-" + PatientMiddleName + ",";
            str += "PatientLastName-" + PatientLastName + ",";
            str += "BillDate-" + BillDate + ",";
            str += "AccountNo-" + AccountNo + ",";
            str += "PaymentDue-" + PaymentDue + ",";
            str += "MailFirstName-" + MailFirstName + ",";
            str += "MailMiddleName-" + MailMiddleName + ",";
            str += "MailLastName-" + MailLastName + ",";
            str += "MailAddressLine1-" + MailAddressLine1 + ",";
            str += "MailAddressLine2-" + MailAddressLine2 + ",";
            str += "MailCity-" + MailCity + ",";
            str += "MailState-" + MailState + ",";
            str += "MailZip-" + MailZip + ",";
            str += "RenderedName-" + RenderedName + ",";
            str += "RenderedAddressLine1-" + RenderedAddressLine1 + ",";
            str += "RenderedAddressLine2-" + RenderedAddressLine2 + ",";
            str += "RenderedCity-" + RenderedCity + ",";
            str += "RenderedState-" + RenderedState + ",";
            str += "RenderedZip-" + RenderedZip + ",";
            str += "PayableTo-" + PayableTo + ",";
            str += "Unknowing1-" + Unknowing1 + ",";
            str += "Unknowing2-" + Unknowing2 + ",";
            str += "AgingCurrent-" + AgingCurrent + ",";
            str += "Aging31_60-" + Aging31_60 + ",";
            str += "Aging61_90-" + Aging61_90 + ",";
            str += "Aging91_120-" + Aging91_120 + ",";
            str += "Aging120-" + Aging120 + ",";
            str += "InquireyPhone-" + InquireyPhone + ",";
            str += "NumberOfStatement-" + StatementPageSapreted.Sum(s => s.Count) + ",";

            str += "IMBarcode-" + IMBarcode + ",";
            str += "SortPosition-" + SortPosition + ",";
            str += "TrayNumber-" + TrayNumber + ",";

            str += "PatientID-" + ID + ",";
            str += "PatientStatus-" + Enum.GetName(typeof(PatientMailingStatus), AddressStatus);            

            return str;
        }

        public enum PatientMailingStatus
        {          
            Imported,
            Verified,
            Printed
        }

        public string test()
        {
            return PatientFirstName;
        }
    }
}

