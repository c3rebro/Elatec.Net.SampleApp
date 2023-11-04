using Elatec.NET;
using Elatec.NET.Model;

using System;
using System.Threading.Tasks;

namespace ElatecNetSampleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ChipModel chip = new ChipModel();

            //portNumber 0 is auto Detect e.g. USB it is not implemented to be changed during runtime
            ReaderDevice readerDevice = new ReaderDevice(0);

            if(ReaderDevice.Instance.Connect())
            {
                await ReaderDevice.Instance.BeepAsync(3, 500, 1500, 100);
                chip = ReaderDevice.Instance.GetSingleChip(true); // HF = true / only 13,56Mhz chips please (False = 125kHz)

                Console.WriteLine("CardType: {0}, UID: {1}, Multitype: {2}", chip.CardType, chip.UID, chip.Slave != null ? chip.Slave.CardType.ToString() : "no other technology");

                switch (chip.CardType)
                {
                    case ChipType.DESFireEV1_8K:
                        Console.WriteLine("\nDesfire EV1 8K found.\nList AppIDs if any:\n\n");

                        foreach (uint appID in await ReaderDevice.Instance.GetDesfireAppIDsAsync() ?? new uint[] {0x0})
                        {
                            Console.WriteLine("Found: AppID {0}\n", appID);
                        }

                        break;

                    default:
                        Console.WriteLine(chip.CardType.ToString());
                        break;
                }
            }
        }
    }

    public class ReaderDevice
    {
        public ReaderDevice() { }
        public ReaderDevice(int portNumber) { PortNumber = portNumber; }

        private static object syncRoot = new object();
        private static TWN4ReaderDevice instance;

        public static int PortNumber { get; set; } 

        // Singleton, do not create new object when already occupied
        public static TWN4ReaderDevice Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new TWN4ReaderDevice(PortNumber);
                        return instance;
                    }
                    else if (instance != null && !(instance is TWN4ReaderDevice))
                    {
                        instance = new TWN4ReaderDevice(PortNumber);
                        return instance;
                    }
                    else
                    {
                        return instance;
                    }

                }

            }
        }
    }
}
