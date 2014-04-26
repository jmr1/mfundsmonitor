namespace MutualFundsMonitor
{
    partial class MFMForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
//        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /*protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }*/

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCheck = new System.Windows.Forms.Button();
            this.listViewFunds = new System.Windows.Forms.ListView();
            this.ColumnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeaderDayChange = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTotal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBuyDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // buttonCheck
            // 
            this.buttonCheck.Location = new System.Drawing.Point(13, 13);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(75, 23);
            this.buttonCheck.TabIndex = 0;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // listViewFunds
            // 
            this.listViewFunds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewFunds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeaderName,
            this.ColumnHeaderDate,
            this.ColumnHeaderDayChange,
            this.columnHeaderTotal,
            this.columnHeaderBuyDate});
            this.listViewFunds.FullRowSelect = true;
            this.listViewFunds.GridLines = true;
            this.listViewFunds.Location = new System.Drawing.Point(13, 43);
            this.listViewFunds.Name = "listViewFunds";
            this.listViewFunds.Size = new System.Drawing.Size(805, 336);
            this.listViewFunds.TabIndex = 1;
            this.listViewFunds.UseCompatibleStateImageBehavior = false;
            this.listViewFunds.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewFunds_MouseClick);
            this.listViewFunds.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewFunds_MouseUp);
            // 
            // ColumnHeaderName
            // 
            this.ColumnHeaderName.Text = "Name";
            this.ColumnHeaderName.Width = -1;
            // 
            // ColumnHeaderDate
            // 
            this.ColumnHeaderDate.Text = "Date";
            this.ColumnHeaderDate.Width = 100;
            // 
            // ColumnHeaderDayChange
            // 
            this.ColumnHeaderDayChange.Text = "Day Change";
            this.ColumnHeaderDayChange.Width = 80;
            // 
            // columnHeaderTotal
            // 
            this.columnHeaderTotal.Text = "Total";
            this.columnHeaderTotal.Width = 140;
            // 
            // columnHeaderBuyDate
            // 
            this.columnHeaderBuyDate.Text = "Buy Date";
            this.columnHeaderBuyDate.Width = -2;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(503, 64);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(250, 25);
            this.webBrowser1.TabIndex = 2;
            // 
            // MFMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 391);
            this.Controls.Add(this.listViewFunds);
            this.Controls.Add(this.buttonCheck);
            this.Controls.Add(this.webBrowser1);
            this.Name = "MFMForm";
            this.Text = "Mutual Funds Monitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MFMForm_FormClosing);
            this.Resize += new System.EventHandler(this.MFMForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.ListView listViewFunds;
        private System.Windows.Forms.ColumnHeader ColumnHeaderName;
        private System.Windows.Forms.ColumnHeader ColumnHeaderDate;
        private System.Windows.Forms.ColumnHeader ColumnHeaderDayChange;
        private System.Windows.Forms.ColumnHeader columnHeaderTotal;
        private System.Windows.Forms.ColumnHeader columnHeaderBuyDate;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}

