using FluentAssertions;
using Purchasely.Domain.Entities;
using Purchasely.Domain.Enums;

namespace Purchasely.UnitTests.Domain;

public class RequisitionTests
{
    private static Requisition CreateDraftRequisition(List<RequisitionLine>? lines = null)
    {
     lines ??= [CreateLine()];
     return Requisition.Create(
        priority: Purchasely.Domain.Enums.Priority.Normal,
        justification: "Test justification",
        requesterId: Guid.NewGuid(),
        lines: lines
     );
    }

    private static RequisitionLine CreateLine() =>
        RequisitionLine.Create(
            productId: Guid.NewGuid(),
            quantityRequested: 5,
            estimatedUnitPrice: 100);

    public class Create
    {
        [Fact]
        public void Should_create_requisiton_with_draft_status()
        {
            var requisition = CreateDraftRequisition();

            requisition.Status.Should().Be(Purchasely.Domain.Enums.RequisitionStatus.Draft);
        }

        [Fact]
        public void Should_create_requisition_with_lines()
        {
            var lines = new List<RequisitionLine> { CreateLine(), CreateLine() };
            var requisition = CreateDraftRequisition(lines);

            requisition.Lines.Should().HaveCount(2);
        }
    }

    public class Submit
    {
        [Fact]
        public void Should_change_status_to_submitted()
        {
            var requisition = CreateDraftRequisition();

            requisition.Submit();

            requisition.Status.Should().Be(RequisitionStatus.Submitted);
        }

        [Fact]
        public void Should_set_submitted_at()
        {
            var requisition = CreateDraftRequisition();
            var before = DateTime.UtcNow;

            requisition.Submit();

            requisition.SubmittedAt.Should().NotBeNull();
            requisition.SubmittedAt.Should().BeOnOrAfter(before);
        }

        [Fact]
        public void Should_not_submit_if_already_submitted()
        {
            var requisition = CreateDraftRequisition();
            requisition.Submit();

            var act = () => requisition.Submit();

            act.Should().Throw<InvalidOperationException>();
        }
    }

    public class Approve
    {
        [Fact]
        public void Should_change_status_to_approved()
        {
            var requisition = CreateDraftRequisition();
            requisition.Submit();

            requisition.Approve(Guid.NewGuid());

            requisition.Status.Should().Be(RequisitionStatus.Approved);
        }

        [Fact]
        public void Should_add_approval_record()
        {
            var requisition = CreateDraftRequisition();
            requisition.Submit();
            var approverId = Guid.NewGuid();

            requisition.Approve(approverId);

            requisition.Approvals.Should().HaveCount(1);
            requisition.Approvals.First().ApproverId.Should().Be(approverId);
        }

        [Fact]
        public void Should_not_approve_if_not_submitted()
        {
            var requisition = CreateDraftRequisition();

            var act = () => requisition.Approve(Guid.NewGuid());

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Should_not_approve_twice_by_same_approver()
        {
            var requisition = CreateDraftRequisition();
            requisition.Submit();
            var approverId = Guid.NewGuid();
            requisition.Approve(approverId);

            var act = () => requisition.Approve(approverId);

            act.Should().Throw<InvalidOperationException>();
        }
    }

    public class Reject
    {
        [Fact]
        public void Should_change_status_to_rejected()
        {
            var requisition = CreateDraftRequisition();
            requisition.Submit();

            requisition.Reject();

            requisition.Status.Should().Be(RequisitionStatus.Rejected);
        }

        [Fact]
        public void Should_not_reject_if_not_submitted()
        {
            var requisition = CreateDraftRequisition();

            var act = () => requisition.Reject();

            act.Should().Throw<InvalidOperationException>();
        }
    }

    public class CanTransitionTo
    {
        [Theory]
        [InlineData(RequisitionStatus.Draft, RequisitionStatus.Submitted, true)]
        [InlineData(RequisitionStatus.Draft, RequisitionStatus.Approved, false)]
        [InlineData(RequisitionStatus.Draft, RequisitionStatus.Rejected, false)]
        [InlineData(RequisitionStatus.Submitted, RequisitionStatus.Approved, true)]
        [InlineData(RequisitionStatus.Submitted, RequisitionStatus.Rejected, true)]
        [InlineData(RequisitionStatus.Submitted, RequisitionStatus.Draft, false)]
        [InlineData(RequisitionStatus.Approved, RequisitionStatus.ConvertedToPO, true)]
        [InlineData(RequisitionStatus.Approved, RequisitionStatus.Submitted, false)]
        [InlineData(RequisitionStatus.Rejected, RequisitionStatus.Submitted, false)]
        [InlineData(RequisitionStatus.ConvertedToPO, RequisitionStatus.Approved, false)]
        public void Should_validate_transition(
            RequisitionStatus from,
            RequisitionStatus to,
            bool expected)
        {
            var requisition = CreateDraftRequisition();

            ForceStatus(requisition, from);

            requisition.CanTransitionTo(to).Should().Be(expected);
        }

        private static void ForceStatus(Requisition requisition, RequisitionStatus status)
        {
            typeof(Requisition)
                .GetProperty(nameof(Requisition.Status))!
                .SetValue(requisition, status);
        }
    }
}