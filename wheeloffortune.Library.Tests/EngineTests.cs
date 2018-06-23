using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace wheeloffortune.Library.Tests
{
    public class EngineTests
    {
        private const int ShiftsPerDay = 2;
        private const int DayCount = 10;
        private const int EngineersCount = 10;

        [Theory]
        [InlineData(0,1,10)]
        [InlineData(11,1,10)]
        public void InvalidShiftCountThrowsException_EngineContructorTest(int shifts, int days, int engineers)
        {
            var exception=Assert.Throws<ArgumentOutOfRangeException>(()=>new Engine(shifts, days, engineers));
            Assert.Equal("shiftsPerDay",exception.ParamName);
        }

        [Theory]
        [InlineData(6, 0, 1)]
        [InlineData(6, 11, 1)]
        public void InvalidDayCountThrowsException_EngineContructorTest(int shifts, int days, int engineers)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Engine(shifts, days, engineers));
            Assert.Equal("dayCount", exception.ParamName);
        }

        [Theory]
        [InlineData(1, 10, 0)]
        [InlineData(1, 10, 11)]
        public void InvalidEngineerCountThrowsException_EngineContructorTest(int shifts, int days, int engineers)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new Engine(shifts, days, engineers));
            Assert.Equal("engineersCount", exception.ParamName);
        }

        [Fact]
        public void EngineerHasAtMostOneShiftADay_GenerateScheduleTest()
        {
            //Arrange
            var engine = new Engine(ShiftsPerDay, DayCount, EngineersCount);

            //Act
            var schedule = engine.GenerateSchedule();

            //Assert
            Assert.True(EngineerHasAtMostOneShiftADay(schedule));
        }

        [Fact]
        public void EngineersDoNotHaveShiftsOnConsecutiveDays_GenerateScheduleTest()
        {
            //Arrange
            var engine = new Engine(ShiftsPerDay, DayCount, EngineersCount);

            //Act
            var schedule = engine.GenerateSchedule();

            //Assert
            Assert.True(EngineersDoNotHaveShiftsOnConsecutiveDays(schedule));
        }

        [Fact]
        public void EachEngineerCompletedTwoShiftsInPeriod_GenerateScheduleTest()
        {
            //Arrange
            var engine = new Engine(ShiftsPerDay, DayCount, EngineersCount);

            //Act
            var schedule = engine.GenerateSchedule();

            //Assert
            Assert.True(EachEngineerCompletedTwoShiftsInPeriod(schedule));
        }


        private static bool EachEngineerCompletedTwoShiftsInPeriod(int[,] schedule)
        {
            var shiftCount = new Dictionary<int, int>();
            for (var d = 0; d < DayCount; d++)
                for (var s = 0; s < ShiftsPerDay; s++)
                {
                    var engineer = schedule[d, s];
                    if (shiftCount.ContainsKey(engineer))
                        shiftCount[engineer] += 1;
                    else
                        shiftCount.Add(engineer, 1);
                }

            return shiftCount.All(sc => sc.Value >= 2);
        }

        public bool EngineerHasAtMostOneShiftADay(int[,] schedule)
        {
            for (var d = 0; d < DayCount; d++)
            {
                var shiftCount = new HashSet<int>();
                for (var s = 0; s < ShiftsPerDay; s++)
                {
                    var engineer = schedule[d, s];
                    var added=shiftCount.Add(engineer);
                    if (!added)
                        return false;
                }
            }

            return true;
        }

        public bool EngineersDoNotHaveShiftsOnConsecutiveDays(int[,] schedule)
        {
            for (var d = 1; d < DayCount; d++)
            {
                var shifts1 = new HashSet<int>();
                var shifts2 = new HashSet<int>();
                for (var s = 0; s < ShiftsPerDay; s++)
                {
                    var engineer1 = schedule[d-1, s];
                    shifts1.Add(engineer1);
                    var engineer2 = schedule[d, s];
                    shifts2.Add(engineer2);
                }
                if (shifts1.Intersect(shifts2).Any())
                    return false;
            }

            return true;
        }
    }
}
