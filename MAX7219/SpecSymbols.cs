namespace MAX7219
{
    public enum SpecSymbols : byte
    {
        /// <summary>
        /// Символ "-"
        /// </summary>
        Minus = 0x0A,
        /// <summary>
        /// Символ "Е"
        /// </summary>
        E = 0x0B,
        /// <summary>
        /// Символ "Н"
        /// </summary>
        H = 0x0C,
        /// <summary>
        /// Символ "L"
        /// </summary>
        L = 0x0D,
        /// <summary>
        /// Символ "Р"
        /// </summary>
        P = 0x0E,
        /// <summary>
        /// Пустой символ(ничего не светится)
        /// </summary>
        Empty = 0x0F,
        /// <summary>
        /// Зажигает точку в указанном сегменте.
        /// </summary>
        Dot = 0x80
    }
}