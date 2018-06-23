using System;
using System.Collections.Generic;
using System.Linq;
using wheeloffortune.Library.Model;
using wheeloffortune.Models;

namespace wheeloffortune.Library
{
    public class ShiftsService
    {
        private readonly StorageBase<int[,]> _scheduleStorage;
        private readonly StorageBase<IList<Engineer>> _engineerStorage;
        private readonly Engine _engine;
        private static int[,] _schedule;
        private IList<Engineer> _engineers;

        public int[,] Schedule
        {
            get
            {
                _schedule = _schedule ?? _scheduleStorage.Load();
                _schedule = _schedule ?? _engine.GenerateSchedule();

                return _schedule;
            }
        }

        public IList<EngineerView> Engineers
        {
            get
            {
                _engineers = _engineers ?? _engineerStorage.Load();
                _engineers = _engineers ??
                             Enumerable.Range(1, 10).Select(n => new Engineer
                             {
                                 Id = n,
                                 Name = $"Engineer_{n}",
                                 Address = $"Engineer_{n} address"
                             }).ToArray();

                return Map(_engineers);
            }
        }

        private static IList<EngineerView> Map(IEnumerable<Engineer> engineers)
        {
            return engineers.Select(e => new EngineerView
            {
                Name = e.Name,
                Address = e.Address
            }).ToList();
        }

        public ShiftsService(
            Engine engine,
            StorageBase<int[,]> scheduleStorage,
            StorageBase<IList<Engineer>> engineerStorage)
        {
            _engine = engine;
            _scheduleStorage = scheduleStorage;
            _engineerStorage = engineerStorage;
        }

        public IList<Shift> GetShiftsViewData()
        {
            var engineers = Engineers;
            var shiftday = DateTime.Now;
            var schedule = Schedule;
            var shifts = new List<Shift>();

            for (var day = 0; day < _engine.DayCount; day++)
            {
                while (shiftday.DayOfWeek == DayOfWeek.Saturday
                       || shiftday.DayOfWeek == DayOfWeek.Sunday)
                    shiftday=shiftday.AddDays(1);
                for (var shiftNo = 0; shiftNo < _engine.ShiftsPerDayCount; shiftNo++)
                {
                    shifts.Add(new Shift
                    {
                        EngineerName = engineers[schedule[day, shiftNo]-1].Name,
                        ShiftDate = shiftday,
                        ShiftNumber = shiftNo
                    });
                }
                shiftday = shiftday.AddDays(1);
            }

            return shifts;
        }

        public void GenerateNewSchedule()
        {
            _schedule= _engine.GenerateSchedule();
        }

        public void SaveSchedule()
        {
            _scheduleStorage.Save(Schedule);
        }

        public void LoadSchedule()
        {
            _schedule=_scheduleStorage.Load();
        }
    }
}