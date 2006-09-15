using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Data;
using System.Net;
using Modbus.Util;

namespace Modbus.Message
{
	class WriteSingleCoilRequestResponse : ModbusMessageWithData<RegisterCollection>, IModbusMessage
	{
		private const int _minimumFrameSize = 6;

		public WriteSingleCoilRequestResponse()
		{
		}

		public WriteSingleCoilRequestResponse(byte slaveAddress, ushort startAddress, bool coilState)
			: base(slaveAddress, Modbus.WriteSingleCoil)
		{
			StartAddress = startAddress;
			Data = new RegisterCollection(coilState ? Modbus.CoilOn : Modbus.CoilOff);
		}

		public override int MinimumFrameSize
		{
			get { return _minimumFrameSize; }
		}

		public ushort StartAddress
		{
			get { return MessageImpl.StartAddress; }
			set { MessageImpl.StartAddress = value; }
		}

		protected override void InitializeUnique(byte[] frame)
		{
			StartAddress = (ushort) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
			Data = new RegisterCollection(CollectionUtil.Slice<byte>(frame, 4, 2));
		}
	}
}
