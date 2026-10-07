using DekanPlus.Application.Common.Rules.Interfaces;
using DekanPlus.Domain.Entities;
using DekanPlus.Domain.Enums;

namespace DekanPlus.Application.Common.Rules;

public record EnrollStudentContext(
    DateOnly BirthDate,
    int EnrollmentYear,
    bool ActiveStudentExists) : IRuleContext;

public record ExpelStudentContext(
    Student Student,
    DateOnly Date,
    ExpulsionReason Reason) : IRuleContext;

public record TransferStudentContext(
    Student Student,
    Group TargetGroup) : IRuleContext;

public record ReinstateStudentContext(
    Student Student) : IRuleContext;

public record CreateSpecialtyContext(
    Direction Direction,
    string StateCode) : IRuleContext;

public record ChangeSpecialtyStateCodeContext(
    Direction Direction,
    string StateCode) : IRuleContext;
