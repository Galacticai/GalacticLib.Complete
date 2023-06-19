using GalacticLib.Drawing.Colors;

namespace GalacticLib.Drawing.Images;
internal class Image {
    #region this object

    public int Width { get; }
    public int Height { get; }
    public float AspectRatio { get; }
    private Color[] _Pixels { get; }

    public Image() : this(1, 1, Color.Preset.Transparent) { }
    public Image(Color[,] pixels, float aspectRatio = 1) : this(pixels.GetLength(0), pixels.GetLength(1), aspectRatio, FlattenGrid(pixels)) { }
    private Image(int width, int height, params Color[] pixels) : this(width, height, 1, pixels) { }
    private Image(int width, int height, float aspectRatio, params Color[] pixels) {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be >0");
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be >0");
        if (pixels.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(pixels), "Pixels count must be >0");

        Width = width;
        Height = height;
        AspectRatio = aspectRatio;
        _Pixels = new Color[Width * Height];
        _Pixels = pixels;
    }

    #endregion
    #region Shortcuts

    public int PixelCount => Width * Height;
    public int PixelBytesCount => PixelCount * 4;
    public Color[,] PixelsGrid => AsGrid();


    #endregion
    #region Methods

    /// <summary> Get all the pixels' <see cref="Color"/> channels into 1 linear <see cref="byte"/> list </summary>
    /// <returns> List of { <see cref="Color.Red"/>, <see cref="Color.Green"/>, <see cref="Color.Blue"/>, <see cref="Color.Alpha"/>, ... } </returns>
    public List<byte> RawPixels
        => _Pixels.Aggregate(
            new List<byte>(),
            (list, color) => {
                list.Add(color.Red);
                list.Add(color.Green);
                list.Add(color.Blue);
                list.Add(color.Alpha);
                return list;
            }
        );

    //for (int row = 0; row < Width; row++) {
    //    for (int column = 0; column < Height; column++) {
    //        Color color = _Pixels[row * column];
    //        int index = ((row * Width) + column) * 4;
    //        pixels[index] = color.Red;
    //        pixels[index + 1] = color.Green;
    //        pixels[index + 2] = color.Blue;
    //        pixels[index + 3] = color.Alpha;
    //        Console.WriteLine(index + 3 + " " + PixelBytesCount);
    //    }
    //}
    //return pixels;


    public Color[,] AsGrid() {
        Color[,] grid = new Color[Width, Height];
        int index = 0;
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                try {
                    grid[x, y] = _Pixels[index];
                } catch {
                    grid[x, y] = Color.Preset.Transparent;
                }
                index++;
            }
        }
        return grid;
    }

    public static Color[] FlattenGrid(Color[,] grid) {
        int width = grid.GetLength(0), height = grid.GetLength(1);
        Color[] array = new Color[width * height];
        int i = 0;
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                try {
                    array[i] = grid[x, y];
                } catch {
                    array[i] = Color.Preset.Transparent;
                }
                i++;
            }
        }
        return array;
    }

    #endregion
    #region Operators
    #region Conversion

    public static implicit operator Image(Color[,] data) => new(data);
    public static implicit operator Color[,](Image image) => (Color[,])image._Pixels.Clone();

    #endregion
    #endregion
}
