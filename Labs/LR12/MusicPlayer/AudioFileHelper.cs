using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicPlayer
{
    public class AudioFileHelper
    {
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static byte[] ReadAudioFile(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        public static string CreateTempFile(byte[] audioData, string extension = ".mp3")
        {
            string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + extension);
            File.WriteAllBytes(tempFile, audioData);
            return tempFile;
        }

        // Простое чтение ID3 тегов из MP3 файла
        public static Tuple<string, string, string, TimeSpan> ReadMetadata(string filePath)
        {
            string title = Path.GetFileNameWithoutExtension(filePath);
            string artist = "Неизвестен";
            string album = null;
            TimeSpan duration = TimeSpan.Zero;

            try
            {
                if (Path.GetExtension(filePath).ToLower() == ".mp3")
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        // Проверяем наличие ID3v2 тега
                        fs.Seek(0, SeekOrigin.Begin);
                        byte[] header = new byte[3];
                        fs.Read(header, 0, 3);

                        if (header[0] == 0x49 && header[1] == 0x44 && header[2] == 0x33) // "ID3"
                        {
                            // Пропускаем заголовок ID3v2 (10 байт)
                            fs.Seek(10, SeekOrigin.Begin);

                            // Ищем фрейм TIT2 (название)
                            string frameId = "";
                            while (fs.Position < fs.Length - 10)
                            {
                                byte[] frameHeader = new byte[10];
                                fs.Read(frameHeader, 0, 10);
                                frameId = Encoding.ASCII.GetString(frameHeader, 0, 4);

                                int frameSize = (frameHeader[4] << 24) | (frameHeader[5] << 16) |
                                                (frameHeader[6] << 8) | frameHeader[7];

                                if (frameId == "TIT2") // Название
                                {
                                    byte[] data = new byte[frameSize];
                                    fs.Read(data, 0, frameSize);
                                    title = Encoding.UTF8.GetString(data, 1, data.Length - 1);
                                    title = title.TrimEnd('\0');
                                }
                                else if (frameId == "TPE1") // Исполнитель
                                {
                                    byte[] data = new byte[frameSize];
                                    fs.Read(data, 0, frameSize);
                                    artist = Encoding.UTF8.GetString(data, 1, data.Length - 1);
                                    artist = artist.TrimEnd('\0');
                                }
                                else if (frameId == "TALB") // Альбом
                                {
                                    byte[] data = new byte[frameSize];
                                    fs.Read(data, 0, frameSize);
                                    album = Encoding.UTF8.GetString(data, 1, data.Length - 1);
                                    album = album.TrimEnd('\0');
                                }
                                else
                                {
                                    fs.Seek(frameSize, SeekOrigin.Current);
                                }
                            }
                        }
                    }

                    // Примерная длительность MP3 (битрейт 128kbps)
                    FileInfo fi = new FileInfo(filePath);
                    long fileSize = fi.Length;
                    duration = TimeSpan.FromSeconds((fileSize / 128.0) / 125.0);
                }
                else if (Path.GetExtension(filePath).ToLower() == ".wav")
                {
                    // Чтение WAV заголовка
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        fs.Seek(40, SeekOrigin.Begin);
                        byte[] dataSizeBytes = new byte[4];
                        fs.Read(dataSizeBytes, 0, 4);
                        int dataSize = BitConverter.ToInt32(dataSizeBytes, 0);

                        fs.Seek(24, SeekOrigin.Begin);
                        byte[] sampleRateBytes = new byte[4];
                        fs.Read(sampleRateBytes, 0, 4);
                        int sampleRate = BitConverter.ToInt32(sampleRateBytes, 0);

                        fs.Seek(34, SeekOrigin.Begin);
                        byte[] bitsPerSampleBytes = new byte[2];
                        fs.Read(bitsPerSampleBytes, 0, 2);
                        int bitsPerSample = BitConverter.ToInt16(bitsPerSampleBytes, 0);

                        fs.Seek(32, SeekOrigin.Begin);
                        byte[] channelsBytes = new byte[2];
                        fs.Read(channelsBytes, 0, 2);
                        int channels = BitConverter.ToInt16(channelsBytes, 0);

                        int bytesPerSecond = sampleRate * channels * (bitsPerSample / 8);
                        if (bytesPerSecond > 0)
                        {
                            duration = TimeSpan.FromSeconds((double)dataSize / bytesPerSecond);
                        }
                    }
                }
            }
            catch
            {
                // Если ошибка, используем значения по умолчанию
            }

            return Tuple.Create(title, artist, album, duration);
        }
    }
}