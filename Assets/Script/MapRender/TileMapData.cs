public class TileMapData
{
    public int width;
    public int height;
    public TileData[,] tiles;

    public TileMapData(int width, int height)
    {
        this.width = width;
        this.height = height;
        tiles = new TileData[width, height];
    }
}
