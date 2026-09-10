using HorsesForCourses.Abstractions;
using HorsesForCourses.Domain.Coaches.InvalidationReasons;
using HorsesForCourses.Domain.Courses;
using HorsesForCourses.Domain.Skills;
using HorsesForCourses.ValidationHelpers;
using HorsesForCourses.Domain.Courses.InvalidationReasons;

namespace HorsesForCourses.Domain.Coaches;

public class Coach : DomainEntity<Coach>
{
    public CoachName Name { get; init; } = CoachName.Empty;
    public CoachEmail Email { get; init; } = CoachEmail.Empty;

    public IReadOnlySet<Skill> Skills => skills.ToHashSet();
    private readonly HashSet<Skill> skills = [];

    public IReadOnlyList<Course> AssignedCourses => assignedCourses.AsReadOnly();
    private readonly List<Course> assignedCourses = [];

    private Coach(string name, string email)
    {
        Name = new CoachName(name);
        Email = new CoachEmail(email);
    }

    public static Coach Create(string name, string email)
    {
        return new(name, email);
    }

    public virtual Coach UpdateSkills(IEnumerable<string> newSkills)
    {
        NotAllowedWhenThereAreDuplicateSkills();
        OverwriteSkills();
        return this;
        void NotAllowedWhenThereAreDuplicateSkills()
            => newSkills.NoDuplicatesAllowed(a => new CoachAlreadyHasSkill(string.Join(",", a)));
        void OverwriteSkills()
        {
            var newSkillObj = newSkills.Select(Skill.From)
                .ToList();
            skills.Clear();
            newSkillObj.ForEach(a => skills.Add(a));
        }
    }

    public bool IsSuitableFor(Course course)
        => course.RequiredSkills.All(Skills.Contains);

    public bool IsAvailableFor(Course course)
        => CheckIf.ImAvailable(this).For(course);

    public void AssignCourse(Course course)
    {
        if (!course.IsConfirmed)
            throw new CourseNotYetConfirmed();

        if (!ReferenceEquals(course.AssignedCoach, this))
            throw new CoachCourseAssignmentOutOfSync();

        if (assignedCourses.Contains(course))
            return;

        assignedCourses.Add(course);
    }
}
