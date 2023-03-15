using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;

namespace MAX7219
{
    public class Max7219 : IMax7219
    {
        private SpiDevice _max7219;

        /// <inheritdoc />
        public async Task<bool> Initialization(int chipSelect, int clock, SpiMode spiMode = SpiMode.Mode0)
        {
            SpiConnectionSettings spiConnectionSettings = new SpiConnectionSettings(chipSelect);
            spiConnectionSettings.DataBitLength = 8;
            spiConnectionSettings.ClockFrequency = clock;
            spiConnectionSettings.Mode = spiMode; //
            SpiController spiController = await SpiController.GetDefaultAsync();
            _max7219 = spiController.GetDevice(spiConnectionSettings);

            return _max7219 != null;
        }

        /// <inheritdoc />
        public async Task<bool> Initialization(string busName, int chipSelect, int clock, SpiMode spiMode = SpiMode.Mode0)
        {
            SpiConnectionSettings spiConnectionSettings = new SpiConnectionSettings(chipSelect);
            spiConnectionSettings.DataBitLength = 8;
            spiConnectionSettings.ClockFrequency = clock;
            spiConnectionSettings.Mode = spiMode; //
            string bus = SpiDevice.GetDeviceSelector(busName);
            var aqs = await DeviceInformation.FindAllAsync(bus);

            if (aqs.Any())
            {
                _max7219 = await SpiDevice.FromIdAsync(aqs.First().Id, spiConnectionSettings);
            }

            return _max7219 != null;
        }

        /// <inheritdoc />
        public void Max7219Config()
        {
            // Отключаем тест дисплея
            _max7219.Write(new byte[] { (byte)Registers.DisplayTest, 0 });
            // Включаем режим shutdown
            _max7219.Write(new byte[] { (byte)Registers.Shutdown, 1 });
            // Устанавливаем интенсивность свечения дисплея
            _max7219.Write(new byte[] { (byte)Registers.Intensity, 3 });
            // Устанавливаем количество работающих сегментов 
            _max7219.Write(new byte[] { (byte)Registers.ScanLimit, 7 });
            // Включаем режим декодирования
            _max7219.Write(new byte[] { (byte)Registers.DecodeMode, 0xFF });
            // Записываем значение в 0 сегмент
            _max7219.Write(new byte[] { (byte)Registers.Digit0, 0 });
        }

        /// <inheritdoc />
        public void SendCmd(byte register, byte data)
        {
            _max7219.Write(new byte[] { register, data });
        }

        /// <inheritdoc />
        public void DisplayNumber(int number, bool showAll = false)
        {
            if (!showAll)
            {
                if (number > 9999999)
                {
                    SendCmd((byte)Registers.ScanLimit, 7);
                }
                else if (number > 999999)
                {
                    SendCmd((byte)Registers.ScanLimit, 6);
                }
                else if (number > 99999)
                {
                    SendCmd((byte)Registers.ScanLimit, 5);
                }
                else if (number > 9999)
                {
                    SendCmd((byte)Registers.ScanLimit, 4);
                }
                else if (number > 999)
                {
                    SendCmd((byte)Registers.ScanLimit, 3);
                }
                else if (number > 99)
                {
                    SendCmd((byte)Registers.ScanLimit, 2);
                }
                else if (number > 9)
                {
                    SendCmd((byte)Registers.ScanLimit, 1);
                }
                else
                {
                    SendCmd((byte)Registers.ScanLimit, 0);
                    return;
                }
            }

            SendCmd((byte)Registers.Digit7, (byte)(number / 10000000));
            SendCmd((byte)Registers.Digit6, (byte)((number / 1000000) % 10));
            SendCmd((byte)Registers.Digit5, (byte)((number / 100000) % 10));
            SendCmd((byte)Registers.Digit4, (byte)((number / 10000) % 10));
            SendCmd((byte)Registers.Digit3, (byte)((number / 1000) % 10));
            SendCmd((byte)Registers.Digit2, (byte)((number / 100) % 10));
            SendCmd((byte)Registers.Digit1, (byte)((number / 10) % 10));
            SendCmd((byte)Registers.Digit0, (byte)(number % 10));
        }
    }
}