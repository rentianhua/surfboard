namespace HiiP.Framework.Security.UserManagement
{
    partial class UserMaintenance
    {
        /// <summary>
        /// The presenter used by this view.
        /// </summary>
        private HiiP.Framework.Security.UserManagement.UserMaintenancePresenter _presenter = null;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_presenter != null)
                    _presenter.Dispose();

                if (components != null)
                    components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("User ID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Status");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Created On");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("User Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Gender");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Title");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Email");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Telephone No");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Mobile");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Office");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AllOffices");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("First Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Last Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Master Office", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Select", 1);
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel11 = new Infragistics.Win.Misc.UltraLabel();
            this.dt_createdto = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.txt_email = new HiiP.Framework.Common.Client.ExtendedUltraTextEditor();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            this.txt_display = new HiiP.Framework.Common.Client.ExtendedUltraTextEditor();
            this.ultraLabel10 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.cb_userType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.txt_username = new HiiP.Framework.Common.Client.ExtendedUltraTextEditor();
            this.dt_createdfrom = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.txt_office = new HiiP.Framework.Common.Client.ExtendedUltraTextEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cb_status = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_reset = new Infragistics.Win.Misc.UltraButton();
            this.btn_search = new Infragistics.Win.Misc.UltraButton();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.closeButton = new Infragistics.Win.Misc.UltraButton();
            this.btn_disable = new Infragistics.Win.Misc.UltraButton();
            this.btn_assignroles = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonSelect = new Infragistics.Win.Misc.UltraButton();
            this.ug_userlist = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.lbl_recordCount = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel9 = new Infragistics.Win.Misc.UltraLabel();
            this.userListUltraDataSource = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt_createdto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_email)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_display)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_userType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_username)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_createdfrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_office)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_status)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ug_userlist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userListUltraDataSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.tableLayoutPanel1);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(978, 144);
            this.ultraGroupBox1.TabIndex = 0;
            this.ultraGroupBox1.Text = "Search criteria";
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel11, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.dt_createdto, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.txt_email, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel8, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txt_display, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel10, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel5, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.cb_userType, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel4, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.txt_username, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dt_createdfrom, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txt_office, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cb_status, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.ultraLabel2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 18);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(972, 123);
            this.tableLayoutPanel1.TabIndex = 22;
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance12.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance12.TextHAlignAsString = "Right";
            this.ultraLabel3.Appearance = appearance12;
            this.ultraLabel3.Location = new System.Drawing.Point(18, 35);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(89, 19);
            this.ultraLabel3.TabIndex = 6;
            this.ultraLabel3.Text = "Created from";
            // 
            // ultraLabel11
            // 
            this.ultraLabel11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance9.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.TextHAlignAsString = "Right";
            this.ultraLabel11.Appearance = appearance9;
            this.ultraLabel11.Location = new System.Drawing.Point(346, 65);
            this.ultraLabel11.Name = "ultraLabel11";
            this.ultraLabel11.Size = new System.Drawing.Size(89, 19);
            this.ultraLabel11.TabIndex = 14;
            this.ultraLabel11.Text = "User type";
            // 
            // dt_createdto
            // 
            this.dt_createdto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dt_createdto.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.dt_createdto.Location = new System.Drawing.Point(441, 33);
            this.dt_createdto.MaskInput = "{date}";
            this.dt_createdto.Name = "dt_createdto";
            this.dt_createdto.Size = new System.Drawing.Size(163, 23);
            this.dt_createdto.TabIndex = 9;
            this.dt_createdto.Value = null;
            // 
            // txt_email
            // 
            this.txt_email.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_email.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txt_email.Location = new System.Drawing.Point(762, 33);
            this.txt_email.MaxLength = 128;
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(202, 23);
            this.txt_email.TabIndex = 11;
            // 
            // ultraLabel8
            // 
            this.ultraLabel8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance8.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance8.TextHAlignAsString = "Right";
            this.ultraLabel8.Appearance = appearance8;
            this.ultraLabel8.Location = new System.Drawing.Point(18, 65);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(89, 19);
            this.ultraLabel8.TabIndex = 12;
            this.ultraLabel8.Text = "Office";
            // 
            // txt_display
            // 
            this.txt_display.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_display.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txt_display.Location = new System.Drawing.Point(762, 3);
            this.txt_display.MaxLength = 256;
            this.txt_display.Name = "txt_display";
            this.txt_display.Size = new System.Drawing.Size(203, 23);
            this.txt_display.TabIndex = 5;
            // 
            // ultraLabel10
            // 
            this.ultraLabel10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance1.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.TextHAlignAsString = "Right";
            this.ultraLabel10.Appearance = appearance1;
            this.ultraLabel10.Location = new System.Drawing.Point(350, 33);
            this.ultraLabel10.Name = "ultraLabel10";
            this.ultraLabel10.Size = new System.Drawing.Size(85, 24);
            this.ultraLabel10.TabIndex = 8;
            this.ultraLabel10.Text = "To";
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance5.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance5.TextHAlignAsString = "Right";
            this.ultraLabel5.Appearance = appearance5;
            this.ultraLabel5.Location = new System.Drawing.Point(663, 35);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(93, 19);
            this.ultraLabel5.TabIndex = 10;
            this.ultraLabel5.Text = "Email";
            // 
            // cb_userType
            // 
            this.cb_userType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cb_userType.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.cb_userType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cb_userType.Location = new System.Drawing.Point(441, 63);
            this.cb_userType.Name = "cb_userType";
            this.cb_userType.Size = new System.Drawing.Size(163, 23);
            this.cb_userType.SyncWithCurrencyManager = false;
            this.cb_userType.TabIndex = 15;
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance2.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.TextHAlignAsString = "Right";
            this.ultraLabel4.Appearance = appearance2;
            this.ultraLabel4.Location = new System.Drawing.Point(663, 5);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(93, 19);
            this.ultraLabel4.TabIndex = 4;
            this.ultraLabel4.Text = "Display name";
            // 
            // txt_username
            // 
            this.txt_username.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_username.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txt_username.Location = new System.Drawing.Point(113, 3);
            this.txt_username.MaxLength = 128;
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(212, 23);
            this.txt_username.TabIndex = 1;
            // 
            // dt_createdfrom
            // 
            this.dt_createdfrom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.dt_createdfrom.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.dt_createdfrom.Location = new System.Drawing.Point(113, 33);
            this.dt_createdfrom.MaskInput = "{date}";
            this.dt_createdfrom.Name = "dt_createdfrom";
            this.dt_createdfrom.Size = new System.Drawing.Size(163, 23);
            this.dt_createdfrom.TabIndex = 7;
            this.dt_createdfrom.Value = null;
            // 
            // txt_office
            // 
            this.txt_office.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txt_office.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.txt_office.Location = new System.Drawing.Point(113, 63);
            this.txt_office.MaxLength = 64;
            this.txt_office.Name = "txt_office";
            this.txt_office.Size = new System.Drawing.Size(212, 23);
            this.txt_office.TabIndex = 13;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance7.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.TextHAlignAsString = "Right";
            this.ultraLabel1.Appearance = appearance7;
            this.ultraLabel1.Location = new System.Drawing.Point(18, 5);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ultraLabel1.Size = new System.Drawing.Size(89, 19);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "User ID";
            // 
            // cb_status
            // 
            this.cb_status.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cb_status.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.cb_status.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cb_status.Location = new System.Drawing.Point(441, 3);
            this.cb_status.Name = "cb_status";
            this.cb_status.Size = new System.Drawing.Size(163, 23);
            this.cb_status.SyncWithCurrencyManager = false;
            this.cb_status.TabIndex = 3;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            appearance4.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.TextHAlignAsString = "Right";
            this.ultraLabel2.Appearance = appearance4;
            this.ultraLabel2.Location = new System.Drawing.Point(350, 3);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(85, 24);
            this.ultraLabel2.TabIndex = 2;
            this.ultraLabel2.Text = "Status";
            // 
            // flowLayoutPanel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 6);
            this.flowLayoutPanel1.Controls.Add(this.btn_reset);
            this.flowLayoutPanel1.Controls.Add(this.btn_search);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 93);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(966, 30);
            this.flowLayoutPanel1.TabIndex = 16;
            // 
            // btn_reset
            // 
            this.btn_reset.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btn_reset.Location = new System.Drawing.Point(876, 3);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(87, 25);
            this.btn_reset.TabIndex = 21;
            this.btn_reset.Text = "&Reset";
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btn_search
            // 
            this.btn_search.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btn_search.Location = new System.Drawing.Point(783, 3);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(87, 25);
            this.btn_search.TabIndex = 20;
            this.btn_search.Text = "&Search";
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.flowLayoutPanel);
            this.ultraGroupBox2.Controls.Add(this.ug_userlist);
            this.ultraGroupBox2.Controls.Add(this.lbl_recordCount);
            this.ultraGroupBox2.Controls.Add(this.ultraLabel9);
            this.ultraGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox2.Location = new System.Drawing.Point(0, 144);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(978, 369);
            this.ultraGroupBox2.TabIndex = 1;
            this.ultraGroupBox2.Text = "Search result";
            this.ultraGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel.Controls.Add(this.closeButton);
            this.flowLayoutPanel.Controls.Add(this.btn_disable);
            this.flowLayoutPanel.Controls.Add(this.btn_assignroles);
            this.flowLayoutPanel.Controls.Add(this.ultraButtonSelect);
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel.Location = new System.Drawing.Point(3, 335);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(972, 31);
            this.flowLayoutPanel.TabIndex = 3;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.closeButton.Location = new System.Drawing.Point(882, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(87, 25);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // btn_disable
            // 
            this.btn_disable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_disable.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btn_disable.Enabled = false;
            this.btn_disable.Location = new System.Drawing.Point(789, 3);
            this.btn_disable.Name = "btn_disable";
            this.btn_disable.Size = new System.Drawing.Size(87, 25);
            this.btn_disable.TabIndex = 2;
            this.btn_disable.Text = "&Disable";
            this.btn_disable.Click += new System.EventHandler(this.btn_disable_Click);
            // 
            // btn_assignroles
            // 
            this.btn_assignroles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_assignroles.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btn_assignroles.Enabled = false;
            this.btn_assignroles.Location = new System.Drawing.Point(678, 3);
            this.btn_assignroles.Name = "btn_assignroles";
            this.btn_assignroles.Size = new System.Drawing.Size(105, 25);
            this.btn_assignroles.TabIndex = 1;
            this.btn_assignroles.Text = "&Assign roles";
            this.btn_assignroles.Click += new System.EventHandler(this.ultraButton3_Click);
            // 
            // ultraButtonSelect
            // 
            this.ultraButtonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButtonSelect.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.ultraButtonSelect.Location = new System.Drawing.Point(567, 3);
            this.ultraButtonSelect.Name = "ultraButtonSelect";
            this.ultraButtonSelect.Size = new System.Drawing.Size(105, 25);
            this.ultraButtonSelect.TabIndex = 0;
            this.ultraButtonSelect.Text = "Se&lect";
            this.ultraButtonSelect.Visible = false;
            this.ultraButtonSelect.Click += new System.EventHandler(this.ultraButtonSelect_Click);
            // 
            // ug_userlist
            // 
            this.ug_userlist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ug_userlist.DisplayLayout.Appearance = appearance3;
            this.ug_userlist.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn1.Header.VisiblePosition = 1;
            ultraGridColumn1.RowLayoutColumnInfo.OriginX = 2;
            ultraGridColumn1.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn1.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn1.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn2.Header.VisiblePosition = 2;
            ultraGridColumn2.RowLayoutColumnInfo.OriginX = 4;
            ultraGridColumn2.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn2.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn2.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn3.Format = "G";
            ultraGridColumn3.Header.Caption = "Created on";
            ultraGridColumn3.Header.VisiblePosition = 3;
            ultraGridColumn3.RowLayoutColumnInfo.OriginX = 6;
            ultraGridColumn3.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn3.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(132, 0);
            ultraGridColumn3.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn3.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn4.Header.Caption = "Display name";
            ultraGridColumn4.Header.VisiblePosition = 4;
            ultraGridColumn4.RowLayoutColumnInfo.OriginX = 8;
            ultraGridColumn4.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn4.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn4.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn5.Header.VisiblePosition = 5;
            ultraGridColumn5.RowLayoutColumnInfo.OriginX = 10;
            ultraGridColumn5.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn5.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn5.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn6.Header.VisiblePosition = 6;
            ultraGridColumn6.RowLayoutColumnInfo.OriginX = 12;
            ultraGridColumn6.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn6.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(103, 0);
            ultraGridColumn6.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn6.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn7.Header.VisiblePosition = 7;
            ultraGridColumn7.RowLayoutColumnInfo.OriginX = 14;
            ultraGridColumn7.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn7.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn7.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn8.Header.Caption = "Telephone No.";
            ultraGridColumn8.Header.VisiblePosition = 8;
            ultraGridColumn8.RowLayoutColumnInfo.OriginX = 16;
            ultraGridColumn8.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn8.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn8.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn9.Header.VisiblePosition = 9;
            ultraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn10.Header.Caption = "Default office";
            ultraGridColumn10.Header.VisiblePosition = 11;
            ultraGridColumn10.RowLayoutColumnInfo.OriginX = 24;
            ultraGridColumn10.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn10.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn10.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn11.Header.Caption = "All offices";
            ultraGridColumn11.Header.VisiblePosition = 12;
            ultraGridColumn12.Header.VisiblePosition = 13;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn13.Header.VisiblePosition = 14;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn14.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn14.Header.Caption = "Master office";
            ultraGridColumn14.Header.VisiblePosition = 10;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn14.RowLayoutColumnInfo.OriginX = 22;
            ultraGridColumn14.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn14.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn14.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn15.DataType = typeof(bool);
            ultraGridColumn15.DefaultCellValue = false;
            ultraGridColumn15.Header.Caption = "";
            ultraGridColumn15.Header.VisiblePosition = 0;
            ultraGridColumn15.RowLayoutColumnInfo.OriginX = 0;
            ultraGridColumn15.RowLayoutColumnInfo.OriginY = 0;
            ultraGridColumn15.RowLayoutColumnInfo.PreferredCellSize = new System.Drawing.Size(33, 0);
            ultraGridColumn15.RowLayoutColumnInfo.SpanX = 2;
            ultraGridColumn15.RowLayoutColumnInfo.SpanY = 2;
            ultraGridColumn15.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            ultraGridColumn15.Width = 45;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15});
            this.ug_userlist.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ug_userlist.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ug_userlist.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance6.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            this.ug_userlist.DisplayLayout.GroupByBox.Appearance = appearance6;
            appearance25.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ug_userlist.DisplayLayout.GroupByBox.BandLabelAppearance = appearance25;
            this.ug_userlist.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance26.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance26.BackColor2 = System.Drawing.SystemColors.Control;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance26.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ug_userlist.DisplayLayout.GroupByBox.PromptAppearance = appearance26;
            this.ug_userlist.DisplayLayout.MaxColScrollRegions = 1;
            this.ug_userlist.DisplayLayout.MaxRowScrollRegions = 1;
            appearance27.BackColor = System.Drawing.SystemColors.Window;
            appearance27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ug_userlist.DisplayLayout.Override.ActiveCellAppearance = appearance27;
            appearance28.BackColor = System.Drawing.SystemColors.Highlight;
            appearance28.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ug_userlist.DisplayLayout.Override.ActiveRowAppearance = appearance28;
            this.ug_userlist.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ug_userlist.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ug_userlist.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ug_userlist.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ug_userlist.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            this.ug_userlist.DisplayLayout.Override.CardAreaAppearance = appearance29;
            appearance30.BorderColor = System.Drawing.Color.Silver;
            appearance30.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ug_userlist.DisplayLayout.Override.CellAppearance = appearance30;
            this.ug_userlist.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ug_userlist.DisplayLayout.Override.CellPadding = 0;
            appearance31.BackColor = System.Drawing.SystemColors.Control;
            appearance31.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance31.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance31.BorderColor = System.Drawing.SystemColors.Window;
            this.ug_userlist.DisplayLayout.Override.GroupByRowAppearance = appearance31;
            appearance32.TextHAlignAsString = "Left";
            this.ug_userlist.DisplayLayout.Override.HeaderAppearance = appearance32;
            this.ug_userlist.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ug_userlist.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance33.BackColor = System.Drawing.SystemColors.Window;
            appearance33.BorderColor = System.Drawing.Color.Silver;
            this.ug_userlist.DisplayLayout.Override.RowAppearance = appearance33;
            this.ug_userlist.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ug_userlist.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.ug_userlist.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ug_userlist.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
            appearance34.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ug_userlist.DisplayLayout.Override.TemplateAddRowAppearance = appearance34;
            this.ug_userlist.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ug_userlist.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ug_userlist.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this.ug_userlist.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ug_userlist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ug_userlist.Location = new System.Drawing.Point(12, 52);
            this.ug_userlist.Name = "ug_userlist";
            this.ug_userlist.Size = new System.Drawing.Size(957, 272);
            this.ug_userlist.TabIndex = 2;
            this.ug_userlist.DoubleClickCell += new Infragistics.Win.UltraWinGrid.DoubleClickCellEventHandler(this.ug_userlist_DoubleClickCell);
            this.ug_userlist.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ug_userlist_CellChange);
            this.ug_userlist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ug_userlist_KeyDown);
            // 
            // lbl_recordCount
            // 
            appearance10.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            this.lbl_recordCount.Appearance = appearance10;
            this.lbl_recordCount.Location = new System.Drawing.Point(118, 31);
            this.lbl_recordCount.Name = "lbl_recordCount";
            this.lbl_recordCount.Size = new System.Drawing.Size(73, 25);
            this.lbl_recordCount.TabIndex = 1;
            this.lbl_recordCount.Text = "0";
            // 
            // ultraLabel9
            // 
            appearance11.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            this.ultraLabel9.Appearance = appearance11;
            this.ultraLabel9.Location = new System.Drawing.Point(12, 31);
            this.ultraLabel9.Name = "ultraLabel9";
            this.ultraLabel9.Size = new System.Drawing.Size(93, 25);
            this.ultraLabel9.TabIndex = 0;
            this.ultraLabel9.Text = "Total record(s):";
            // 
            // UserMaintenance
            // 
            this.AcceptButton = this.btn_search;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(754, 345);
            this.AutoSize = true;
            this.CancelButton = this.closeButton;
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.ultraGroupBox1);
            this.Key = "SearchUser";
            this.Name = "UserMaintenance";
            this.Size = new System.Drawing.Size(978, 513);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dt_createdto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_email)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_display)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_userType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_username)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_createdfrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_office)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_status)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            this.flowLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ug_userlist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userListUltraDataSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private HiiP.Framework.Common.Client.ExtendedUltraTextEditor txt_office;
        private HiiP.Framework.Common.Client.ExtendedUltraTextEditor txt_display;
        private HiiP.Framework.Common.Client.ExtendedUltraTextEditor txt_username;
        private HiiP.Framework.Common.Client.ExtendedUltraTextEditor txt_email;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dt_createdfrom;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cb_status;
        private Infragistics.Win.Misc.UltraButton btn_reset;
        private Infragistics.Win.Misc.UltraButton btn_search;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.Misc.UltraButton btn_assignroles;
        private Infragistics.Win.Misc.UltraButton btn_disable;
        private Infragistics.Win.Misc.UltraLabel lbl_recordCount;
        private Infragistics.Win.Misc.UltraLabel ultraLabel9;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource userListUltraDataSource;
        private Infragistics.Win.UltraWinGrid.UltraGrid ug_userlist;
        private Infragistics.Win.Misc.UltraLabel ultraLabel11;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cb_userType;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dt_createdto;
        private Infragistics.Win.Misc.UltraLabel ultraLabel10;
        private Infragistics.Win.Misc.UltraButton closeButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private Infragistics.Win.Misc.UltraButton ultraButtonSelect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}

