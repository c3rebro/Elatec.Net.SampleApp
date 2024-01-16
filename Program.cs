using Elatec.NET;
using Elatec.NET.Cards;
using Elatec.NET.Cards.Mifare;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ElatecNetSampleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var reader = TWN4ReaderDevice.Instance.FirstOrDefault();

            if (await reader.ConnectAsync())
            {
                BaseChip chip = new BaseChip();

                await reader.BeepAsync(100, 1500, 500, 100);
                chip = await reader.GetSingleChipAsync();

                Console.WriteLine("CardType: {0}, UID: {1}, Multitype: ", Enum.GetName(typeof(ChipType), chip.ChipType), chip.UIDHexString);

                switch (chip.ChipType)
                {
                    case ChipType.MIFARE:

                        await reader.PlayMelody(120, MySongs.OhWhenTheSaints);
                        await reader.LedBlinkAsync(Leds.Green, 500, 500);

                        MifareChip mifareChip = (MifareChip)chip;

                        Console.WriteLine("\nFound: {0}\n", mifareChip.SubType);

                        switch (mifareChip.SubType)
                        {
                            case MifareChipSubType.DESFireEV1_256:
                            case MifareChipSubType.DESFireEV1_2K:
                            case MifareChipSubType.DESFireEV1_4K:
                            case MifareChipSubType.DESFireEV1_8K:
                                if(reader.IsTWN4LegicReader)
                                {
                                    // undocumented in elatec's devkit (as of customersupport): if the Reader is a TWN4 Multitec with LEGIC capabilities.
                                    // SelectTag is not working. Instead, a SearchTag must be used. The SelectTag is then executed internally.
                                    if(reader.IsTWN4LegicReader)
                                    {
                                        await reader.SearchTagAsync();
                                    }
                                }
                                else
                                {
                                    await reader.ISO14443A_SelectTagAsync(chip.UID);
                                }

                                await reader.MifareDesfire_SelectApplicationAsync(0);
                                await reader.MifareDesfire_CreateApplicationAsync(
                                    DESFireAppAccessRights.KS_DEFAULT,
                                    DESFireKeyType.DF_KEY_AES,
                                    1,
                                    0x3060);

                                var appIDs = await reader.MifareDesfire_GetAppIDsAsync();

                                foreach(var appID in appIDs)
                                {
                                    Console.WriteLine("\nFound AppID(s): {0}\n", appID.ToString("X8"));
                                }
                                break;
                        }

                        break;

                    default:
                        Console.WriteLine("Chip Found: {0}", Enum.GetName(typeof(ChipType), chip.ChipType));
                        break;
                }
            }  
        }
    }

    static class MySongs
    {
        public static List<TWN4ReaderDevice.Tone> OhWhenTheSaints
        {
            get => new List<TWN4ReaderDevice.Tone>()
            {
                new TWN4ReaderDevice.Tone() { Value = 4,  Volume = 0, Pitch = NotePitch.PAUSE },
                new TWN4ReaderDevice.Tone() { Value = 4,  Pitch = NotePitch.C3 },
                new TWN4ReaderDevice.Tone() { Value = 4,  Pitch = NotePitch.E3 },
                new TWN4ReaderDevice.Tone() { Value = 4,  Pitch = NotePitch.F3 },

                new TWN4ReaderDevice.Tone() { Pitch = NotePitch.G3 },

                new TWN4ReaderDevice.Tone() { Value = 4,  Volume = 0, Pitch = NotePitch.PAUSE },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.C3 },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.E3 },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.F3 },

                new TWN4ReaderDevice.Tone() { Pitch = NotePitch.G3 },
                // 1
                new TWN4ReaderDevice.Tone() { Value = 4,  Volume = 0, Pitch = NotePitch.PAUSE },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.C3 },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.E3 },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.F3 },

                new TWN4ReaderDevice.Tone() { Value = 8, Pitch = NotePitch.G3 },
                new TWN4ReaderDevice.Tone() { Value = 8, Pitch = NotePitch.E3 },

                new TWN4ReaderDevice.Tone() { Value = 8, Pitch = NotePitch.C3 },
                new TWN4ReaderDevice.Tone() { Value = 8, Pitch = NotePitch.E3 },

                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.E3 },
                new TWN4ReaderDevice.Tone() { Value = 12, Pitch = NotePitch.D3 },
                // 2
                new TWN4ReaderDevice.Tone() { Value = 4,  Volume = 0, Pitch = NotePitch.PAUSE },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.E3 },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.E3 },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.D3 },

                new TWN4ReaderDevice.Tone() { Value = 12, Pitch = NotePitch.C3 },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.C3 },

                new TWN4ReaderDevice.Tone() { Value = 8, Pitch = NotePitch.E3 },
                new TWN4ReaderDevice.Tone() { Value = 8, Pitch = NotePitch.G3 },

                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.G3 },
                new TWN4ReaderDevice.Tone() { Value = 12, Pitch = NotePitch.F3 },
                // 3
                new TWN4ReaderDevice.Tone() { Value = 4,  Volume = 0, Pitch = NotePitch.PAUSE },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.F3 },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.E3 },
                new TWN4ReaderDevice.Tone() { Value = 4, Pitch = NotePitch.F3 },

                new TWN4ReaderDevice.Tone() { Value = 8, Pitch = NotePitch.G3 },
                new TWN4ReaderDevice.Tone() { Value = 8, Pitch = NotePitch.E3 },

                new TWN4ReaderDevice.Tone() { Value = 8, Pitch = NotePitch.C3 },
                new TWN4ReaderDevice.Tone() { Value = 8, Pitch = NotePitch.D3 },

                new TWN4ReaderDevice.Tone() { Pitch = NotePitch.C3 }
                // 4
            };
        }
    }
}
