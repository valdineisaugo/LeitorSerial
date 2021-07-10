
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LeitorSerialPesagem
{
    class Program
    {
        static void Main(string[] args)
        {
            string model = args[0];
            string comPort = args[1];
            int baud = Int32.Parse(args[2]);
            readSerialPort(comPort, baud, model);
        }
        static void readSerialPort(string port, int baudRate, string model)
        {
            SerialPort port1 = new SerialPort();
            try
            {
                port1.PortName = port;
                port1.BaudRate = baudRate;
                port1.ReadTimeout = 2000;
                port1.DataBits = 8;
                port1.Parity = Parity.None;
                port1.Open();
                if (model.Equals("0") || model.Equals("1") || model.Equals("2")) // Digi-tron
                {
                    string data = "";
                    int entrada = 0;
                    do
                    {
                        entrada = port1.ReadByte();
                        if (entrada == 13)
                        {
                            for (int i = 0; i <= 7; i++)
                            {
                                entrada = port1.ReadByte();
                                data += (char)entrada;
                            }
                            break;
                        }

                    } while (true);
                    if (data.Length == 8)
                    {
                        Console.WriteLine(Regex.Replace(data, @"[^0-9]", "").Trim());
                        //Console.WriteLine(data.Trim().Replace(".", ""));
                    }
                    else
                        Console.WriteLine("999");

                }
                else if (model.Equals("4")) // 9091 // 8530-40
                {
                    string data = "";
                    int entrada = 0;
                    do
                    {
                        entrada = port1.ReadByte();
                        if (entrada == 2)
                        {
                            for (int i = 0; i <= 14; i++)
                            {
                                entrada = port1.ReadByte();
                                data += (char)entrada;
                            }
                            break;
                        }

                    } while (true);
                    if (data.Length == 15)
                    {
                        //data = Regex.Replace(data, @"[^0-9]", "");
                        data = data.Substring(3, 6);
                        //Console.WriteLine("B1 : " + data);
                        if (data.Length == 6)
                        {
                            data = data.Replace(" ", "0");
                            Console.WriteLine(Regex.Replace(data, @"[^0-9]", ""));
                        }
                        else
                            Console.WriteLine("999");

                    }
                    else
                        Console.WriteLine("999");

                }
                else if (model.Equals("6")) //8530-40          
                {
                    string data = "";
                    int entrada = 0;
                    do
                    {
                        entrada = port1.ReadByte();
                        if (entrada == 13)
                        {
                            for (int i = 0; i <= 8; i++)
                            {
                                entrada = port1.ReadByte();
                                data += (char)entrada;
                            }
                            break;
                        }

                    } while (true);
                    // Console.WriteLine(data + " - " + data.Length); 
                    if (data.Length == 9)
                    {
                        Console.WriteLine(Regex.Replace(data, @"[^0-9]", "").Substring(1));
                    }
                    else
                        Console.WriteLine("999");

                }
                else if (model.Equals("5")) //BJ850
                {
                    string data = "";
                    int entrada = 0;
                    do
                    {
                        entrada = port1.ReadByte();
                        if (entrada == 10)
                        {
                            for (int i = 0; i <= 7; i++)
                            {
                                entrada = port1.ReadByte();
                                data += (char)entrada;
                            }
                            break;
                        }
                        //else
                        //  Console.WriteLine("999");

                    } while (true);
                    Console.WriteLine(Regex.Replace(data, @"[^0-9]", ""));
                }
                else if (model.Equals("7")) //Saturno SBR
                {
                    string data = "";
                    int entrada = 0;
                    do
                    {
                        entrada = port1.ReadByte();
                        if (entrada == 10)
                        {
                            for (int i = 0; i <= 7; i++)
                            {
                                entrada = port1.ReadByte();
                                data += (char)entrada;
                            }
                            break;
                        }

                    } while (true);
                    Console.WriteLine(Regex.Replace(data, @"[^0-9]", ""));
                }
                else if (model.Equals("8")) //WT21          
                {
                    string data = "";
                    int entrada = 0;
                    do
                    {
                        entrada = port1.ReadByte();
                        if (entrada == 10)
                        {
                            for (int i = 0; i <= 15; i++)
                            {
                                entrada = port1.ReadByte();
                                if(entrada == 32)
                                    data += '0';
                                else
                                    data += (char)entrada;
                            }
                            break;
                        }

                    } while (true);
                    //Console.WriteLine(data + " - " + data.Length); 
                    if (data.Length == 16)
                    {

                        var result = data.Split('+')[1];
                        Console.WriteLine(Regex.Replace(result, @"[^0-9]", "").Substring(1));
                    }
                    else
                        Console.WriteLine("999");

                }
            }
            catch (Exception)
            {
                if (port1.IsOpen)
                {
                    port1.Close();

                }
                Console.WriteLine("999");
                System.Environment.Exit(0);
            }

        }
    }
}
