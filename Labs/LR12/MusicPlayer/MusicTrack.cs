using System;

namespace MusicPlayer.Models
{
    public class MusicTrack
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public TimeSpan? Duration { get; set; }
        public string FilePath { get; set; }
        public int PlayCount { get; set; }
        public DateTime DateAdded { get; set; }
        public byte[] AudioData { get; set; }

        public string DurationFormatted => Duration?.ToString(@"mm\:ss") ?? "00:00";
        public string DateAddedFormatted => DateAdded.ToString("dd.MM.yyyy HH:mm");
    }
}