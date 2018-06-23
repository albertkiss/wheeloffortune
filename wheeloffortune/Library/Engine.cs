using System;
using System.Collections.Generic;
using System.Linq;

namespace wheeloffortune.Library
{
    public class Engine
    {
        public const int MaxShifts = 6;
        public const int MaxDays = 10;
        public const int MaxEngineers = 10;

        public int ShiftsPerDayCount { get; } // = 2;
        public int DayCount { get; } // = 10;
        public int EngineerCount => _engineers.Count();


        private int[,] _schedule;
        private readonly IEnumerable<int> _engineers;
        private readonly Random _randgen = new Random();

        public Engine(int shiftsPerDayCount, int dayCount, int engineersCount)
        {
            if (shiftsPerDayCount <= 0 || shiftsPerDayCount > MaxShifts)
                throw new ArgumentOutOfRangeException(nameof(shiftsPerDayCount));
            if (dayCount <= 0 || dayCount > MaxDays)
                throw new ArgumentOutOfRangeException(nameof(dayCount));
            if (engineersCount <= 0 || engineersCount >MaxEngineers)
                throw new ArgumentOutOfRangeException(nameof(engineersCount));

            ShiftsPerDayCount = shiftsPerDayCount;
            DayCount = dayCount;
            _engineers = Enumerable.Range(1, engineersCount);
        }

        public int[,] GenerateSchedule()
        {
            _schedule = new int[DayCount, ShiftsPerDayCount];

            for (int day = 0; day < DayCount; day++)
            {
                for (int shift = 0; shift < ShiftsPerDayCount; shift++)
                {
                    _schedule[day, shift] = GetEngineer(day);
                }
            }

            return _schedule;
        }

        private int GetEngineer(int today)
        {
            var availableWithShifts = _engineers.Except(GetWhoisWorking(today))
                                     .Except(GetWhoisWorking(today - 1))
                                     .ToDictionary(e => e, e => GetShifts(e, today));
            var minimumShift = availableWithShifts.Min(a => a.Value);
            var available = availableWithShifts.Where(a => a.Value == minimumShift).Select(a => a.Key).ToArray();

            return available[_randgen.Next(0, available.Length)];
        }

        private IEnumerable<int> GetWhoisWorking(int day)
        {
            var working = new List<int>();

            if (day < 0)
                return working;

            for (var i = 0; i < ShiftsPerDayCount; i++)
            {
                if (_schedule[day, i] > 0) working.Add(_schedule[day, i]);
            }

            return working;
        }

        private int GetShifts(int engineer, int day)
        {
            var shifts = 0;
            for (var d = 0; d < day; d++)
                for (var s = 0; s < ShiftsPerDayCount; s++)
                {
                    if (_schedule[d, s] == engineer) shifts++;
                }
            return shifts;
        }
    }
}