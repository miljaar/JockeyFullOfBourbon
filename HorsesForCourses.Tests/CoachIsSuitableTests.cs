using HorsesForCourses.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Tests;

public class CoachIsSuitableTests : DomainTest
{
    [Fact]
    public void Coach_Is_Suitable_When_Skills_match()
    {
        var coach = GetCoach();

        var course = GetCourse();

        Assert.True(coach.IsSuitableFor(course));
    }

    [Fact]
    public void Coach_Is_Not_Suitable_When_Skills_Doesnt_match()
    {
        var coach = GetCoach();

        var course = GetCourse(requiredSkills: ["WO"]);
        var courseExtraSkill = GetCourse(requiredSkills: ["Wiskunde", "WO"]);

        Assert.False(coach.IsSuitableFor(course));
        Assert.False(coach.IsSuitableFor(courseExtraSkill));
    }
}