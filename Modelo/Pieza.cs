using System.Collections.Generic;

namespace ChessGame.Model
{

    /// <summary>
    /// Clase base para definir las diferentes piezas de ajedrez
    /// </summary>
    abstract class Pieza
    {

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Pieza"/>
        /// </summary>
        /// <param name="color">Color de la pieza</param>
        public Pieza(ColoresPosibles color)
        {
            Color = color;
        }

        /// <summary>
        /// Color de la pieza
        /// </summary>
        public ColoresPosibles Color { get; set; }

        /// <summary>
        /// Celda en la que se encuentra la pieza
        /// </summary>
        public Celda CeldaActual { get; internal set; }

        /// <summary>
        /// Devuelve las celdas a las que puede moverse la pieza
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<Celda> ListaDeDestinosPosibles();

        /// <summary>
        /// Comprueba si una celda puede ser destino de un movimiento de la pieza:
        /// si está vacía o contiene una pieza de un color diferente
        /// </summary>
        /// <param name="celdaCandidata">Celda a comprobar</param>
        /// <returns>true o false indicando si puede ser destino del movimiento</returns>
        protected virtual bool EsUnDestinoPosible(Celda celdaCandidata)
        {
            if (celdaCandidata == null) return false;

            return (celdaCandidata.Pieza == null || celdaCandidata.Pieza.Color != Color);
        }

        /// <summary>
        /// Devuelve la lista de los posibles destinos de movimiento de la pieza
        /// en una dirección determinada
        /// La dirección a tomar viene dada por un incremento hacia adelante y
        /// otro horizontal
        /// </summary>
        /// <param name="incrementoHaciaAdelante">Incremento hacia adelante.
        /// Si la dirección es hacia atrás será un valor negativo.</param>
        /// <param name="incrementoHaciaDerecha">Incremento hacia la derecha.
        /// Si la dirección es hacia la izquierda será un valor negativo.</param>
        /// <returns>Lista de celdas posibles destinos de movimiento</returns>
        protected IEnumerable<Celda> DestinosPosibles(
            int incrementoHaciaAdelante, int incrementoHaciaDerecha)
        {
            TableroDeAjedrez tablero = CeldaActual.Tablero;

            List<Celda> celdas = new List<Celda>();
            int adelante = incrementoHaciaAdelante;
            int derecha = incrementoHaciaDerecha;
            bool piezaOFinalDetectado = false;
            while (!piezaOFinalDetectado)
            {
                Celda destino = tablero.GetSquare(CeldaActual, new Movimiento { unidadesHaciaAdelante = adelante, unidadesHaciaDerecha = derecha });
                if (EsUnDestinoPosible(destino))
                    celdas.Add(destino);
                piezaOFinalDetectado = (destino == null || destino.Pieza != null);
                adelante += incrementoHaciaAdelante;
                derecha += incrementoHaciaDerecha;
            }
            return celdas;
        }
        /// <summary>
        /// Devuelve la lista de los posibles destinos de movimiento de la pieza
        /// a partir de una lista de movimientos
        /// </summary>
        /// <param name="moves">Lista de movimientos que puede realizar la pieza</param>
        /// <returns>Lista de celdas posibles destinos</returns>
        protected IEnumerable<Celda> CeldasPosiblesParaMovimientosDados(IEnumerable<Movimiento> moves)
        {
            TableroDeAjedrez tablero = CeldaActual.Tablero;
            List<Celda> listaDeCeldasPosibles = new List<Celda>();
            foreach (Movimiento move in moves)
            {
                Celda celdaCandidata = tablero.GetSquare(CeldaActual, move);
                if (EsUnDestinoPosible(celdaCandidata))
                    listaDeCeldasPosibles.Add(celdaCandidata);
            }
            return listaDeCeldasPosibles;

        }

    }

    /// <summary>
    /// Rey
    /// </summary>
    class Rey : Pieza
    {

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Rey"/>
        /// </summary>
        /// <param name="color">Color de la pieza</param>
        public Rey(ColoresPosibles color): base(color) { }

