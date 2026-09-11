using HorsesForCourses.Domain.Courses;
using HorsesForCourses.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Tests;

public class CourseTimeslotTests : DomainTest
{
    [Fact]
    public void Course_Accepts_Timeslot()
    {
        var course = Course.Create("Statistiek"
            , new DateOnly(2026, 01, 01)
            , new DateOnly(2026, 12, 31))
            .UpdateTimeSlots([
                (CourseDay.Monday, 10, 16),
                (CourseDay.Tuesday, 10, 16)], x => x);

        Assert.Equal(2, course.TimeSlots.Count);
    }

    [Fact]
    public void Course_Throws_When_Tileslots_Overlap()
    {
        var course = Course.Create("Statistiek"
            , new DateOnly(2026, 01, 01)
            , new DateOnly(2026, 12, 31))
            .UpdateTimeSlots([
                (CourseDay.Monday, 10, 16)], x => x);

        Assert.Throws<OverlappingTimeSlots>(() => course.UpdateTimeSlots([
                (CourseDay.Monday, 11, 16),
                (CourseDay.Monday, 13, 16)], x => x));
        Assert.Contains(TimeSlot.From(CourseDay.Monday, 10, 16), course.TimeSlots);
    }
}