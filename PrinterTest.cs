using System;
using System.IO;
using System.Text;
using System.Threading;
using System.IO.Ports;
//using System.Collections.Generic;
//using System.Drawing;
using ThermalDotNet;
using Microsoft.SPOT;

namespace ThermalPrinterTestApp
{
	class PrinterClass
	{
        SerialPort printerPort;
        ThermalPrinter printer;

        public PrinterClass(string printerPortName = "COM3")
        {
            //Serial port init
            printerPort = new SerialPort(printerPortName, 19200);

            if (printerPort != null)
            {
                Debug.Print("Port ok");
                if (printerPort.IsOpen)
                {
                    printerPort.Close();
                }
            }

            Debug.Print("Opening port");

            try
            {
                printerPort.Open();
            }
            catch
            {
                Debug.Print("I/O error");
                //Environment.Exit(0);
            }

            //Printer init
            printer = new ThermalPrinter(printerPort, 9, 110, 10);
            printer.Reset();
        }

        public void TestBarcode()
		{
            printer.WakeUp(); 
            ThermalPrinter.BarcodeType myType = ThermalPrinter.BarcodeType.ean13;
			string myData = "3350030103392";
            printer.SetBarcodeLeftSpace(25);
			printer.WriteLine(myType.ToString() + ", data: " + myData);
			printer.SetLargeBarcode(true);
			printer.LineFeed();
			printer.PrintBarcode(myType,myData);
            printer.LineFeed(2);
		}

        /*
		static void TestImage(ThermalPrinter printer)
		{
			printer.WriteLine("Test image:");
			Bitmap img = new Bitmap("../../../mono-logo.png");
			printer.LineFeed();
			printer.PrintImage(img);
			printer.LineFeed();
			printer.WriteLine("Image OK");
		}*/

        public void PrintTest()
		{
			printer.WakeUp();
            Debug.Print(printer.ToString());
			
			//System.Threading.Thread.Sleep(5000);
			printer.SetBarcodeLeftSpace(25);
			TestBarcode();
            printer.LineFeed(3);
			
			//System.Threading.Thread.Sleep(5000);
			//TestImage();

            //System.Threading.Thread.Sleep(5000);

			printer.WriteLineSleepTimeMs = 200;
			printer.WriteLine("Default style");
			printer.WriteLine("PrintingStyle.Bold",ThermalPrinter.PrintingStyle.Bold);
			printer.WriteLine("PrintingStyle.DeleteLine",ThermalPrinter.PrintingStyle.DeleteLine);
			printer.WriteLine("PrintingStyle.DoubleHeight",ThermalPrinter.PrintingStyle.DoubleHeight);
			printer.WriteLine("PrintingStyle.DoubleWidth",ThermalPrinter.PrintingStyle.DoubleWidth);
			printer.WriteLine("PrintingStyle.Reverse",ThermalPrinter.PrintingStyle.Reverse);
			printer.WriteLine("PrintingStyle.Underline",ThermalPrinter.PrintingStyle.Underline);
			printer.WriteLine("PrintingStyle.Updown",ThermalPrinter.PrintingStyle.Updown);
			printer.WriteLine("PrintingStyle.ThickUnderline",ThermalPrinter.PrintingStyle.ThickUnderline);
			printer.SetAlignCenter();
			printer.WriteLine("BIG TEXT!",((byte)ThermalPrinter.PrintingStyle.Bold +
				(byte)ThermalPrinter.PrintingStyle.DoubleHeight +
				(byte)ThermalPrinter.PrintingStyle.DoubleWidth));
			printer.SetAlignLeft();
			printer.WriteLine("Default style again");			
			printer.LineFeed(3);

            printer.Sleep();
		}
	}
}
