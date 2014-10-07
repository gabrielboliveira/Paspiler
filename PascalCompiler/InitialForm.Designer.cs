﻿namespace PascalCompiler
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
            this.gridViewValidTokens = new System.Windows.Forms.DataGridView();
            this.gridViewNotValidTokens = new System.Windows.Forms.DataGridView();
            this.gridViewErrors = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewValidTokens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewNotValidTokens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // _codeTextBox
            // 
            this._codeTextBox.AcceptsTab = true;
            this._codeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._codeTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._codeTextBox.Location = new System.Drawing.Point(12, 12);
            this._codeTextBox.MinimumSize = new System.Drawing.Size(300, 300);
            this._codeTextBox.Name = "_codeTextBox";
            this._codeTextBox.Size = new System.Drawing.Size(905, 544);
            this._codeTextBox.TabIndex = 1;
            this._codeTextBox.Text = "program teste;\nvar oi:integer;\nbegin\n\toi := 1;\n\tif oi = 1 then\n\tbegin\n\t\toi := 2;\n" +
    "\tend;\nend.";
            // 
            // gridViewValidTokens
            // 
            this.gridViewValidTokens.AllowUserToAddRows = false;
            this.gridViewValidTokens.AllowUserToDeleteRows = false;
            this.gridViewValidTokens.AllowUserToOrderColumns = true;
            this.gridViewValidTokens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewValidTokens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewValidTokens.Location = new System.Drawing.Point(505, 12);
            this.gridViewValidTokens.Name = "gridViewValidTokens";
            this.gridViewValidTokens.ReadOnly = true;
            this.gridViewValidTokens.RowHeadersVisible = false;
            this.gridViewValidTokens.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridViewValidTokens.Size = new System.Drawing.Size(203, 543);
            this.gridViewValidTokens.TabIndex = 2;
            this.gridViewValidTokens.Visible = false;
            // 
            // gridViewNotValidTokens
            // 
            this.gridViewNotValidTokens.AllowUserToAddRows = false;
            this.gridViewNotValidTokens.AllowUserToDeleteRows = false;
            this.gridViewNotValidTokens.AllowUserToOrderColumns = true;
            this.gridViewNotValidTokens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewNotValidTokens.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewNotValidTokens.Location = new System.Drawing.Point(714, 12);
            this.gridViewNotValidTokens.Name = "gridViewNotValidTokens";
            this.gridViewNotValidTokens.ReadOnly = true;
            this.gridViewNotValidTokens.RowHeadersVisible = false;
            this.gridViewNotValidTokens.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridViewNotValidTokens.Size = new System.Drawing.Size(203, 543);
            this.gridViewNotValidTokens.TabIndex = 3;
            this.gridViewNotValidTokens.Visible = false;
            // 
            // gridViewErrors
            // 
            this.gridViewErrors.AllowUserToAddRows = false;
            this.gridViewErrors.AllowUserToDeleteRows = false;
            this.gridViewErrors.AllowUserToOrderColumns = true;
            this.gridViewErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridViewErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewErrors.Location = new System.Drawing.Point(12, 392);
            this.gridViewErrors.Name = "gridViewErrors";
            this.gridViewErrors.ReadOnly = true;
            this.gridViewErrors.RowHeadersVisible = false;
            this.gridViewErrors.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridViewErrors.Size = new System.Drawing.Size(905, 163);
            this.gridViewErrors.TabIndex = 4;
            this.gridViewErrors.Visible = false;
            // 
            // InitialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 568);
            this.Controls.Add(this.gridViewErrors);
            this.Controls.Add(this.gridViewNotValidTokens);
            this.Controls.Add(this.gridViewValidTokens);
            this.Controls.Add(this._codeTextBox);
            this.Name = "InitialForm";
            this.Text = "Paspiler";
            ((System.ComponentModel.ISupportInitialize)(this.gridViewValidTokens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewNotValidTokens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox _codeTextBox;
        private System.Windows.Forms.DataGridView gridViewValidTokens;
        private System.Windows.Forms.DataGridView gridViewNotValidTokens;
        private System.Windows.Forms.DataGridView gridViewErrors;

    }
}

