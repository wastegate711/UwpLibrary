using Windows.Devices.I2c;

namespace PCF8574
{
    public interface IPcf8574
    {
        /// <summary>
        /// Получает I2C контроллер по умолчанию (в Raspberry pi 2 и 3 это I2С1)
        /// </summary>
        /// <param name="address">Адрес устройства.</param>
        /// <param name="pinNumber">Номер вывода к которому подключен вывод микросхемы "Interrupt".</param>
        /// <param name="busSpeed">Скорость.</param>
        void Initialization(int address, int pinNumber = int.MaxValue, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode);

        /// <summary>
        /// Получает I2C контроллер.
        /// </summary>
        /// <param name="interfaceName">Имя контроллера (пример "i2c1" или "i2c2")</param>
        /// <param name="address">Адрес устройства.</param>
        /// <param name="pinNumber">Номер вывода к которому подключен вывод микросхемы "Interrupt".</param>
        /// <param name="busSpeed">Скорость.</param>
        void Initialization(string interfaceName, int address, int pinNumber = int.MaxValue, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode);

        /// <summary>
        /// Удаляет объекты и закрывает дескрипторы.
        /// </summary>
        void Dispose();
    }
}