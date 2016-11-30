using ChessGame.Controls;
using ChessGame.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ChessGame
{

    /// <summary>
    /// Formulario con el tablero de juego
    /// </summary>
    public partial class ChessGameForm : Form
    {
        private ControladorDeRed controlador;
        private Juego _game = new Juego();
        private List<SquareControl> _squares = new List<SquareControl>();

        delegate void nuevaNotificacionCallback(object sender, EventArgs e);
        delegate void hayRivalCallback(object sender, EventArgs e);
        delegate void reiniciarEstadoDelFormularioCallback(object sender, EventArgs e);
        delegate void piezaMovidaCallback(object sender, PiezaMovidaEventArgs e);

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ChessGameForm"/>
        /// </summary>
        public ChessGameForm()
        {
            InitializeComponent();
            panelBoard.Visible = false;
        }

        /// <summary>
        /// Método que controla la pulsación de una celda por parte del usuario
        /// </summary>
        private void Square_CheckedChanged(object sender, EventArgs e)
        {
            SquareControl square = (SquareControl)sender;
            if (square.Checked)
            {
                if (_game.Estado == EstadoDelJuego.EsperandoComenzarMovimiento)
                {
                    // Si el juego está pendiente de iniciar movimiento se selecciona la celda pulsada
                    _game.SeleccionarCelda(square.BoardSquare);
                }
                else if (_game.Estado == EstadoDelJuego.EsperandoTerminarMovimiento)
                {
                    // Si el juego está pendiente de finalizar movimiento se ejecuta el movimiento correspondiente
                    // y se deseleccionan las celdas del movimiento
                    if (_game.realizarMovimiento(square.BoardSquare))
                    {
                        square.Checked = false;
                        _squares.First(s => s.Checked).Checked = false;
                    }
                }
            }
            else
            {
                // Si el juego está pendiente de completar el movimiento se deselecciona la celda
                if (_game.Estado == EstadoDelJuego.EsperandoTerminarMovimiento)
                    _game.DeseleccionarCelda();
            }
        }

        /// <summary>
        /// Controlador para el evento de cambio de estado del juego
        /// </summary>
        private void Game_StateChanged(object sender, EventArgs e)
        {
            switch (_game.Estado)
            {
                case EstadoDelJuego.EsperandoComenzarMovimiento:
                    // Si el juego está pendiente de iniciar movimiento habilitamos las celdas
                    // seleccionables (las que tienen piezas del color del jugador)
                    IEnumerable<Celda> selectionableSquares = _game.DevolverCeldasQuePuedenSerSeleccionadas();
                    foreach (SquareControl square in _squares)
                    {
                        square.Enabled = selectionableSquares.Contains(square.BoardSquare);
                        square.FlatAppearance.BorderSize = 0;
                    }
                    break;
                case EstadoDelJuego.EsperandoTerminarMovimiento:
                    // Si el juego está pendiente de finalizar el movimiento habilitamos:
                    // - las celdas posible destino del movimiento
                    // - la celda inicio del movimiento para dar la posibilidad de deseleccionarla
                    // También establecemos un borde en las celdas destino para marcárselas al usuario
                    IEnumerable<Celda> destinationSquares = _game.CeldasDeDestinoPosibles();
                    foreach (SquareControl square in _squares)
                    {
                        square.Enabled = destinationSquares.Contains(square.BoardSquare)
                            || square.BoardSquare==_game.CeldaSeleccionada;
                        if (square.Enabled)
                            square.FlatAppearance.BorderSize = 1;
                    }
                    break;
                case EstadoDelJuego.EsperandoRival:
                    deshabilitarCeldas();
                    break;
                case EstadoDelJuego.JugadorGana:
                    deshabilitarCeldas();
                    controlador.enviarAlServidor("terminado");
                    MessageBox.Show(string.Format("Felicidades {0}, ganaste !", controlador.NombreDelJugador));
                    break;
                case EstadoDelJuego.RivalGana:
                    // Si ha finalizado el juego deshabilitamos todas las celdas 
                    deshabilitarCeldas();
                    MessageBox.Show(string.Format("Lo lamento {0}, perdiste !", controlador.NombreDelJugador));
                    break;
            }
        }

        private void deshabilitarCeldas ()
        {
            foreach (SquareControl square in _squares)
            {
                square.Enabled = false;
                square.FlatAppearance.BorderSize = 0;
            }
        }


        private void reiniciarEstadoDelFormulario(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                reiniciarEstadoDelFormularioCallback delegado = new reiniciarEstadoDelFormularioCallback(reiniciarEstadoDelFormulario);
                this.Invoke(delegado, new object[] { sender, e });
            }
            else
            {
                panelBoard.Visible = false;
                textBoxIP.Focus();
                textBoxIP.Enabled = true;
                textBoxPuerto.Enabled = true;
                textBoxNombre.Enabled = true;
                buttonConectar.Enabled = true;
            }
        }

        private void textBoxChat_TextChanged(object sender, EventArgs e)
        {
            textBoxChat.SelectionStart = textBoxChat.Text.Length;
            textBoxChat.ScrollToCaret();
        }

        private void buttonConectar_Click(object sender, EventArgs e)
        {
            if (textBoxIP.TextLength == 0 || textBoxPuerto.TextLength == 0 || textBoxNombre.TextLength == 0)
                MessageBox.Show("Debe completar todos los datos solicitados antes de conectar al servidor");
            else
            {
                this.controlador = new ControladorDeRed(textBoxIP.Text, textBoxPuerto.Text, textBoxNombre.Text);
                this.controlador.nuevaNotificacion += this.nuevaNotificacion;
                this.controlador.hayRival += this.hayRival;
                this.controlador.moverFichaRival += this.moverFichaDelRival;
                this.controlador.reiniciarFormulario += this.reiniciarEstadoDelFormulario;
                this._game.piezaMovida += this.enviarMovimientoAlServidor;

                if (controlador.conectar())
                {
                    textBoxChat.Clear();
                    textBoxIP.Enabled = false;
                    textBoxPuerto.Enabled = false;
                    textBoxNombre.Enabled = false;
                    buttonConectar.Enabled = false;
                }
                else reiniciarEstadoDelFormulario(this,null);
            }
        }

        private void comenzarJuego()
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(comenzarJuego));
            else
            {
                panelBoard.Visible = true;
                // Creamos los controles para las celdas del tablero
                foreach (Celda square in _game.Tablero)
                {
                    SquareControl control = new SquareControl();
                    control.BoardSquare = square;
                    _squares.Add(control);
                    control.CheckedChanged += Square_CheckedChanged;
                }
                panelBoard.Controls.AddRange(_squares.ToArray());
                // Controlador para el evento de cambio de estado del juego
                _game.cambioDeEstado += Game_StateChanged;
                // Inicia el juego colocando en la parte superior el color seleccionado
                _game.Iniciar((controlador.Color == 0) ? ColoresPosibles.Blanco : ColoresPosibles.Negro);
            }
        }

        // SUCESOS TRAS LOS EVENTOS
        private void nuevaNotificacion(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            { 
                nuevaNotificacionCallback delegado = new nuevaNotificacionCallback(nuevaNotificacion);
                this.Invoke(delegado, new object[] { sender, e });
            }
            else
                textBoxChat.Text = textBoxChat.Text + (textBoxChat.Text == "" ? "" : Environment.NewLine) + controlador.Dato;
        }
        
        private void hayRival(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                hayRivalCallback delegado = new hayRivalCallback(hayRival);
                this.Invoke(delegado, new object[] { sender, e });
            }
            else
            {
                //Thread ctThread = new Thread(comenzarJuego);
                //ctThread.Start();
                comenzarJuego();
            }
        }

        private void enviarMovimientoAlServidor(object sender, PiezaMovidaEventArgs e)
        {
            controlador.enviarAlServidor(e.movida);
        }

        private void moverFichaDelRival(object sender, EventArgs e)
        {
            _game.realizarMovimientoDelRival(controlador.Dato);
        }


    }
}
