using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ChessGame.Controls
{

    class ControladorDeRed
    {
        private TcpClient clientSocket;
        private NetworkStream serverStream;
        private String direccionIP;
        private Int32 puerto;
        private String nombreDelJugador;
        private String dato;
        private Int32 color;

        //CABECERAS PARA EVENTOS
        public event EventHandler nuevaNotificacion;
        public event EventHandler hayRival;
        public event EventHandler moverFichaRival;
        public event EventHandler reiniciarFormulario;

        protected virtual void OnNuevaNotificacion(EventArgs e)
        {
            if (nuevaNotificacion != null)
                nuevaNotificacion(this, e);
        }

        protected virtual void OnHayRival(EventArgs e)
        {
            if (hayRival != null)
                hayRival(this, e);
        }

        protected virtual void OnMoverFichaRival(EventArgs e)
        {
            if (moverFichaRival != null)
                moverFichaRival(this, e);
        }

        protected virtual void OnReiniciarFormulario(EventArgs e)
        {
            if (reiniciarFormulario != null)
                reiniciarFormulario(this, e);
        }

        public ControladorDeRed(String ip, String puerto, String nombre)
        {
            this.direccionIP = ip;
            this.puerto = Convert.ToInt32(puerto);
            this.nombreDelJugador = nombre;
        }

        public ControladorDeRed(String ip, Int32 puerto, String nombre)
        {
            this.direccionIP = ip;
            this.puerto = puerto;
            this.nombreDelJugador = nombre;
        }

        public String Dato
        {
            get { return this.dato; }
            set { this.dato = value; }
        }

        public String NombreDelJugador
        {
            get { return this.nombreDelJugador; }
            set { this.nombreDelJugador = value; }
        }

        public Int32 Color
        {
            get { return this.color; }
            set { this.color = value; }
        }


        /// <summary>
        /// Conecta con el servidor de ajedrez
        /// </summary>
        /// <returns>Si se produjo la conexión</returns>
        public bool conectar()
        {
            try
            {
                clientSocket = new TcpClient();
                clientSocket.Connect(this.direccionIP, this.puerto);
                byte[] outStream = Encoding.ASCII.GetBytes(this.nombreDelJugador + "\0");
                serverStream = clientSocket.GetStream();
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                Thread ctThread = new Thread(recibirDelServidor);
                ctThread.Start();
                return true;
            }
            catch (Exception ex)
            {
                this.dato = "Error: No fue posible conectarse al servidor!" +
                    Environment.NewLine +
                    ex.ToString().Substring(0, ex.ToString().IndexOf(Environment.NewLine));

                OnNuevaNotificacion(EventArgs.Empty);
                OnReiniciarFormulario(EventArgs.Empty);
                return false;
            }
        }

        /// <summary>
        /// Conecta con el servidor de ajedrez
        /// </summary>
        /// <returns>Si se produjo la conexión</returns>
        public void enviarAlServidor(string cadena)
        {
            byte[] outStream = Encoding.ASCII.GetBytes(cadena + "\0");
            try
            {
                this.dato = cadena;
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
                OnNuevaNotificacion(EventArgs.Empty);
            }
            catch (Exception ex)
            {
                this.dato = "Error: No fue posible enviar el mensaje!" +
                    Environment.NewLine +
                    ex.ToString().Substring(0, ex.ToString().IndexOf(Environment.NewLine));

                OnNuevaNotificacion(EventArgs.Empty);
            }
        }

        private void recibirDelServidor()
        {
            bool finCiclo = false;

            while (!finCiclo)
            {
                try
                {
                    byte[] inStream = new byte[100000];
                    serverStream.Read(inStream, 0, clientSocket.ReceiveBufferSize);
                    this.dato = Encoding.ASCII.GetString(inStream);
                    this.dato = this.dato.Substring(0, this.dato.IndexOf("\0"));
                    if (this.dato.Length == 4) OnMoverFichaRival(EventArgs.Empty);
                    else chequearSiHayRival();
                    
                    OnNuevaNotificacion(EventArgs.Empty);
                }
                catch (System.Exception ex)
                {
                    this.dato = "Error: el servidor no se encuentra disponible! " +
                       Environment.NewLine +
                       ex.ToString().Substring(0, ex.ToString().IndexOf(Environment.NewLine));

                    OnNuevaNotificacion(EventArgs.Empty);
                    OnReiniciarFormulario(EventArgs.Empty);
                    finCiclo = true;
                }
            }
        }

        private void chequearSiHayRival()
        {
            if (this.dato.Contains("Comenzar"))
            {
                this.color = Int32.Parse( this.dato.Substring(8, 1) );
                OnHayRival(EventArgs.Empty);
            }
        }

    }
}
