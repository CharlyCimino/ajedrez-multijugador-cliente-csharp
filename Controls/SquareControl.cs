using ChessGame.Model;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGame.Controls
{

    /// <summary>
    /// Define un control para  las celdas del tablero de ajedrez
    /// </summary>
    class SquareControl: CheckBox
    {

        // Colores de fondo de las celdas
        private Color DarkColor = Color.Gray;
        private Color LightColor = Color.LightGray;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="SquareControl"/>
        /// </summary>
        public SquareControl()
        {
            FlatStyle = FlatStyle.Flat;
            Appearance = Appearance.Button;
            BackgroundImageLayout = ImageLayout.Zoom;
            FlatAppearance.BorderColor = Color.IndianRed;
            FlatAppearance.BorderSize = 0;
        }

        /// <summary>
        /// Objeto Square del tablero asociado al control
        /// </summary>
        private Celda _boardSquare;
        public Celda BoardSquare
        {
            get { return _boardSquare; }
            set
            {
                if (_boardSquare != value)
                {
                    if (_boardSquare != null)
                        _boardSquare.piezaCambiada -= BoardSquare_PieceChanged;
                    _boardSquare = value;
                    SetSizeAndLocation();
                    SetBackColor();
                    SetImage();
                    _boardSquare.piezaCambiada += BoardSquare_PieceChanged;
                }
            }
        }

        /// <summary>
        /// Actualiza la imagen del control cuando cambia la pieza de la celda
        /// </summary>
        private void BoardSquare_PieceChanged(object sender, EventArgs e)
        {
            SetImage();
        }

        private Control _previousParent;
        /// <summary>
        /// Recalcula el tamaño y la posición del control cuando cambia
        /// el control padre
        /// </summary>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            SetSizeAndLocation();
            if (_previousParent != null)
                _previousParent.Resize -= Parent_Resize;
            Parent.Resize += Parent_Resize;
            _previousParent = Parent;
        }

        /// <summary>
        /// Recalcula el tamaño y la posición del control cuando el control
        /// padre cambia de tamaño
        /// </summary>
        private void Parent_Resize(object sender, EventArgs e)
        {
            SetSizeAndLocation();
        }

        /// <summary>
        /// Establece el tamaño y la posición del control en función del tamaño
        /// del control padre y de la fila y columna de la celda que representa
        /// </summary>
        private void SetSizeAndLocation()
        {
            if (Parent != null)
            {
                int squareSize = (int)(Parent.Width > Parent.Height ?
                    Math.Floor((decimal)(Parent.Height / 8)) : Math.Floor((decimal)(Parent.Width / 8)));
                Size = new Size(squareSize, squareSize);
                if (_boardSquare != null)
                    Location = new Point(_boardSquare.Columna * squareSize, _boardSquare.Fila * squareSize);
            }
        }

        /// <summary>
        /// Establece el color de fondo de la celda
        /// </summary>
        private void SetBackColor()
        {
            if (_boardSquare == null) return;

            BackColor = ((_boardSquare.Fila + _boardSquare.Columna) % 2 == 0 ? LightColor : DarkColor);
        }

        /// <summary>
        /// Establece la imagen a mostrar en función de la pieza posicionada en la celda
        /// </summary>
        private void SetImage()
        {
            if (_boardSquare==null || _boardSquare.Pieza==null)
            {
                BackgroundImage = null;
            }
            else
            {
                string image = string.Format("{0}_{1}"
                    , _boardSquare.Pieza.GetType().Name, Enum.GetName(typeof(ColoresPosibles), _boardSquare.Pieza.Color));
                BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject(image.ToLower());
            }
        }

    }
}