        /// <summary>
        /// Movimientos que puede realizar el rey
        /// </summary>
        private Movimiento[] movimientos =
        {
            new Movimiento() {unidadesHaciaAdelante=1, unidadesHaciaDerecha=-1 },
            new Movimiento() {unidadesHaciaAdelante=1, unidadesHaciaDerecha=0 },
            new Movimiento() {unidadesHaciaAdelante=1, unidadesHaciaDerecha=1 },
            new Movimiento() {unidadesHaciaAdelante=0, unidadesHaciaDerecha=-1 },
            new Movimiento() {unidadesHaciaAdelante=0, unidadesHaciaDerecha=1 },
            new Movimiento() {unidadesHaciaAdelante=-1, unidadesHaciaDerecha=-1 },
            new Movimiento() {unidadesHaciaAdelante=-1, unidadesHaciaDerecha=0 },
            new Movimiento() {unidadesHaciaAdelante=-1, unidadesHaciaDerecha=1 }
        };

        /// <summary>
        /// Devuelve las celdas a las que puede moverse la pieza
        /// </summary>
        /// <returns>Lista de celdas</returns>
        public override IEnumerable<Celda> ListaDeDestinosPosibles()
        {
            if (CeldaActual == null) return null;

            return CeldasPosiblesParaMovimientosDados(movimientos);
        }

    }

    /// <summary>
    /// Torre
    /// </summary>
    class Torre : Pieza
    {

        /// <summary>
        /// Inicializa una nueva clase de <see cref="Torre"/>
        /// </summary>
        /// <param name="color">Color de la Pieza</param>
        public Torre(ColoresPosibles color) : base(color) { }

        /// <summary>
        /// Devuelve las celdas a las que puede moverse la pieza
        /// </summary>
        /// <returns>Lista de celdas</returns>
        public override IEnumerable<Celda> ListaDeDestinosPosibles()
        {
            if (CeldaActual == null) return null;

            List<Celda> listaDeCeldasPosibles = new List<Celda>();
            // A la izquierda
            listaDeCeldasPosibles.AddRange(DestinosPosibles(0, -1));
            // A la derecha
            listaDeCeldasPosibles.AddRange(DestinosPosibles(0, 1));
            // Hacia adelante
            listaDeCeldasPosibles.AddRange(DestinosPosibles(1, 0));
            // Hacia atrás
            listaDeCeldasPosibles.AddRange(DestinosPosibles(-1, 0));

            return listaDeCeldasPosibles;
        }

    }

    /// <summary>
    /// Alfil
    /// </summary>
    class Alfil : Pieza
    {

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Alfil"/>
        /// </summary>
        /// <param name="color">Color de la pieza</param>
        public Alfil(ColoresPosibles color) : base(color) { }

        /// <summary>
        /// Devuelve las celdas a las que puede moverse la pieza
        /// </summary>
        /// <returns>Lista de celdas</returns>
        public override IEnumerable<Celda> ListaDeDestinosPosibles()
        {
            if (CeldaActual == null) return null;

            List<Celda> listaDeCeldasPosibles = new List<Celda>();
            // Hacia adelante a la izquierda
            listaDeCeldasPosibles.AddRange(DestinosPosibles(1, -1));
            // Hacia adelante a la derecha
            listaDeCeldasPosibles.AddRange(DestinosPosibles(1, 1));
            // Hacia atrás a la izquierda
            listaDeCeldasPosibles.AddRange(DestinosPosibles(-1, -1));
            // Hacia atrás a la derecha
            listaDeCeldasPosibles.AddRange(DestinosPosibles(-1, 1));

            return listaDeCeldasPosibles;
        }
    }

    /// <summary>
    /// Reina
    /// </summary>
    class Reina : Pieza
    {

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Reina"/>
        /// </summary>
        /// <param name="color">Color de la pieza</param>
        public Reina(ColoresPosibles color): base(color) { }

        /// <summary>
        /// Devuelve las celdas a las que puede moverse la pieza
        /// </summary>
        /// <returns>Lista de celdas</returns>
        public override IEnumerable<Celda> ListaDeDestinosPosibles()
        {
            if (CeldaActual == null) return null;

            List<Celda> listaDeCeldasPosibles = new List<Celda>();
            // Hacia adelante a la izquierda
            listaDeCeldasPosibles.AddRange(DestinosPosibles(1, -1));
            // Hacia adelante
            listaDeCeldasPosibles.AddRange(DestinosPosibles(1, 0));
            // Hacia adelante a la derecha
            listaDeCeldasPosibles.AddRange(DestinosPosibles(1, 1));
            // Hacia la izquierda
            listaDeCeldasPosibles.AddRange(DestinosPosibles(0, -1));
            // Hacia la derecha
            listaDeCeldasPosibles.AddRange(DestinosPosibles(0, 1));
            // Hacia atrás a la izquierda
            listaDeCeldasPosibles.AddRange(DestinosPosibles(-1, -1));
            // Hacia atrás
            listaDeCeldasPosibles.AddRange(DestinosPosibles(-1, 0));
            // Hacia atrás a la derecha
            listaDeCeldasPosibles.AddRange(DestinosPosibles(-1, 1));

            return listaDeCeldasPosibles;
        }
    }

