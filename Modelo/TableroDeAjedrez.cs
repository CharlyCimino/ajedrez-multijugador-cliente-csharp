using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ChessGame.Model
{

    /// <summary>
    /// Tablero de juego de ajedrez
    /// </summary>
    class TableroDeAjedrez : List<Celda>
    {

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="TableroDeAjedrez"/>
        /// </summary>
        public TableroDeAjedrez()
        {
            for (int fila = 0; fila < 8; fila++)
            {
                for (int columna = 0; columna < 8; columna++)
                    Add(new Celda(this, fila, columna));
            }
            JugadorArriba = ColoresPosibles.Blanco;
            
        }

        /// <summary>
        /// Establece o devuelve el color del jugador cuyas piezas se posicionan
        /// en la parte superior del tablero
        /// </summary>
        public ColoresPosibles JugadorArriba { get; set; }

        /// <summary>
        /// Devuelve la celda ubicada en la fila y columna especificadas
        /// </summary>
        /// <param name="fila">Fila</param>
        /// <param name="columna">Columna</param>
        /// <returns>Objeto Square de la fila y columna</returns>
        public Celda devolverCelda(int fila, int columna)
        {
            return this.FirstOrDefault(s => s.Fila == fila && s.Columna == columna);
        }

        /// <summary>
        /// Devuelve la fila destino de la pieza después de aplicar un desplazamiento a partir de 
        /// una celda origen
        /// </summary>
        /// <param name="celdaOrigen">Celda origen</param>
        /// <param name="movimiento">Movimiento a realizar a partir de la celda origen</param>
        /// <returns>Objeto Square destino del movimiento</returns>
        public Celda GetSquare(Celda celdaOrigen, Movimiento movimiento)
        {
            Pieza pieza = celdaOrigen.Pieza;
            if (pieza == null) return null;

            return devolverCelda(celdaOrigen.Fila 
                    + (pieza.Color == JugadorArriba ? movimiento.unidadesHaciaAdelante : -movimiento.unidadesHaciaAdelante)
                , celdaOrigen.Columna + movimiento.unidadesHaciaDerecha);
        }

        /// <summary>
        /// Inicializa el tablero de juego colocando las piezas en su posición inicial
        /// </summary>
        public void colocarLasPiezas()
        {
            limpiarTablero();

            ColoresPosibles jugadorAbajo = (JugadorArriba == ColoresPosibles.Blanco ? ColoresPosibles.Negro : ColoresPosibles.Blanco);
            // Reyes
            int columna = (JugadorArriba == ColoresPosibles.Blanco ? 4 : 3);
            devolverCelda(0, columna).Pieza = new Rey(JugadorArriba);
            devolverCelda(7, columna).Pieza = new Rey(jugadorAbajo);
            // Reinas
            columna = (columna == 4 ? 3 : 4);
            devolverCelda(0, columna).Pieza = new Reina(JugadorArriba);
            devolverCelda(7, columna).Pieza = new Reina(jugadorAbajo);
            // Torres
            devolverCelda(0, 0).Pieza = new Torre(JugadorArriba);
            devolverCelda(0, 7).Pieza = new Torre(JugadorArriba);
            devolverCelda(7, 0).Pieza = new Torre(jugadorAbajo);
            devolverCelda(7, 7).Pieza = new Torre(jugadorAbajo);
            // Caballos
            devolverCelda(0, 1).Pieza = new Caballo(JugadorArriba);
            devolverCelda(0, 6).Pieza = new Caballo(JugadorArriba);
            devolverCelda(7, 1).Pieza = new Caballo(jugadorAbajo);
            devolverCelda(7, 6).Pieza = new Caballo(jugadorAbajo);
            // Alfiles
            devolverCelda(0, 2).Pieza = new Alfil(JugadorArriba);
            devolverCelda(0, 5).Pieza = new Alfil(JugadorArriba);
            devolverCelda(7, 2).Pieza = new Alfil(jugadorAbajo);
            devolverCelda(7, 5).Pieza = new Alfil(jugadorAbajo);
            // Peones
            for (int i = 0; i < 8; i++)
            {
                devolverCelda(1, i).Pieza = new Peon(JugadorArriba);
                devolverCelda(6, i).Pieza = new Peon(jugadorAbajo);
            }
        }

        /// <summary>
        /// Inicializa el tablero de juego colocando las piezas en su posición inicial
        /// </summary>
        /// <param name="colorDelJugadorSuperior">Color de las piezas a colocar en la parte
        /// superior del tablero</param>
        public void ArrancarTablero(ColoresPosibles colorDelJugadorSuperior)
        {
            JugadorArriba = colorDelJugadorSuperior;
            colocarLasPiezas();
        }

        /// <summary>
        /// Mueve una pieza de una celda a otra
        /// </summary>
        /// <param name="celdaDesde">Celda desde la que se realiza el movimiento</param>
        /// <param name="celdaHacia">Celda destino del movimiento</param>
        /// <returns>true o false indicando si el movimiento se ha realizado</returns>
        public bool Mover(Celda celdaDesde, Celda celdaHacia)
        {
            if (celdaDesde != null && celdaHacia != null && celdaDesde.Pieza != null)
            {
                celdaHacia.Pieza = celdaDesde.Pieza;
                celdaDesde.Pieza = null;
                return true;
            }
            else
            {
                return false;
            }
                
        }

        /// <summary>
        /// Elimina todas las piezas del tablero
        /// </summary>
        private void limpiarTablero()
        {
            foreach (Celda celda in this)
                celda.Pieza = null;
        }

    }

    /// <summary>
    /// Celda del tablero de ajedrez
    /// </summary>
    class Celda
    {

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Celda"/>
        /// </summary>
        /// <param name="tablero">Referencia al tablero al que pertenece la celda</param>
        /// <param name="fila">Fila (índice basado en 0)</param>
        /// <param name="columna">Columna (índice basado en 0)</param>
        public Celda(TableroDeAjedrez tablero, int fila, int columna)
        {
            Tablero = tablero;
            Fila = fila;
            Columna = columna;
        }

        /// <summary>
        /// Tablero al que pertenece la celda
        /// </summary>
        public TableroDeAjedrez Tablero { get; set; }
        /// <summary>
        /// Fila (índice basado en 0)
        /// </summary>
        public int Fila { get; set; }
        /// <summary>
        /// Columna (índice basado en 0)
        /// </summary>
        public int Columna { get; set; }

        /// <summary>
        /// Devuelve o establece la pieza que se encuentra en la celda
        /// </summary>
        private Pieza _pieza;
        public Pieza Pieza
        {
            get { return _pieza; }
            set
            {
                bool cambio = _pieza != value;
                _pieza = value;
                if (_pieza != null)
                    _pieza.CeldaActual = this;
                if (cambio) OnPiezaCambiada(new EventArgs());
            }
        }

        /// <summary>
        /// Se produce cuando cambia la pieza de la celda
        /// </summary>
        public event EventHandler piezaCambiada;

        /// <summary>
        /// Genera el evento PieceChanged
        /// </summary>
        protected virtual void OnPiezaCambiada(EventArgs e)
        {
            EventHandler handler = piezaCambiada;
            if (handler != null)
                handler(this, e);
        }

    }

}
