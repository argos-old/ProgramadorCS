namespace ProgramadorCS
{
    partial class FormEjecuciones
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
            this.textBoxProceso = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.botonBuscar = new DevComponents.DotNetBar.ButtonX();
            this.botonAceptar = new DevComponents.DotNetBar.ButtonX();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.botonProbar = new DevComponents.DotNetBar.ButtonX();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxProceso
            // 
            // 
            // 
            // 
            this.textBoxProceso.Border.Class = "TextBoxBorder";
            this.textBoxProceso.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxProceso.ButtonCustom.Image = global::ProgramadorCS.Properties.Resources.broom16;
            this.textBoxProceso.ButtonCustom.Visible = true;
            this.textBoxProceso.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProceso.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxProceso.Location = new System.Drawing.Point(10, 10);
            this.textBoxProceso.Name = "textBoxProceso";
            this.textBoxProceso.Size = new System.Drawing.Size(349, 23);
            this.textBoxProceso.TabIndex = 0;
            this.textBoxProceso.ButtonCustomClick += new System.EventHandler(this.botonAuxTextBoxLimpiar_Click);
            this.textBoxProceso.TextChanged += new System.EventHandler(this.textBoxProceso_TextChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.textBoxProceso);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 10, 5, 5);
            this.panel1.Size = new System.Drawing.Size(451, 38);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.botonBuscar);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(359, 10);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.panel2.Size = new System.Drawing.Size(87, 23);
            this.panel2.TabIndex = 1;
            // 
            // botonBuscar
            // 
            this.botonBuscar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.botonBuscar.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.botonBuscar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.botonBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonBuscar.Location = new System.Drawing.Point(5, 0);
            this.botonBuscar.Name = "botonBuscar";
            this.botonBuscar.Size = new System.Drawing.Size(77, 23);
            this.botonBuscar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.botonBuscar.Symbol = "";
            this.botonBuscar.SymbolSize = 14F;
            this.botonBuscar.TabIndex = 0;
            this.botonBuscar.Text = "&Buscar";
            this.botonBuscar.Click += new System.EventHandler(this.botonBuscar_Click);
            // 
            // botonAceptar
            // 
            this.botonAceptar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.botonAceptar.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.botonAceptar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.botonAceptar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonAceptar.Location = new System.Drawing.Point(0, 0);
            this.botonAceptar.Name = "botonAceptar";
            this.botonAceptar.Size = new System.Drawing.Size(123, 29);
            this.botonAceptar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.botonAceptar.TabIndex = 2;
            this.botonAceptar.Text = "&Aceptar";
            this.botonAceptar.Click += new System.EventHandler(this.botonAceptar_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 50);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(10, 0, 5, 5);
            this.panel3.Size = new System.Drawing.Size(451, 34);
            this.panel3.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.botonProbar);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(10, 0);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.panel5.Size = new System.Drawing.Size(128, 29);
            this.panel5.TabIndex = 1;
            // 
            // botonProbar
            // 
            this.botonProbar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.botonProbar.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.botonProbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.botonProbar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.botonProbar.Location = new System.Drawing.Point(0, 0);
            this.botonProbar.Name = "botonProbar";
            this.botonProbar.Size = new System.Drawing.Size(118, 29);
            this.botonProbar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.botonProbar.TabIndex = 3;
            this.botonProbar.Text = "&Probar";
            this.botonProbar.Click += new System.EventHandler(this.botonProbar_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.botonAceptar);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(318, 0);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.panel4.Size = new System.Drawing.Size(128, 29);
            this.panel4.TabIndex = 0;
            // 
            // FormEjecuciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 84);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "FormEjecuciones";
            this.Text = "FormEjecuciones";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEjecuciones_FormClosing);
            this.Load += new System.EventHandler(this.FormEjecuciones_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX textBoxProceso;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private DevComponents.DotNetBar.ButtonX botonBuscar;
        private DevComponents.DotNetBar.ButtonX botonAceptar;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private DevComponents.DotNetBar.ButtonX botonProbar;
    }
}