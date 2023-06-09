﻿namespace RCp_Phase1
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            btnRequest = new Button();
            lblIP = new Label();
            rtbRequest = new RichTextBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            mtbIP = new MaskedTextBox();
            label1 = new Label();
            btnConnect = new Button();
            rtbResults = new RichTextBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(rtbResults, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(643, 324);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel4, 0, 1);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 28.93082F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 71.06918F));
            tableLayoutPanel2.Size = new Size(315, 318);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(btnRequest, 0, 2);
            tableLayoutPanel4.Controls.Add(lblIP, 0, 0);
            tableLayoutPanel4.Controls.Add(rtbRequest, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 95);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(309, 220);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // btnRequest
            // 
            btnRequest.Dock = DockStyle.Fill;
            btnRequest.Enabled = false;
            btnRequest.Location = new Point(3, 157);
            btnRequest.Name = "btnRequest";
            btnRequest.Size = new Size(303, 60);
            btnRequest.TabIndex = 1;
            btnRequest.Text = "Request file";
            btnRequest.UseVisualStyleBackColor = true;
            btnRequest.Click += btnRequest_Click;
            // 
            // lblIP
            // 
            lblIP.AutoSize = true;
            lblIP.Dock = DockStyle.Fill;
            lblIP.Location = new Point(3, 0);
            lblIP.Name = "lblIP";
            lblIP.Size = new Size(303, 44);
            lblIP.TabIndex = 4;
            lblIP.Text = "Path to File (relative to <none>)";
            lblIP.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rtbRequest
            // 
            rtbRequest.DetectUrls = false;
            rtbRequest.Dock = DockStyle.Fill;
            rtbRequest.Enabled = false;
            rtbRequest.Location = new Point(3, 47);
            rtbRequest.Name = "rtbRequest";
            rtbRequest.Size = new Size(303, 104);
            rtbRequest.TabIndex = 0;
            rtbRequest.Text = "";
            rtbRequest.TextChanged += rtbRequest_TextChanged;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(mtbIP, 0, 1);
            tableLayoutPanel3.Controls.Add(label1, 0, 0);
            tableLayoutPanel3.Controls.Add(btnConnect, 0, 2);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 39.53489F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 40.69767F));
            tableLayoutPanel3.Size = new Size(309, 86);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // mtbIP
            // 
            mtbIP.AllowPromptAsInput = false;
            mtbIP.Anchor = AnchorStyles.None;
            mtbIP.BeepOnError = true;
            mtbIP.Culture = new System.Globalization.CultureInfo("");
            mtbIP.Location = new Point(77, 22);
            mtbIP.Mask = "990.990.990.990";
            mtbIP.Name = "mtbIP";
            mtbIP.Size = new Size(155, 23);
            mtbIP.TabIndex = 0;
            mtbIP.TextAlign = HorizontalAlignment.Center;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(303, 17);
            label1.TabIndex = 1;
            label1.Text = "IP Address";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnConnect
            // 
            btnConnect.Dock = DockStyle.Fill;
            btnConnect.Location = new Point(3, 53);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(303, 30);
            btnConnect.TabIndex = 1;
            btnConnect.Text = "Connect";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // rtbResults
            // 
            rtbResults.BackColor = SystemColors.Window;
            rtbResults.Dock = DockStyle.Fill;
            rtbResults.Location = new Point(324, 3);
            rtbResults.Name = "rtbResults";
            rtbResults.ReadOnly = true;
            rtbResults.Size = new Size(316, 318);
            rtbResults.TabIndex = 1;
            rtbResults.Text = "";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(643, 324);
            Controls.Add(tableLayoutPanel1);
            MaximizeBox = false;
            MaximumSize = new Size(659, 363);
            MinimumSize = new Size(659, 363);
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Very Simple Web Client";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private MaskedTextBox mtbIP;
        private RichTextBox rtbResults;
        private Label label1;
        private Button btnConnect;
        private TableLayoutPanel tableLayoutPanel4;
        private Label lblIP;
        private RichTextBox rtbRequest;
        private Button btnRequest;
    }
}