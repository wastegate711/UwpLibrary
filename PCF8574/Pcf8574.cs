using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;

namespace PortExpanders.PCF8574
{
    public class Pcf8574 : IPcf8574, IDisposable
    {
        private I2cDevice _pcf8574;
        private GpioPin _interrupt;

        /// <inheritdoc />
        public event Action<byte> PinChanged;


        /// <inheritdoc />
        public void Dispose()
        {
            _pcf8574.Dispose();
            _interrupt.Dispose();
        }

        /// <inheritdoc />
        public async Task<bool> Initialization(int address, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode)
        {
            var connectSetting = new I2cConnectionSettings(address);
            connectSetting.BusSpeed = busSpeed;
            var controller = await I2cController.GetDefaultAsync();
            _pcf8574 = controller.GetDevice(connectSetting);

            if (_pcf8574 == null)
                return false;

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> Initialization(
            string interfaceName,
            int address,
            I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode)
        {
            var aqs = I2cDevice.GetDeviceSelector(interfaceName);
            var info = await DeviceInformation.FindAllAsync(aqs);
            var connectSetting = new I2cConnectionSettings(address);
            connectSetting.BusSpeed = busSpeed;
            _pcf8574 = await I2cDevice.FromIdAsync(info.First().Id, connectSetting);

            if (_pcf8574 == null)
                return false;

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> Initialization(int address, int pinInterrupt, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode)
        {
            var connectSetting = new I2cConnectionSettings(address);
            connectSetting.BusSpeed = busSpeed;
            var controller = await I2cController.GetDefaultAsync();
            _pcf8574 = controller.GetDevice(connectSetting);

            var gpioController = await GpioController.GetDefaultAsync();
            _interrupt = gpioController.OpenPin(pinInterrupt);

            if (_pcf8574 == null | _interrupt == null)
                return false;

            _interrupt.SetDriveMode(GpioPinDriveMode.InputPullUp);
            _interrupt.DebounceTimeout = TimeSpan.FromMilliseconds(1);
            _interrupt.ValueChanged += Interrupt_ValueChanged;

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> Initialization(
            string interfaceName,
            int address,
            int pinInterrupt,
            I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode)
        {
            var aqs = I2cDevice.GetDeviceSelector(interfaceName);
            var info = await DeviceInformation.FindAllAsync(aqs);
            var connectSetting = new I2cConnectionSettings(address);
            connectSetting.BusSpeed = busSpeed;
            _pcf8574 = await I2cDevice.FromIdAsync(info.First().Id, connectSetting);

            var gpioController = await GpioController.GetDefaultAsync();
            _interrupt = gpioController.OpenPin(pinInterrupt);

            if (_pcf8574 == null)
                return false;

            _interrupt.SetDriveMode(GpioPinDriveMode.InputPullUp);
            _interrupt.DebounceTimeout = TimeSpan.FromMilliseconds(1);
            _interrupt.ValueChanged += Interrupt_ValueChanged;

            return true;
        }

        /// <inheritdoc />
        public void SetPinState(byte pin)
        {
            byte[] rxData = new byte[1];
            byte[]txData= new byte[1];

            _pcf8574.Read(rxData);
            txData[0] = (byte)(rxData[0] ^ pin);
            _pcf8574.Write(txData);
        }

        /// <inheritdoc />
        public byte ReadPinState()
        {
            byte[] rxData = new byte[1];

            _pcf8574.Read(rxData);

            return rxData[0];
        }

        private void Interrupt_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            byte[] rxData = new byte[1];

            if (args.Edge == GpioPinEdge.FallingEdge)
            {
                _pcf8574.Read(rxData);
                PinChanged?.Invoke(rxData[0]);
            }
        }
    }
}
