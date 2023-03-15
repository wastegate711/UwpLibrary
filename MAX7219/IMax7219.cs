using System.Threading.Tasks;
using Windows.Devices.Spi;

namespace MAX7219
{
    public interface IMax7219
    {
        /// <summary>
        /// Инициализация и настройка подключения к драйверу, получение контроллера SPI по умолчанию
        /// (Raspberry 2 и 3 это SPI0).
        /// </summary>
        /// <param name="chipSelect">Номер линии чип ChipSelect.</param>
        /// <param name="clock">Частота шины.</param>
        /// <param name="spiMode">Режим работы SPI.</param>
        /// <returns>Если получен дескриптор шины вернет true, иначе вернет false.</returns>
        Task<bool> Initialization(int chipSelect, int clock, SpiMode spiMode = SpiMode.Mode0);

        /// <summary>
        /// Инициализация и настройка подключения к драйверу и получение контроллера SPI.
        /// </summary>
        /// <param name="busName">Имя контроллера SPI, если такого контроллера не найдено, то вернет false.</param>
        /// <param name="chipSelect">Номер линии чип ChipSelect.</param>
        /// <param name="clock">Частота шины.</param>
        /// <param name="spiMode">Режим работы SPI.</param>
        /// <returns>Если получен дескриптор шины вернет true, иначе вернет false.</returns>
        Task<bool> Initialization(string busName, int chipSelect, int clock, SpiMode spiMode = SpiMode.Mode0);

        /// <summary>
        /// Базовая настройка драйвера дисплея по умолчанию.
        /// </summary>
        void Max7219Config();

        /// <summary>
        /// Отправляет данные в драйвер.
        /// </summary>
        /// <param name="register">Адрес регистра.</param>
        /// <param name="data">Данные.</param>
        void SendCmd(byte register, byte data);

        /// <summary>
        /// Выводит число на дисплей.
        /// </summary>
        /// <param name="number">Число для вывода.</param>
        /// <param name="showAll">По умолчанию сегменты на которых нет чисел выключаются, если
        /// установить true будут работать все сегменты.</param>
        void DisplayNumber(int number, bool showAll = false);
    }
}