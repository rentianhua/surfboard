
//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by the "Add View" recipe.
//
// For more information see: 
// ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.scsf.2007may/SCSF/html/02-09-010-ModelViewPresenter_MVP.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

namespace HiiP.Framework.Security.SessionManagement
{
    partial class SessionManagementView
    {
        /// <summary>
        /// The presenter used by this view.
        /// </summary>
        private SessionManagementPresenter _presenter;

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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SessionID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FullName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StartTime");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastActivityTime");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("User Name");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IPAddress");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HostName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Select", 0);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            this.lbl_totalsessionnumber = new Infragistics.Win.Misc.UltraLabel();
            this.lbl_totalsessiontitle = new Infragistics.Win.Misc.UltraLabel();
            this.ug_sessionlist = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.SessionManagementLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraDataSource3 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.closeButton = new Infragistics.Win.Misc.UltraButton();
            this.refreshButton = new Infragistics.Win.Misc.UltraButton();
            this.killSessionButton = new Infragistics.Win.Misc.UltraButton();
            this.sessionUltraGroupBox = new Infragistics.Win.Misc.UltraGroupBox();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.ug_sessionlist)).BeginInit();
            this.SessionManagementLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionUltraGroupBox)).BeginInit();
            this.sessionUltraGroupBox.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_totalsessionnumber
            // 
            this.lbl_totalsessionnumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_totalsessionnumber.Location = new System.Drawing.Point(149, 3);
            this.lbl_totalsessionnumber.Name = "lbl_totalsessionnumber";
            this.lbl_totalsessionnumber.Size = new System.Drawing.Size(800, 16);
            this.lbl_totalsessionnumber.TabIndex = 1;
            this.lbl_totalsessionnumber.Text = "0";
            // 
            // lbl_totalsessiontitle
            // 
            this.lbl_totalsessiontitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_totalsessiontitle.Location = new System.Drawing.Point(9, 3);
            this.lbl_totalsessiontitle.Name = "lbl_totalsessiontitle";
            this.lbl_totalsessiontitle.Size = new System.Drawing.Size(134, 16);
            this.lbl_totalsessiontitle.TabIndex = 0;
            this.lbl_totalsessiontitle.Text = "Total active session(s):";
            // 
            // ug_sessionlist
            // 
            this.SessionManagementLayoutPanel.SetColumnSpan(this.ug_sessionlist, 2);
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ug_sessionlist.DisplayLayout.Appearance = appearance1;
            this.ug_sessionlist.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn1.Header.Caption = "Session ID";
            ultraGridColumn1.Header.VisiblePosition = 1;
            ultraGridColumn1.Width = 100;
            ultraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn2.Header.Caption = "User ID";
            ultraGridColumn2.Header.VisiblePosition = 2;
            ultraGridColumn2.Width = 120;
            ultraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn3.Header.Caption = "User name";
            ultraGridColumn3.Header.VisiblePosition = 3;
            ultraGridColumn3.Width = 130;
            ultraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn4.Format = "G";
            ultraGridColumn4.Header.Caption = "Start time";
            ultraGridColumn4.Header.VisiblePosition = 4;
            ultraGridColumn4.MaskInput = "{date} {time}";
            ultraGridColumn4.Width = 145;
            ultraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn5.Format = "G";
            ultraGridColumn5.Header.Caption = "Last activity time";
            ultraGridColumn5.Header.VisiblePosition = 5;
            ultraGridColumn5.MaskInput = "{date} {time}";
            ultraGridColumn5.Width = 143;
            ultraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn6.Header.Caption = "User name";
            ultraGridColumn6.Header.VisiblePosition = 6;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn6.Width = 106;
            ultraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn7.Header.Caption = "IP address";
            ultraGridColumn7.Header.VisiblePosition = 7;
            ultraGridColumn7.Width = 118;
            ultraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn8.Header.Caption = "Host name";
            ultraGridColumn8.Header.VisiblePosition = 8;
            ultraGridColumn8.Width = 67;
            ultraGridColumn9.DataType = typeof(bool);
            ultraGridColumn9.Header.Caption = "";
            ultraGridColumn9.Header.VisiblePosition = 0;
            ultraGridColumn9.Width = 23;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9});
            appearance2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            ultraGridBand1.Header.Appearance = appearance2;
            this.ug_sessionlist.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ug_sessionlist.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ug_sessionlist.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.ug_sessionlist.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ug_sessionlist.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.ug_sessionlist.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ug_sessionlist.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.ug_sessionlist.DisplayLayout.MaxColScrollRegions = 1;
            this.ug_sessionlist.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ug_sessionlist.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ug_sessionlist.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.ug_sessionlist.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ug_sessionlist.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ug_sessionlist.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.ug_sessionlist.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ug_sessionlist.DisplayLayout.Override.CellAppearance = appearance9;
            this.ug_sessionlist.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ug_sessionlist.DisplayLayout.Override.CellPadding = 0;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.BorderColor = System.Drawing.SystemColors.Window;
            this.ug_sessionlist.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.TextHAlignAsString = "Left";
            this.ug_sessionlist.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.ug_sessionlist.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ug_sessionlist.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.ug_sessionlist.DisplayLayout.Override.RowAppearance = appearance12;
            this.ug_sessionlist.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ug_sessionlist.DisplayLayout.Override.TemplateAddRowAppearance = appearance16;
            this.ug_sessionlist.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ug_sessionlist.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ug_sessionlist.DisplayLayout.TabNavigation = Infragistics.Win.UltraWinGrid.TabNavigation.NextControl;
            this.ug_sessionlist.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ug_sessionlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ug_sessionlist.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ug_sessionlist.Location = new System.Drawing.Point(9, 25);
            this.ug_sessionlist.Name = "ug_sessionlist";
            this.ug_sessionlist.Size = new System.Drawing.Size(940, 432);
            this.ug_sessionlist.TabIndex = 1;
            this.ug_sessionlist.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ug_sessionlist_KeyDown);
            // 
            // SessionManagementLayoutPanel
            // 
            this.SessionManagementLayoutPanel.AutoScroll = true;
            this.SessionManagementLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.SessionManagementLayoutPanel.ColumnCount = 4;
            this.SessionManagementLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.SessionManagementLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.SessionManagementLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SessionManagementLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.SessionManagementLayoutPanel.Controls.Add(this.ug_sessionlist, 1, 1);
            this.SessionManagementLayoutPanel.Controls.Add(this.lbl_totalsessionnumber, 2, 0);
            this.SessionManagementLayoutPanel.Controls.Add(this.lbl_totalsessiontitle, 1, 0);
            this.SessionManagementLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionManagementLayoutPanel.Location = new System.Drawing.Point(3, 0);
            this.SessionManagementLayoutPanel.Name = "SessionManagementLayoutPanel";
            this.SessionManagementLayoutPanel.RowCount = 2;
            this.SessionManagementLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.SessionManagementLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SessionManagementLayoutPanel.Size = new System.Drawing.Size(958, 460);
            this.SessionManagementLayoutPanel.TabIndex = 0;
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(197, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(87, 25);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "&Close";
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Location = new System.Drawing.Point(11, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(87, 25);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "&Refresh";
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // killSessionButton
            // 
            this.killSessionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.killSessionButton.Location = new System.Drawing.Point(104, 3);
            this.killSessionButton.Name = "killSessionButton";
            this.killSessionButton.Size = new System.Drawing.Size(87, 25);
            this.killSessionButton.TabIndex = 2;
            this.killSessionButton.Text = "&Kill session";
            this.killSessionButton.Click += new System.EventHandler(this.killSessionButton_Click);
            // 
            // sessionUltraGroupBox
            // 
            this.sessionUltraGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sessionUltraGroupBox.Controls.Add(this.SessionManagementLayoutPanel);
            this.sessionUltraGroupBox.Location = new System.Drawing.Point(5, 9);
            this.sessionUltraGroupBox.Name = "sessionUltraGroupBox";
            this.sessionUltraGroupBox.Size = new System.Drawing.Size(964, 463);
            this.sessionUltraGroupBox.TabIndex = 0;
            this.sessionUltraGroupBox.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel.Controls.Add(this.closeButton);
            this.flowLayoutPanel.Controls.Add(this.killSessionButton);
            this.flowLayoutPanel.Controls.Add(this.refreshButton);
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel.Location = new System.Drawing.Point(679, 475);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(287, 36);
            this.flowLayoutPanel.TabIndex = 4;
            // 
            // SessionManagementView
            // 
            this.AcceptButton = this.refreshButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.closeButton;
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.sessionUltraGroupBox);
            this.Key = "SessionMaintenance";
            this.Name = "SessionManagementView";
            this.Size = new System.Drawing.Size(978, 513);
            ((System.ComponentModel.ISupportInitialize)(this.ug_sessionlist)).EndInit();
            this.SessionManagementLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sessionUltraGroupBox)).EndInit();
            this.sessionUltraGroupBox.ResumeLayout(false);
            this.flowLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lbl_totalsessionnumber;
        private Infragistics.Win.Misc.UltraLabel lbl_totalsessiontitle;
        private Infragistics.Win.UltraWinGrid.UltraGrid ug_sessionlist;
        private System.Windows.Forms.TableLayoutPanel SessionManagementLayoutPanel;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource3;
        private Infragistics.Win.Misc.UltraButton killSessionButton;
        private Infragistics.Win.Misc.UltraButton refreshButton;
        private Infragistics.Win.Misc.UltraButton closeButton;
        private Infragistics.Win.Misc.UltraGroupBox sessionUltraGroupBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;


    }
}

