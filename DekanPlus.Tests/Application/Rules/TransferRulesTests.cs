using DekanPlus.Application.Common.Rules;
using DekanPlus.Application.Common.Rules.Implementations.Students;
using DekanPlus.Tests.TestHelpers;

namespace DekanPlus.Tests.Application.Rules;

public class TransferRulesTests
{
    /// <summary>Перевести студента до тієї самої групи, в якій він уже навчається, не можна.</summary>
    [Fact]
    public void TransferTarget_SameGroup_IsRejected()
    {
        var group = TestFactory.CreateGroup();
        var student = TestFactory.CreateStudentInGroup(group);

        var rule = new TransferTargetRule();

        Assert.False(rule.Apply(new TransferStudentContext(student, group)).IsSuccess);
        Assert.True(rule.Apply(new TransferStudentContext(student, TestFactory.CreateGroup(group.SpecialtyId))).IsSuccess);
    }

    /// <summary>
    /// Переведення в групу іншого фаху заборонене, доки це не дозволено в налаштуваннях;
    /// у межах того самого фаху переведення завжди можливе.
    /// </summary>
    [Fact]
    public void TransferSameSpecialty_DifferentSpecialty_DependsOnOption()
    {
        var currentGroup = TestFactory.CreateGroup();
        var student = TestFactory.CreateStudentInGroup(currentGroup);
        var sameSpecialtyGroup = TestFactory.CreateGroup(currentGroup.SpecialtyId);
        var otherSpecialtyGroup = TestFactory.CreateGroup();

        var strict = new TransferSameSpecialtyRule(new StudentRulesOptions { AllowTransferBetweenSpecialties = false });
        var lenient = new TransferSameSpecialtyRule(new StudentRulesOptions { AllowTransferBetweenSpecialties = true });

        Assert.False(strict.Apply(new TransferStudentContext(student, otherSpecialtyGroup)).IsSuccess);
        Assert.True(strict.Apply(new TransferStudentContext(student, sameSpecialtyGroup)).IsSuccess);
        Assert.True(lenient.Apply(new TransferStudentContext(student, otherSpecialtyGroup)).IsSuccess);
    }
}
