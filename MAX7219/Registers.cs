namespace MAX7219
{
    /// <summary>
    /// Адресы регистров.
    /// </summary>
    public enum Registers : byte
    {
        /// <summary>
        /// "Not operation" ничего не выполнять.
        /// </summary>
        Nop = 0x00,
        /// <summary>
        /// Адрес 0 разряда
        /// </summary>
        Digit0 = 0x01,
        /// <summary>
        /// Адрес 1 разряда
        /// </summary>
        Digit1 = 0x02,
        /// <summary>
        /// Адрес 2 разряда
        /// </summary>
        Digit2 = 0x03,
        /// <summary>
        /// Адрес 3 разряда
        /// </summary>
        Digit3 = 0x04,
        /// <summary>
        /// Адрес 4 разряда
        /// </summary>
        Digit4 = 0x05,
        /// <summary>
        /// Адрес 5 разряда
        /// </summary>
        Digit5 = 0x06,
        /// <summary>
        /// Адрес 6 разряда
        /// </summary>
        Digit6 = 0x07,
        /// <summary>
        /// Адрес 7 разряда
        /// </summary>
        Digit7 = 0x08,
        /// <summary>
        /// Режим декодирования
        /// </summary>
        DecodeMode = 0x09,
        /// <summary>
        /// Интенсивность свечения дисплея
        /// </summary>
        Intensity = 0x0A,
        /// <summary>
        /// Количество сегментов которые будут светиться
        /// </summary>
        ScanLimit = 0x0B,
        /// <summary>
        /// Режим Shutdown
        /// </summary>
        Shutdown = 0x0C,
        /// <summary>
        /// Режим тестирования дисплея
        /// </summary>
        DisplayTest = 0x0F
    }
}