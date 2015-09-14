
//----------------------------------------------------------------------------------------
// patterns & practices - Smart Client Software Factory - Guidance Package
//
// This file was generated by the "Add View" recipe.
//
// For more information see: 
// ms-help://MS.VSCC.v90/MS.VSIPCC.v90/ms.practices.scsf.2008apr/SCSF/html/02-09-010-ModelViewPresenter_MVP.htm
//
// Latest version of this Guidance Package: http://go.microsoft.com/fwlink/?LinkId=62182
//----------------------------------------------------------------------------------------

namespace HiiP.Framework.Settings
{
    partial class InstrumentationFilterView
    {
        /// <summary>
        /// The presenter used by this view.
        /// </summary>
        private HiiP.Framework.Settings.InstrumentationFilterViewPresenter _presenter = null;

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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraOptionSet = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ultraComboEditorUserName = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.UserName = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.btn_cancel = new Infragistics.Win.Misc.UltraButton();
            this.btn_ok = new Infragistics.Win.Misc.UltraButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.validationProvider1 = new Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraOptionSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditorUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.ultraLabel1);
            this.ultraGroupBox1.Controls.Add(this.ultraOptionSet);
            this.ultraGroupBox1.Controls.Add(this.ultraComboEditorUserName);
            this.ultraGroupBox1.Controls.Add(this.UserName);
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(297, 102);
            this.ultraGroupBox1.TabIndex = 0;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // ultraLabel1
            // 
            appearance13.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance13.TextHAlignAsString = "Right";
            this.ultraLabel1.Appearance = appearance13;
            this.ultraLabel1.Location = new System.Drawing.Point(10, 20);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(81, 23);
            this.ultraLabel1.TabIndex = 3;
            this.ultraLabel1.Text = "* Category";
            // 
            // ultraOptionSet
            // 
            appearance2.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            this.ultraOptionSet.Appearance = appearance2;
            this.ultraOptionSet.BackColor = System.Drawing.Color.Transparent;
            this.ultraOptionSet.BackColorInternal = System.Drawing.Color.Transparent;
            this.ultraOptionSet.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraOptionSet.GlyphStyle = Infragistics.Win.GlyphStyle.Office2007;
            valueListItem1.DataValue = "Instrumentation";
            valueListItem1.DisplayText = "Instrumentation";
            valueListItem2.DataValue = "Monitoring";
            valueListItem2.DisplayText = "Performance";
            this.ultraOptionSet.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.ultraOptionSet.Location = new System.Drawing.Point(99, 15);
            this.ultraOptionSet.Name = "ultraOptionSet";
            this.ultraOptionSet.Size = new System.Drawing.Size(143, 33);
            this.ultraOptionSet.TabIndex = 2;
            // 
            // ultraComboEditorUserName
            // 
            this.ultraComboEditorUserName.DisplayMember = "";
            this.ultraComboEditorUserName.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this.ultraComboEditorUserName.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraComboEditorUserName.Location = new System.Drawing.Point(99, 62);
            this.ultraComboEditorUserName.Name = "ultraComboEditorUserName";
            this.validationProvider1.SetPerformValidation(this.ultraComboEditorUserName, true);
            this.ultraComboEditorUserName.Size = new System.Drawing.Size(180, 23);
            this.validationProvider1.SetSourcePropertyName(this.ultraComboEditorUserName, "UserName");
            this.ultraComboEditorUserName.SyncWithCurrencyManager = false;
            this.ultraComboEditorUserName.TabIndex = 1;
            this.ultraComboEditorUserName.ValueMember = "";
            // 
            // UserName
            // 
            appearance1.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.TextHAlignAsString = "Right";
            this.UserName.Appearance = appearance1;
            this.UserName.Location = new System.Drawing.Point(10, 66);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(81, 23);
            this.UserName.TabIndex = 0;
            this.UserName.Text = "* User name";
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.btn_cancel);
            this.ultraGroupBox2.Controls.Add(this.btn_ok);
            this.ultraGroupBox2.Location = new System.Drawing.Point(0, 100);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(297, 41);
            this.ultraGroupBox2.TabIndex = 1;
            this.ultraGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // btn_cancel
            // 
            this.btn_cancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btn_cancel.Location = new System.Drawing.Point(204, 8);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 25);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Text = "&Cancel";
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.ButtonStyle = Infragistics.Win.UIElementButtonStyle.WindowsVistaButton;
            this.btn_ok.Location = new System.Drawing.Point(123, 8);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 25);
            this.btn_ok.TabIndex = 0;
            this.btn_ok.Text = "&Save";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // validationProvider1
            // 
            this.validationProvider1.ErrorProvider = this.errorProvider1;
            this.validationProvider1.RulesetName = "User Info Operation Rule Set";
            this.validationProvider1.SourceTypeName = "HiiP.Framework.Security.UserManagement.BusinessEntity.UserInfoEntity,HiiP.Framewo" +
                "rk.Security.UserManagement.BusinessEntity";
            this.validationProvider1.ValueConvert += new System.EventHandler<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.ValueConvertEventArgs>(this.validationProvider1_ValueConvert);
            // 
            // InstrumentationFilterView
            // 
            this.AcceptButton = this.btn_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.CheckDirtyRequired = true;
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "InstrumentationFilterView";
            this.Size = new System.Drawing.Size(299, 141);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraOptionSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditorUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.Misc.UltraLabel UserName;
        private Infragistics.Win.Misc.UltraButton btn_ok;
        private Infragistics.Win.Misc.UltraButton btn_cancel;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraComboEditorUserName;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider validationProvider1;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet ultraOptionSet;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
    }
}

