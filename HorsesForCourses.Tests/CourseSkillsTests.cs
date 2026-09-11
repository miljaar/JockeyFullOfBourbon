using HorsesForCourses.Domain.Courses;
using HorsesForCourses.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Domain.Courses.TimeSlots;
using HorsesForCourses.Domain.Skills;

namespace HorsesForCourses.Tests;

public class CourseSkillsTests : DomainTest
{
    [Fact]
    public void Course_Does_Accept_Skills()
    {
        var course = Course.Create("Statistiek"
            , new DateOnly(2026, 01, 01)
            , new DateOnly(2026, 12, 31))
            .UpdateRequiredSkills(["Wiskunde", "Taal"]);

        Assert.Equal(2, course.RequiredSkills.Count);
    }

    [Fact]
    public void Course_Can_Update_Skills()
    {
        var course = Course.Create("Statistiek"
            , new DateOnly(2026, 01, 01)
            , new DateOnly(2026, 12, 31))
            .UpdateRequiredSkills(["Wiskunde", "Taal"]);
        course.UpdateRequiredSkills(["Chemie"]);
        Assert.DoesNotContain(Skill.From("Wiskunde"), course.RequiredSkills);
        Assert.DoesNotContain(Skill.From("Taal"), course.RequiredSkills);
    }

    [Fact]
    public void Course_Can_Update_Skills_Atomic()
    {
        var course = Course.Create("Statistiek"
            , new DateOnly(2026, 01, 01)
            , new DateOnly(2026, 12, 31))
            .UpdateRequiredSkills(["Wiskunde", "Taal"]);
        course.UpdateRequiredSkills(["Chemie"]);
        Assert.DoesNotContain(Skill.From("Wiskunde"), course.RequiredSkills);
        Assert.DoesNotContain(Skill.From("Taal"), course.RequiredSkills);
        Assert.Throws<SkillValueCanNotBeEmpty>(() => course.UpdateRequiredSkills([""]));
        Assert.Contains(Skill.From("Chemie"), course.RequiredSkills);
    }

    [Fact]
    public void Course_Throws_When_Duplicate_Skills()
    {
        var course = Course.Create("Statistiek"
            , new DateOnly(2026, 01, 01)
            , new DateOnly(2026, 12, 31));

        var exception = Assert.Throws<CourseAlreadyHasSkill>(() =>
            course.UpdateRequiredSkills(["Wiskunde", "Taal", "Wiskunde", "Taal"]));
        Assert.Empty(course.RequiredSkills);
        Assert.Equal("Wiskunde,Taal", exception.Message);
    }

    [Fact]
    public void Course_Throws_When_Updating_Skills_After_Confirmed()
    {
        var course = Course.Create("Statistiek"
            , new DateOnly(2026, 01, 01)
            , new DateOnly(2026, 12, 31))
            .UpdateRequiredSkills(["Wiskunde"])
            .UpdateTimeSlots([(CourseDay.Monday, 10, 16)], x => x)
            .Confirm();

        Assert.Throws<CourseAlreadyConfirmed>(() =>
            course.UpdateRequiredSkills(["Chemie"]));
        Assert.Equal([Skill.From("Wiskunde")], course.RequiredSkills);
    }
}