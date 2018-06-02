namespace AvisosRep
{
    partial class FormAvisos
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.expandablePanel1 = new DevComponents.DotNetBar.ExpandablePanel();
            this.botonControlVolumen = new DevComponents.DotNetBar.ButtonX();
            this.sliderVolumen = new DevComponents.DotNetBar.Controls.Slider();
            this.botonParada = new DevComponents.DotNetBar.ButtonX();
            this.botonReproducir = new DevComponents.DotNetBar.ButtonX();
            this.panelArriba = new System.Windows.Forms.Panel();
            this.labelMarquesina = new System.Windows.Forms.Label();
            this.panelAbajo = new System.Windows.Forms.Panel();
            this.labelPie = new System.Windows.Forms.Label();
            this.panelIzda = new System.Windows.Forms.Panel();
            this.panelDcha = new System.Windows.Forms.Panel();
            this.labelCuerpo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.botonReproducirAvisoVisual = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.timerFechaHora = new System.Windows.Forms.Timer(this.components);
            this.timerMarquesinaMovil = new System.Windows.Forms.Timer(this.components);
            this.timerConmutaColores = new System.Windows.Forms.Timer(this.components);
            this.expandablePanel1.SuspendLayout();
            this.panelArriba.SuspendLayout();
            this.panelAbajo.SuspendLayout();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2007Black;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154))))));
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.expandablePanel1.Controls.Add(this.botonControlVolumen);
            this.expandablePanel1.Controls.Add(this.sliderVolumen);
            this.expandablePanel1.Controls.Add(this.botonParada);
            this.expandablePanel1.Controls.Add(this.botonReproducir);
            this.expandablePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.expandablePanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.expandablePanel1.HideControlsWhenCollapsed = true;
            this.expandablePanel1.Location = new System.Drawing.Point(0, 308);
            this.expandablePanel1.Name = "expandablePanel1";
            this.expandablePanel1.Size = new System.Drawing.Size(591, 132);
            this.expandablePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.expandablePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandablePanel1.Style.GradientAngle = 90;
            this.expandablePanel1.TabIndex = 1;
            this.expandablePanel1.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.expandablePanel1.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.TitleStyle.GradientAngle = 90;
            this.expandablePanel1.TitleText = "Sonido";
            // 
            // botonControlVolumen
            // 
            this.botonControlVolumen.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.botonControlVolumen.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.botonControlVolumen.Location = new System.Drawing.Point(12, 43);
            this.botonControlVolumen.Name = "botonControlVolumen";
            this.botonControlVolumen.Size = new System.Drawing.Size(40, 21);
            this.botonControlVolumen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.botonControlVolumen.Symbol = "";
            this.botonControlVolumen.SymbolSize = 12F;
            this.botonControlVolumen.TabIndex = 11;
            this.botonControlVolumen.Click += new System.EventHandler(this.botonControlVolumen_Click);
            // 
            // sliderVolumen
            // 
            this.sliderVolumen.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.sliderVolumen.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.sliderVolumen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sliderVolumen.LabelWidth = 55;
            this.sliderVolumen.Location = new System.Drawing.Point(58, 43);
            this.sliderVolumen.Name = "sliderVolumen";
            this.sliderVolumen.Size = new System.Drawing.Size(517, 21);
            this.sliderVolumen.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.sliderVolumen.TabIndex = 10;
            this.sliderVolumen.Text = "Volumen";
            this.sliderVolumen.Value = 0;
            this.sliderVolumen.ValueChanged += new System.EventHandler(this.sliderVolumen_ValueChanged);
            // 
            // botonParada
            // 
            this.botonParada.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.botonParada.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.botonParada.Location = new System.Drawing.Point(329, 80);
            this.botonParada.Name = "botonParada";
            this.botonParada.Size = new System.Drawing.Size(246, 40);
            this.botonParada.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.botonParada.Symbol = "";
            this.botonParada.SymbolSize = 18F;
            this.botonParada.TabIndex = 9;
            this.botonParada.Click += new System.EventHandler(this.botonParada_Click);
            // 
            // botonReproducir
            // 
            this.botonReproducir.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.botonReproducir.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.botonReproducir.Location = new System.Drawing.Point(12, 80);
            this.botonReproducir.Name = "botonReproducir";
            this.botonReproducir.Size = new System.Drawing.Size(253, 40);
            this.botonReproducir.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.botonReproducir.Symbol = "";
            this.botonReproducir.SymbolSize = 18F;
            this.botonReproducir.TabIndex = 7;
            this.botonReproducir.Click += new System.EventHandler(this.botonReproducir_Click);
            // 
            // panelArriba
            // 
            this.panelArriba.Controls.Add(this.labelMarquesina);
            this.panelArriba.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelArriba.Location = new System.Drawing.Point(27, 0);
            this.panelArriba.Name = "panelArriba";
            this.panelArriba.Size = new System.Drawing.Size(537, 38);
            this.panelArriba.TabIndex = 10;
            // 
            // labelMarquesina
            // 
            this.labelMarquesina.AutoSize = true;
            this.labelMarquesina.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMarquesina.Location = new System.Drawing.Point(190, 9);
            this.labelMarquesina.Name = "labelMarquesina";
            this.labelMarquesina.Size = new System.Drawing.Size(156, 23);
            this.labelMarquesina.TabIndex = 0;
            this.labelMarquesina.Text = "esto es una prueba";
            this.labelMarquesina.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelAbajo
            // 
            this.panelAbajo.Controls.Add(this.labelPie);
            this.panelAbajo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAbajo.Location = new System.Drawing.Point(27, 271);
            this.panelAbajo.Name = "panelAbajo";
            this.panelAbajo.Size = new System.Drawing.Size(537, 37);
            this.panelAbajo.TabIndex = 9;
            // 
            // labelPie
            // 
            this.labelPie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPie.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPie.Location = new System.Drawing.Point(0, 0);
            this.labelPie.Name = "labelPie";
            this.labelPie.Size = new System.Drawing.Size(537, 37);
            this.labelPie.TabIndex = 1;
            this.labelPie.Text = "Pie";
            this.labelPie.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelIzda
            // 
            this.panelIzda.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIzda.Location = new System.Drawing.Point(0, 0);
            this.panelIzda.Name = "panelIzda";
            this.panelIzda.Size = new System.Drawing.Size(27, 308);
            this.panelIzda.TabIndex = 8;
            // 
            // panelDcha
            // 
            this.panelDcha.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelDcha.Location = new System.Drawing.Point(564, 0);
            this.panelDcha.Name = "panelDcha";
            this.panelDcha.Size = new System.Drawing.Size(27, 308);
            this.panelDcha.TabIndex = 7;
            // 
            // labelCuerpo
            // 
            this.labelCuerpo.BackColor = System.Drawing.Color.Transparent;
            this.labelCuerpo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCuerpo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCuerpo.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCuerpo.Location = new System.Drawing.Point(27, 38);
            this.labelCuerpo.Name = "labelCuerpo";
            this.labelCuerpo.Size = new System.Drawing.Size(537, 233);
            this.labelCuerpo.TabIndex = 6;
            this.labelCuerpo.Text = "Cuerpo";
            this.labelCuerpo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(537, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "Pie";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // botonReproducirAvisoVisual
            // 
            this.botonReproducirAvisoVisual.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.botonReproducirAvisoVisual.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.botonReproducirAvisoVisual.Location = new System.Drawing.Point(12, 80);
            this.botonReproducirAvisoVisual.Name = "botonReproducirAvisoVisual";
            this.botonReproducirAvisoVisual.Size = new System.Drawing.Size(152, 40);
            this.botonReproducirAvisoVisual.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.botonReproducirAvisoVisual.Symbol = "";
            this.botonReproducirAvisoVisual.SymbolSize = 18F;
            this.botonReproducirAvisoVisual.TabIndex = 7;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(216, 80);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(152, 40);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.Symbol = "";
            this.buttonX1.SymbolSize = 18F;
            this.buttonX1.TabIndex = 8;
            // 
            // timerFechaHora
            // 
            this.timerFechaHora.Interval = 1000;
            this.timerFechaHora.Tick += new System.EventHandler(this.timerFechaHora_Tick);
            // 
            // timerMarquesinaMovil
            // 
            this.timerMarquesinaMovil.Interval = 30;
            this.timerMarquesinaMovil.Tick += new System.EventHandler(this.timerMarquesinaMovil_Tick);
            // 
            // timerConmutaColores
            // 
            this.timerConmutaColores.Interval = 1000;
            this.timerConmutaColores.Tick += new System.EventHandler(this.timerConmutaColores_Tick);
            // 
            // FormAvisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 440);
            this.Controls.Add(this.labelCuerpo);
            this.Controls.Add(this.panelArriba);
            this.Controls.Add(this.panelAbajo);
            this.Controls.Add(this.panelIzda);
            this.Controls.Add(this.panelDcha);
            this.Controls.Add(this.expandablePanel1);
            this.DoubleBuffered = true;
            this.EnableGlass = false;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(607, 485);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(607, 39);
            this.Name = "FormAvisos";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormAvisos_Load);
            this.expandablePanel1.ResumeLayout(false);
            this.panelArriba.ResumeLayout(false);
            this.panelArriba.PerformLayout();
            this.panelAbajo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ExpandablePanel expandablePanel1;
        private DevComponents.DotNetBar.Controls.Slider sliderVolumen;
        private DevComponents.DotNetBar.ButtonX botonParada;
        private DevComponents.DotNetBar.ButtonX botonReproducir;
        private System.Windows.Forms.Panel panelArriba;
        private System.Windows.Forms.Panel panelAbajo;
        private System.Windows.Forms.Panel panelIzda;
        private System.Windows.Forms.Panel panelDcha;
        internal System.Windows.Forms.Label labelCuerpo;
        private System.Windows.Forms.Label labelMarquesina;
        private System.Windows.Forms.Label labelPie;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.ButtonX botonReproducirAvisoVisual;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX botonControlVolumen;
        private System.Windows.Forms.Timer timerFechaHora;
        private System.Windows.Forms.Timer timerMarquesinaMovil;
        private System.Windows.Forms.Timer timerConmutaColores;
    }
}