    /// <summary>
    /// Caballo
    /// </summary>
    class Caballo : Pieza
    {

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Caballo"/>
        /// </summary>
        /// <param name="color">Color de la pieza</param>
        public Caballo(ColoresPosibles color): base(color) { }

        /// <summary>
        /// Movimmientos que puede realizar el caballo
        /// </summary>
        private Movimiento[] movimientos =
        {
            new Movimiento {unidadesHaciaAdelante=1, unidadesHaciaDerecha=-2 },
            new Movimiento {unidadesHaciaAdelante=2, unidadesHaciaDerecha=-1 },
            new Movimiento {unidadesHaciaAdelante=2, unidadesHaciaDerecha=1 },
            new Movimiento {unidadesHaciaAdelante=1, unidadesHaciaDerecha=2 },
            new Movimiento {unidadesHaciaAdelante=-1, unidadesHaciaDerecha=-2 },
            new Movimiento {unidadesHaciaAdelante=-2, unidadesHaciaDerecha=-1 },
            new Movimiento {unidadesHaciaAdelante=-2, unidadesHaciaDerecha=1 },
            new Movimiento {unidadesHaciaAdelante=-1, unidadesHaciaDerecha=2 }
        };

        /// <summary>
        /// Devuelve las celdas a las que puede moverse la pieza
        /// </summary>
        /// <returns>Lista de celdas</returns>
        public override IEnumerable<Celda> ListaDeDestinosPosibles()
        {
            if (CeldaActual == null) return null;

            return CeldasPosiblesParaMovimientosDados(movimientos);
        }
    }

    /// <summary>
    /// Peón
    /// </summary>
    class Peon : Pieza
    {

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Peon"/>
        /// </summary>
        /// <param name="color">Color de la pieza</param>
        public Peon(ColoresPosibles color): base(color) { }

        /// <summary>
        /// Devuelve las celdas a las que puede moverse la pieza
        /// </summary>
        /// <returns>Lista de celdas</returns>
        public override IEnumerable<Celda> ListaDeDestinosPosibles()
        {
            if (CeldaActual == null) return null;

            TableroDeAjedrez tablero = CeldaActual.Tablero;
            bool estaEnPosicionInicial = (tablero.GetSquare(CeldaActual, new Movimiento { unidadesHaciaAdelante = -2, unidadesHaciaDerecha = 0 }) == null);

            List<Celda> listaDeCeldasPosibles = new List<Celda>();
            Celda celdaDeDestino = tablero.GetSquare(CeldaActual, new Movimiento { unidadesHaciaAdelante = 1, unidadesHaciaDerecha = 0 });
            if (celdaDeDestino != null && celdaDeDestino.Pieza == null)
            {
                listaDeCeldasPosibles.Add(celdaDeDestino);
                if (estaEnPosicionInicial)
                {
                    celdaDeDestino = tablero.GetSquare(CeldaActual, new Movimiento { unidadesHaciaAdelante = 2, unidadesHaciaDerecha = 0 });
                    if (celdaDeDestino != null && celdaDeDestino.Pieza == null)
                        listaDeCeldasPosibles.Add(celdaDeDestino);
                }
            }
            celdaDeDestino = tablero.GetSquare(CeldaActual, new Movimiento { unidadesHaciaAdelante = 1, unidadesHaciaDerecha = -1 });
            if (celdaDeDestino != null && celdaDeDestino.Pieza != null && celdaDeDestino.Pieza.Color != Color)
                listaDeCeldasPosibles.Add(celdaDeDestino);
            celdaDeDestino = tablero.GetSquare(CeldaActual, new Movimiento { unidadesHaciaAdelante = 1, unidadesHaciaDerecha = 1 });
            if (celdaDeDestino != null && celdaDeDestino.Pieza != null && celdaDeDestino.Pieza.Color != Color)
                listaDeCeldasPosibles.Add(celdaDeDestino);

            return listaDeCeldasPosibles;
        }
    }

}
