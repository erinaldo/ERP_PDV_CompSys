namespace PDV.VIEW.Forms.Cadastro.Suprimentos
{
    partial class FCA_ConversaoUM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCA_ConversaoUM));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ovCMB_UnidadeEntrada = new MetroFramework.Controls.MetroComboBox();
            this.ovCMB_UnidadeSaida = new MetroFramework.Controls.MetroComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ovTXT_Fator = new PDV.UTIL.Components.Custom.EditDecimal();
            this.button2 = new System.Windows.Forms.Button();
            this.ovTXT_Produto = new System.Windows.Forms.MaskedTextBox();
            this.ovTXT_CodProduto = new System.Windows.Forms.MaskedTextBox();
            this.metroButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.metroButton4 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ovTXT_Fator)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label1.Location = new System.Drawing.Point(23, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "* Produto:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label2.Location = new System.Drawing.Point(23, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "* UN. Entrada:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label3.Location = new System.Drawing.Point(189, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 49;
            this.label3.Text = "* UN. Saida:";
            // 
            // ovCMB_UnidadeEntrada
            // 
            this.ovCMB_UnidadeEntrada.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ovCMB_UnidadeEntrada.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.ovCMB_UnidadeEntrada.FormattingEnabled = true;
            this.ovCMB_UnidadeEntrada.ItemHeight = 19;
            this.ovCMB_UnidadeEntrada.Location = new System.Drawing.Point(26, 87);
            this.ovCMB_UnidadeEntrada.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ovCMB_UnidadeEntrada.Name = "ovCMB_UnidadeEntrada";
            this.ovCMB_UnidadeEntrada.Size = new System.Drawing.Size(127, 25);
            this.ovCMB_UnidadeEntrada.TabIndex = 3;
            this.ovCMB_UnidadeEntrada.UseSelectable = true;
            this.ovCMB_UnidadeEntrada.SelectedIndexChanged += new System.EventHandler(this.ovCMB_UnidadeEntrada_SelectedIndexChanged);
            // 
            // ovCMB_UnidadeSaida
            // 
            this.ovCMB_UnidadeSaida.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ovCMB_UnidadeSaida.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.ovCMB_UnidadeSaida.FormattingEnabled = true;
            this.ovCMB_UnidadeSaida.ItemHeight = 19;
            this.ovCMB_UnidadeSaida.Location = new System.Drawing.Point(192, 87);
            this.ovCMB_UnidadeSaida.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ovCMB_UnidadeSaida.Name = "ovCMB_UnidadeSaida";
            this.ovCMB_UnidadeSaida.Size = new System.Drawing.Size(127, 25);
            this.ovCMB_UnidadeSaida.TabIndex = 4;
            this.ovCMB_UnidadeSaida.UseSelectable = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label4.Location = new System.Drawing.Point(23, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "* Fator:";
            // 
            // ovTXT_Fator
            // 
            this.ovTXT_Fator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ovTXT_Fator.DecimalPlaces = 4;
            this.ovTXT_Fator.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ovTXT_Fator.Location = new System.Drawing.Point(26, 145);
            this.ovTXT_Fator.Maximum = new decimal(new int[] {
            -1530494977,
            232830,
            0,
            262144});
            this.ovTXT_Fator.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ovTXT_Fator.Name = "ovTXT_Fator";
            this.ovTXT_Fator.Sigla = "FT";
            this.ovTXT_Fator.Size = new System.Drawing.Size(226, 21);
            this.ovTXT_Fator.TabIndex = 5;
            this.ovTXT_Fator.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ovTXT_Fator.ThousandsSeparator = true;
            this.ovTXT_Fator.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.ovTXT_Fator.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ovTXT_Fator.vdValorDecimal = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ovTXT_Fator.viPrecisao = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ovTXT_Fator.viTamanho = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.button2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.Location = new System.Drawing.Point(149, 28);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(30, 22);
            this.button2.TabIndex = 2;
            this.button2.Text = "...";
            this.button2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ovTXT_Produto
            // 
            this.ovTXT_Produto.Enabled = false;
            this.ovTXT_Produto.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ovTXT_Produto.Location = new System.Drawing.Point(192, 28);
            this.ovTXT_Produto.Name = "ovTXT_Produto";
            this.ovTXT_Produto.ReadOnly = true;
            this.ovTXT_Produto.Size = new System.Drawing.Size(280, 21);
            this.ovTXT_Produto.TabIndex = 127;
            // 
            // ovTXT_CodProduto
            // 
            this.ovTXT_CodProduto.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.ovTXT_CodProduto.Location = new System.Drawing.Point(26, 28);
            this.ovTXT_CodProduto.Name = "ovTXT_CodProduto";
            this.ovTXT_CodProduto.Size = new System.Drawing.Size(120, 21);
            this.ovTXT_CodProduto.TabIndex = 1;
            this.ovTXT_CodProduto.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ovTXT_CodProduto_KeyUp);
            // 
            // metroButton3
            // 
            this.metroButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButton3.Appearance.ForeColor = System.Drawing.Color.Black;
            this.metroButton3.Appearance.Options.UseForeColor = true;
            this.metroButton3.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("metroButton3.ImageOptions.Image")));
            this.metroButton3.Location = new System.Drawing.Point(384, 216);
            this.metroButton3.Name = "metroButton3";
            this.metroButton3.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.metroButton3.Size = new System.Drawing.Size(88, 33);
            this.metroButton3.TabIndex = 129;
            this.metroButton3.Text = "Salvar";
            this.metroButton3.Click += new System.EventHandler(this.metroButton3_Click);
            // 
            // metroButton4
            // 
            this.metroButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroButton4.Appearance.ForeColor = System.Drawing.Color.Black;
            this.metroButton4.Appearance.Options.UseForeColor = true;
            this.metroButton4.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("metroButton4.ImageOptions.Image")));
            this.metroButton4.Location = new System.Drawing.Point(290, 216);
            this.metroButton4.Name = "metroButton4";
            this.metroButton4.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.metroButton4.Size = new System.Drawing.Size(88, 33);
            this.metroButton4.TabIndex = 128;
            this.metroButton4.Text = "Cancelar";
            this.metroButton4.Click += new System.EventHandler(this.metroButton4_Click);
            // 
            // FCA_ConversaoUM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.metroButton3);
            this.Controls.Add(this.metroButton4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ovTXT_Produto);
            this.Controls.Add(this.ovTXT_CodProduto);
            this.Controls.Add(this.ovTXT_Fator);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ovCMB_UnidadeSaida);
            this.Controls.Add(this.ovCMB_UnidadeEntrada);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 309);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(486, 293);
            this.Name = "FCA_ConversaoUM";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cadastro de Conversão de Unidade de Medida";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FCA_ConversaoUM_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ovTXT_Fator)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private MetroFramework.Controls.MetroComboBox ovCMB_UnidadeEntrada;
        private MetroFramework.Controls.MetroComboBox ovCMB_UnidadeSaida;
        private System.Windows.Forms.Label label4;
        private UTIL.Components.Custom.EditDecimal ovTXT_Fator;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.MaskedTextBox ovTXT_Produto;
        private System.Windows.Forms.MaskedTextBox ovTXT_CodProduto;
        private DevExpress.XtraEditors.SimpleButton metroButton3;
        private DevExpress.XtraEditors.SimpleButton metroButton4;
    }
}