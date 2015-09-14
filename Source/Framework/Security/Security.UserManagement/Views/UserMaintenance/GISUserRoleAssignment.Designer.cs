namespace HiiP.Framework.Security.UserManagement
{
    partial class GISUserRoleAssignment
    {

        /// <summary>
        /// The presenter used by this view.
        /// </summary>
        private HiiP.Framework.Security.UserManagement.GISUserRoleAssignmentPresenter  _presenter = null;

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
            if (disposing && (components != null))
            {
                components.Dispose();
                if (_ETHelper != null) _ETHelper.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("ETTable", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RoleID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RoleName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RoleType");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsSupervisor");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TickFlag", 0);
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
            Infragistics.Win.Appearance appearance222 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("ETTable", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RoleID");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RoleName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UserName");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("RoleType");
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IsSupervisor", -1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TickFlag", 0);
            Infragistics.Win.Appearance appearance239 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance262 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance263 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance264 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance265 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance266 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance267 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance282 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance283 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance284 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance285 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance220 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance221 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn7 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn8 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleName");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn9 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn10 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("UserName");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn11 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleType");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn12 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("IsSupervisor");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn13 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleName");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn14 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn15 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("UserName");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn16 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleType");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn17 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("IsSupervisor");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn18 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn19 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn20 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleName");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn21 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn22 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("UserName");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn23 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleType");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn24 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("IsSupervisor");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn1 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleID");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn2 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleName");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn3 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("Description");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn4 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("UserName");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn5 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("RoleType");
            Infragistics.Win.UltraWinDataSource.UltraDataColumn ultraDataColumn6 = new Infragistics.Win.UltraWinDataSource.UltraDataColumn("IsSupervisor");
            this.ultraGroupBox32 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraGridGISAssignedRoles = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraButtonGISUnassign = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonGISAssign = new Infragistics.Win.Misc.UltraButton();
            this.ultraGridGISAllRoles = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraLabel77 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTextEditorGISRole = new HiiP.Framework.Common.Client.ExtendedUltraTextEditor();
            this.ultraLabel76 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTextEditorGISDesc = new HiiP.Framework.Common.Client.ExtendedUltraTextEditor();
            this.ultraButtonGISSearch = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonGISReset = new Infragistics.Win.Misc.UltraButton();
            this.ultraGroupBox31 = new Infragistics.Win.Misc.UltraGroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ultraDataSource1 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraDataSource2 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraDataSource3 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            this.ultraDataSource4 = new Infragistics.Win.UltraWinDataSource.UltraDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox32)).BeginInit();
            this.ultraGroupBox32.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridGISAssignedRoles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridGISAllRoles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorGISRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorGISDesc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox31)).BeginInit();
            this.ultraGroupBox31.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource4)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox32
            // 
            this.ultraGroupBox32.Controls.Add(this.ultraGridGISAssignedRoles);
            this.ultraGroupBox32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox32.Location = new System.Drawing.Point(475, 3);
            this.ultraGroupBox32.Name = "ultraGroupBox32";
            this.tableLayoutPanel1.SetRowSpan(this.ultraGroupBox32, 5);
            this.ultraGroupBox32.Size = new System.Drawing.Size(397, 511);
            this.ultraGroupBox32.TabIndex = 7;
            this.ultraGroupBox32.Text = "Assigned roles";
            this.ultraGroupBox32.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // ultraGridGISAssignedRoles
            // 
            this.ultraGridGISAssignedRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGridGISAssignedRoles.DataSource = this.ultraDataSource4;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridGISAssignedRoles.DisplayLayout.Appearance = appearance1;
            ultraGridColumn1.Header.VisiblePosition = 6;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn2.Header.Caption = "Role name";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 226;
            ultraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 281;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn7.DataType = typeof(bool);
            ultraGridColumn7.Header.Caption = "";
            ultraGridColumn7.Header.VisiblePosition = 0;
            ultraGridColumn7.NullText = "DEL";
            ultraGridColumn7.Width = 29;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7});
            this.ultraGridGISAssignedRoles.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ultraGridGISAssignedRoles.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridGISAssignedRoles.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridGISAssignedRoles.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridGISAssignedRoles.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.ultraGridGISAssignedRoles.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridGISAssignedRoles.DisplayLayout.GroupByBox.Hidden = true;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridGISAssignedRoles.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.ultraGridGISAssignedRoles.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridGISAssignedRoles.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.CellAppearance = appearance8;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.RowAppearance = appearance11;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridGISAssignedRoles.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.ultraGridGISAssignedRoles.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridGISAssignedRoles.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridGISAssignedRoles.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ultraGridGISAssignedRoles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ultraGridGISAssignedRoles.Location = new System.Drawing.Point(3, 18);
            this.ultraGridGISAssignedRoles.Name = "ultraGridGISAssignedRoles";
            this.ultraGridGISAssignedRoles.Size = new System.Drawing.Size(388, 487);
            this.ultraGridGISAssignedRoles.TabIndex = 0;
            this.ultraGridGISAssignedRoles.Text = "ultraGrid11";
            // 
            // ultraButtonGISUnassign
            // 
            this.ultraButtonGISUnassign.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.ultraButtonGISUnassign.Location = new System.Drawing.Point(405, 239);
            this.ultraButtonGISUnassign.Name = "ultraButtonGISUnassign";
            this.ultraButtonGISUnassign.Size = new System.Drawing.Size(64, 25);
            this.ultraButtonGISUnassign.TabIndex = 6;
            this.ultraButtonGISUnassign.Text = "&Unassign";
            this.ultraButtonGISUnassign.Click += new System.EventHandler(this.ultraButtonGISUnassign_Click);
            // 
            // ultraButtonGISAssign
            // 
            this.ultraButtonGISAssign.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.ultraButtonGISAssign.Location = new System.Drawing.Point(405, 208);
            this.ultraButtonGISAssign.Name = "ultraButtonGISAssign";
            this.ultraButtonGISAssign.Size = new System.Drawing.Size(64, 25);
            this.ultraButtonGISAssign.TabIndex = 5;
            this.ultraButtonGISAssign.Text = "Ass&ign";
            this.ultraButtonGISAssign.Click += new System.EventHandler(this.ultraButtonGISAssign_Click);
            // 
            // ultraGridGISAllRoles
            // 
            this.ultraGridGISAllRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGridGISAllRoles.DataSource = this.ultraDataSource1;
            appearance222.BackColor = System.Drawing.SystemColors.Window;
            appearance222.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraGridGISAllRoles.DisplayLayout.Appearance = appearance222;
            ultraGridColumn8.Header.VisiblePosition = 0;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn9.Header.Caption = "Role name";
            ultraGridColumn9.Header.VisiblePosition = 2;
            ultraGridColumn9.Width = 226;
            ultraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn10.Header.VisiblePosition = 3;
            ultraGridColumn10.Width = 277;
            ultraGridColumn11.Header.VisiblePosition = 4;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn12.Header.VisiblePosition = 5;
            ultraGridColumn12.Hidden = true;
            ultraGridColumn13.Header.VisiblePosition = 6;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn14.DataType = typeof(bool);
            ultraGridColumn14.Header.Caption = "";
            ultraGridColumn14.Header.VisiblePosition = 1;
            ultraGridColumn14.Width = 27;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12,
            ultraGridColumn13,
            ultraGridColumn14});
            this.ultraGridGISAllRoles.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.ultraGridGISAllRoles.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridGISAllRoles.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance239.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance239.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance239.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance239.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridGISAllRoles.DisplayLayout.GroupByBox.Appearance = appearance239;
            appearance262.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridGISAllRoles.DisplayLayout.GroupByBox.BandLabelAppearance = appearance262;
            this.ultraGridGISAllRoles.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraGridGISAllRoles.DisplayLayout.GroupByBox.Hidden = true;
            appearance263.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance263.BackColor2 = System.Drawing.SystemColors.Control;
            appearance263.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance263.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraGridGISAllRoles.DisplayLayout.GroupByBox.PromptAppearance = appearance263;
            this.ultraGridGISAllRoles.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraGridGISAllRoles.DisplayLayout.MaxRowScrollRegions = 1;
            appearance264.BackColor = System.Drawing.SystemColors.Window;
            appearance264.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraGridGISAllRoles.DisplayLayout.Override.ActiveCellAppearance = appearance264;
            appearance265.BackColor = System.Drawing.SystemColors.Highlight;
            appearance265.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraGridGISAllRoles.DisplayLayout.Override.ActiveRowAppearance = appearance265;
            this.ultraGridGISAllRoles.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.ultraGridGISAllRoles.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.ultraGridGISAllRoles.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraGridGISAllRoles.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance266.BackColor = System.Drawing.SystemColors.Window;
            this.ultraGridGISAllRoles.DisplayLayout.Override.CardAreaAppearance = appearance266;
            appearance267.BorderColor = System.Drawing.Color.Silver;
            appearance267.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraGridGISAllRoles.DisplayLayout.Override.CellAppearance = appearance267;
            this.ultraGridGISAllRoles.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraGridGISAllRoles.DisplayLayout.Override.CellPadding = 0;
            appearance282.BackColor = System.Drawing.SystemColors.Control;
            appearance282.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance282.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance282.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance282.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraGridGISAllRoles.DisplayLayout.Override.GroupByRowAppearance = appearance282;
            appearance283.TextHAlignAsString = "Left";
            this.ultraGridGISAllRoles.DisplayLayout.Override.HeaderAppearance = appearance283;
            this.ultraGridGISAllRoles.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridGISAllRoles.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance284.BackColor = System.Drawing.SystemColors.Window;
            appearance284.BorderColor = System.Drawing.Color.Silver;
            this.ultraGridGISAllRoles.DisplayLayout.Override.RowAppearance = appearance284;
            this.ultraGridGISAllRoles.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance285.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraGridGISAllRoles.DisplayLayout.Override.TemplateAddRowAppearance = appearance285;
            this.ultraGridGISAllRoles.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridGISAllRoles.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridGISAllRoles.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.ultraGridGISAllRoles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ultraGridGISAllRoles.Location = new System.Drawing.Point(3, 125);
            this.ultraGridGISAllRoles.Name = "ultraGridGISAllRoles";
            this.ultraGridGISAllRoles.Size = new System.Drawing.Size(387, 380);
            this.ultraGridGISAllRoles.TabIndex = 6;
            this.ultraGridGISAllRoles.Text = "ultraGrid17";
            // 
            // ultraLabel77
            // 
            appearance220.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            this.ultraLabel77.Appearance = appearance220;
            this.ultraLabel77.AutoSize = true;
            this.ultraLabel77.Location = new System.Drawing.Point(19, 35);
            this.ultraLabel77.Name = "ultraLabel77";
            this.ultraLabel77.Size = new System.Drawing.Size(62, 16);
            this.ultraLabel77.TabIndex = 0;
            this.ultraLabel77.Text = "Role name";
            // 
            // ultraTextEditorGISRole
            // 
            this.ultraTextEditorGISRole.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ultraTextEditorGISRole.Location = new System.Drawing.Point(100, 31);
            this.ultraTextEditorGISRole.Name = "ultraTextEditorGISRole";
            this.ultraTextEditorGISRole.Size = new System.Drawing.Size(278, 23);
            this.ultraTextEditorGISRole.TabIndex = 1;
            // 
            // ultraLabel76
            // 
            appearance221.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            this.ultraLabel76.Appearance = appearance221;
            this.ultraLabel76.AutoSize = true;
            this.ultraLabel76.Location = new System.Drawing.Point(19, 66);
            this.ultraLabel76.Name = "ultraLabel76";
            this.ultraLabel76.Size = new System.Drawing.Size(65, 16);
            this.ultraLabel76.TabIndex = 2;
            this.ultraLabel76.Text = "Description";
            // 
            // ultraTextEditorGISDesc
            // 
            this.ultraTextEditorGISDesc.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ultraTextEditorGISDesc.Location = new System.Drawing.Point(100, 62);
            this.ultraTextEditorGISDesc.Name = "ultraTextEditorGISDesc";
            this.ultraTextEditorGISDesc.Size = new System.Drawing.Size(278, 23);
            this.ultraTextEditorGISDesc.TabIndex = 3;
            // 
            // ultraButtonGISSearch
            // 
            this.ultraButtonGISSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.ultraButtonGISSearch.Location = new System.Drawing.Point(194, 92);
            this.ultraButtonGISSearch.Name = "ultraButtonGISSearch";
            this.ultraButtonGISSearch.Size = new System.Drawing.Size(87, 25);
            this.ultraButtonGISSearch.TabIndex = 4;
            this.ultraButtonGISSearch.Text = "Search";
            this.ultraButtonGISSearch.Click += new System.EventHandler(this.ultraButtonGISSearch_Click);
            // 
            // ultraButtonGISReset
            // 
            this.ultraButtonGISReset.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.ultraButtonGISReset.Location = new System.Drawing.Point(290, 92);
            this.ultraButtonGISReset.Name = "ultraButtonGISReset";
            this.ultraButtonGISReset.Size = new System.Drawing.Size(87, 25);
            this.ultraButtonGISReset.TabIndex = 5;
            this.ultraButtonGISReset.Text = "Reset";
            this.ultraButtonGISReset.Click += new System.EventHandler(this.ultraButtonGISReset_Click);
            // 
            // ultraGroupBox31
            // 
            this.ultraGroupBox31.Controls.Add(this.ultraButtonGISReset);
            this.ultraGroupBox31.Controls.Add(this.ultraButtonGISSearch);
            this.ultraGroupBox31.Controls.Add(this.ultraTextEditorGISDesc);
            this.ultraGroupBox31.Controls.Add(this.ultraLabel76);
            this.ultraGroupBox31.Controls.Add(this.ultraTextEditorGISRole);
            this.ultraGroupBox31.Controls.Add(this.ultraLabel77);
            this.ultraGroupBox31.Controls.Add(this.ultraGridGISAllRoles);
            this.ultraGroupBox31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox31.Location = new System.Drawing.Point(3, 3);
            this.ultraGroupBox31.Name = "ultraGroupBox31";
            this.tableLayoutPanel1.SetRowSpan(this.ultraGroupBox31, 5);
            this.ultraGroupBox31.Size = new System.Drawing.Size(396, 511);
            this.ultraGroupBox31.TabIndex = 4;
            this.ultraGroupBox31.Text = "Available roles";
            this.ultraGroupBox31.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ultraGroupBox31, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ultraButtonGISUnassign, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.ultraGroupBox32, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.ultraButtonGISAssign, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(875, 517);
            this.tableLayoutPanel1.TabIndex = 38;
            // 
            // ultraDataSource1
            // 
            ultraDataColumn7.DataType = typeof(short);
            this.ultraDataSource1.Band.Columns.AddRange(new object[] {
            ultraDataColumn7,
            ultraDataColumn8,
            ultraDataColumn9,
            ultraDataColumn10,
            ultraDataColumn11,
            ultraDataColumn12});
            this.ultraDataSource1.Band.Key = "ETTable";
            // 
            // ultraDataSource2
            // 
            ultraDataColumn18.DataType = typeof(short);
            this.ultraDataSource2.Band.Columns.AddRange(new object[] {
            ultraDataColumn13,
            ultraDataColumn14,
            ultraDataColumn15,
            ultraDataColumn16,
            ultraDataColumn17,
            ultraDataColumn18});
            this.ultraDataSource2.Band.Key = "ETTable";
            // 
            // ultraDataSource3
            // 
            ultraDataColumn19.DataType = typeof(short);
            this.ultraDataSource3.Band.Columns.AddRange(new object[] {
            ultraDataColumn19,
            ultraDataColumn20,
            ultraDataColumn21,
            ultraDataColumn22,
            ultraDataColumn23,
            ultraDataColumn24});
            this.ultraDataSource3.Band.Key = "ETTable";
            // 
            // ultraDataSource4
            // 
            ultraDataColumn1.DataType = typeof(short);
            this.ultraDataSource4.Band.Columns.AddRange(new object[] {
            ultraDataColumn1,
            ultraDataColumn2,
            ultraDataColumn3,
            ultraDataColumn4,
            ultraDataColumn5,
            ultraDataColumn6});
            this.ultraDataSource4.Band.Key = "ETTable";
            // 
            // GISUserRoleAssignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GISUserRoleAssignment";
            this.Size = new System.Drawing.Size(875, 517);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox32)).EndInit();
            this.ultraGroupBox32.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridGISAssignedRoles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridGISAllRoles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorGISRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorGISDesc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox31)).EndInit();
            this.ultraGroupBox31.ResumeLayout(false);
            this.ultraGroupBox31.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDataSource4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox32;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridGISAssignedRoles;
        private Infragistics.Win.Misc.UltraButton ultraButtonGISUnassign;
        private Infragistics.Win.Misc.UltraButton ultraButtonGISAssign;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridGISAllRoles;
        private Infragistics.Win.Misc.UltraLabel ultraLabel77;
        private HiiP.Framework.Common.Client.ExtendedUltraTextEditor ultraTextEditorGISRole;
        private Infragistics.Win.Misc.UltraLabel ultraLabel76;
        private HiiP.Framework.Common.Client.ExtendedUltraTextEditor ultraTextEditorGISDesc;
        private Infragistics.Win.Misc.UltraButton ultraButtonGISSearch;
        private Infragistics.Win.Misc.UltraButton ultraButtonGISReset;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox31;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource4;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource1;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource2;
        private Infragistics.Win.UltraWinDataSource.UltraDataSource ultraDataSource3;
    }
}
