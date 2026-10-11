using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Enums;
using DekanPlus.Domain.ValueObjects;
using DekanPlus.Tests.TestHelpers;

namespace DekanPlus.Tests.Domain;

public class StudentTests
{
    private static readonly DateOnly ExpulsionDate = new(2026, 10, 1);

    /// <summary>Відрахований студент не дозволяє змінювати ПІБ, адреси, вид навчання та переводити до іншої групи.</summary>
    [Fact]
    public void ExpelledStudent_RejectsAllChanges()
    {
        var student = TestFactory.CreateStudent();
        student.Expel(ExpulsionDate, ExpulsionReason.OwnReason);

        Assert.Throws<InvalidOperationException>(() => student.ChangeFullName(FullName.Create("Петренко", "Петро")));
        Assert.Throws<InvalidOperationException>(() => student.ChangeAddresses("Нова адреса", null));
        Assert.Throws<InvalidOperationException>(() => student.ChangeStudyForm(StudyForm.Externship));
        Assert.Throws<InvalidOperationException>(() => student.TransferToGroup(Guid.CreateVersion7()));
    }

    /// <summary>Повторне відрахування вже відрахованого студента неможливе і не перезаписує першу причину.</summary>
    [Fact]
    public void Expel_AlreadyExpelled_ThrowsAndKeepsOriginalReason()
    {
        var student = TestFactory.CreateStudent();
        student.Expel(ExpulsionDate, ExpulsionReason.AcademicFailure);

        Assert.Throws<InvalidOperationException>(() => student.Expel(ExpulsionDate, ExpulsionReason.OwnReason));
        Assert.Equal(ExpulsionReason.AcademicFailure, student.Expulsion!.Reason);
    }

    /// <summary>Поновлення знімає відрахування, після чого студента знову можна змінювати; поновити не відрахованого не можна.</summary>
    [Fact]
    public void Reinstate_ClearsExpulsionAndAllowsChangesAgain()
    {
        var student = TestFactory.CreateStudent();
        student.Expel(ExpulsionDate, ExpulsionReason.OwnReason);

        student.Reinstate();

        Assert.False(student.IsExpelled);
        student.ChangeStudyForm(StudyForm.FullTimeBudget);
        Assert.Throws<InvalidOperationException>(() => student.Reinstate());
    }

    /// <summary>Невизначений вид навчання (Unknown) не приймається при створенні: це захист від значення за замовчуванням.</summary>
    [Fact]
    public void Create_UnknownStudyForm_IsRejected()
    {
        Assert.Throws<ArgumentException>(() => TestFactory.CreateStudent(studyForm: StudyForm.Unknown));
    }

    /// <summary>
    /// Відсутня причина відрахування (значення enum за замовчуванням) не має перетворюватися на «за заявою»:
    /// Unknown та неіснуюче значення відхиляються.
    /// </summary>
    [Theory]
    [InlineData(ExpulsionReason.Unknown)]
    [InlineData((ExpulsionReason)99)]
    public void Expel_InvalidReason_IsRejected(ExpulsionReason reason)
    {
        var student = TestFactory.CreateStudent();

        Assert.Throws<ArgumentException>(() => student.Expel(ExpulsionDate, reason));
        Assert.False(student.IsExpelled);
    }

    /// <summary>ПІБ обрізає зайві пробіли, а порожнє по батькові зберігається як null (відсутнє).</summary>
    [Fact]
    public void FullName_TrimsValuesAndNormalizesEmptyMiddleName()
    {
        var fullName = FullName.Create("  Іваненко ", " Іван ", "   ");

        Assert.Equal("Іваненко", fullName.LastName);
        Assert.Equal("Іван", fullName.FirstName);
        Assert.Null(fullName.MiddleName);
    }
}
