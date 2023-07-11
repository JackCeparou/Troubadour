namespace T4.Plugins.Troubadour;

public sealed partial class ItemIcon
{
    public float Scale { get; init; } = 1f;
    public float OffsetX { get; init; } = 0f;
    public float OffsetY { get; init; } = 0f;
    public Func<IItem, InventoryFeatures, bool> Show { get; init; } = (_, _) => true;
    public Func<IItem, InventoryFeatures, ITexture> Texture { get; init; }
    public Func<IItem, InventoryFeatures, ITexture> BackgroundTexture { get; init; }

    public float Draw(IItem item, InventoryFeatures features, float x, float y, bool expandUpwards, bool alignRight)
    {
        var texture = Texture.Invoke(item, features);
        if (texture is null)
            return 0;
        if (!Show.Invoke(item, features))
            return 0;

        y += OffsetY;
        var size = features.IconSize;
        var backgroundTexture = BackgroundTexture?.Invoke(item, features);
        backgroundTexture?.Draw(x, y, size, size);

        size = (float)Math.Floor(size);
        if (alignRight)
        {
            x -= size + OffsetX;
        }
        else
        {
            x += OffsetX;
        }

        if (expandUpwards)
        {
            y -= size / 2;
        }

        var scaledSize = size * Scale;
        var textureX = x + ((size - scaledSize) / 2);
        var textureY = y + ((size - scaledSize) / 2);
        BackgroundTexture?.Invoke(item, features)?.Draw(textureX, textureY, scaledSize, scaledSize);
        texture.Draw(textureX, textureY, scaledSize, scaledSize);
        // DebugLineStyle.DrawRectangle(textureX, textureY, scaledSize, scaledSize);
        return scaledSize; // + OffsetX;
    }
}