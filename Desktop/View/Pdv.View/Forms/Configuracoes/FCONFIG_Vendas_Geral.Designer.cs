namespace PDV.VIEW.Forms.Configuracoes
{
    partial class FCONFIG_Vendas_Geral
    {
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
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCONFIG_Vendas_Geral));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ovRB_Valor = new System.Windows.Forms.RadioButton();
            this.ovRB_Percentual = new System.Windows.Forms.RadioButton();
            this.label29 = new System.Windows.Forms.Label();
            this.ovTXT_CHAVE_ATENTICACAO = new System.Windows.Forms.TextBox();
            this.metroButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.metroButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.gridLookUpEditTipoDeOperacao = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tipoOperacaoPadraoPDVGridLookUpEdit = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEditTipoDeOperacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tipoOperacaoPadraoPDVGridLookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ovRB_Valor);
            this.groupBox2.Controls.Add(this.ovRB_Percentual);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(3, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(479, 45);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Desconto";
            // 
            // ovRB_Valor
            // 
            this.ovRB_Valor.AutoSize = true;
            this.ovRB_Valor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ovRB_Valor.Location = new System.Drawing.Point(145, 13);
            this.ovRB_Valor.Name = "ovRB_Valor";
            this.ovRB_Valor.Size = new System.Drawing.Size(73, 17);
            this.ovRB_Valor.TabIndex = 4;
            this.ovRB_Valor.Text = "(R$) Valor";
            this.ovRB_Valor.UseVisualStyleBackColor = true;
            // 
            // ovRB_Percentual
            // 
            this.ovRB_Percentual.AutoSize = true;
            this.ovRB_Percentual.Checked = true;
            this.ovRB_Percentual.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ovRB_Percentual.Location = new System.Drawing.Point(29, 13);
            this.ovRB_Percentual.Name = "ovRB_Percentual";
            this.ovRB_Percentual.Size = new System.Drawing.Size(110, 17);
            this.ovRB_Percentual.TabIndex = 3;
            this.ovRB_Percentual.TabStop = true;
            this.ovRB_Percentual.Text = "(%) Porcentagem";
            this.ovRB_Percentual.UseVisualStyleBackColor = true;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(0, 72);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(89, 13);
            this.label29.TabIndex = 40;
            this.label29.Text = "Tef Autenticação";
            // 
            // ovTXT_CHAVE_ATENTICACAO
            // 
            this.ovTXT_CHAVE_ATENTICACAO.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ovTXT_CHAVE_ATENTICACAO.Location = new System.Drawing.Point(3, 88);
            this.ovTXT_CHAVE_ATENTICACAO.Name = "ovTXT_CHAVE_ATENTICACAO";
            this.ovTXT_CHAVE_ATENTICACAO.Size = new System.Drawing.Size(469, 21);
            this.ovTXT_CHAVE_ATENTICACAO.TabIndex = 41;
            // 
            // metroButton4
            // 
            this.metroButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButton4.Appearance.ForeColor = System.Drawing.Color.Black;
            this.metroButton4.Appearance.Options.UseForeColor = true;
            this.metroButton4.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("metroButton4.ImageOptions.Image")));
            this.metroButton4.Location = new System.Drawing.Point(408, 216);
            this.metroButton4.Name = "metroButton4";
            this.metroButton4.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.metroButton4.Size = new System.Drawing.Size(64, 33);
            this.metroButton4.TabIndex = 119;
            this.metroButton4.Text = "Salvar";
            this.metroButton4.Click += new System.EventHandler(this.metroButton4_Click);
            // 
            // metroButton5
            // 
            this.metroButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButton5.Appearance.ForeColor = System.Drawing.Color.Black;
            this.metroButton5.Appearance.Options.UseForeColor = true;
            this.metroButton5.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("metroButton5.ImageOptions.Image")));
            this.metroButton5.Location = new System.Drawing.Point(327, 216);
            this.metroButton5.Name = "metroButton5";
            this.metroButton5.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.metroButton5.Size = new System.Drawing.Size(75, 33);
            this.metroButton5.TabIndex = 118;
            this.metroButton5.Text = "Cancelar";
            this.metroButton5.Click += new System.EventHandler(this.metroButton5_Click);
            // 
            // gridLookUpEditTipoDeOperacao
            // 
            this.gridLookUpEditTipoDeOperacao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLookUpEditTipoDeOperacao.EditValue = "";
            this.gridLookUpEditTipoDeOperacao.Location = new System.Drawing.Point(3, 128);
            this.gridLookUpEditTipoDeOperacao.Name = "gridLookUpEditTipoDeOperacao";
            this.gridLookUpEditTipoDeOperacao.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridLookUpEditTipoDeOperacao.Properties.Appearance.Options.UseFont = true;
            this.gridLookUpEditTipoDeOperacao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpEditTipoDeOperacao.Properties.DisplayMember = "Nome";
            this.gridLookUpEditTipoDeOperacao.Properties.PopupView = this.gridView3;
            this.gridLookUpEditTipoDeOperacao.Properties.ValueMember = "Cod";
            this.gridLookUpEditTipoDeOperacao.Size = new System.Drawing.Size(469, 20);
            this.gridLookUpEditTipoDeOperacao.TabIndex = 120;
            // 
            // gridView3
            // 
            this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 121;
            this.label1.Text = "Tipo de Operação Padrão do APP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 13);
            this.label2.TabIndex = 123;
            this.label2.Text = "Tipo de Operação Padrão do PDV";
            // 
            // tipoOperacaoPadraoPDVGridLookUpEdit
            // 
            this.tipoOperacaoPadraoPDVGridLookUpEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tipoOperacaoPadraoPDVGridLookUpEdit.EditValue = "";
            this.tipoOperacaoPadraoPDVGridLookUpEdit.Location = new System.Drawing.Point(3, 170);
            this.tipoOperacaoPadraoPDVGridLookUpEdit.Name = "tipoOperacaoPadraoPDVGridLookUpEdit";
            this.tipoOperacaoPadraoPDVGridLookUpEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tipoOperacaoPadraoPDVGridLookUpEdit.Properties.Appearance.Options.UseFont = true;
            this.tipoOperacaoPadraoPDVGridLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.tipoOperacaoPadraoPDVGridLookUpEdit.Properties.DisplayMember = "Nome";
            this.tipoOperacaoPadraoPDVGridLookUpEdit.Properties.PopupView = this.gridView1;
            this.tipoOperacaoPadraoPDVGridLookUpEdit.Properties.ValueMember = "Cod";
            this.tipoOperacaoPadraoPDVGridLookUpEdit.Size = new System.Drawing.Size(469, 20);
            this.tipoOperacaoPadraoPDVGridLookUpEdit.TabIndex = 122;
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // FCONFIG_Vendas_Geral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tipoOperacaoPadraoPDVGridLookUpEdit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridLookUpEditTipoDeOperacao);
            this.Controls.Add(this.metroButton4);
            this.Controls.Add(this.metroButton5);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.ovTXT_CHAVE_ATENTICACAO);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCONFIG_Vendas_Geral";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geral";
            this.Load += new System.EventHandler(this.FCONFIG_Vendas_Geral_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEditTipoDeOperacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tipoOperacaoPadraoPDVGridLookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton ovRB_Valor;
        private System.Windows.Forms.RadioButton ovRB_Percentual;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox ovTXT_CHAVE_ATENTICACAO;
        private DevExpress.XtraEditors.SimpleButton metroButton4;
        private DevExpress.XtraEditors.SimpleButton metroButton5;
        private DevExpress.XtraEditors.GridLookUpEdit gridLookUpEditTipoDeOperacao;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.GridLookUpEdit tipoOperacaoPadraoPDVGridLookUpEdit;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}