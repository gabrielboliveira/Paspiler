namespace PascalCompiler
{
    partial class InitialForm
    {
	// Testando commit
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
            this._codeTextBox = new System.Windows.Forms.RichTextBox();
            this.gridViewTokens = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTokens)).BeginInit();
            this.SuspendLayout();
            // 
            // _codeTextBox
            // 
            this._codeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._codeTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._codeTextBox.Location = new System.Drawing.Point(12, 12);
            this._codeTextBox.Name = "_codeTextBox";
            this._codeTextBox.Size = new System.Drawing.Size(568, 458);
            this._codeTextBox.TabIndex = 1;
            this._codeTextBox.Text = "";
            this._codeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._codeTextBox_KeyDown);
            this._codeTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._codeTextBox_KeyPress);
            // 
            // gridViewTokens
            // 
            this.gridViewTokens.AllowUserToAddRows = false;
            this.gridViewTokens.AllowUserToDeleteRows = false;
            this.gridViewTokens.AllowUserToOrderColumns = true;
            this.gridViewTokens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewTokens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewTokens.Location = new System.Drawing.Point(586, 13);
            this.gridViewTokens.Name = "gridViewTokens";
            this.gridViewTokens.ReadOnly = true;
            this.gridViewTokens.RowHeadersVisible = false;
            this.gridViewTokens.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridViewTokens.Size = new System.Drawing.Size(206, 457);
            this.gridViewTokens.TabIndex = 2;
            // 
            // InitialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 482);
            this.Controls.Add(this.gridViewTokens);
            this.Controls.Add(this._codeTextBox);
            this.Name = "InitialForm";
            this.Text = "Paspiler";
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTokens)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox _codeTextBox;
        private System.Windows.Forms.DataGridView gridViewTokens;

    }
}

