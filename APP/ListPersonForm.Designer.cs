﻿
namespace SpeakRec
{
    partial class ListPersonForm
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
            this.listViewPerson = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listViewPerson
            // 
            this.listViewPerson.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.listViewPerson.HideSelection = false;
            this.listViewPerson.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.listViewPerson.Location = new System.Drawing.Point(12, 12);
            this.listViewPerson.Name = "listViewPerson";
            this.listViewPerson.Size = new System.Drawing.Size(776, 426);
            this.listViewPerson.TabIndex = 0;
            this.listViewPerson.UseCompatibleStateImageBehavior = false;
            this.listViewPerson.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewPerson_ItemCheck);
            // 
            // ListPersonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listViewPerson);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ListPersonForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Danh sách";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ListPersonForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewPerson;
    }
}