﻿using System;
using System.Threading;
using NAudio.Wave;
using System.IO;

namespace Tetris
{
    public class Program
    {
        private static IWavePlayer waveOut;
        private static AudioFileReader audioFile;
        private static string[] playlist =
        {
            "7pm.mp3",
            "9pm.mp3",
            "3-32 Museum - Welcome to the Museum!.mp3",
        };
        private static Random random = new Random();

        public static void Main(string[] args)
        {
            PlayBackgroundMusic();

        Start:
            string input;
            bool gameOver = false;
            Tetrominos.NewPiece();
            Thread inputThread = new Thread(Tetrominos.ReadInput);
            inputThread.Start();
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;

            while (true)
            {
                if (Grid.IsGameOver())
                {
                    Console.Clear();
                    Console.WriteLine("G A M E  O V E R");
                    Console.WriteLine($"Score: {Grid.score}");
                    Console.WriteLine();
                    Console.WriteLine("www.youtube.com/watch?v=sDipbctxGC4");
                    Console.WriteLine("        .\r\n       -.\\_.--._.______.-------.___.---------.___\r\n       )`.                                       `-._\r\n      (                                              `---.\r\n      /o                                                  `.\r\n     (                                                      \\\r\n   _.'`.  _                                                  L\r\n   .'/| \"\" \"\"\"\"._                                            |\r\n      |          \\             |                             J\r\n                  \\-._          \\                             L\r\n                  /   `-.        \\                            J\r\n                 /      /`-.      )_                           `\r\n                /    .-'    `    J  \"\"\"\"-----.`-._             |\\            \r\n              .'   .'        L   F            `-. `-.___        \\`.\r\n           ._/   .'          )  )                `-    .'\"\"\"\"`.  \\)\r\n__________((  _.'__       .-'  J              _.-'   .'        `. \\\r\n                   \"\"\"\"\"\"\"((  .'--.__________(   _.-'___________)..|----------------._____\r\n                            \"\"                \"\"\"               ``U'\r\n");
                    Console.WriteLine("Brett Mariani 2025");

                    input = Console.ReadLine();
                    if (input == "yes")
                    {
                        Console.Clear();
                        goto Start;
                    }
                    else
                        break;
                }

                if (!Tetrominos.pause)
                {
                    Grid.ClearFullRows();
                    Tetrominos.DrawBoard();
                    Thread.Sleep(Math.Max(50, 400 - (Grid.score * 10)));
                    Tetrominos.MovePiece(0, 1);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            Console.SetCursorPosition(0, 0);
            goto Start;
        }

        private static void PlayBackgroundMusic()
        {
            try
            {
                PlayRandomSong();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing sound: {ex.Message}");
            }
        }

        private static void PlayRandomSong()
        {
            if (waveOut != null)
            {
                waveOut.Dispose();
                audioFile.Dispose();
            }

            int randomIndex = random.Next(playlist.Length); // Get a random index
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, playlist[randomIndex]);

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Error: MP3 file not found at " + filePath);
                return;
            }

            waveOut = new WaveOutEvent();
            audioFile = new AudioFileReader(filePath);

            waveOut.PlaybackStopped += (s, e) =>
            {
                PlayRandomSong(); // Play another random song when current one ends
            };

            waveOut.Init(audioFile);
            waveOut.Play();
        }
    }
}