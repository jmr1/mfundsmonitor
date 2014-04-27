using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using mshtml;
using System.Threading;
using System.Diagnostics;
using System.Xml;

namespace MutualFundsMonitor
{
    public partial class MFMForm : Form
    {
        private bool form_closing = false;
        private NotifyIcon  trayIcon;
        private ContextMenu trayMenu;
        private List<FundInfo> fundInfoList = new List<FundInfo>();
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public MFMForm()
        {
            InitializeComponent();

            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("&Show", OnShow);
            trayMenu.MenuItems.Add("&Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon      = new NotifyIcon();
            trayIcon.Text = "Mutual Funds Monitor";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible     = true;
            trayIcon.DoubleClick += OnShow;

            /*string host = "http://www.morningstar.co.uk/uk/funds/snapshot/snapshot.aspx?id=";

            FundInfoListAddItem("Aberdeen Emerging Markets Bond Fund A Class Acc", host + "F00000MF50");
            FundInfoListAddItem("M&G Strategic Corporate Bond X Acc", host + "F0GBR04FIG");
            FundInfoListAddItem("Troy Trojan I Acc", host + "F0GBR06OFJ");

            FundInfoListAddTransaction("Aberdeen Emerging Markets Bond Fund A Class Acc", "22/10/2012", 819.54f, 122.02f);
            FundInfoListAddTransaction("M&G Strategic Corporate Bond X Acc", "23/12/2011", 1191.8990f, 84.06f);
            FundInfoListAddTransaction("M&G Strategic Corporate Bond X Acc", "22/10/2012", 1076.7740f, 92.87f);*/

            timer.Tick += new EventHandler(timer_Tick);
            //timer.Interval = 3660 * 1000;
            timer.Interval = 120 * 1000;
            timer.Enabled = true;
            timer.Start();

            //write_xml();
            read_xml();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (this.buttonCheck.Enabled)
            {
                Thread thread = new Thread(new ThreadStart(UpdateFunds));
                thread.Start();
            }
        }

        protected void FundInfoListAddItem(string name, string link)
        {
            string [] row = { "", "", "", "" };

            ListViewItem item = listViewFunds.Items.Add(name);
            item.SubItems.AddRange(row);
            fundInfoList.Add(new FundInfo(name, item, link));
        }

        protected bool FundInfoListAddTransaction(string name, string date, float units, float price)
        {
            bool found = false;
            foreach (FundInfo fund in fundInfoList)
            {
                if (fund.Name == name)
                {
                    found = true;
                    fund.AddTransaction(units, price, date);
                    string text = fund.Item.SubItems[4].Text;
                    if (text != "")
                        text += ", ";
                    text += date;
                    fund.Item.SubItems[4].Text = text;
                    break;
                }
            }

            return found;
        }

        protected bool write_xml()
        {
            using (XmlWriter writer = XmlWriter.Create("funds.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Funds");

                foreach (FundInfo info in fundInfoList)
                {
                    writer.WriteStartElement("Fund");

                    writer.WriteElementString("Name", info.Name);
                    writer.WriteElementString("Link", info.Link);

                    if (info.Transactions.Count > 0)
                    {
                        writer.WriteStartElement("Transactions");

                        foreach (TransactionInfo trans in info.Transactions)
                        {
                            writer.WriteStartElement("Transaction");

                            writer.WriteElementString("Date", trans.Date);
                            writer.WriteElementString("Price", trans.Price.ToString());
                            writer.WriteElementString("Units", trans.Units.ToString());

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            return true;
        }

        protected bool read_xml()
        {
            using (XmlReader reader = XmlReader.Create("funds.xml"))
            {
                reader.MoveToContent();
                string name = "";

                if (!reader.Read())
                    return false;

                while (!reader.EOF)
                {
                    if(reader.IsStartElement("Fund"))
                    {
                        if (!reader.Read())
                            return false;

                        if (reader.IsStartElement("Name"))
                        {
                            if (!reader.Read())
                                return false;
                            name = reader.Value;
                        }

                        if (!reader.Read())
                            return false;

                        if (!reader.Read())
                            return false;

                        if (reader.IsStartElement("Link"))
                        {
                            if (!reader.Read())
                                return false;

                            FundInfoListAddItem(name, reader.Value);
                        }

                        if (!reader.Read())
                            return false;

                        if (!reader.Read())
                            return false;
                    }

                    if (reader.IsStartElement("Transactions"))
                    {
                        if (!reader.Read())
                            return false;

                        string date = "";
                        float price = 0.0f;

                        while (reader.IsStartElement("Transaction"))
                        {
                            if (!reader.Read())
                                return false;

                            if (reader.IsStartElement("Date"))
                            {
                                if (!reader.Read())
                                    return false;
                                date = reader.Value;
                            }

                            if (!reader.Read())
                                return false;

                            if (!reader.Read())
                                return false;

                            if (reader.IsStartElement("Price"))
                            {
                                if (!reader.Read())
                                    return false;
                                price = float.Parse(reader.Value);
                            }

                            if (!reader.Read())
                                return false;

                            if (!reader.Read())
                                return false;

                            if (reader.IsStartElement("Units"))
                            {
                                if (!reader.Read())
                                    return false;

                                FundInfoListAddTransaction(name, date, float.Parse(reader.Value), price);
                            }

                            if (!reader.Read())
                                return false;

                            if (!reader.Read())
                                return false;

                            if (!reader.Read())
                                return false;
                        }

                        if (!reader.Read())
                            return false;
                    }

                    if (!reader.Read())
                        break; // no more Fund's
                }
            }

            FundInfoListAddItem("Portfolio Summary", "");

            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible       = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
            listViewFunds.View = View.Details;

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnShow(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal; // So that when form is shown back again the form is not minimized
            Visible = true; // Show form window.
            ShowInTaskbar = true; // Show in taskbar.
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void MFMForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Visible = false; // Hide form window.
                WindowState = FormWindowState.Normal; // So that when form is shown back again the form is not minimized
                ShowInTaskbar = false; // Remove from taskbar.
            }
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(UpdateFunds));
            thread.Start();
        }

        protected void UpdateFunds()
        {
            ButtonSetEnable(false);

            float portfolio_money_buy = 0.0f;
            float portfolio_money_now = 0.0f;
            float portfolio_day_change = 0.0f;
            bool portfolio_any_change = false;

            foreach (FundInfo fundInfo in fundInfoList)
            {
                if (fundInfo.Name == "Portfolio Summary" && portfolio_any_change == true)
                {
                    float portfolio_day_change_percent = ((portfolio_money_now - portfolio_day_change) * 100.0f) / portfolio_day_change;
                    float portfolio_total_percent = ((portfolio_money_now - portfolio_money_buy) * 100.0f) / portfolio_money_buy;
                    if (!form_closing)
                    {
                        ListViewSubItemSetText(fundInfo.Item.SubItems[2], portfolio_day_change_percent.ToString("F2") + "%");
                        ListViewSubItemSetText(fundInfo.Item.SubItems[3], portfolio_total_percent.ToString("F2") + "%");
                        ListViewRedrawItems(fundInfo.Item, false);
                    }
                    else
                    {
                        return;
                    }

                    break;
                }

                if (fundInfo.Link.Length == 0)
                    continue;

                string plainText = "";

                DialogResult result = DialogResult.Ignore;
                do
                {
                    try
                    {
                        this.webBrowser1.DocumentCompleted += (s, e) =>
                        {
                            if(this.webBrowser1.ReadyState == WebBrowserReadyState.Complete)
                                plainText = this.webBrowser1.DocumentText;
                        };

                        this.webBrowser1.Navigate(fundInfo.Link);
                    }
                    catch (Exception e)
                    {
                        result = MessageBox.Show("Link openning error: " + fundInfo.Link + "\n" + e.Message + "\nTry again?", fundInfo.Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                    }
                } while (result == DialogResult.Yes);

                if (result == DialogResult.No)
                    continue;

                if (result == DialogResult.Cancel)
                {
                    ButtonSetEnable(true);
                    return;
                }

                while (plainText == "") // wait for webcontent to be loaded
                    System.Threading.Thread.Sleep(1);

                int idx = plainText.IndexOf("NAV");
                int end = plainText.IndexOf("%", idx) + 1;
                string value = plainText.Substring(idx, end - idx);

                bool gbp = value.IndexOf("GBP") != -1;

                Regex rDate = new Regex(@"\d{1,2}/\d{1,2}/\d{2,4}");
                Match mDate = rDate.Match(value);
                string date = mDate.Value;

                Regex rGBXValue = new Regex(@"(GBX|GBP)\s(\d*\.?\d*)");
                Match mGBXValue = rGBXValue.Match(value);

                string price = "";
                if (mGBXValue.Groups.Count > 0)
                    price = mGBXValue.Groups[mGBXValue.Groups.Count - 1].Value;

                Regex rDayChange = new Regex(@"(\+|-)?\d*\.?\d*%");
                Match mDayChange = rDayChange.Match(value);
                string day_change = mDayChange.Value;

                fundInfo.Price = float.Parse(price);
                if (gbp)
                    fundInfo.Price *= 100.0f;

                string total_report = fundInfo.get_total_report();

                string current_date = fundInfo.Item.SubItems[1].Text;
                string current_day_change = fundInfo.Item.SubItems[2].Text;
                string current_total = fundInfo.Item.SubItems[3].Text;

                bool redraw = false;
                if(date != current_date)
                {
                    if (!form_closing)
                        ListViewSubItemSetText(fundInfo.Item.SubItems[1], date);
                    else
                        return;
                    redraw = true;
                }
                if (day_change != current_day_change)
                {
                    if (!form_closing)
                        ListViewSubItemSetText(fundInfo.Item.SubItems[2], day_change);
                    else
                        return;
                    redraw = true;
                }
                if (total_report != current_total)
                {
                    if (!form_closing)
                        ListViewSubItemSetText(fundInfo.Item.SubItems[3], total_report);
                    else
                        return;
                    redraw = true;
                }

                if (redraw)
                {
                    portfolio_any_change = true;
                    if (!form_closing)
                        ListViewRedrawItems(fundInfo.Item, false);
                    else
                        return;
                }

                foreach (TransactionInfo tr in fundInfo.Transactions)
                {
                    float transaction_money_now = (tr.Units * fundInfo.Price);
                    portfolio_money_buy += (tr.Units * tr.Price);
                    portfolio_money_now += transaction_money_now;
                    portfolio_day_change += transaction_money_now - (transaction_money_now * (float.Parse(day_change.Remove(day_change.Length-1)) / 100.0f));
                }
            }

            if (form_closing)
                return;
            ButtonSetEnable(true);
        }

        delegate void ListViewSubItemSetTextCallback(ListViewItem.ListViewSubItem listItem, string text);

        private void ListViewSubItemSetText(ListViewItem.ListViewSubItem listItem, string text)
        {
            if (this.listViewFunds.InvokeRequired)
            {
                ListViewSubItemSetTextCallback callback = new ListViewSubItemSetTextCallback(ListViewSubItemSetText);
                this.Invoke(callback, new object[] { listItem, text });
            }
            else
            {
                listItem.Text = text;
            }
        }

        delegate void ListViewRedrawItemsCallback(ListViewItem item, bool invalidateOnly);

        private void ListViewRedrawItems(ListViewItem item, bool invalidateOnly)
        {
            if (this.listViewFunds.InvokeRequired)
            {
                ListViewRedrawItemsCallback callback = new ListViewRedrawItemsCallback(ListViewRedrawItems);
                this.Invoke(callback, new object[] { item, invalidateOnly });
            }
            else
            {
                item.UseItemStyleForSubItems = false;
                //item.BackColor = Color.Gray;
                /*item.SubItems[0].BackColor = Color.Gray;
                item.SubItems[1].BackColor = Color.Gray;
                item.SubItems[2].BackColor = Color.Gray;*/

                foreach (ListViewItem.ListViewSubItem subitem in item.SubItems)
                    subitem.BackColor = Color.LightGray;

                if (item.SubItems[2].Text.Length > 0 && item.SubItems[2].Text[0] == '-')
                {
                    item.SubItems[2].ForeColor = Color.Red;
                    //item.UseItemStyleForSubItems = false;
                }
                else
                {
                    item.SubItems[2].ForeColor = SystemColors.WindowText;
                }

                this.listViewFunds.RedrawItems(item.Index, item.Index, invalidateOnly);
            }
        }

        delegate void ButtonSetEnableCallback(bool enable);

        private void ButtonSetEnable(bool enable)
        {
            if (this.buttonCheck.InvokeRequired)
            {
                ButtonSetEnableCallback callback = new ButtonSetEnableCallback(ButtonSetEnable);
                this.Invoke(callback, new object[] { enable });
            }
            else
            {
                this.buttonCheck.Enabled = enable;
            }
        }

        private void listViewFunds_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                ListView list = sender as ListView;

                foreach (ListViewItem item in list.Items)
                {
                    if (item.Bounds.Contains(e.Location))
                    {
                        //item.BackColor = SystemColors.Window; // normal item
                        foreach (ListViewItem.ListViewSubItem subitem in item.SubItems)
                            subitem.BackColor = SystemColors.Window; // normal item
                        break;
                    }
                }
                break;
            }
        }

        private void MFMForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            form_closing = true;
        }

        private void listViewFunds_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    ContextMenu menu = new ContextMenu();

                    MenuItem menuOpen = new MenuItem("Open");
                    MenuItem menuSeparator = new MenuItem("-");
                    MenuItem menuMarkRead = new MenuItem("Mark as Read");
                    MenuItem menuMarkAllRead = new MenuItem("Mark all as Read");

                    menu.MenuItems.Add(menuOpen);
                    menu.MenuItems.Add(menuSeparator);
                    menu.MenuItems.Add(menuMarkRead);
                    menu.MenuItems.Add(menuMarkAllRead);

                    menu.Show(this.listViewFunds, e.Location, LeftRightAlignment.Right);

                    menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
                    menuMarkRead.Click += new System.EventHandler(this.menuMarkRead_Click);
                    menuMarkAllRead.Click += new System.EventHandler(this.menuMarkAllRead_Click);

                    break;
            }
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu menu = menuItem.GetContextMenu();

                ListView listView = menu.SourceControl as ListView;

                string text = listView.FocusedItem.Text;

                foreach (FundInfo fund in fundInfoList)
                {
                    if (fund.Name == text)
                    {
                        Process.Start(fund.Link);
                        break;
                    }
                }
            }
        }

        private void menuMarkRead_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu menu = menuItem.GetContextMenu();

                ListView listView = menu.SourceControl as ListView;

                string text = listView.FocusedItem.Text;

                foreach (ListViewItem item in listViewFunds.Items)
                {
                    if (item.Text == text)
                    {
                        foreach (ListViewItem.ListViewSubItem subitem in item.SubItems)
                            subitem.BackColor = SystemColors.Window; // normal item
                        break;
                    }
                }
            }
        }

        private void menuMarkAllRead_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu menu = menuItem.GetContextMenu();

                ListView listView = menu.SourceControl as ListView;

                foreach (ListViewItem item in listViewFunds.Items)
                {
                    foreach (ListViewItem.ListViewSubItem subitem in item.SubItems)
                        subitem.BackColor = SystemColors.Window; // normal item
                }
            }
        }

        
    }

    

    
}
