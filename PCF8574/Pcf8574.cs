using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;

namespace PCF8574
{
    public class Pcf8574 : IPcf8574, IDisposable
    {
        private I2cDevice _pcf8574;
        GpioPin _interrupt;

        public void Dispose()
        {
            _pcf8574.Dispose();
            _interrupt.Dispose();
        }

        /// <inheritdoc />
        public async void Initialization(int address, int pinNumber = int.MaxValue, I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode)
        {
            var connectSetting = new I2cConnectionSettings(address);
            connectSetting.BusSpeed = busSpeed;
            var controller = await I2cController.GetDefaultAsync();
            _pcf8574 = controller.GetDevice(connectSetting);

            if (_pcf8574 == null)
                throw new NullReferenceException("_pcf8574");

            if (pinNumber != int.MaxValue)
            {
                var gpioController = await GpioController.GetDefaultAsync();
                _interrupt = gpioController.OpenPin(pinNumber);
                _interrupt.Write(GpioPinValue.High);
                _interrupt.SetDriveMode(GpioPinDriveMode.OutputOpenDrainPullUp);
                _interrupt.ValueChanged += Interrupt_ValueChanged;
            }
        }

        /// <inheritdoc />
        public async void Initialization(string interfaceName,
            int address,
            int pinNumber = int.MaxValue,
            I2cBusSpeed busSpeed = I2cBusSpeed.StandardMode)
        {
            var list = I2cDevice.GetDeviceSelector(interfaceName);
            var connectSetting = new I2cConnectionSettings(address);
            connectSetting.BusSpeed = busSpeed;
            var devInfo = await DeviceInformation.FindAllAsync(list);
            _pcf8574 = await I2cDevice.FromIdAsync(devInfo.First().Id, connectSetting);

            if (_pcf8574 == null)
                throw new NullReferenceException("_pcf8574");

            if (pinNumber != int.MaxValue)
            {
                var gpioController = await GpioController.GetDefaultAsync();
                _interrupt = gpioController.OpenPin(pinNumber);
                _interrupt.Write(GpioPinValue.High);
                _interrupt.SetDriveMode(GpioPinDriveMode.OutputOpenDrainPullUp);
                _interrupt.ValueChanged += Interrupt_ValueChanged;
            }
        }

        private void Interrupt_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.FallingEdge)
            {

            }
        }
    }
}
