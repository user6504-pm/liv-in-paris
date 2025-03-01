using System;

namespace Test
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        private void InitializeComponent()
        {
            this.panelGraphe = new System.Windows.Forms.Panel();
            this.SuspendLayout();

            this.panelGraphe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraphe.Location = new System.Drawing.Point(0, 0);
            this.panelGraphe.Name = "panelGraphe";
            this.panelGraphe.Size = new System.Drawing.Size(800, 450);
            this.panelGraphe.TabIndex = 0;
      
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelGraphe);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGraphe;
    }
}

