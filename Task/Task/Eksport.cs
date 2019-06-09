using System;

namespace Task
{
    internal class Eksport
    {
        public string Nazwa { get; set; }
        public DateTime? Data { get; set; }
        public TimeSpan? Godzina { get; set; }
        public string Uzytkownik { get; set; }
        public string Lokal { get; set; }
    }
}