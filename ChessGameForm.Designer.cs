namespace ChessGame
{
    partial class ChessGameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChessGameForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxChat = new System.Windows.Forms.TextBox();
            this.buttonConectar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPuerto = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxNombre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelBoard = new System.Windows.Forms.Panel();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxChat);
            this.groupBox1.Controls.Add(this.buttonConectar);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxPuerto);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxIP);
            this.groupBox1.Controls.Add(this.textBoxNombre);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 378);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(366, 148);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conexión";
            // 
            // textBoxChat
            // 
            this.textBoxChat.BackColor = System.Drawing.Color.MidnightBlue;
            this.textBoxChat.ForeColor = System.Drawing.Color.White;
            this.textBoxChat.Location = new System.Drawing.Point(161, 9);
            this.textBoxChat.Multiline = true;
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.ReadOnly = true;
            this.textBoxChat.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxChat.Size = new System.Drawing.Size(204, 139);
            this.textBoxChat.TabIndex = 22;
            this.textBoxChat.TabStop = false;
            // 
            // buttonConectar
            // 
            this.buttonConectar.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.buttonConectar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonConectar.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConectar.ForeColor = System.Drawing.Color.Black;
            this.buttonConectar.Location = new System.Drawing.Point(9, 113);
            this.buttonConectar.Name = "buttonConectar";
            this.buttonConectar.Size = new System.Drawing.Size(146, 29);
            this.buttonConectar.TabIndex = 13;
            this.buttonConectar.Text = "Conectar";
            this.buttonConectar.UseVisualStyleBackColor = false;
            this.buttonConectar.Click += new System.EventHandler(this.buttonConectar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(97, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = ":";
            // 
            // textBoxPuerto
            // 
            this.textBoxPuerto.BackColor = System.Drawing.Color.MidnightBlue;
            this.textBoxPuerto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPuerto.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxPuerto.Location = new System.Drawing.Point(110, 86);
            this.textBoxPuerto.Name = "textBoxPuerto";
            this.textBoxPuerto.Size = new System.Drawing.Size(45, 22);
            this.textBoxPuerto.TabIndex = 11;
            this.textBoxPuerto.Text = "8888";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Dirección del servidor:";
            // 
            // textBoxIP
            // 
            this.textBoxIP.BackColor = System.Drawing.Color.MidnightBlue;
            this.textBoxIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxIP.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxIP.Location = new System.Drawing.Point(9, 86);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(86, 22);
            this.textBoxIP.TabIndex = 9;
            this.textBoxIP.Text = "localhost";
            // 
            // textBoxNombre
            // 
            this.textBoxNombre.BackColor = System.Drawing.Color.MidnightBlue;
            this.textBoxNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNombre.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxNombre.Location = new System.Drawing.Point(9, 43);
            this.textBoxNombre.Name = "textBoxNombre";
            this.textBoxNombre.Size = new System.Drawing.Size(146, 22);
            this.textBoxNombre.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(6, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Su nombre:";
            // 
            // panelBoard
            // 
            this.panelBoard.Cursor = System.Windows.Forms.Cursors.NoMove2D;
            this.panelBoard.Location = new System.Drawing.Point(8, 0);
            this.panelBoard.Name = "panelBoard";
            this.panelBoard.Size = new System.Drawing.Size(372, 372);
            this.panelBoard.TabIndex = 0;
            // 
            // panelLogo
            // 
            this.panelLogo.BackgroundImage = global::ChessGame.Properties.Resources.DibujoCaemci;
            this.panelLogo.Location = new System.Drawing.Point(8, 0);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(372, 372);
            this.panelLogo.TabIndex = 1;
            // 
            // ChessGameForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(389, 527);
            this.Controls.Add(this.panelBoard);
            this.Controls.Add(this.panelLogo);
            this.Controls.Add(this.groupBox1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(530, 570);
            this.Name = "ChessGameForm";
            this.Text = "Ajedrez Multijugador";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBoard;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxNombre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPuerto;
        private System.Windows.Forms.Button buttonConectar;
        private System.Windows.Forms.TextBox textBoxChat;
        private System.Windows.Forms.Panel panelLogo;
    }
}

