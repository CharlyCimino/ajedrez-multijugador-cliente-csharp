using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ChessGame.Model
{

    /// <summary>
    /// Colores posibles que se pueden tomar
    /// </summary>
    enum ColoresPosibles
    {
        Blanco = 0, Negro = 1
    }
    

    /// <summary>
    /// Posibles estados que puede tomar el juego
    /// </summary>
    enum EstadoDelJuego
    {
        EsperandoComenzarMovimiento,
        EsperandoTerminarMovimiento,
        EsperandoRival,
        JugadorGana,
        RivalGana
    }

    /// <summary>
    /// Estructura que define un movimiento
    /// </summary>
    struct Movimiento
    {
        /// <summary>
        /// Número de posiciones a avanzar hacia adelante
        /// Si se mueve hacia atrás el valor será negativo
        /// El movimiento se realizará hacia arriba o haciaa abajo en función
        /// del color de la pieza a la que se aplica
        /// </summary>
        public int unidadesHaciaAdelante;
        /// <summary>
        /// Número de posiciones a mover hacia la derecha
        /// Si el movimiento es hacia la izquierda el valor será negativo
        /// </summary>
        public int unidadesHaciaDerecha;
    }

    /// <summary>
    /// Representa el juego, manteniendo el estado en el que se encuentra
    /// </summary>
    class Juego
    {
        public event EventHandler<PiezaMovidaEventArgs> piezaMovida;

        protected virtual void OnpiezaMovida(PiezaMovidaEventArgs e)
        {
            EventHandler<PiezaMovidaEventArgs> handler = piezaMovida;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Juego"/>
        /// </summary>
        public Juego()
        {
            Tablero = new TableroDeAjedrez();
        }

        /// <summary>
        /// Color del jugador
        /// </summary>
        ColoresPosibles ColorDelJugador;

        /// <summary>
        /// Tablero del juego
        /// </summary>
        public TableroDeAjedrez Tablero { get; private set; }

        /// <summary>
        /// Estado en el que se encuentra el juego
        /// Cuando se asigna un nuevo estado se lanza el evento <see cref="cambioDeEstado"/>
        /// </summary>
        private EstadoDelJuego _estado;
        public EstadoDelJuego Estado {
            get { return _estado; }
            private set
            {
                if (_estado != value)
                {
                    _estado = value;
                    OnCambioDeEstado(new EventArgs());
                }
            }
        }

        /// <summary>
        /// Devuelve la celda seleccionada por el usuario
        /// </summary>
        public Celda CeldaSeleccionada { get; private set; }

        /// <summary>
        /// Inicia un nuevo juego disponiendo las piezas del color indicado
        /// en la parte superior del tablero
        /// </summary>
        /// <param name="jugadorArriba">Color de las piezas de la parte superior del tablero</param>
        public void Iniciar(ColoresPosibles jugadorArriba)
        {
            Tablero.ArrancarTablero(jugadorArriba);
            // Se establece el estado a través de la variable privada para que no se genere
            // el evento StateChanged ya que lo lanzamos manualmente
            
            if (jugadorArriba == 0) //Si el de arriba es blanco, espera el turno.
            { 
                _estado = EstadoDelJuego.EsperandoRival;
                ColorDelJugador = ColoresPosibles.Negro;
            }
            else
            {
                _estado = EstadoDelJuego.EsperandoComenzarMovimiento;
                ColorDelJugador = ColoresPosibles.Blanco;
            }

            OnCambioDeEstado(new EventArgs());
        }

        /// <summary>
        /// Devuelve las celdas que pueden ser seleccionadas: las celdas con piezas del color del jugador
        /// cuando el juego está pendiente de que éste inicie el movimiento
        /// </summary>
        /// <returns>Las celdas que pueden ser seleccionadas</returns>
        public IEnumerable<Celda> DevolverCeldasQuePuedenSerSeleccionadas()
        {
            if (Estado != EstadoDelJuego.EsperandoTerminarMovimiento && Estado != EstadoDelJuego.EsperandoComenzarMovimiento)
                return null;

            return Tablero.Where(s => s.Pieza != null
                && s.Pieza.Color == ColorDelJugador);
        }

        /// <summary>
        /// Selecciona la celda indicada como celda inicio del movimiento
        /// </summary>
        /// <param name="celdaASeleccionar">Celda a seleccionar</param>
        /// <returns>true o false indicando si se ha realizado la selección
        /// El juego debe estar pendiente de iniciar un movimiento y la celda ser una de
        /// las celdas seleccionables</returns>
        public bool SeleccionarCelda(Celda celdaASeleccionar)
        {
            if (celdaASeleccionar == null || celdaASeleccionar.Pieza == null) return false;

            if (Estado==EstadoDelJuego.EsperandoComenzarMovimiento && celdaASeleccionar.Pieza.Color==ColorDelJugador)
            {
                CeldaSeleccionada = celdaASeleccionar;
                Estado = EstadoDelJuego.EsperandoTerminarMovimiento;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deselecciona la celda actualmente seleccionada
        /// </summary>
        /// <returns>true o false indicando si se ha realizado la deselección
        /// Para que exista celda seleccionada el juego debe estar pediente de finalizar movimiento</returns>
        public bool DeseleccionarCelda()
        {
            if (Estado==EstadoDelJuego.EsperandoTerminarMovimiento)
            {
                CeldaSeleccionada = null;
                Estado = EstadoDelJuego.EsperandoComenzarMovimiento;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Devuelve las celdas que pueden ser destino del movimiento en función
        /// de la celda actualmente seleccionada.
        /// Los posibles destinos dependen de la pieza contenida en la celda seleccionada.
        /// </summary>
        /// <returns>Las celdas que pueden ser destino del movimiento</returns>
        public IEnumerable<Celda> CeldasDeDestinoPosibles()
        {
            if (CeldaSeleccionada == null) return null;

            return CeldaSeleccionada.Pieza.ListaDeDestinosPosibles();
        }

        /// <summary>
        /// Ejecuta el movimiento desde la celda seleccionada a la 
        /// celda indicada
        /// </summary>
        /// <param name="celdaDeDestino">Celda destino del movimiento</param>
        /// <returns>true o false en función de si se ha podido realizar
        /// el movimiento</returns>
        public bool realizarMovimiento(Celda celdaDeDestino)
        {
            if (CeldaSeleccionada == null) return false;

            if (Tablero.Mover(CeldaSeleccionada, celdaDeDestino))
            {
                //ENVIAR DATO AL SERVIDOR
                PiezaMovidaEventArgs args = new PiezaMovidaEventArgs();
                args.movida = this.CeldaSeleccionada.Fila.ToString() + this.CeldaSeleccionada.Columna.ToString() +
                                celdaDeDestino.Fila.ToString() + celdaDeDestino.Columna.ToString();
                OnpiezaMovida(args);

                this.CeldaSeleccionada = null;
                // Una vez realizado el movimiento se comprueba si ha finalizado
                // (si hay ganador)
                if (!chequearSiGano())
                    Estado = EstadoDelJuego.EsperandoRival;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Ejecuta el movimiento desde la celda seleccionada a la 
        /// celda indicada del rival
        /// </summary>
        /// <param name="movida">Celda origen y destino en formato String</param>
        public void realizarMovimientoDelRival(String movida)
        {
            Celda origen = Tablero.devolverCelda(Int32.Parse(movida.Substring(0, 1)), Int32.Parse(movida.Substring(1, 1)));
            Celda destino = Tablero.devolverCelda(Int32.Parse(movida.Substring(2, 1)), Int32.Parse(movida.Substring(3, 1)));
            
            Tablero.Mover(origen, destino);
            if (!chequearSiGano())
                 Estado = EstadoDelJuego.EsperandoComenzarMovimiento;
        }

        /// <summary>
        /// Se produce cuando cambia el estado del juego
        /// </summary>
        public event EventHandler cambioDeEstado;

        /// <summary>
        /// Genera el evento StateChanged
        /// </summary>
        protected virtual void OnCambioDeEstado(EventArgs e)
        {
            EventHandler handler = cambioDeEstado;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Comprueba si el juego ha finalizado (si existe ganador)
        /// </summary>
        /// <returns>true o false indicando si el juego ha finalizado</returns>
        private bool chequearSiGano()
        {
            // Si únicamente queda un rey el juego ha terminado
            IEnumerable<Pieza> reyes = Tablero.Select(s => s.Pieza)
                .Where(p => p != null && p is Rey);
            if (reyes.Count()==1)
            {
                Estado = (reyes.First().Color == ColorDelJugador ? 
                    EstadoDelJuego.JugadorGana : EstadoDelJuego.RivalGana);
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public class PiezaMovidaEventArgs : EventArgs
    {
        public String movida { get; set; }
    }
}
