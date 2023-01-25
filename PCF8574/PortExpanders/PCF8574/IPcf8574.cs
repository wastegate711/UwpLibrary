using System;
using System.Threading.Tasks;
using Windows.Devices.I2c;

namespace PeripheralDevices.PortExpanders.PCF8574
{
    public interface IPcf8574
    {
        /// <summary>
        /// Событие генерируется на любом восходящем или нисходящем
        /// фронте входов порта микросхемы Interrupt.
        /// Прерывание сбрасывается на высокий уровень, когда данные в порту изменяются на исходные настройки или
        /// данные считываются или записываются ведущим устройством.
        /// </summary>
        event Action<byte> PinChanged;

        /// <summary>
        /// Получает I2C контроллер по умолчанию (в Raspberry pi 2 и 3 это I2С1)
        /// </summary>
        /// <param name="address">Адрес устройства.</param>
        /// <param name="busSpeed">Скорость.</param>
        /// <returns>Если дескрипторы получены вернет true, иначе false.</returns>
        Task<bool> Initialization(int address, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode);

        /// <summary>
        /// Получает I2C контроллер по умолчанию (в Raspberry pi 2 и 3 это I2С1)
        /// </summary>
        /// <param name="address">Адрес устройства.</param>
        /// <param name="pinInterrupt">Номер вывода к которому подключен вывод микросхемы "Interrupt".</param>
        /// <param name="busSpeed">Скорость I2C шины.</param>
        /// <returns>Если дескрипторы получены вернет true, иначе false.</returns>
        Task<bool> Initialization(int address, int pinInterrupt, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode);

        /// <summary>
        /// Получает I2C контроллер.
        /// </summary>
        /// <param name="interfaceName">Имя контроллера (пример "I2C1")</param>
        /// <param name="address">Адрес устройства.</param>
        /// <param name="busSpeed">Скорость I2C шины.</param>
        /// <returns>Если дескрипторы получены вернет true, иначе false.</returns>
        Task<bool> Initialization(string interfaceName, int address, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode);

        /// <summary>
        /// Получает I2C контроллер.
        /// </summary>
        /// <param name="interfaceName">Имя контроллера (пример "i2c1" или "i2c2")</param>
        /// <param name="address">Адрес устройства.</param>
        /// <param name="pinNumber">Номер вывода к которому подключен вывод микросхемы "Interrupt".</param>
        /// <param name="busSpeed">Скорость.</param>
        /// <returns>Если дескрипторы получены вернет true, иначе false.</returns>
        Task<bool> Initialization(string interfaceName, int address, int pinNumber, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode);

        /// <summary>
        /// Удаляет объекты и закрывает дескрипторы.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Управляет состоянием выводов микросхемы.
        /// </summary>
        /// <param name="pin">Номер вывода микросхемы на котором нужно утановить 0 или 1
        /// можно представить в виде битового представления 0b0000_0000</param>
        void SetPinState(byte pin);

        /// <summary>
        /// Считывает состояние выводов микросхемы.
        /// </summary>
        /// <returns>Возвращает состояние всех выводов в виде байта.</returns>
        byte ReadPinState();
    }
}