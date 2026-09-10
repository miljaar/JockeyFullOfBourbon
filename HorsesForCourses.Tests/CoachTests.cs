using HorsesForCourses.Domain.Coaches;
using HorsesForCourses.Domain.Coaches.InvalidationReasons;
using HorsesForCourses.Domain.Courses.InvalidationReasons;
using HorsesForCourses.Domain.Skills;

namespace HorsesForCourses.Tests;

public class CoachTests : DomainTest
{
    [Fact]
    public void Coach_Creation_Accept_Name_And_Mail()
    {
        var coach = Coach.Create("Xavier", "xavier@vrt.be");
        Assert.IsType<Coach>(coach);
        Assert.Equal("Xavier", coach.Name.Value);
        Assert.Equal("xavier@vrt.be", coach.Email.Value);
    }

    [Fact]
    public void Coach_Does_Accept_Skills()
    {
        Assert.Equal(3, GetCoach().Skills.Count);
    }

    [Fact]
    public void Coach_Can_Update_Skills()
    {
        var coach = GetCoach();
        coach.UpdateSkills(["Chemie"]);
        Assert.DoesNotContain(Skill.From("Wiskunde"), coach.Skills);
    }

    [Fact]
    public void Coach_Can_Update_Skills_Atomic()
    {
        var coach = GetCoach();
        coach.UpdateSkills(["Chemie"]);
        Assert.Throws<SkillValueCanNotBeEmpty>(() => coach.UpdateSkills([""]));
        Assert.DoesNotContain(Skill.From("Chemie"), coach.Skills);
    }

    [Fact]
    public void Coach_Throws_When_Duplicate_Skills()
    {
        var coach = Coach.Create("Xavier", "xavier@vrt.be");

        var exception = Assert.Throws<CoachAlreadyHasSkill>(() =>
            coach.UpdateSkills(["Wiskunde", "Taal", "Wiskunde", "Taal"]));
        Assert.Empty(coach.Skills);
        Assert.Equal("Wiskunde,Taal", exception.Message);
    }

    [Fact]
    public void Coach_Can_Assign_Valid_Course_And_Does_Not_Accept_Duplicates()
    {
        var coach = GetCoach();

        var course = GetCourse().AssignCoach(coach);

        Assert.Single(coach.AssignedCourses);
        Assert.Contains(course, coach.AssignedCourses);
        coach.AssignCourse(course);
        Assert.Single(coach.AssignedCourses);
    }

    [Fact]
    public void Coach_Throws_When_Assigned_Notconfirmed_Course()
    {
        var coach = GetCoach();

        var course = GetCourse(confirned: false);

        Assert.Throws<CourseNotYetConfirmed>(() => coach.AssignCourse(course));
        Assert.DoesNotContain(course, coach.AssignedCourses);
    }

    [Fact]
    public void Coach_Throws_When_Trying_Assigning_Course_Before_Coach()
    {
        var coach = GetCoach();

        var course = GetCourse();

        Assert.Throws<CoachCourseAssignmentOutOfSync>(() => coach.AssignCourse(course));
        Assert.DoesNotContain(course, coach.AssignedCourses);
    }
}