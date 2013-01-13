using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MutualFundsMonitor
{
    class TransactionInfo
    {
        private float m_units = 0.0f;
        private float m_price = 0.0f;
        private string m_date;

        public TransactionInfo(float units, float price, string date)
        {
            this.m_units = units;
            this.m_price = price;
            this.m_date = date;
        }

        public float Units
        {
            get { return m_units; }
        }

        public float Price
        {
            get { return m_price; }
        }

        public string Date
        {
            get { return m_date; }
        }
    }
}
