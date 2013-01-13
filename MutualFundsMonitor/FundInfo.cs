using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MutualFundsMonitor
{
    class FundInfo
    {
        private string m_name;
        private ListViewItem m_item;
        private string m_link;
        private List<TransactionInfo> m_transactions = new List<TransactionInfo>();
        private float m_price = 0.0f;
        private string m_date;

        public FundInfo(string name, ListViewItem item, string link)
        {
            this.m_name = name;
            this.m_item = item;
            this.m_link = link;
        }

        public string Name
        {
            get { return m_name; }
            //set { m_name = value; }
        }

        public ListViewItem Item
        {
            get { return m_item; }
            //set { m_item = value; }
        }

        public string Link
        {
            get { return m_link; }
            //set { m_link = value; }
        }

        public float Price
        {
            get { return m_price; }
            set { m_price = value; }
        }

        public string Date
        {
            get { return m_date; }
            set { m_date = value; }
        }

        public List<TransactionInfo> Transactions
        {
            get { return m_transactions; }
        }

        public void AddTransaction(float units, float price, string date)
        {
            TransactionInfo transaction = new TransactionInfo(units, price, date);
            m_transactions.Add(transaction);
        }

        public string get_report()
        {
            string report = "";

            if (m_transactions.Count > 1)
            {
                float average_price = 0.0f;
                float total_units = 0.0f;

                foreach (TransactionInfo t in m_transactions)
                {
                    average_price += (t.Price * t.Units);
                    total_units += t.Units;
                }

                average_price /= total_units;

                float val = ((m_price * 100.0f) / average_price) - 100;
                report += val.ToString("F2");
                report += "% ( ";

            }

            foreach (TransactionInfo t in m_transactions)
            {
                float val = ((m_price * 100.0f) / t.Price) - 100;
                report += val.ToString("F2");
                report += "% ";
            }

            if (m_transactions.Count > 1)
                report += ")";

            return report;
        }
    }
}
