
namespace Linkedin_Data_Clean
{
    partial class Form1
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
            this.TxtDirectory = new System.Windows.Forms.TextBox();
            this.BtnBuscar = new System.Windows.Forms.Button();
            this.GbCleanData = new System.Windows.Forms.GroupBox();
            this.BtnCleanClientes = new System.Windows.Forms.Button();
            this.GbCleanData.SuspendLayout();
            this.SuspendLayout();
            // 
            // TxtDirectory
            // 
            this.TxtDirectory.Location = new System.Drawing.Point(12, 415);
            this.TxtDirectory.Name = "TxtDirectory";
            this.TxtDirectory.Size = new System.Drawing.Size(695, 23);
            this.TxtDirectory.TabIndex = 0;
            // 
            // BtnBuscar
            // 
            this.BtnBuscar.Location = new System.Drawing.Point(713, 414);
            this.BtnBuscar.Name = "BtnBuscar";
            this.BtnBuscar.Size = new System.Drawing.Size(75, 23);
            this.BtnBuscar.TabIndex = 1;
            this.BtnBuscar.Text = "Buscar";
            this.BtnBuscar.UseVisualStyleBackColor = true;
            this.BtnBuscar.Click += new System.EventHandler(this.BtnBuscar_Click);
            // 
            // GbCleanData
            // 
            this.GbCleanData.Controls.Add(this.BtnCleanClientes);
            this.GbCleanData.Location = new System.Drawing.Point(12, 12);
            this.GbCleanData.Name = "GbCleanData";
            this.GbCleanData.Size = new System.Drawing.Size(325, 249);
            this.GbCleanData.TabIndex = 2;
            this.GbCleanData.TabStop = false;
            this.GbCleanData.Text = "Clean Data";
            // 
            // BtnCleanClientes
            // 
            this.BtnCleanClientes.Location = new System.Drawing.Point(6, 22);
            this.BtnCleanClientes.Name = "BtnCleanClientes";
            this.BtnCleanClientes.Size = new System.Drawing.Size(75, 23);
            this.BtnCleanClientes.TabIndex = 2;
            this.BtnCleanClientes.Text = "Clientes";
            this.BtnCleanClientes.UseVisualStyleBackColor = true;
            this.BtnCleanClientes.Click += new System.EventHandler(this.BtnCleanClientes_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GbCleanData);
            this.Controls.Add(this.BtnBuscar);
            this.Controls.Add(this.TxtDirectory);
            this.Name = "Form1";
            this.Text = "Form1";
            this.GbCleanData.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtDirectory;
        private System.Windows.Forms.Button BtnBuscar;
        private System.Windows.Forms.GroupBox GbCleanData;
        private System.Windows.Forms.Button BtnCleanClientes;
    }
}

