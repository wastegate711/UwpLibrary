using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;

namespace PeripheralDevices.PortExpanders.PCF8575
{
    public class Pcf8575 : IPcf8575
    {
        private I2cDevice _pcf8575;
        private GpioPin _interrupt;
        /// <inheritdoc />
        public event Action<byte[]> PinChanged;

        /// <inheritdoc />
        public async Task<bool> Initialization(int address, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode)
        {
            var connectSetting = new I2cConnectionSettings(address);
            connectSetting.BusSpeed = busSpeed;
            var controller = await I2cController.GetDefaultAsync();
            _pcf8575 = controller.GetDevice(connectSetting);

            if (_pcf8575 == null)
                return false;

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> Initialization(
            int address,
            int pinInterrupt,
            I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode,
            TimeSpan timeout = default)
        {
            var connectSetting = new I2cConnectionSettings(address);
            connectSetting.BusSpeed = busSpeed;
            var controller = await I2cController.GetDefaultAsync();
            _pcf8575 = controller.GetDevice(connectSetting);

            var gpioController = await GpioController.GetDefaultAsync();
            _interrupt = gpioController.OpenPin(pinInterrupt);

            if (_pcf8575 == null | _interrupt == null)
                return false;

            _interrupt.SetDriveMode(GpioPinDriveMode.InputPullUp);
            _interrupt.DebounceTimeout = timeout;
            _interrupt.ValueChanged += Interrupt_ValueChanged;

            return true;
        }

        private void Interrupt_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            byte[] rxData = new byte[2];

            if (args.Edge == GpioPinEdge.FallingEdge || args.Edge == GpioPinEdge.RisingEdge)
            {
                _pcf8575.Read(rxData);
                PinChanged?.Invoke(rxData);
            }
        }

        /// <inheritdoc />
        public async Task<bool> Initialization(string interfaceName, int address, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode)
        {
            var aqs = I2cDevice.GetDeviceSelector(interfaceName);
            var info = await DeviceInformation.FindAllAsync(aqs);
            var connectSetting = new I2cConnectionSettings(address);
            connectSetting.BusSpeed = busSpeed;
            _pcf8575 = await I2cDevice.FromIdAsync(info.First().Id, connectSetting);

            if (_pcf8575 == null)
                return false;

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> Initialization(
            string interfaceName,
            int address,
            int pinInterrupt,
            I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode,
            TimeSpan timeout = default)
        {
            var aqs = I2cDevice.GetDeviceSelector(interfaceName);
            var info = await DeviceInformation.FindAllAsync(aqs);
            var connectSetting = new I2cConnectionSettings(address);
            connectSetting.BusSpeed = busSpeed;
            _pcf8575 = await I2cDevice.FromIdAsync(info.First().Id, connectSetting);

            var gpioController = await GpioController.GetDefaultAsync();
            _interrupt = gpioController.OpenPin(pinInterrupt);

            if (_pcf8575 == null)
                return false;

            _interrupt.SetDriveMode(GpioPinDriveMode.InputPullUp);
            _interrupt.DebounceTimeout = timeout;
            _interrupt.ValueChanged += Interrupt_ValueChanged;

            return true;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _pcf8575.Dispose();
            _interrupt.Dispose();
        }

        /// <inheritdoc />
        public void SetPinState(ushort pin)
        {
            byte[] rxData = new byte[2];
            byte[] txData = new byte[2];

            // считываем состояние выводов, чтобы сохранить конфигурацию.
            _pcf8575.Read(rxData);
            txData[0] = (byte)(rxData[0] ^ pin >> 8);
            txData[1] = (byte)(rxData[1] ^ pin);
            _pcf8575.Write(txData);
        }

        /// <inheritdoc />
        public byte[] ReadPinState()
        {
            byte[] rxData = new byte[2];

            _pcf8575.Read(rxData);

            return rxData;
        }

        /// <inheritdoc />
        public void ResetState()
        {
            byte[] txData = { 0xFF, 0xFF };

            _pcf8575.Write(txData);
        }
    }
}