using HorsesForCourses.Domain.Coaches;
using HorsesForCourses.Domain.Courses;
using HorsesForCourses.Domain.Courses.TimeSlots;

namespace HorsesForCourses.Tests;

public class DomainTest
{
    public DomainTest()
    {

    }

    public Coach GetCoach(string name = "Xavier", List<string>? skills = null)
    {
        skills ??= ["Wiskunde", "Taal", "Geschiedenis"];
        return Coach.Create(name, $"{name.ToLower()}@vrt.be")
            .UpdateSkills(skills);
    }

    public Course GetCourse(string name = "Statistiek"
        , DateOnly? start = null
        , DateOnly? stop = null
        , bool confirned = true
        , List<(CourseDay Day, int Start, int Stop)>? timeSlots = null
        , List<string>? requiredSkills = null)
    {
        var startDate = start ??= new DateOnly(2026, 01, 01);
        var stopDate = stop ??= new DateOnly(2026, 12, 31);
        timeSlots ??= [(CourseDay.Monday, 10, 16)];
        requiredSkills ??= ["Wiskunde", "Taal"];

        var course = Course.Create(name
            , startDate
            , stopDate)
            .UpdateRequiredSkills(requiredSkills)
            .UpdateTimeSlots(timeSlots, x => x);

        if (confirned)
            course.Confirm();

        return course;
    }
}